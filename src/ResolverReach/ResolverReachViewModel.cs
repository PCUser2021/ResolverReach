using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.IO.Compression;
using System.Net.Http.Headers;
using System.Reflection;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Text.Json;
using Avalonia.Media;
using DynamicData;
using DynamicData.Binding;
using ReactiveUI.Avalonia.Reactive;
using ReactiveUI.Reactive;

namespace ResolverReach;

public sealed class ResolverReachViewModel : ReactiveObject, IActivatableViewModel, IDisposable
{
    private const int MaximumEndpointsPerRun = 512;
    private static readonly HttpClient Http = new(new SocketsHttpHandler
    {
        ConnectTimeout = TimeSpan.FromSeconds(12),
        PooledConnectionLifetime = TimeSpan.FromMinutes(5),
        AutomaticDecompression = System.Net.DecompressionMethods.None
    })
    {
        Timeout = TimeSpan.FromSeconds(20)
    };

    private readonly CompositeDisposable _lifetime = new();
    private readonly SourceCache<ResolverResult, string> _resultCache = new(x => x.Endpoint);
    private readonly BehaviorSubject<ResolverDashboardState> _stateSubject = new(ResolverDashboardState.Initial);
    private readonly object _stateGate = new();
    private readonly object _runGate = new();
    private CancellationTokenSource? _activeRunCancellation;
    private readonly object _logGate = new();
    private readonly List<string> _activityLog = new();
    private readonly List<ResolverLogRow> _resultHistory = new();
    private readonly DateTimeOffset _sessionStartedAt = DateTimeOffset.Now;
    private int _runSequence;
    private readonly ObservableAsPropertyHelper<ResolverDashboardState> _dashboardState;
    private readonly ObservableAsPropertyHelper<bool> _hasResults;
    private readonly ObservableAsPropertyHelper<bool> _isEmptyResults;
    private readonly ObservableAsPropertyHelper<bool> _isBusy;
    private string _inputLinks = string.Empty;

    public ViewModelActivator Activator { get; } = new();
        public ReadOnlyObservableCollection<ResolverResult> Results { get; }

    public string InputLinks
    {
        get => _inputLinks;
        set => this.RaiseAndSetIfChanged(ref _inputLinks, value);
    }

    public string VersionLabel { get; } = "Version " + GetProductVersion();

    public ResolverDashboardState DashboardState => _dashboardState.Value;
    public bool HasResults => _hasResults.Value;
    public bool IsEmptyResults => _isEmptyResults.Value;
    public bool IsBusy => _isBusy.Value;

    public ReactiveCommand<Unit, Unit> CheckCatalogCommand { get; }
    public ReactiveCommand<Unit, Unit> CheckLinksCommand { get; }
    public ReactiveCommand<Unit, Unit> CancelCommand { get; }

    private static string GetProductVersion()
    {
        string? version = typeof(ResolverReachViewModel).Assembly
            .GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion;
        return string.IsNullOrWhiteSpace(version) ? "2.4.0" : version.Split('+', 2)[0];
    }

