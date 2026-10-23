using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using System.Diagnostics;
using System.IO.Compression;
using System.Text;
using System.Text.Json;

namespace JDownloaderReach;

public partial class MainWindow : Window
{
    private readonly JDownloaderLocalApiClient _jDownloaderApi = new();
    private readonly List<string> _activityLog = new();
    private readonly object _logGate = new();
    private readonly string _settingsDirectory = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "JDownloaderReach");
    private string SettingsPath => Path.Combine(_settingsDirectory, "settings.json");

    public MainWindow()
    {
        InitializeComponent();
        LoadSettings();
        RecordActivity("JDownloader Reach prototype 0.1.0 started.");
        SetStatus("Ready. If the local API is not enabled, start JDownloader and enable its local API first.");
    }

    private void LoadSettings()
    {
        AppSettings settings = new();
        try
        {
            if (File.Exists(SettingsPath))
                settings = JsonSerializer.Deserialize<AppSettings>(File.ReadAllText(SettingsPath)) ?? new AppSettings();
        }
        catch (Exception ex)
        {
            RecordActivity("Could not load settings: " + ex.Message);
        }

        JDownloaderPathBox.Text = ExistingPath(settings.JDownloaderPath, FindJDownloaderPath()) ?? string.Empty;
        ResolverReachPathBox.Text = ExistingPath(
            settings.ResolverReachPath,
            Path.Combine(AppContext.BaseDirectory, "tools", "ResolverReach", "ResolverReach.exe"),
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "Resolver Reach", "ResolverReach.exe"),
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Programs", "Resolver Reach", "ResolverReach.exe")) ?? string.Empty;
        GgufImporterPathBox.Text = ExistingPath(
            settings.GgufImporterPath,
            Path.Combine(AppContext.BaseDirectory, "tools", "OllamaGgufImporter", "Ollama-GGUF-Importer.exe"),
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "Ollama GGUF Importer", "Ollama-GGUF-Importer.exe"),
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Programs", "Ollama GGUF Importer", "Ollama-GGUF-Importer.exe")) ?? string.Empty;
    }

    private static string? ExistingPath(params string?[] paths) =>
        paths.FirstOrDefault(path => !string.IsNullOrWhiteSpace(path) && File.Exists(path));

    private static string? FindJDownloaderPath()
    {
        string programFiles = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
        string programFilesX86 = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86);
        string local = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

        return ExistingPath(
            Path.Combine(AppContext.BaseDirectory, "JDownloader2.exe"),
            Path.Combine(programFiles, "JDownloader", "JDownloader2.exe"),
            Path.Combine(programFiles, "JDownloader 2.0", "JDownloader2.exe"),
            Path.Combine(programFilesX86, "JDownloader", "JDownloader2.exe"),
            Path.Combine(programFilesX86, "JDownloader 2.0", "JDownloader2.exe"),
            Path.Combine(local, "JDownloader 2.0", "JDownloader2.exe"));
    }

    private void SaveSettings()
    {
        try
        {
            Directory.CreateDirectory(_settingsDirectory);
            var settings = new AppSettings
            {
                JDownloaderPath = JDownloaderPathBox.Text?.Trim() ?? string.Empty,
                ResolverReachPath = ResolverReachPathBox.Text?.Trim() ?? string.Empty,
                GgufImporterPath = GgufImporterPathBox.Text?.Trim() ?? string.Empty
            };
            File.WriteAllText(SettingsPath, JsonSerializer.Serialize(settings, new JsonSerializerOptions { WriteIndented = true }));
        }
        catch (Exception ex)
        {
            RecordActivity("Could not save settings: " + ex.Message);
        }
    }

    private void RecordActivity(string message)
    {
        string line = DateTimeOffset.Now.ToString("O") + " " + message;
        lock (_logGate)
            _activityLog.Add(line);
    }

    private void SetStatus(string message)
    {
        StatusText.Text = message;
        RecordActivity(message);
    }

    private void LaunchExecutable(string? path, string displayName)
    {
        SaveSettings();
        if (string.IsNullOrWhiteSpace(path))
        {
            SetStatus("Select the " + displayName + " executable first.");
            return;
        }

        string fullPath;
        try
        {
            fullPath = Path.GetFullPath(path.Trim());
        }
        catch (Exception ex)
        {
            SetStatus("Invalid " + displayName + " path: " + ex.Message);
            return;
        }

        if (!File.Exists(fullPath))
        {
            SetStatus("File not found for " + displayName + ": " + fullPath);
            return;
        }

        try
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = fullPath,
                WorkingDirectory = Path.GetDirectoryName(fullPath) ?? AppContext.BaseDirectory,
                UseShellExecute = true
            });
            SetStatus("Launched " + displayName + ".");
        }
        catch (Exception ex)
        {
            SetStatus("Could not launch " + displayName + ": " + ex.Message);
        }
    }

    private async Task BrowseExecutableAsync(TextBox target, string displayName)
    {
        TopLevel? topLevel = TopLevel.GetTopLevel(this);
        if (topLevel is null)
        {
            SetStatus("The file picker is not available.");
            return;
        }

        IReadOnlyList<IStorageFile> selected = await topLevel.StorageProvider.OpenFilePickerAsync(
            new FilePickerOpenOptions
            {
                Title = "Select " + displayName + " executable",
                AllowMultiple = false,
                FileTypeFilter =
                [
                    new FilePickerFileType("Windows executable")
                    {
                        Patterns = ["*.exe"]
                    }
                ]
            });

        if (selected.Count == 0)
            return;

        target.Text = selected[0].Path.LocalPath;
        SaveSettings();
        SetStatus("Selected " + displayName + " executable.");
    }

    private async void BrowseJDownloaderButton_Click(object? sender, RoutedEventArgs e) =>
        await BrowseExecutableAsync(JDownloaderPathBox, "JDownloader");

    private async void BrowseResolverReachButton_Click(object? sender, RoutedEventArgs e) =>
        await BrowseExecutableAsync(ResolverReachPathBox, "Resolver Reach");

    private async void BrowseGgufImporterButton_Click(object? sender, RoutedEventArgs e) =>
        await BrowseExecutableAsync(GgufImporterPathBox, "Ollama GGUF Importer");

    private void LaunchJDownloaderButton_Click(object? sender, RoutedEventArgs e) =>
        LaunchExecutable(JDownloaderPathBox.Text, "JDownloader");

    private void LaunchResolverReachButton_Click(object? sender, RoutedEventArgs e) =>
        LaunchExecutable(ResolverReachPathBox.Text, "Resolver Reach");

    private void LaunchGgufImporterButton_Click(object? sender, RoutedEventArgs e) =>
        LaunchExecutable(GgufImporterPathBox.Text, "Ollama GGUF Importer");

    private void ClearLinksButton_Click(object? sender, RoutedEventArgs e)
    {
        LinkInputBox.Text = string.Empty;
        SetStatus("Link input cleared.");
    }

    private async void CheckApiButton_Click(object? sender, RoutedEventArgs e)
    {
        try
        {
            string state = await _jDownloaderApi.GetStateAsync();
            SetStatus("JDownloader local API responded. Engine state: " + state);
        }
        catch (Exception ex)
        {
            SetStatus("Could not connect to JDownloader at 127.0.0.1:3128. Start JDownloader and enable its local API if available. Details: " + ex.Message);
        }
    }

    private async void AddLinksButton_Click(object? sender, RoutedEventArgs e)
    {
        string links = LinkInputBox.Text ?? string.Empty;
        try
        {
            bool accepted = await _jDownloaderApi.AddLinksToLinkGrabberAsync(links);
            SetStatus(accepted
                ? "JDownloader accepted the link list for LinkGrabber."
                : "JDownloader did not accept the link list. Inspect LinkGrabber and the API response.");
        }
        catch (Exception ex)
        {
            SetStatus("Could not send links to JDownloader. Confirm the local API is enabled. Details: " + ex.Message);
        }
    }

    private async void RefreshQueueButton_Click(object? sender, RoutedEventArgs e)
    {
        try
        {
            IReadOnlyList<string> packages = await _jDownloaderApi.GetDownloadPackagesAsync();
            QueueSummaryBox.Text = packages.Count == 0
                ? "JDownloader responded, but the API returned no download packages."
                : string.Join(Environment.NewLine + Environment.NewLine, packages);
            SetStatus("Updated queue snapshot: " + packages.Count + " package(s).");
        }
        catch (Exception ex)
        {
            QueueSummaryBox.Text = "Queue is not available yet. Start JDownloader and enable its local API if available." +
                Environment.NewLine + ex.Message;
            SetStatus("Could not refresh the queue. Details: " + ex.Message);
        }
    }

    private async void StartDownloadsButton_Click(object? sender, RoutedEventArgs e)
    {
        try
        {
            bool started = await _jDownloaderApi.StartDownloadsAsync();
            SetStatus(started
                ? "JDownloader accepted the start-downloads request."
                : "JDownloader did not confirm the start request.");
        }
        catch (Exception ex)
        {
            SetStatus("Could not start downloads. Confirm JDownloader is running and its local API is enabled. Details: " + ex.Message);
        }
    }

    private async void SaveLogsButton_Click(object? sender, RoutedEventArgs e)
    {
        TopLevel? topLevel = TopLevel.GetTopLevel(this);
        if (topLevel is null)
        {
            SetStatus("The ZIP save dialog is not available.");
            return;
        }

        try
        {
            IStorageFile? destination = await topLevel.StorageProvider.SaveFilePickerAsync(
                new FilePickerSaveOptions
                {
                    Title = "Save JDownloader Reach session logs",
                    SuggestedFileName = "JDownloaderReach-Logs-" + DateTime.Now.ToString("yyyyMMdd-HHmmss") + ".zip",
                    DefaultExtension = "zip",
                    FileTypeChoices =
                    [
                        new FilePickerFileType("ZIP archive")
                        {
                            Patterns = ["*.zip"],
                            MimeTypes = ["application/zip"]
                        }
                    ]
                });

            if (destination is null)
            {
                SetStatus("Save Logs cancelled.");
                return;
            }

            SaveSettings();
            string[] logSnapshot;
            lock (_logGate)
                logSnapshot = _activityLog.ToArray();

            await using Stream output = await destination.OpenWriteAsync();
            using (ZipArchive archive = new(output, ZipArchiveMode.Create))
            {
                WriteTextEntry(archive, "activity.log", string.Join(Environment.NewLine, logSnapshot));
                WriteTextEntry(archive, "settings.json", File.Exists(SettingsPath) ? File.ReadAllText(SettingsPath) : "{}");
                WriteTextEntry(archive, "session.json", JsonSerializer.Serialize(new
                {
                    Product = "JDownloader Reach",
                    Version = "0.1.0",
                    StartedAt = logSnapshot.FirstOrDefault(),
                    SavedAt = DateTimeOffset.Now,
                    OperatingSystem = Environment.OSVersion.ToString(),
                    Runtime = System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription,
                    ApiBaseAddress = "http://127.0.0.1:3128/"
                }, new JsonSerializerOptions { WriteIndented = true }));
            }

            SetStatus("Saved session logs to " + destination.Name + ".");
        }
        catch (Exception ex)
        {
            SetStatus("Save Logs failed: " + ex.Message);
        }
    }

    private static void WriteTextEntry(ZipArchive archive, string name, string content)
    {
        ZipArchiveEntry entry = archive.CreateEntry(name, CompressionLevel.Optimal);
        using Stream entryStream = entry.Open();
        using var writer = new StreamWriter(entryStream, new UTF8Encoding(false));
        writer.Write(content);
    }

    protected override void OnClosed(EventArgs e)
    {
        SaveSettings();
        _jDownloaderApi.Dispose();
        base.OnClosed(e);
    }

    private sealed class AppSettings
    {
        public string JDownloaderPath { get; set; } = string.Empty;
        public string ResolverReachPath { get; set; } = string.Empty;
        public string GgufImporterPath { get; set; } = string.Empty;
    }
}
