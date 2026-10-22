using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Net.Http.Headers;
using System.Text;
using Avalonia;
using Avalonia.Controls;
using FluentAvalonia.UI.Windowing;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Platform.Storage;
using Avalonia.Threading;

namespace ResolverReach;

public partial class TesterWindow : AppWindow, INotifyPropertyChanged
{
    private const int MaximumEndpointsPerRun = 512;
    private static readonly string[] SampleEndpoints =
    [
        "https://cloudflare-dns.com/dns-query",
        "https://dns.google/dns-query",
        "https://dns.quad9.net/dns-query",
        "https://dns.adguard-dns.com/dns-query"
    ];

    private static readonly HttpClient Http = new(new SocketsHttpHandler
    {
        ConnectTimeout = TimeSpan.FromSeconds(5),
        PooledConnectionLifetime = TimeSpan.FromMinutes(5),
        AutomaticDecompression = System.Net.DecompressionMethods.None
    })
    {
        Timeout = TimeSpan.FromSeconds(10)
    };

    private string _endpointInput = string.Join(Environment.NewLine, SampleEndpoints);
    private readonly Action<string, string, string>? _reportStatus;
    private CancellationTokenSource? _runCancellation;

    private event PropertyChangedEventHandler? _propertyChanged;
    event PropertyChangedEventHandler? INotifyPropertyChanged.PropertyChanged
    {
        add => _propertyChanged += value;
        remove => _propertyChanged -= value;
    }
    public ObservableCollection<ResolverResult> Results { get; } = new();

