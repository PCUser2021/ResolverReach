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
        SetStatusRibbon("Resolver diagnostics ready", "No test run yet · HTTPS DNS exchange validation", "#512144");
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

    private async void RunTests_Click(object? sender, RoutedEventArgs e)
    {
        if (_runCancellation is not null) return;

        string[] endpoints = EndpointInput
            .Split(['\r', '\n', ';'], StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToArray();

        if (endpoints.Length == 0)
        {
            StatusText.Text = "Enter at least one HTTPS DoH endpoint URL.";
            return;
        }

        if (endpoints.Length > 50)
        {
            StatusText.Text = $"You entered {endpoints.Length} unique URLs. The limit is 50 per run.";
            return;
        }

        Results.Clear();
        EmptyState.IsVisible = false;
        ResultsScroll.IsVisible = true;
        ExportButton.IsEnabled = false;
        RunButton.IsEnabled = false;
        SamplesButton.IsEnabled = false;
        EndpointBox.IsEnabled = false;
        CancelButton.IsEnabled = true;
        TestProgress.Value = 0;
        ProgressText.Text = $"0 / {endpoints.Length}";
        StatusText.Text = $"Testing {endpoints.Length} endpoint(s)…";
        SetStatusRibbon("Testing DNS resolvers", $"Running valid DoH exchanges · 0 of {endpoints.Length} completed", "#66315A");

        var cancellation = new CancellationTokenSource();
        _runCancellation = cancellation;
        using var gate = new SemaphoreSlim(6, 6);
        int completed = 0;

        try
        {
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
                });
            }).ToArray();

            await Task.WhenAll(jobs);
            int passed = Results.Count(x => x.Status == "PASS");
            int dnsErrors = Results.Count(x => x.Status == "DNS ERROR");
            int failed = Results.Count(x => x.Status == "FAIL");
            int cancelled = Results.Count(x => x.Status == "CANCELLED");
            StatusText.Text = cancellation.IsCancellationRequested
                ? $"Stopped · {passed} pass · {dnsErrors} DNS error · {failed} failed · {cancelled} cancelled"
                : $"Complete · {passed} pass · {dnsErrors} DNS error · {failed} failed";
            SetStatusRibbon(
                cancellation.IsCancellationRequested ? "Resolver tests stopped" : passed > 0 ? "DNS diagnostics complete" : "No resolver passed validation",
                $"{passed} valid · {dnsErrors} DNS errors · {failed} failed" + (cancelled > 0 ? $" · {cancelled} cancelled" : string.Empty),
                cancellation.IsCancellationRequested ? "#634221" : passed > 0 ? "#28563F" : "#6A283C");
            ExportButton.IsEnabled = Results.Count > 0;
        }
        catch (Exception ex)
        {
            StatusText.Text = $"Test run error: {ShortError(ex.Message)}";
            SetStatusRibbon("Test run error", ShortError(ex.Message), "#6A283C");
        }
        finally
        {
            _runCancellation = null;
            cancellation.Dispose();
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
                "Enter an absolute HTTPS URL with a hostname. User-info and URL fragments are not allowed.");
        }

        byte[] query = DnsWire.BuildExampleAQuery(out ushort queryId);
        var clock = Stopwatch.StartNew();

        try
        {
            using var request = new HttpRequestMessage(HttpMethod.Post, uri)
            {
                Content = new ByteArrayContent(query),
                Version = new Version(2, 0),
                VersionPolicy = HttpVersionPolicy.RequestVersionOrLower
            };
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/dns-message");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/dns-message"));
            request.Headers.UserAgent.ParseAdd("ResolverReach/1.0");

            using HttpResponseMessage response = await Http.SendAsync(
                request, HttpCompletionOption.ResponseHeadersRead, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                clock.Stop();
                return new ResolverResult(input, "FAIL", (int)response.StatusCode, clock.Elapsed.TotalMilliseconds,
                    "No DNS response", $"HTTP {(int)response.StatusCode} ({response.ReasonPhrase ?? "request rejected"}).");
            }

            string? mediaType = response.Content.Headers.ContentType?.MediaType;
            if (!string.Equals(mediaType, "application/dns-message", StringComparison.OrdinalIgnoreCase))
            {
                clock.Stop();
                return new ResolverResult(input, "FAIL", (int)response.StatusCode, clock.Elapsed.TotalMilliseconds,
                    "Unexpected content type", $"Expected application/dns-message, received {mediaType ?? "no Content-Type"}.");
            }

            byte[] payload = await response.Content.ReadAsByteArrayAsync(cancellationToken);
            clock.Stop();

            if (payload.Length > 65535)
            {
                return new ResolverResult(input, "FAIL", (int)response.StatusCode, clock.Elapsed.TotalMilliseconds,
                    "Invalid DNS response", "Response exceeded the maximum DNS message length.");
            }

            if (!DnsWire.TryValidateResponse(payload, queryId, out int rcode, out int answers, out string validationError))
            {
                return new ResolverResult(input, "FAIL", (int)response.StatusCode, clock.Elapsed.TotalMilliseconds,
                    "Invalid DNS response", validationError);
            }

            string rcodeName = DnsWire.RcodeName(rcode);
            string detail = $"{answers} answer(s) · {payload.Length} DNS bytes · valid response";
            string status = rcode == 0 ? "PASS" : "DNS ERROR";
            if (rcode != 0)
                detail = $"Valid DoH exchange, but DNS returned {rcodeName}. {answers} answer(s).";

            return new ResolverResult(input, status, (int)response.StatusCode, clock.Elapsed.TotalMilliseconds,
                $"RCODE {rcodeName}", detail);
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
                "Timed out", "Request timed out after 10 seconds.");
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
