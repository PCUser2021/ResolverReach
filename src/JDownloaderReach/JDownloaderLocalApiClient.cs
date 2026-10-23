using System.Text.Json;

namespace JDownloaderReach;

internal sealed class JDownloaderLocalApiClient : IDisposable
{
    private static readonly Uri LocalApiBaseAddress = new("http://127.0.0.1:3128/");
    private readonly HttpClient _httpClient = new()
    {
        BaseAddress = LocalApiBaseAddress,
        Timeout = TimeSpan.FromSeconds(5)
    };

    public async Task<string> GetStateAsync(CancellationToken cancellationToken = default)
    {
        string body = await GetTextAsync("downloads/getJDState", cancellationToken);
        if (string.IsNullOrWhiteSpace(body))
            return "The API returned an empty state response.";

        try
        {
            return JsonSerializer.Deserialize<string>(body) ?? body.Trim();
        }
        catch (JsonException)
        {
            return body.Trim();
        }
    }

    public async Task<IReadOnlyList<string>> GetDownloadPackagesAsync(CancellationToken cancellationToken = default)
    {
        string query = Uri.EscapeDataString(
            "{\"startAt\":0,\"maxResults\":100,\"childCount\":true,\"size\":true,\"hosts\":true,\"done\":true,\"enabled\":true}");
        string body = await GetTextAsync("downloads/queryPackages?" + query, cancellationToken);

        using JsonDocument document = JsonDocument.Parse(body);
        JsonElement collection = document.RootElement;
        if (collection.ValueKind == JsonValueKind.Object &&
            collection.TryGetProperty("data", out JsonElement data) &&
            data.ValueKind == JsonValueKind.Array)
        {
            collection = data;
        }

        if (collection.ValueKind != JsonValueKind.Array)
            throw new InvalidDataException("JDownloader returned a queue response that was not a JSON array.");

        var packages = new List<string>();
        foreach (JsonElement item in collection.EnumerateArray())
        {
            string name = ReadValue(item, "name") ?? "(unnamed package)";
            string id = ReadValue(item, "uuid") ?? "n/a";
            string count = ReadValue(item, "childCount") ?? "?";
            string size = ReadValue(item, "size") ?? "unknown";
            string done = ReadValue(item, "done") ?? "?";
            string host = ReadValue(item, "hosts") ?? "";

            packages.Add(
                name + " | links=" + count +
                " | size=" + size +
                " | complete=" + done +
                (string.IsNullOrWhiteSpace(host) ? "" : " | hosts=" + host) +
                " | packageId=" + id);
        }

        return packages;
    }

    public async Task<bool> AddLinksToLinkGrabberAsync(string links, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(links))
            throw new ArgumentException("Enter at least one link.", nameof(links));

        string[] parameters =
        [
            JsonSerializer.Serialize(links.Trim()),
            JsonSerializer.Serialize("JDownloader Reach"),
            JsonSerializer.Serialize(string.Empty),
            JsonSerializer.Serialize(string.Empty),
            JsonSerializer.Serialize(string.Empty)
        ];

        string query = string.Join("&", parameters.Select(Uri.EscapeDataString));
        string requestPath = "linkcollector/addLinks?" + query;
        if (requestPath.Length > 7000)
            throw new ArgumentOutOfRangeException(nameof(links), "The encoded link list is too long for one local API request. Split it into smaller batches.");

        return ParseBoolean(await GetTextAsync(requestPath, cancellationToken));
    }

    public async Task<bool> StartDownloadsAsync(CancellationToken cancellationToken = default) =>
        ParseBoolean(await GetTextAsync("downloads/start", cancellationToken));

    private async Task<string> GetTextAsync(string relativePath, CancellationToken cancellationToken)
    {
        using HttpResponseMessage response = await _httpClient.GetAsync(relativePath, cancellationToken);
        string body = await response.Content.ReadAsStringAsync(cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException(
                "JDownloader local API returned HTTP " + ((int)response.StatusCode) + " " +
                response.ReasonPhrase + ". Response: " + body);
        }

        return body;
    }

    private static bool ParseBoolean(string body)
    {
        string value = body.Trim();
        if (string.Equals(value, "true", StringComparison.OrdinalIgnoreCase) || value == "\"true\"")
            return true;
        if (string.Equals(value, "false", StringComparison.OrdinalIgnoreCase) || value == "\"false\"")
            return false;

        try
        {
            using JsonDocument document = JsonDocument.Parse(value);
            if (document.RootElement.ValueKind == JsonValueKind.True)
                return true;
            if (document.RootElement.ValueKind == JsonValueKind.False)
                return false;
        }
        catch (JsonException)
        {
        }

        throw new InvalidDataException("JDownloader returned an unrecognized result: " + value);
    }

    private static string? ReadValue(JsonElement element, string name)
    {
        if (element.ValueKind != JsonValueKind.Object || !element.TryGetProperty(name, out JsonElement value))
            return null;

        return value.ValueKind switch
        {
            JsonValueKind.String => value.GetString(),
            JsonValueKind.Number => value.ToString(),
            JsonValueKind.True => "true",
            JsonValueKind.False => "false",
            JsonValueKind.Null => null,
            _ => value.ToString()
        };
    }

    public void Dispose() => _httpClient.Dispose();
}