    public ResolverReachViewModel()
    {
        var resultConnection = _resultCache.Connect()
            .ObserveOn(RxSchedulers.TaskpoolScheduler)
            .SortAndBind(
                out ReadOnlyObservableCollection<ResolverResult> displayedResults,
                SortExpressionComparer<ResolverResult>.Ascending(x => x.Endpoint),
                new SortAndBindOptions { Scheduler = RxSchedulers.MainThreadScheduler })
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
        _hasResults = hasResults.ToProperty(this, x => x.HasResults, false, scheduler: RxSchedulers.MainThreadScheduler);
        _isEmptyResults = hasResults.Select(value => !value)
            .ToProperty(this, x => x.IsEmptyResults, true, scheduler: RxSchedulers.MainThreadScheduler);

        _dashboardState = _stateSubject
            .DistinctUntilChanged()
            .ToProperty(this, x => x.DashboardState, ResolverDashboardState.Initial, scheduler: RxSchedulers.MainThreadScheduler);

        IObservable<bool> canCheckLinks = this.WhenAnyValue(x => x.InputLinks)
            .Select(text => !string.IsNullOrWhiteSpace(text))
            .DistinctUntilChanged();

        CheckCatalogCommand = ReactiveCommand.CreateFromTask(
            (CancellationToken token) => Task.Run(() => RunEndpointCheckAsync(true, string.Empty, token), token),
            outputScheduler: RxSchedulers.MainThreadScheduler);
        CheckLinksCommand = ReactiveCommand.CreateFromTask(
            (CancellationToken token) =>
            {
                string capturedInput = InputLinks;
                return Task.Run(() => RunEndpointCheckAsync(false, capturedInput, token), token);
            },
            canCheckLinks,
            outputScheduler: RxSchedulers.MainThreadScheduler);
        _isBusy = Observable.CombineLatest(
                CheckCatalogCommand.IsExecuting,
                CheckLinksCommand.IsExecuting,
                (catalog, custom) => catalog || custom)
            .DistinctUntilChanged()
            .ToProperty(this, x => x.IsBusy, false, scheduler: RxSchedulers.MainThreadScheduler);

        CancelCommand = ReactiveCommand.Create(
            CancelActiveRun,
            this.WhenAnyValue(x => x.IsBusy),
            RxSchedulers.MainThreadScheduler);

        _lifetime.Add(_dashboardState);
        _lifetime.Add(_hasResults);
        _lifetime.Add(_isEmptyResults);
        _lifetime.Add(_isBusy);
        _lifetime.Add(CheckCatalogCommand);
        _lifetime.Add(CheckLinksCommand);
        _lifetime.Add(CancelCommand);

        AppendActivityLog($"Application session started. Version {GetProductVersion()}; OS {Environment.OSVersion}; runtime {Environment.Version}.");
    }

    public void RecordUiActivity(string message) => AppendActivityLog(message);

    public void SetStatusMessage(string message)
    {
        AppendActivityLog("UI status: " + message);
        UpdateState(state => state with { StatusText = message });
    }

    private void AppendActivityLog(string message)
    {
        string line = $"{DateTimeOffset.Now:O}\t{message}";
        lock (_logGate)
            _activityLog.Add(line);
    }

    private void RecordEndpointResult(int runNumber, string mode, ResolverResult result)
    {
        var row = new ResolverLogRow(
            DateTimeOffset.Now, runNumber, mode, result.Endpoint, result.Status,
            result.HttpStatusCode, result.LatencyMilliseconds, result.DnsSummary, result.Detail);
        lock (_logGate)
        {
            _resultHistory.Add(row);
            _activityLog.Add($"{row.Timestamp:O}\tRESULT\tRun #{runNumber} [{mode}] {result.Status} | {result.Endpoint} | {result.HttpLabel} | {result.LatencyLabel} | {result.DnsSummary} | {result.Detail}");
        }
    }

    private static string CsvEscape(string? value)
    {
        value ??= string.Empty;
        return "\"" + value.Replace("\"", "\"\"") + "\"";
    }

    private static string BuildResultsCsv(IEnumerable<ResolverLogRow> rows)
    {
        var builder = new StringBuilder();
        builder.AppendLine("timestamp,run_number,mode,endpoint,status,http_status,latency_ms,dns_summary,details");
        foreach (ResolverLogRow row in rows)
        {
            builder.Append(row.Timestamp.ToString("O", CultureInfo.InvariantCulture)).Append(',')
                .Append(row.RunNumber.ToString(CultureInfo.InvariantCulture)).Append(',')
                .Append(CsvEscape(row.Mode)).Append(',')
                .Append(CsvEscape(row.Endpoint)).Append(',')
                .Append(CsvEscape(row.Status)).Append(',')
                .Append(row.HttpStatusCode?.ToString(CultureInfo.InvariantCulture) ?? string.Empty).Append(',')
                .Append(row.LatencyMilliseconds?.ToString("F2", CultureInfo.InvariantCulture) ?? string.Empty).Append(',')
                .Append(CsvEscape(row.DnsSummary)).Append(',')
                .Append(CsvEscape(row.Detail)).AppendLine();
        }
        return builder.ToString();
    }

