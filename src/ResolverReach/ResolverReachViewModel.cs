using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Net.Http.Headers;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using Avalonia.Media;
using DynamicData;
using DynamicData.Binding;
using ReactiveUI;
using ReactiveUI.Avalonia;

namespace ResolverReach;

public sealed class ResolverReachViewModel : ReactiveObject, IActivatableViewModel, IDisposable
{
    private const int MaximumEndpointsPerRun = 512;
    private static readonly HttpClient Http = new(new SocketsHttpHandler
    {
        ConnectTimeout = TimeSpan.FromSeconds(5),
        PooledConnectionLifetime = TimeSpan.FromMinutes(5),
        AutomaticDecompression = System.Net.DecompressionMethods.None
    })
    {
        Timeout = TimeSpan.FromSeconds(10)
    };

    private readonly CompositeDisposable _lifetime = new();
    private readonly SourceCache<ResolverResult, string> _resultCache = new(x => x.Endpoint);
    private readonly BehaviorSubject<ResolverDashboardState> _stateSubject = new(ResolverDashboardState.Initial);
    private readonly object _stateGate = new();
    private readonly object _runGate = new();
    private CancellationTokenSource? _activeRunCancellation;
    private readonly ObservableAsPropertyHelper<ResolverDashboardState> _dashboardState;
    private readonly ObservableAsPropertyHelper<bool> _hasResults;
    private readonly ObservableAsPropertyHelper<bool> _isEmptyResults;
    private readonly ObservableAsPropertyHelper<bool> _isBusy;
    private string _inputLinks = string.Empty;

    public ViewModelActivator Activator { get; } = new();
    public Interaction<Unit, string?> ClipboardTextInteraction { get; } = new();
    public ReadOnlyObservableCollection<ResolverResult> Results { get; }

    public string InputLinks
    {
        get => _inputLinks;
        set => this.RaiseAndSetIfChanged(ref _inputLinks, value);
    }

    public ResolverDashboardState DashboardState => _dashboardState.Value;
    public bool HasResults => _hasResults.Value;
    public bool IsEmptyResults => _isEmptyResults.Value;
    public bool IsBusy => _isBusy.Value;

    public ReactiveCommand<Unit, Unit> CheckCatalogCommand { get; }
    public ReactiveCommand<Unit, Unit> CheckLinksCommand { get; }
    public ReactiveCommand<Unit, Unit> PasteLinksCommand { get; }
    public ReactiveCommand<Unit, Unit> CancelCommand { get; }