    public string EndpointInput
    {
        get => _endpointInput;
        set
        {
            if (_endpointInput == value) return;
            _endpointInput = value;
            _propertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(EndpointInput)));
        }
    }

    public TesterWindow(Action<string, string, string>? reportStatus = null)
    {
        InitializeComponent();
        _reportStatus = reportStatus;
        TitleBar.ExtendsContentIntoTitleBar = true;
        TitleBar.TitleBarHitTestType = TitleBarHitTestType.Complex;
        TransparencyLevelHint = [WindowTransparencyLevel.Mica, WindowTransparencyLevel.AcrylicBlur];
        DataContext = this;
        ResultsList.ItemsSource = Results;
        EmptyState.IsVisible = true;
        ResultsScroll.IsVisible = false;
        ProgressText.Text = "0 tested";
        TestProgress.Value = 0;
        SetStatusRibbon("Resolver diagnostics ready", "No test run yet · HTTPS DNS exchange validation", "#0E355D");
    }

    private void SetStatusRibbon(string title, string detail, string fill)
    {
        StatusRibbonTitle.Text = title;
        StatusRibbonText.Text = detail;
        StatusRibbon.Background = new SolidColorBrush(Color.Parse(fill));
        _reportStatus?.Invoke(title, detail, fill);
    }

    private void Samples_Click(object? sender, RoutedEventArgs e)
    {
        if (_runCancellation is not null) return;
        EndpointInput = string.Join(Environment.NewLine, SampleEndpoints);
        StatusText.Text = "Sample endpoints loaded. Review the list, then run the test.";
    }

    private void Cancel_Click(object? sender, RoutedEventArgs e)
    {
        if (_runCancellation is null) return;
        _runCancellation.Cancel();
        StatusText.Text = "Cancellation requested…";
        SetStatusRibbon("Stopping resolver tests", "Waiting for active HTTPS requests to finish", "#634221");
    }

    private async void RunTests_Click(object? sender, RoutedEventArgs e) =>
        await RunEndpointCheckAsync(loadCurlWikiCatalog: false);

    private async void CheckNow_Click(object? sender, RoutedEventArgs e) =>
        await CheckAllCurlResolversAsync();

    public Task CheckAllCurlResolversAsync() =>
        RunEndpointCheckAsync(loadCurlWikiCatalog: true);

    private async Task RunEndpointCheckAsync(bool loadCurlWikiCatalog)
    {
        if (_runCancellation is not null) return;

        var cancellation = new CancellationTokenSource();
        _runCancellation = cancellation;
        CheckNowButton.IsEnabled = false;
        RunButton.IsEnabled = false;
        SamplesButton.IsEnabled = false;
        EndpointBox.IsEnabled = false;
        CancelButton.IsEnabled = true;
        ExportButton.IsEnabled = false;

        try
        {
            string[] endpoints;
            if (loadCurlWikiCatalog)
            {
                StatusText.Text = "Fetching the current curl/curl DoH resolver table…";
                SetStatusRibbon("Loading cURL resolver catalog", "Fetching the live Base URL column from GitHub Wiki", "#0E355D");
                endpoints = (await CurlWikiResolverCatalog.FetchEndpointsAsync(cancellation.Token)).ToArray();
                EndpointInput = string.Join(Environment.NewLine, endpoints);
                StatusText.Text = $"Loaded {endpoints.Length} unique resolver URLs. Starting DoH checks…";
            }
            else
            {
                endpoints = EndpointInput
                    .Split(['\r', '\n', ';'], StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .ToArray();
            }

            if (endpoints.Length == 0)
            {
                StatusText.Text = "No HTTPS DoH endpoint URLs were found.";
                SetStatusRibbon("No endpoints found", "The source list contained no usable HTTPS resolver URLs", "#6A283C");
                return;
            }

            if (endpoints.Length > MaximumEndpointsPerRun)
            {
                StatusText.Text = $"Found {endpoints.Length} unique URLs; safety limit is {MaximumEndpointsPerRun} per run. No partial test was started.";
                SetStatusRibbon("Resolver list exceeds safety limit", $"{endpoints.Length} found · limit {MaximumEndpointsPerRun}", "#6A283C");
                return;
            }

            Results.Clear();
            EmptyState.IsVisible = false;
            ResultsScroll.IsVisible = true;
            TestProgress.Value = 0;
            ProgressText.Text = $"0 / {endpoints.Length}";
            StatusText.Text = loadCurlWikiCatalog
                ? $"Testing all {endpoints.Length} unique URLs from the current cURL wiki list…"
                : $"Testing {endpoints.Length} endpoint(s)…";
            SetStatusRibbon(
                loadCurlWikiCatalog ? "Checking cURL DoH resolver catalog" : "Testing DNS resolvers",
                $"Validated DNS exchanges · 0 of {endpoints.Length} completed",
                "#0E355D");

            using var gate = new SemaphoreSlim(8, 8);
            int completed = 0;

            Task[] jobs = endpoints.Select(async endpoint =>
            {
                ResolverResult result;
                bool enteredGate = false;
                try
                {
                    await gate.WaitAsync(cancellation.Token);
                    enteredGate = true;
                    result = await TestEndpointAsync(endpoint, cancellation.Token);
                }
                catch (OperationCanceledException)
                {
                    result = new ResolverResult(endpoint, "CANCELLED", null, null, "Not completed", "Test cancelled by user.");
                }
                catch (Exception ex)
                {
                    result = new ResolverResult(endpoint, "FAIL", null, null, "No DNS response", ShortError(ex.Message));
                }
                finally
                {
                    if (enteredGate) gate.Release();
                }

                int finished = Interlocked.Increment(ref completed);
                await Dispatcher.UIThread.InvokeAsync(() =>
                {
                    Results.Add(result);
                    TestProgress.Value = 100.0 * finished / endpoints.Length;
                    ProgressText.Text = $"{finished} / {endpoints.Length}";
                    StatusText.Text = cancellation.IsCancellationRequested
                        ? $"Cancellation requested · {finished} of {endpoints.Length} completed"
                        : $"Tested {finished} of {endpoints.Length} endpoint(s)…";

                    if (finished % 8 == 0 || finished == endpoints.Length)
                    {
                        int responsive = Results.Count(x => x.Status is "PASS" or "DNS ERROR");
                        SetStatusRibbon(
                            loadCurlWikiCatalog ? "Checking cURL DoH resolver catalog" : "Testing DNS resolvers",
                            $"{finished}/{endpoints.Length} completed · {responsive} valid DoH responses so far",
                            "#0E355D");
                    }
                });
            }).ToArray();

            await Task.WhenAll(jobs);

            int healthy = Results.Count(x => x.Status == "PASS");
            int dnsErrors = Results.Count(x => x.Status == "DNS ERROR");
            int reachable = healthy + dnsErrors;
            int failed = Results.Count(x => x.Status == "FAIL");
            int cancelled = Results.Count(x => x.Status == "CANCELLED");
            int completedResults = Results.Count(x => x.Status != "CANCELLED");
            double percentReachable = completedResults == 0 ? 0 : 100.0 * reachable / completedResults;
            string cancelledSuffix = cancelled > 0 ? $" · {cancelled} cancelled" : string.Empty;

            if (loadCurlWikiCatalog)
            {
                StatusText.Text = cancellation.IsCancellationRequested
                    ? $"Stopped · {reachable}/{completedResults} completed endpoints responded ({percentReachable:F1}%) · {failed} unavailable{cancelledSuffix}"
                    : $"Catalog check complete · {reachable}/{endpoints.Length} reachable ({percentReachable:F1}%) · {healthy} NOERROR · {dnsErrors} DNS errors · {failed} unavailable";

                string title = cancellation.IsCancellationRequested
                    ? "cURL resolver check stopped"
                    : failed == 0 && dnsErrors == 0
                        ? "All listed resolvers are healthy"
                        : failed == 0
                            ? "All endpoints replied; some DNS errors"
                            : $"{reachable} of {endpoints.Length} endpoints responded";
                string detail = cancellation.IsCancellationRequested
                    ? $"{reachable} valid DoH responses · {failed} unavailable · partial run"
                    : $"{reachable}/{endpoints.Length} valid DoH responses ({percentReachable:F1}%) · {healthy} NOERROR · {dnsErrors} DNS RCODE errors · {failed} failed";
                SetStatusRibbon(title, detail,
                    cancellation.IsCancellationRequested ? "#634221" :
                    failed == 0 && dnsErrors == 0 ? "#28563F" :
                    failed == 0 ? "#634221" : "#0E355D");
            }
            else
            {
                StatusText.Text = cancellation.IsCancellationRequested
                    ? $"Stopped · {healthy} pass · {dnsErrors} DNS error · {failed} failed · {cancelled} cancelled"
                    : $"Complete · {healthy} pass · {dnsErrors} DNS error · {failed} failed";
                SetStatusRibbon(
                    cancellation.IsCancellationRequested ? "Resolver tests stopped" : healthy > 0 ? "DNS diagnostics complete" : "No resolver passed validation",
                    $"{reachable} valid DoH response(s) · {healthy} NOERROR · {dnsErrors} DNS errors · {failed} failed" + cancelledSuffix,
                    cancellation.IsCancellationRequested ? "#634221" : healthy > 0 ? "#28563F" : "#6A283C");
            }

            ExportButton.IsEnabled = Results.Count > 0;
        }
        catch (OperationCanceledException) when (cancellation.IsCancellationRequested)
        {
            StatusText.Text = "Check cancelled before the full resolver list was tested.";
            SetStatusRibbon("Resolver check cancelled", "Any completed results remain available for review", "#634221");
        }
        catch (Exception ex)
        {
            StatusText.Text = $"Check failed: {ShortError(ex.Message)}";
            SetStatusRibbon("Resolver check failed", ShortError(ex.Message), "#6A283C");
        }
        finally
        {
            _runCancellation = null;
            cancellation.Dispose();
            CheckNowButton.IsEnabled = true;
            RunButton.IsEnabled = true;
            SamplesButton.IsEnabled = true;
            EndpointBox.IsEnabled = true;
            CancelButton.IsEnabled = false;
            ExportButton.IsEnabled = Results.Count > 0;
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

    private async void Export_Click(object? sender, RoutedEventArgs e)
    {
        if (Results.Count == 0) return;

        try
        {
            var saveOptions = new FilePickerSaveOptions
            {
                Title = "Export Resolver Reach results",
                SuggestedFileName = $"ResolverReach-results-{DateTime.Now:yyyyMMdd-HHmmss}.csv",
                DefaultExtension = "csv",
                FileTypeChoices =
                [
                    new FilePickerFileType("CSV file") { Patterns = ["*.csv"] }
                ]
            };

            IStorageFile? file = await StorageProvider.SaveFilePickerAsync(saveOptions);
            if (file is null) return;

            await using Stream stream = await file.OpenWriteAsync();
            await using var writer = new StreamWriter(stream, new UTF8Encoding(encoderShouldEmitUTF8Identifier: true));
            await writer.WriteLineAsync("Endpoint,Status,HttpStatus,LatencyMs,DnsSummary,Detail");
            foreach (ResolverResult item in Results)
            {
                string[] values =
                [
                    item.Endpoint,
                    item.Status,
                    item.HttpStatusCode?.ToString(CultureInfo.InvariantCulture) ?? "",
                    item.LatencyMilliseconds?.ToString("F1", CultureInfo.InvariantCulture) ?? "",
                    item.DnsSummary,
                    item.Detail
                ];
                await writer.WriteLineAsync(string.Join(",", values.Select(CsvEscape)));
            }

            StatusText.Text = $"Exported {Results.Count} result(s) to CSV.";
        }
        catch (Exception ex)
        {
            StatusText.Text = $"CSV export failed: {ShortError(ex.Message)}";
        }
    }

    private static string CsvEscape(string value)
    {
        string quote = ((char)34).ToString();
        return quote + value.Replace(quote, quote + quote, StringComparison.Ordinal) + quote;
    }

    private static string ShortError(string message)
    {
        const int limit = 180;
        message = message.Replace('\r', ' ').Replace('\n', ' ').Trim();
        return message.Length > limit ? message[..limit] + "…" : message;
    }
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