    private static async Task WriteZipEntryAsync(ZipArchive archive, string name, string content)
    {
        ZipArchiveEntry entry = archive.CreateEntry(name, CompressionLevel.Optimal);
        await using Stream entryStream = entry.Open();
        await using var writer = new StreamWriter(entryStream, new UTF8Encoding(false));
        await writer.WriteAsync(content);
        await writer.FlushAsync();
    }

    public async Task WriteLogsZipAsync(Stream destination)
    {
        ArgumentNullException.ThrowIfNull(destination);

        string[] activity;
        ResolverLogRow[] history;
        lock (_logGate)
        {
            activity = _activityLog.ToArray();
            history = _resultHistory.ToArray();
        }

        ResolverLogRow[] visibleResults = Results
            .Select(result => new ResolverLogRow(
                DateTimeOffset.Now, 0, "Currently visible results", result.Endpoint, result.Status,
                result.HttpStatusCode, result.LatencyMilliseconds, result.DnsSummary, result.Detail))
            .ToArray();
        ResolverDashboardState state = DashboardState;
        DateTimeOffset exportedAt = DateTimeOffset.Now;
        var sessionDetails = new
        {
            Product = "Resolver Reach",
            ProductVersion = GetProductVersion(),
            SessionStartedAt = _sessionStartedAt,
            ExportedAt = exportedAt,
            OperatingSystem = Environment.OSVersion.ToString(),
            RuntimeVersion = Environment.Version.ToString(),
            CurrentStatus = state.StatusText,
            CurrentTitle = state.Title,
            CurrentDetail = state.Detail,
            CurrentProgressPercent = state.ProgressValue,
            CurrentProgressLabel = state.ProgressLabel,
            ActivityEntryCount = activity.Length,
            HistoricalEndpointResultCount = history.Length,
            CurrentVisibleResultCount = visibleResults.Length
        };
        var summary = new StringBuilder()
            .AppendLine("Resolver Reach — full diagnostic log archive")
            .AppendLine($"Product version: {GetProductVersion()}")
            .AppendLine($"Session started: {_sessionStartedAt:O}")
            .AppendLine($"Archive created: {exportedAt:O}")
            .AppendLine($"Operating system: {Environment.OSVersion}")
            .AppendLine($"Runtime: {Environment.Version}")
            .AppendLine($"Activity log entries: {activity.Length}")
            .AppendLine($"Historical endpoint results: {history.Length}")
            .AppendLine($"Currently visible endpoint results: {visibleResults.Length}")
            .AppendLine($"Current state: {state.Title}")
            .AppendLine($"Current status: {state.StatusText}")
            .AppendLine($"Progress: {state.ProgressLabel} ({state.ProgressValue:F1}%)")
            .ToString();

        using var archive = new ZipArchive(destination, ZipArchiveMode.Create, leaveOpen: true);
        await WriteZipEntryAsync(archive, "session-summary.txt", summary).ConfigureAwait(false);
        await WriteZipEntryAsync(archive, "activity.log", string.Join(Environment.NewLine, activity) + Environment.NewLine).ConfigureAwait(false);
        await WriteZipEntryAsync(archive, "resolver-results-history.csv", BuildResultsCsv(history)).ConfigureAwait(false);
        await WriteZipEntryAsync(archive, "current-visible-results.csv", BuildResultsCsv(visibleResults)).ConfigureAwait(false);
        await WriteZipEntryAsync(archive, "session.json", JsonSerializer.Serialize(sessionDetails, new JsonSerializerOptions { WriteIndented = true })).ConfigureAwait(false);
    }