    public ResolverReachViewModel()
    {
        var resultConnection = _resultCache.Connect()
            .ObserveOn(RxApp.TaskpoolScheduler)
            .Sort(SortExpressionComparer<ResolverResult>.Ascending(x => x.Endpoint))
            .ObserveOn(RxApp.MainThreadScheduler)
            .Bind(out ReadOnlyObservableCollection<ResolverResult> displayedResults)
            .Subscribe();
        Results = displayedResults;
        _lifetime.Add(resultConnection);
        _lifetime.Add(_resultCache);
        _lifetime.Add(_stateSubject);

        IObservable<bool> hasResults = _resultCache.Connect()
            .ToCollection()
            .Select(items => items.Count > 0)
            .DistinctUntilChanged()
            .StartWith(false)
            .Replay(1)
            .RefCount();
        _hasResults = hasResults.ToProperty(this, x => x.HasResults, false, scheduler: RxApp.MainThreadScheduler);
        _isEmptyResults = hasResults.Select(value => !value)
            .ToProperty(this, x => x.IsEmptyResults, true, scheduler: RxApp.MainThreadScheduler);

        _dashboardState = _stateSubject
            .DistinctUntilChanged()
            .ToProperty(this, x => x.DashboardState, ResolverDashboardState.Initial, scheduler: RxApp.MainThreadScheduler);

        IObservable<bool> canCheckLinks = this.WhenAnyValue(x => x.InputLinks)
            .Select(text => !string.IsNullOrWhiteSpace(text))
            .DistinctUntilChanged();

        CheckCatalogCommand = ReactiveCommand.CreateFromTask(
            (CancellationToken token) => Task.Run(() => RunEndpointCheckAsync(true, string.Empty, token), token),
            outputScheduler: RxApp.MainThreadScheduler);
        CheckLinksCommand = ReactiveCommand.CreateFromTask(
            (CancellationToken token) =>
            {
                string capturedInput = InputLinks;
                return Task.Run(() => RunEndpointCheckAsync(false, capturedInput, token), token);
            },
            canCheckLinks,
            outputScheduler: RxApp.MainThreadScheduler);
        PasteLinksCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            UpdateState(state => state with
            {
                PastePanelVisible = true,
                StatusText = "Paste or review resolver URLs, then click Check."
            });
            string? clipboardText = await ClipboardTextInteraction.Handle(Unit.Default);
            if (!string.IsNullOrWhiteSpace(clipboardText))
                InputLinks = clipboardText;
        }, outputScheduler: RxApp.MainThreadScheduler);

        _isBusy = Observable.CombineLatest(
                CheckCatalogCommand.IsExecuting,
                CheckLinksCommand.IsExecuting,
                (catalog, custom) => catalog || custom)
            .DistinctUntilChanged()
            .ToProperty(this, x => x.IsBusy, false, scheduler: RxApp.MainThreadScheduler);

        CancelCommand = ReactiveCommand.Create(
            CancelActiveRun,
            this.WhenAnyValue(x => x.IsBusy),
            RxApp.MainThreadScheduler);

        _lifetime.Add(_dashboardState);
        _lifetime.Add(_hasResults);
        _lifetime.Add(_isEmptyResults);
        _lifetime.Add(_isBusy);
        _lifetime.Add(CheckCatalogCommand);
        _lifetime.Add(CheckLinksCommand);
        _lifetime.Add(PasteLinksCommand);
        _lifetime.Add(CancelCommand);

        this.WhenActivated(disposables =>
        {
            PasteLinksCommand.ThrownExceptions
                .Subscribe(ex => UpdateState(state => state with
                {
                    StatusText = "Clipboard error: " + ShortError(ex.Message),
                    Detail = "You can still paste manually with Ctrl+V."
                }))
                .DisposeWith(disposables);
        });
    }

    private void CancelActiveRun()
    {
        lock (_runGate)
            _activeRunCancellation?.Cancel();
    }

    private void UpdateState(Func<ResolverDashboardState, ResolverDashboardState> update)
    {
        lock (_stateGate)
            _stateSubject.OnNext(update(_stateSubject.Value));
    }

    private async Task RunEndpointCheckAsync(bool loadCurlWikiCatalog, string userInput, CancellationToken commandToken)
    {
        if (commandToken.IsCancellationRequested) return;
        using var cancellation = CancellationTokenSource.CreateLinkedTokenSource(commandToken);
        lock (_runGate)
        {
            if (_activeRunCancellation is not null) return;
            _activeRunCancellation = cancellation;
        }

        try
        {
            string[] endpoints;
            if (loadCurlWikiCatalog)
            {
                UpdateState(state => state with
                {
                    Title = "Loading cURL resolver catalog",
                    Detail = "Fetching the live Base URL column from the GitHub Wiki.",
                    StatusText = "Fetching current resolver URLs…",
                    ProgressValue = 0,
                    ProgressLabel = "Loading"
                });
                endpoints = (await CurlWikiResolverCatalog.FetchEndpointsAsync(cancellation.Token).ConfigureAwait(false)).ToArray();
            }
            else
            {
                endpoints = userInput
                    .Split(['\r', '\n', ';'], StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .ToArray();
            }

            if (endpoints.Length == 0)
            {
                UpdateState(state => state with
                {
                    Title = "No resolver URLs found",
                    Detail = "Paste one or more HTTPS DNS-over-HTTPS endpoint URLs and try again.",
                    StatusText = "No valid endpoint text was provided.",
                    ProgressValue = 0,
                    ProgressLabel = "0 tested"
                });
                return;
            }

            if (endpoints.Length > MaximumEndpointsPerRun)
            {
                UpdateState(state => state with
                {
                    Title = "Resolver list exceeds safety limit",
                    Detail = $"Found {endpoints.Length} unique URLs; limit is {MaximumEndpointsPerRun} per run.",
                    StatusText = "No partial test was started.",
                    ProgressValue = 0,
                    ProgressLabel = $"{endpoints.Length} found"
                });
                return;
            }

            _resultCache.Clear();
            UpdateState(state => state with
            {
                Title = loadCurlWikiCatalog ? "Checking cURL DoH resolvers" : "Checking pasted resolvers",
                Detail = "Validating the HTTPS request and returned DNS wire message.",
                StatusText = $"Starting {endpoints.Length} endpoint checks…",
                ProgressValue = 0,
                ProgressLabel = $"0 / {endpoints.Length}"
            });

            using var gate = new SemaphoreSlim(8, 8);
            int completed = 0, healthy = 0, dnsErrors = 0, failed = 0, cancelled = 0;
            var progressGate = new object();

            Task[] jobs = endpoints.Select(async endpoint =>
            {
                ResolverResult result;
                bool enteredGate = false;
                try
                {
                    await gate.WaitAsync(cancellation.Token).ConfigureAwait(false);
                    enteredGate = true;
                    result = await TestEndpointAsync(endpoint, cancellation.Token).ConfigureAwait(false);
                }
                catch (OperationCanceledException)
                {
                    result = new ResolverResult(endpoint, "CANCELLED", null, null, "Not completed", "Cancelled before completion.");
                }
                catch (Exception ex)
                {
                    result = new ResolverResult(endpoint, "FAIL", null, null, "No DNS response", ShortError(ex.Message));
                }
                finally
                {
                    if (enteredGate) gate.Release();
                }

                _resultCache.AddOrUpdate(result);
                switch (result.Status)
                {
                    case "PASS": Interlocked.Increment(ref healthy); break;
                    case "DNS ERROR": Interlocked.Increment(ref dnsErrors); break;
                    case "FAIL": Interlocked.Increment(ref failed); break;
                    case "CANCELLED": Interlocked.Increment(ref cancelled); break;
                }

                int finished = Interlocked.Increment(ref completed);
                if (finished % 4 == 0 || finished == endpoints.Length || cancellation.IsCancellationRequested)
                {
                    lock (progressGate)
                    {
                        int reachable = Volatile.Read(ref healthy) + Volatile.Read(ref dnsErrors);
                        UpdateState(state => state with
                        {
                            Title = cancellation.IsCancellationRequested ? "Stopping resolver check"
                                : loadCurlWikiCatalog ? "Checking cURL DoH resolvers" : "Checking pasted resolvers",
                            Detail = $"{finished} of {endpoints.Length} completed · {reachable} valid DoH responses so far.",
                            StatusText = cancellation.IsCancellationRequested
                                ? $"Cancellation requested · {finished} / {endpoints.Length} completed."
                                : $"Tested {finished} of {endpoints.Length} endpoint(s)…",
                            ProgressValue = 100.0 * finished / endpoints.Length,
                            ProgressLabel = $"{finished} / {endpoints.Length}"
                        });
                    }
                }
            }).ToArray();

            await Task.WhenAll(jobs).ConfigureAwait(false);

            int passTotal = Volatile.Read(ref healthy);
            int dnsTotal = Volatile.Read(ref dnsErrors);
            int failedTotal = Volatile.Read(ref failed);
            int cancelledTotal = Volatile.Read(ref cancelled);
            int finishedTotal = Volatile.Read(ref completed);
            int reachableTotal = passTotal + dnsTotal;
            int completedTotal = finishedTotal - cancelledTotal;
            double percentage = completedTotal == 0 ? 0 : 100.0 * reachableTotal / completedTotal;
            string suffix = cancelledTotal > 0 ? $" · {cancelledTotal} cancelled" : string.Empty;

            string titleFinal;
            string detailFinal;
            string statusFinal;
            if (cancellation.IsCancellationRequested)
            {
                titleFinal = loadCurlWikiCatalog ? "cURL resolver check stopped" : "Custom resolver check stopped";
                detailFinal = $"{reachableTotal} valid DoH responses · {failedTotal} unavailable · partial run.";
                statusFinal = $"Stopped · {reachableTotal}/{completedTotal} completed endpoints responded ({percentage:F1}%) · {failedTotal} unavailable{suffix}";
            }
            else
            {
                titleFinal = failedTotal == 0 && dnsTotal == 0 ? "All listed resolvers are healthy"
                    : failedTotal == 0 ? "All endpoints replied; some DNS errors"
                    : $"{reachableTotal} of {endpoints.Length} endpoints responded";
                detailFinal = $"{reachableTotal}/{endpoints.Length} valid DoH responses ({percentage:F1}%) · {passTotal} NOERROR · {dnsTotal} DNS errors · {failedTotal} failed.";
                statusFinal = $"Complete · {reachableTotal}/{endpoints.Length} reachable ({percentage:F1}%) · {passTotal} NOERROR · {dnsTotal} DNS errors · {failedTotal} unavailable";
            }

            UpdateState(state => state with
            {
                Title = titleFinal,
                Detail = detailFinal,
                StatusText = statusFinal,
                ProgressValue = 100.0,
                ProgressLabel = $"{finishedTotal}/{endpoints.Length}"
            });
        }
        catch (OperationCanceledException) when (cancellation.IsCancellationRequested)
        {
            UpdateState(state => state with
            {
                Title = "Resolver check cancelled",
                Detail = "The active request was cancelled before the catalog check completed.",
                StatusText = "Cancelled before the full resolver list was tested."
            });
        }
        catch (Exception ex)
        {
            UpdateState(state => state with
            {
                Title = "Resolver check failed",
                Detail = ShortError(ex.Message),
                StatusText = "Check failed: " + ShortError(ex.Message)
            });
        }
        finally
        {
            lock (_runGate)
            {
                if (ReferenceEquals(_activeRunCancellation, cancellation))
                    _activeRunCancellation = null;
            }
        }
    }

private static async Task<ResolverResult> TestEndpointAsync(string input, CancellationToken cancellationToken)
    {
        if (!Uri.TryCreate(input, UriKind.Absolute, out Uri? uri) ||
            !string.Equals(uri.Scheme, Uri.UriSchemeHttps, StringComparison.OrdinalIgnoreCase) ||
            string.IsNullOrWhiteSpace(uri.Host) || !string.IsNullOrEmpty(uri.UserInfo) ||
            !string.IsNullOrEmpty(uri.Fragment))
        {
            return new ResolverResult(input, "FAIL", null, null, "Not tested",
                "Endpoint must be an absolute HTTPS URL with a hostname; user-info and URL fragments are not allowed.");
        }

        byte[] query = DnsWire.BuildExampleAQuery(out ushort queryId);
        var clock = Stopwatch.StartNew();

        try
        {
            using var postRequest = new HttpRequestMessage(HttpMethod.Post, uri)
            {
                Content = new ByteArrayContent(query),
                Version = new Version(2, 0),
                VersionPolicy = HttpVersionPolicy.RequestVersionOrLower
            };
            postRequest.Content.Headers.ContentType = new MediaTypeHeaderValue("application/dns-message");
            postRequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/dns-message"));
            postRequest.Headers.UserAgent.ParseAdd("ResolverReach/1.0");

            using HttpResponseMessage postResponse = await Http.SendAsync(
                postRequest, HttpCompletionOption.ResponseHeadersRead, cancellationToken);

            // RFC 8484 supports both POST and GET. Some catalog endpoints only expose GET.
            if (postResponse.StatusCode is System.Net.HttpStatusCode.BadRequest
                or System.Net.HttpStatusCode.MethodNotAllowed
                or System.Net.HttpStatusCode.UnsupportedMediaType
                or System.Net.HttpStatusCode.NotImplemented)
            {
                Uri getUri = BuildGetQueryUri(uri, query);
                using var getRequest = new HttpRequestMessage(HttpMethod.Get, getUri)
                {
                    Version = new Version(2, 0),
                    VersionPolicy = HttpVersionPolicy.RequestVersionOrLower
                };
                getRequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/dns-message"));
                getRequest.Headers.UserAgent.ParseAdd("ResolverReach/1.0");

                using HttpResponseMessage getResponse = await Http.SendAsync(
                    getRequest, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
                return await EvaluateDnsResponseAsync(input, getResponse, queryId, clock, "GET fallback", cancellationToken);
            }

            return await EvaluateDnsResponseAsync(input, postResponse, queryId, clock, "POST", cancellationToken);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            clock.Stop();
            return new ResolverResult(input, "CANCELLED", null, clock.Elapsed.TotalMilliseconds,
                "Not completed", "Test cancelled by user.");
        }
        catch (TaskCanceledException)
        {
            clock.Stop();
            return new ResolverResult(input, "FAIL", null, clock.Elapsed.TotalMilliseconds,
                "Timed out", "Request timed out or was cancelled by the HTTP client.");
        }
        catch (HttpRequestException ex)
        {
            clock.Stop();
            return new ResolverResult(input, "FAIL", null, clock.Elapsed.TotalMilliseconds,
                "HTTPS / TLS failure", ShortError(ex.Message));
        }
        catch (Exception ex)
        {
            clock.Stop();
            return new ResolverResult(input, "FAIL", null, clock.Elapsed.TotalMilliseconds,
                "No DNS response", ShortError(ex.Message));
        }
    }

    private static Uri BuildGetQueryUri(Uri endpoint, byte[] dnsQuery)
    {
        string dnsParameter = Convert.ToBase64String(dnsQuery).TrimEnd('=').Replace('+', '-').Replace('/', '_');
        var builder = new UriBuilder(endpoint);
        string existingQuery = builder.Query.TrimStart('?');
        builder.Query = string.IsNullOrEmpty(existingQuery)
            ? $"dns={dnsParameter}"
            : $"{existingQuery}&dns={dnsParameter}";
        return builder.Uri;
    }

    private static async Task<ResolverResult> EvaluateDnsResponseAsync(
        string endpoint,
        HttpResponseMessage response,
        ushort queryId,
        Stopwatch clock,
        string requestMode,
        CancellationToken cancellationToken)
    {
        if (!response.IsSuccessStatusCode)
        {
            clock.Stop();
            return new ResolverResult(endpoint, "FAIL", (int)response.StatusCode, clock.Elapsed.TotalMilliseconds,
                "No DNS response", $"HTTP {(int)response.StatusCode} ({response.ReasonPhrase ?? "request rejected"}) via {requestMode}.");
        }

        string? mediaType = response.Content.Headers.ContentType?.MediaType;
        if (!string.Equals(mediaType, "application/dns-message", StringComparison.OrdinalIgnoreCase))
        {
            clock.Stop();
            return new ResolverResult(endpoint, "FAIL", (int)response.StatusCode, clock.Elapsed.TotalMilliseconds,
                "Unexpected content type", $"Expected application/dns-message, received {mediaType ?? "no Content-Type"} via {requestMode}.");
        }

        byte[] payload = await response.Content.ReadAsByteArrayAsync(cancellationToken);
        clock.Stop();

        if (payload.Length > 65535)
        {
            return new ResolverResult(endpoint, "FAIL", (int)response.StatusCode, clock.Elapsed.TotalMilliseconds,
                "Invalid DNS response", "Response exceeded the maximum DNS message length.");
        }

        if (!DnsWire.TryValidateResponse(payload, queryId, out int rcode, out int answers, out string validationError))
        {
            return new ResolverResult(endpoint, "FAIL", (int)response.StatusCode, clock.Elapsed.TotalMilliseconds,
                "Invalid DNS response", validationError);
        }

        string rcodeName = DnsWire.RcodeName(rcode);
        string detail = $"{answers} answer(s) · {payload.Length} DNS bytes · valid response via {requestMode}";
        string status = rcode == 0 ? "PASS" : "DNS ERROR";
        if (rcode != 0)
            detail = $"Valid DoH exchange via {requestMode}, but DNS returned {rcodeName}. {answers} answer(s).";

        return new ResolverResult(endpoint, status, (int)response.StatusCode, clock.Elapsed.TotalMilliseconds,
            $"RCODE {rcodeName}", detail);
    }

    private static string ShortError(string message)
    {
        const int limit = 180;
        message = message.Replace('\r', ' ').Replace('\n', ' ').Trim();
        return message.Length > limit ? message[..limit] + "…" : message;
    }
    public void Dispose() => _lifetime.Dispose();
}

public sealed record ResolverDashboardState(
    string Title, string Detail, string StatusText,
    double ProgressValue, string ProgressLabel, bool PastePanelVisible)
{
    public static ResolverDashboardState Initial { get; } = new(
        "Ready to check DNS resolvers",
        "Check the live cURL catalog or open Paste Links to test your own endpoints.",
        "Idle", 0, "0 tested", false);
}

public sealed class ResolverResult
{
    public string Endpoint { get; }
    public string Status { get; }
    public int? HttpStatusCode { get; }
    public double? LatencyMilliseconds { get; }
    public string DnsSummary { get; }
    public string Detail { get; }

    public string HttpLabel => HttpStatusCode.HasValue ? $"HTTP {HttpStatusCode.Value}" : "—";
    public string LatencyLabel => LatencyMilliseconds.HasValue
        ? $"{LatencyMilliseconds.Value.ToString("F0", CultureInfo.InvariantCulture)} ms"
        : "—";

    public IBrush StatusBackground => Status switch
    {
        "PASS" => new SolidColorBrush(Color.Parse("#DCFCE7")),
        "DNS ERROR" => new SolidColorBrush(Color.Parse("#FEF3C7")),
        "CANCELLED" => new SolidColorBrush(Color.Parse("#F1F5F9")),
        _ => new SolidColorBrush(Color.Parse("#FEE2E2"))
    };

    public IBrush StatusForeground => Status switch
    {
        "PASS" => new SolidColorBrush(Color.Parse("#166534")),
        "DNS ERROR" => new SolidColorBrush(Color.Parse("#92400E")),
        "CANCELLED" => new SolidColorBrush(Color.Parse("#475569")),
        _ => new SolidColorBrush(Color.Parse("#991B1B"))
    };

    public ResolverResult(string endpoint, string status, int? httpStatusCode, double? latencyMilliseconds,
        string dnsSummary, string detail)
    {
        Endpoint = endpoint;
        Status = status;
        HttpStatusCode = httpStatusCode;
        LatencyMilliseconds = latencyMilliseconds;
        DnsSummary = dnsSummary;
        Detail = detail;
    }
}

internal static class DnsWire
{
    public static byte[] BuildExampleAQuery(out ushort transactionId)
    {
        transactionId = (ushort)Random.Shared.Next(0, ushort.MaxValue + 1);
        const string domain = "example.com";
        string[] labels = domain.Split('.');
        int encodedNameLength = labels.Sum(x => 1 + Encoding.ASCII.GetByteCount(x)) + 1;
        byte[] packet = new byte[12 + encodedNameLength + 4];

        packet[0] = (byte)(transactionId >> 8);
        packet[1] = (byte)(transactionId & 0xff);
        packet[2] = 0x01; // Recursion Desired
        packet[3] = 0x00;
        packet[4] = 0x00;
        packet[5] = 0x01; // QDCOUNT = 1

        int offset = 12;
        foreach (string label in labels)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(label);
            packet[offset++] = (byte)bytes.Length;
            Buffer.BlockCopy(bytes, 0, packet, offset, bytes.Length);
            offset += bytes.Length;
        }

        packet[offset++] = 0x00; // root label
        packet[offset++] = 0x00;
        packet[offset++] = 0x01; // QTYPE = A
        packet[offset++] = 0x00;
        packet[offset] = 0x01;   // QCLASS = IN
        return packet;
    }

    public static bool TryValidateResponse(byte[] message, ushort expectedId, out int rcode,
        out int answerCount, out string error)
    {
        rcode = -1;
        answerCount = 0;
        error = string.Empty;

        if (message.Length < 12)
        {
            error = "Response is shorter than the 12-byte DNS header.";
            return false;
        }

        ushort id = ReadUInt16(message, 0);
        if (id != expectedId)
        {
            error = "DNS transaction ID did not match the request.";
            return false;
        }

        ushort flags = ReadUInt16(message, 2);
        if ((flags & 0x8000) == 0)
        {
            error = "DNS message did not have the response (QR) bit set.";
            return false;
        }

        if ((flags & 0x7800) != 0)
        {
            error = "DNS response used an unexpected opcode.";
            return false;
        }

        if (ReadUInt16(message, 4) != 1)
        {
            error = "DNS response did not echo exactly one question.";
            return false;
        }

        int offset = 12;
        if (!TryReadName(message, ref offset, out string questionName))
        {
            error = "DNS question name was malformed.";
            return false;
        }

        if (!string.Equals(questionName, "example.com", StringComparison.OrdinalIgnoreCase))
        {
            error = "DNS response question did not match example.com.";
            return false;
        }

        if (offset + 4 > message.Length)
        {
            error = "DNS response ended before the question type and class.";
            return false;
        }

        ushort qtype = ReadUInt16(message, offset);
        ushort qclass = ReadUInt16(message, offset + 2);
        if (qtype != 1 || qclass != 1)
        {
            error = "DNS response question was not type A / class IN.";
            return false;
        }

        answerCount = ReadUInt16(message, 6);
        rcode = flags & 0x000f;
        return true;
    }

    public static string RcodeName(int rcode) => rcode switch
    {
        0 => "NOERROR",
        1 => "FORMERR",
        2 => "SERVFAIL",
        3 => "NXDOMAIN",
        4 => "NOTIMP",
        5 => "REFUSED",
        6 => "YXDOMAIN",
        7 => "YXRRSET",
        8 => "NXRRSET",
        9 => "NOTAUTH",
        10 => "NOTZONE",
        _ => $"RCODE {rcode}"
    };

    private static ushort ReadUInt16(byte[] buffer, int offset) =>
        (ushort)((buffer[offset] << 8) | buffer[offset + 1]);

    private static bool TryReadName(byte[] message, ref int offset, out string name)
    {
        var labels = new List<string>();
        int cursor = offset;
        int? resumeOffset = null;

        for (int step = 0; step < 128 && cursor < message.Length; step++)
        {
            byte length = message[cursor];

            if ((length & 0xc0) == 0xc0)
            {
                if (cursor + 1 >= message.Length)
                {
                    name = string.Empty;
                    return false;
                }

                int pointer = ((length & 0x3f) << 8) | message[cursor + 1];
                if (pointer >= message.Length)
                {
                    name = string.Empty;
                    return false;
                }

                resumeOffset ??= cursor + 2;
                cursor = pointer;
                continue;
            }

            if ((length & 0xc0) != 0)
            {
                name = string.Empty;
                return false;
            }

            cursor++;
            if (length == 0)
            {
                offset = resumeOffset ?? cursor;
                name = string.Join('.', labels);
                return true;
            }

            if (length > 63 || cursor + length > message.Length)
            {
                name = string.Empty;
                return false;
            }

            labels.Add(Encoding.ASCII.GetString(message, cursor, length));
            cursor += length;
        }

        name = string.Empty;
        return false;
    }
}
