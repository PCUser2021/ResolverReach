using System.Net;
using System.Net.Http.Headers;
using System.Net.Sockets;
using System.Text.RegularExpressions;

namespace ResolverReach;

/// <summary>
/// Downloads the live curl/curl wiki and extracts only URLs from the public
/// resolver table's "Base URL" column. Provider homepages and documentation
/// links are deliberately excluded.
/// </summary>
internal static class CurlWikiResolverCatalog
{
    public const string WikiUrl = "https://github.com/curl/curl/wiki/DNS-over-HTTPS";
    private const string RawMarkdownUrl = "https://raw.githubusercontent.com/wiki/curl/curl/DNS-over-HTTPS.md";
    private const int MaximumEndpoints = 512;

    private static readonly HttpClient Http = new(new SocketsHttpHandler
    {
        ConnectTimeout = TimeSpan.FromSeconds(8),
        PooledConnectionLifetime = TimeSpan.FromMinutes(2),
        AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate | DecompressionMethods.Brotli
    })
    {
        Timeout = TimeSpan.FromSeconds(20)
    };

    private static readonly Regex HttpsUrlPattern = new(
        @"https://[^\s<>|""'`]+",
        RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.CultureInvariant);

    public static async Task<IReadOnlyList<string>> FetchEndpointsAsync(CancellationToken cancellationToken)
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, RawMarkdownUrl);
        Version? productVersion = typeof(CurlWikiResolverCatalog).Assembly.GetName().Version;
        string productVersionLabel = productVersion is null
            ? "2.3.0"
            : $"{productVersion.Major}.{productVersion.Minor}.{Math.Max(0, productVersion.Build)}";
        request.Headers.UserAgent.ParseAdd($"ResolverReach/{productVersionLabel}");
        request.Headers.CacheControl = new CacheControlHeaderValue { NoCache = true };

        using HttpResponseMessage response = await Http.SendAsync(
            request, HttpCompletionOption.ResponseContentRead, cancellationToken);
        response.EnsureSuccessStatusCode();

        string markdown = await response.Content.ReadAsStringAsync(cancellationToken);
        IReadOnlyList<string> endpoints = ParseMarkdown(markdown);
        if (endpoints.Count == 0)
        {
            throw new InvalidOperationException(
                "The cURL wiki was downloaded, but no valid HTTPS resolver endpoints were found in its Base URL column.");
        }

        if (endpoints.Count > MaximumEndpoints)
        {
            throw new InvalidOperationException(
                $"The cURL wiki currently lists {endpoints.Count} unique HTTPS resolver endpoints, exceeding the safety limit of {MaximumEndpoints}. No partial catalog was tested.");
        }

        return endpoints;
    }

    internal static IReadOnlyList<string> ParseMarkdown(string markdown)
    {
        string[] lines = markdown.Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries);
        int sectionStart = Array.FindIndex(lines, line =>
            string.Equals(line.Trim(), "# Publicly available servers", StringComparison.OrdinalIgnoreCase));

        if (sectionStart < 0)
        {
            throw new InvalidOperationException(
                "The downloaded cURL wiki does not contain the expected 'Publicly available servers' section.");
        }

        var endpoints = new List<string>();
        var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        bool foundTable = false;
        var pendingRow = new System.Text.StringBuilder();

        for (int index = sectionStart + 1; index < lines.Length; index++)
        {
            string line = lines[index].Trim();
            if (line.StartsWith("*: Tested via", StringComparison.OrdinalIgnoreCase))
            {
                if (pendingRow.Length > 0)
                    AddRow(pendingRow.ToString());
                break;
            }

            if (line.Contains("Who runs it", StringComparison.OrdinalIgnoreCase))
            {
                foundTable = true;
                pendingRow.Clear();
                continue;
            }
            if (!foundTable)
                continue;

            // Ignore Markdown table separators and single-cell letter labels.
            string withoutPipes = line.Replace("|", string.Empty, StringComparison.Ordinal).Trim();
            if (withoutPipes.Length == 0 ||
                withoutPipes.All(character => character is '-' or ':' or ' ' or '\t') ||
                Regex.IsMatch(line, @"^\|\s*\*\*[A-Z0-9-]+\*\*\s*$", RegexOptions.IgnoreCase))
            {
                continue;
            }

            if (pendingRow.Length == 0)
            {
                if (!line.Contains('|'))
                    continue;
                pendingRow.Append(line);
            }
            else
            {
                // The wiki occasionally breaks a very long table row over
                // several physical lines, including before the Base URL cell ends.
                pendingRow.Append(' ').Append(line);
            }

            // The four-column table has at least three pipe separators.
            // Wait to assemble the whole logical row before examining cell #2.
            if (pendingRow.ToString().Count(character => character == '|') >= 3)
            {
                AddRow(pendingRow.ToString());
                pendingRow.Clear();
            }
        }

        if (!foundTable)
        {
            throw new InvalidOperationException(
                "The cURL wiki Base URL table could not be identified. The page layout may have changed.");
        }

        return endpoints;

        void AddRow(string rawRow)
        {
            string row = rawRow.Trim();
            if (row.StartsWith('|'))
                row = row[1..];

            string[] cells = row.Split('|');
            if (cells.Length < 2)
                return;

            string baseUrlCell = cells[1];
            foreach (Match match in HttpsUrlPattern.Matches(baseUrlCell))
            {
                string candidate = match.Value.TrimEnd('.', ',', ';', ':', ')', ']', '}');
                if (!Uri.TryCreate(candidate, UriKind.Absolute, out Uri? uri) ||
                    !string.Equals(uri.Scheme, Uri.UriSchemeHttps, StringComparison.OrdinalIgnoreCase) ||
                    string.IsNullOrWhiteSpace(uri.Host) ||
                    !string.IsNullOrEmpty(uri.UserInfo) ||
                    !string.IsNullOrEmpty(uri.Fragment) ||
                    !IsPublicHostname(uri))
                {
                    continue;
                }

                string canonical = uri.AbsoluteUri;
                if (seen.Add(canonical))
                    endpoints.Add(canonical);

                if (endpoints.Count > MaximumEndpoints)
                {
                    throw new InvalidOperationException(
                        $"The cURL wiki contains more than {MaximumEndpoints} unique HTTPS resolver URLs. No partial catalog was accepted.");
                }
            }
        }
    }

    private static bool IsPublicHostname(Uri uri)
    {
        string host = uri.IdnHost.TrimEnd('.');
        if (host.Length == 0 ||
            !host.Contains('.') ||
            host.Equals("localhost", StringComparison.OrdinalIgnoreCase) ||
            host.EndsWith(".localhost", StringComparison.OrdinalIgnoreCase) ||
            host.EndsWith(".local", StringComparison.OrdinalIgnoreCase) ||
            host.EndsWith(".internal", StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        // Resolver URLs should use TLS hostnames rather than raw IP literals.
        // This also prevents a modified remote list from targeting local services.
        if (IPAddress.TryParse(host, out IPAddress? address))
        {
            if (IPAddress.IsLoopback(address) ||
                address.Equals(IPAddress.Any) ||
                address.Equals(IPAddress.IPv6Any) ||
                address.IsIPv6LinkLocal ||
                address.IsIPv6SiteLocal ||
                address.IsIPv6Multicast)
                return false;

            byte[] bytes = address.GetAddressBytes();
            if (address.AddressFamily == AddressFamily.InterNetwork &&
                (bytes[0] == 10 ||
                 bytes[0] == 127 ||
                 bytes[0] == 0 ||
                 (bytes[0] == 169 && bytes[1] == 254) ||
                 (bytes[0] == 172 && bytes[1] >= 16 && bytes[1] <= 31) ||
                 (bytes[0] == 192 && bytes[1] == 168) ||
                 (bytes[0] == 100 && bytes[1] >= 64 && bytes[1] <= 127) ||
                 (bytes[0] >= 224)))
                return false;
        }

        return true;
    }
}