    private sealed record ResolverLogRow(
        DateTimeOffset Timestamp, int RunNumber, string Mode, string Endpoint, string Status,
        int? HttpStatusCode, double? LatencyMilliseconds, string DnsSummary, string Detail);

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
        int runNumber;
        string mode = loadCurlWikiCatalog ? "cURL resolver catalog" : "pasted endpoints";
        lock (_runGate)
        {
            if (_activeRunCancellation is not null) return;
            _activeRunCancellation = cancellation;
            runNumber = ++_runSequence;
        }

        AppendActivityLog($"Run #{runNumber} started ({mode}).");
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
                AppendActivityLog($"Run #{runNumber}: live cURL catalog returned {endpoints.Length} unique endpoint(s).");
            }
            else
            {
                endpoints = userInput
                    .Split(['\r', '\n', ';'], StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .ToArray();
                AppendActivityLog($"Run #{runNumber}: parsed {endpoints.Length} unique pasted endpoint(s).");
                foreach (string endpoint in endpoints)
                    AppendActivityLog($"Run #{runNumber} target: {endpoint}");
            }

            if (endpoints.Length == 0)
            {
                AppendActivityLog($"Run #{runNumber} stopped: no endpoint URLs found.");
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
                AppendActivityLog($"Run #{runNumber} rejected: {endpoints.Length} unique endpoints exceed the {MaximumEndpointsPerRun} endpoint safety limit.");
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

            using var gate = new SemaphoreSlim(6, 6);
            int completed = 0, healthy = 0, dnsErrors = 0, failed = 0, transportFailed = 0, httpInvalidFailed = 0, cancelled = 0;
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
                RecordEndpointResult(runNumber, mode, result);
                switch (result.Status)
                {
                    case "PASS": Interlocked.Increment(ref healthy); break;
                    case "DNS ERROR": Interlocked.Increment(ref dnsErrors); break;
                    case "FAIL":
                        Interlocked.Increment(ref failed);
                        if (result.HttpStatusCode.HasValue)
                            Interlocked.Increment(ref httpInvalidFailed);
                        else
                            Interlocked.Increment(ref transportFailed);
                        break;
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
            int transportFailuresTotal = Volatile.Read(ref transportFailed);
            int httpInvalidFailuresTotal = Volatile.Read(ref httpInvalidFailed);
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
                statusFinal = $"Stopped · {reachableTotal}/{completedTotal} valid DoH responses ({percentage:F1}%) · {failedTotal} failed checks ({transportFailuresTotal} transport, {httpInvalidFailuresTotal} HTTP/invalid reply){suffix}";
            }
            else
            {
                titleFinal = failedTotal == 0 && dnsTotal == 0 ? "All tested endpoints returned valid DoH responses"
                    : failedTotal == 0 ? "All endpoints returned valid DoH replies; some DNS errors"
                    : $"{reachableTotal} of {endpoints.Length} endpoints returned valid DoH responses";
                detailFinal = $"{reachableTotal}/{endpoints.Length} valid DoH responses ({percentage:F1}%) · {passTotal} NOERROR · {dnsTotal} DNS errors · {failedTotal} failed checks ({transportFailuresTotal} transport, {httpInvalidFailuresTotal} HTTP/invalid reply).";
                statusFinal = $"Complete · {reachableTotal}/{endpoints.Length} valid DoH responses ({percentage:F1}%) · {passTotal} NOERROR · {dnsTotal} DNS errors · {failedTotal} failed checks ({transportFailuresTotal} transport, {httpInvalidFailuresTotal} HTTP/invalid reply)";
            }

            UpdateState(state => state with
            {
                Title = titleFinal,
                Detail = detailFinal,
                StatusText = statusFinal,
                ProgressValue = 100.0,
                ProgressLabel = $"{finishedTotal}/{endpoints.Length}"
            });
            AppendActivityLog($"Run #{runNumber} summary: {statusFinal}.");
        }
        catch (OperationCanceledException) when (cancellation.IsCancellationRequested)
        {
            AppendActivityLog($"Run #{runNumber} cancelled before completion.");
            UpdateState(state => state with
            {
                Title = "Resolver check cancelled",
                Detail = "The active request was cancelled before the catalog check completed.",
                StatusText = "Cancelled before the full resolver list was tested."
            });
        }
        catch (Exception ex)
        {
            AppendActivityLog($"Run #{runNumber} failed: {ex}");
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
            AppendActivityLog($"Run #{runNumber} finished.");
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
            var post = await SendDohRequestWithRetryAsync(HttpMethod.Post, uri, query, cancellationToken).ConfigureAwait(false);
            if (post.Response.StatusCode is System.Net.HttpStatusCode.BadRequest
                or System.Net.HttpStatusCode.MethodNotAllowed
                or System.Net.HttpStatusCode.UnsupportedMediaType
                or System.Net.HttpStatusCode.NotImplemented)
            {
                int postStatus = (int)post.Response.StatusCode;
                post.Response.Dispose();
                // RFC 8484 allows GET and POST. Some published Base URLs expose only GET.
                Uri getUri = BuildGetQueryUri(uri, query);
                var get = await SendDohRequestWithRetryAsync(HttpMethod.Get, getUri, query, cancellationToken).ConfigureAwait(false);
                using (get.Response)
                {
                    return await EvaluateDnsResponseAsync(
                        input, get.Response, queryId, clock,
                        $"GET fallback after POST HTTP {postStatus}; {get.Transport}", cancellationToken).ConfigureAwait(false);
                }
            }

            using (post.Response)
            {
                return await EvaluateDnsResponseAsync(
                    input, post.Response, queryId, clock,
                    $"POST; {post.Transport}", cancellationToken).ConfigureAwait(false);
            }
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
                "Timed out", "Connection/request timed out after the configured timeout and one retry.");
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

    private static HttpRequestMessage CreateDohRequest(
        HttpMethod method, Uri uri, byte[] dnsQuery, bool forceHttp11)
    {
        var request = new HttpRequestMessage(method, uri)
        {
            Version = forceHttp11 ? new Version(1, 1) : new Version(2, 0),
            VersionPolicy = forceHttp11
                ? HttpVersionPolicy.RequestVersionExact
                : HttpVersionPolicy.RequestVersionOrLower
        };

        if (method == HttpMethod.Post)
        {
            request.Content = new ByteArrayContent(dnsQuery);
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/dns-message");
        }

        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/dns-message"));
        request.Headers.UserAgent.ParseAdd($"ResolverReach/{GetProductVersion()}");
        return request;
    }

    private static bool IsHttp2ProtocolFailure(HttpRequestException exception)
    {
        for (Exception? current = exception; current is not null; current = current.InnerException)
        {
            string message = current.Message;
            if (message.Contains("HTTP/2", StringComparison.OrdinalIgnoreCase) ||
                message.Contains("HttpProtocolError", StringComparison.OrdinalIgnoreCase) ||
                message.Contains("protocol error", StringComparison.OrdinalIgnoreCase))
                return true;
        }
        return false;
    }

    private static bool IsTransientHttpStatus(System.Net.HttpStatusCode statusCode)
    {
        int code = (int)statusCode;
        return code is 408 or 425 or 429 or 500 or 502 or 503 or 504;
    }

    private static async Task<(HttpResponseMessage Response, string Transport)> SendPreferredWithHttp11FallbackAsync(
        HttpMethod method, Uri uri, byte[] dnsQuery, CancellationToken cancellationToken)
    {
        try
        {
            using var request = CreateDohRequest(method, uri, dnsQuery, forceHttp11: false);
            HttpResponseMessage response = await Http.SendAsync(
                request, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
            return (response, $"HTTP/{response.Version}");
        }
        catch (HttpRequestException ex) when (IsHttp2ProtocolFailure(ex) && !cancellationToken.IsCancellationRequested)
        {
            using var retryRequest = CreateDohRequest(method, uri, dnsQuery, forceHttp11: true);
            HttpResponseMessage retryResponse = await Http.SendAsync(
                retryRequest, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
            return (retryResponse, "HTTP/1.1 compatibility fallback after HTTP/2 protocol error");
        }
    }

    private static async Task<(HttpResponseMessage Response, string Transport)> SendDohRequestWithRetryAsync(
        HttpMethod method, Uri uri, byte[] dnsQuery, CancellationToken cancellationToken)
    {
        (HttpResponseMessage Response, string Transport) first;
        try
        {
            first = await SendPreferredWithHttp11FallbackAsync(method, uri, dnsQuery, cancellationToken).ConfigureAwait(false);
        }
        catch (TaskCanceledException) when (!cancellationToken.IsCancellationRequested)
        {
            await Task.Delay(TimeSpan.FromMilliseconds(350), cancellationToken).ConfigureAwait(false);
            var retryAfterTimeout = await SendPreferredWithHttp11FallbackAsync(
                method, uri, dnsQuery, cancellationToken).ConfigureAwait(false);
            return (retryAfterTimeout.Response, $"one retry after timeout; {retryAfterTimeout.Transport}");
        }

        if (!IsTransientHttpStatus(first.Response.StatusCode))
            return first;

        int originalStatus = (int)first.Response.StatusCode;
        first.Response.Dispose();
        await Task.Delay(TimeSpan.FromMilliseconds(350), cancellationToken).ConfigureAwait(false);
        var retryAfterStatus = await SendPreferredWithHttp11FallbackAsync(
            method, uri, dnsQuery, cancellationToken).ConfigureAwait(false);
        return (retryAfterStatus.Response, $"one retry after HTTP {originalStatus}; {retryAfterStatus.Transport}");
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

        if (response.Content.Headers.ContentLength is long declaredLength && declaredLength > 65535)
        {
            clock.Stop();
            return new ResolverResult(endpoint, "FAIL", (int)response.StatusCode, clock.Elapsed.TotalMilliseconds,
                "Invalid DNS response", "Response exceeded the maximum DNS message length.");
        }

        // Bound the read itself. Checking only after ReadAsByteArrayAsync would
        // permit an untrusted endpoint to make the process buffer an oversized body.
        await using Stream responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);
        using var payloadBuffer = new MemoryStream(capacity: response.Content.Headers.ContentLength is long length
            ? (int)Math.Clamp(length, 0, 65535)
            : 4096);
        byte[] readBuffer = new byte[8192];
        while (true)
        {
            int read = await responseStream.ReadAsync(readBuffer.AsMemory(), cancellationToken);
            if (read == 0)
                break;

            if (payloadBuffer.Length + read > 65535)
            {
                clock.Stop();
                return new ResolverResult(endpoint, "FAIL", (int)response.StatusCode, clock.Elapsed.TotalMilliseconds,
                    "Invalid DNS response", "Response exceeded the maximum DNS message length.");
            }

            payloadBuffer.Write(readBuffer, 0, read);
        }

        byte[] payload = payloadBuffer.ToArray();
        clock.Stop();

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
    double ProgressValue, string ProgressLabel)
{
    public static ResolverDashboardState Initial { get; } = new(
        "Ready to check DNS resolvers",
        "Choose Test cURL Resolvers or paste HTTPS endpoints and click Test Resolvers.",
        "Idle", 0, "0 / 0");
}

public sealed class ResolverResult
{
    public string Endpoint { get; }
    public string Status { get; }
    public int? HttpStatusCode { get; }
    public double? LatencyMilliseconds { get; }
    public string DnsSummary { get; }
    public string Detail { get; }

    public bool IsPass => string.Equals(Status, "PASS", StringComparison.Ordinal);
    public bool IsDnsError => string.Equals(Status, "DNS ERROR", StringComparison.Ordinal);
    public bool IsFailure => Status is "FAIL" or "CANCELLED";

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
