using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;
using System.Reflection;
using System.Windows.Forms;

namespace OllamaGgufImporter;

internal static class Program
{
    [STAThread]
    private static void Main()
    {
        ApplicationConfiguration.Initialize();
        Application.Run(new ImporterForm());
    }
}

internal sealed class ImporterForm : Form
{
    private static readonly Color Surface = Color.FromArgb(27, 31, 40);
    private static readonly Color SurfaceRaised = Color.FromArgb(36, 42, 53);
    private static readonly Color TextPrimary = Color.FromArgb(239, 243, 250);
    private static readonly Color TextMuted = Color.FromArgb(164, 176, 194);
    private static readonly Color Accent = Color.FromArgb(20, 115, 230);
    private static readonly Color AccentHover = Color.FromArgb(49, 168, 255);

    private readonly ListView _files = new();
    private readonly TextBox _modelStore = new();
    private readonly TextBox _log = new();
    private readonly Label _ollamaStatus = new();
    private readonly Label _status = new();
    private readonly ProgressBar _progress = new();
    private readonly Button _locateButton;
    private readonly Button _removeButton;
    private readonly Button _clearButton;
    private readonly Button _importButton;
    private readonly Button _cancelButton;
    private readonly Button _saveLogsButton;
    private readonly string _modelStorePath;
    private string? _ollamaExecutable;
    private CancellationTokenSource? _importCancellation;

    public ImporterForm()
    {
        Text = "Ollama GGUF Importer";
        Icon = System.Drawing.Icon.ExtractAssociatedIcon(System.Windows.Forms.Application.ExecutablePath)
            ?? System.Drawing.SystemIcons.Application;
        StartPosition = FormStartPosition.CenterScreen;
        MinimumSize = new Size(780, 620);
        Size = new Size(900, 730);
        BackColor = Color.FromArgb(17, 20, 27);
        ForeColor = TextPrimary;
        Font = new Font("Segoe UI", 9.5F);
        AutoScaleMode = AutoScaleMode.Dpi;

        string appVersion = GetProductVersion();
        string profile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        if (string.IsNullOrWhiteSpace(profile))
            profile = Environment.GetEnvironmentVariable("USERPROFILE") ?? AppContext.BaseDirectory;
        _modelStorePath = Path.Combine(profile, ".ollama", "models");
        _ollamaExecutable = FindOllamaExecutable();

        var page = new TableLayoutPanel
        {
            Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 6,
            Padding = new Padding(18, 16, 18, 16), BackColor = BackColor
        };
        page.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        page.RowStyles.Add(new RowStyle(SizeType.Absolute, 76));
        page.RowStyles.Add(new RowStyle(SizeType.Absolute, 48));
        page.RowStyles.Add(new RowStyle(SizeType.Percent, 46));
        page.RowStyles.Add(new RowStyle(SizeType.Absolute, 78));
        page.RowStyles.Add(new RowStyle(SizeType.Percent, 30));
        page.RowStyles.Add(new RowStyle(SizeType.Absolute, 98));
        Controls.Add(page);

        var header = new Panel { Dock = DockStyle.Fill, BackColor = Color.FromArgb(20, 31, 49), Padding = new Padding(16, 10, 16, 8) };
        header.Controls.Add(new Label
        {
            Text = "Ollama GGUF Importer", Font = new Font("Segoe UI Semibold", 18F, FontStyle.Bold),
            ForeColor = Color.White, AutoSize = true, Location = new Point(0, 0)
        });
        header.Controls.Add(new Label
        {
            Text = $"PORTABLE WINDOWS X64  •  v{appVersion}  •  SELECT A MODEL  •  IMPORT INTO OLLAMA",
            ForeColor = Color.FromArgb(190, 214, 245), Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
            AutoSize = true, Location = new Point(2, 38)
        });
        page.Controls.Add(header, 0, 0);

        var commandBar = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill, WrapContents = false, FlowDirection = FlowDirection.LeftToRight,
            BackColor = BackColor, Padding = new Padding(0, 6, 0, 0)
        };
        _locateButton = MakeButton("Locate .GGUF files", Accent, Color.White, 170);
        _removeButton = MakeButton("Remove selected", SurfaceRaised, TextPrimary, 145);
        _clearButton = MakeButton("Clear list", SurfaceRaised, TextPrimary, 105);
        _locateButton.Click += (_, _) => LocateFiles();
        _removeButton.Click += (_, _) => RemoveSelected();
        _clearButton.Click += (_, _) => ClearFiles();
        commandBar.Controls.AddRange([_locateButton, _removeButton, _clearButton]);
        page.Controls.Add(commandBar, 0, 1);

        _files.View = View.Details;
        _files.FullRowSelect = true;
        _files.MultiSelect = true;
        _files.HideSelection = false;
        _files.BorderStyle = BorderStyle.FixedSingle;
        _files.BackColor = Surface;
        _files.ForeColor = TextPrimary;
        _files.Font = new Font("Segoe UI", 9F);
        _files.Columns.Add("GGUF file", 350);
        _files.Columns.Add("Suggested Ollama name", 210);
        _files.Columns.Add("Import status", 130);
        _files.Dock = DockStyle.Fill;
        _files.SelectedIndexChanged += (_, _) => RefreshButtons();
        page.Controls.Add(_files, 0, 2);

        var pathPanel = new TableLayoutPanel
        {
            Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 3,
            BackColor = BackColor, Padding = new Padding(0, 8, 0, 0)
        };
        pathPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 22));
        pathPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 28));
        pathPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        pathPanel.Controls.Add(new Label
        {
            Text = "Default Ollama model store (detected for this Windows user)",
            ForeColor = TextPrimary, Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold),
            AutoSize = true, Dock = DockStyle.Fill
        }, 0, 0);
        _modelStore.ReadOnly = true;
        _modelStore.Text = _modelStorePath;
        _modelStore.Dock = DockStyle.Fill;
        _modelStore.BackColor = Surface;
        _modelStore.ForeColor = Color.FromArgb(199, 220, 248);
        _modelStore.BorderStyle = BorderStyle.FixedSingle;
        _modelStore.Font = new Font("Consolas", 9F);
        pathPanel.Controls.Add(_modelStore, 0, 1);
        pathPanel.Controls.Add(new Label
        {
            Text = "Ollama manages this folder internally. Import uses Modelfile + ollama create; it does not copy GGUF files into the store by hand.",
            ForeColor = TextMuted, AutoSize = false, Dock = DockStyle.Fill,
            TextAlign = ContentAlignment.MiddleLeft
        }, 0, 2);
        page.Controls.Add(pathPanel, 0, 3);

        _log.ReadOnly = true;
        _log.Multiline = true;
        _log.ScrollBars = ScrollBars.Vertical;
        _log.WordWrap = true;
        _log.Dock = DockStyle.Fill;
        _log.BackColor = Surface;
        _log.ForeColor = Color.FromArgb(208, 219, 234);
        _log.BorderStyle = BorderStyle.FixedSingle;
        _log.Font = new Font("Consolas", 8.5F);
        _log.Text = "Ready. Select one or more .gguf files, then click Import.\r\n";
        page.Controls.Add(_log, 0, 4);

        var footer = new TableLayoutPanel
        {
            Dock = DockStyle.Fill, ColumnCount = 2, RowCount = 3,
            BackColor = BackColor, Padding = new Padding(0, 8, 0, 0)
        };
        footer.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 62));
        footer.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 38));
        footer.RowStyles.Add(new RowStyle(SizeType.Absolute, 26));
        footer.RowStyles.Add(new RowStyle(SizeType.Absolute, 24));
        footer.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

        _ollamaStatus.Text = _ollamaExecutable is null
            ? "Ollama CLI not found — install Ollama, then reopen this tool."
            : $"Ollama CLI detected: {_ollamaExecutable}";
        _ollamaStatus.ForeColor = _ollamaExecutable is null ? Color.FromArgb(255, 190, 120) : TextMuted;
        _ollamaStatus.AutoEllipsis = true;
        _ollamaStatus.Dock = DockStyle.Fill;
        _ollamaStatus.TextAlign = ContentAlignment.MiddleLeft;
        footer.Controls.Add(_ollamaStatus, 0, 0);
        footer.SetColumnSpan(_ollamaStatus, 2);

        _status.Text = "Idle";
        _status.Dock = DockStyle.Fill;
        _status.ForeColor = TextMuted;
        _status.TextAlign = ContentAlignment.MiddleLeft;
        footer.Controls.Add(_status, 0, 1);
        _progress.Dock = DockStyle.Fill;
        _progress.Minimum = 0;
        _progress.Maximum = 100;
        _progress.Value = 0;
        footer.Controls.Add(_progress, 1, 1);

        var actionBar = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill, AutoSize = false, WrapContents = false,
            FlowDirection = FlowDirection.RightToLeft, BackColor = BackColor,
            Padding = new Padding(0, 4, 0, 0), Margin = new Padding(0)
        };
        _importButton = MakeButton("Import", Accent, Color.White, 122);
        _importButton.Margin = new Padding(0);
        _importButton.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
        _importButton.Click += async (_, _) => await ImportSelectedAsync();
        _cancelButton = MakeButton("Cancel", SurfaceRaised, TextPrimary, 100);
        _cancelButton.Margin = new Padding(0, 0, 10, 0);
        _cancelButton.Enabled = false;
        _cancelButton.Click += (_, _) => _importCancellation?.Cancel();
        _saveLogsButton = MakeButton("Save Logs", SurfaceRaised, TextPrimary, 116);
        _saveLogsButton.Margin = new Padding(0, 0, 10, 0);
        _saveLogsButton.Click += async (_, _) => await SaveLogsAsync();
        actionBar.Controls.Add(_importButton);
        actionBar.Controls.Add(_cancelButton);
        actionBar.Controls.Add(_saveLogsButton);
        footer.Controls.Add(actionBar, 0, 2);
        footer.SetColumnSpan(actionBar, 2);
        page.Controls.Add(footer, 0, 5);

        RefreshButtons();
        if (_ollamaExecutable is null)
            Log("Ollama CLI was not found in PATH or its usual Windows installation directories. Install/run Ollama before importing.");
        else
            Log("Ollama CLI detected. Imports are registered using the local Ollama service.");
    }

    private async Task SaveLogsAsync()
    {
        using var dialog = new SaveFileDialog
        {
            Title = "Save Ollama GGUF Importer Logs",
            Filter = "Text log files (*.txt)|*.txt|All files (*.*)|*.*",
            DefaultExt = "txt",
            AddExtension = true,
            OverwritePrompt = true,
            FileName = $"Ollama-GGUF-Importer-Logs-{DateTime.Now:yyyyMMdd-HHmmss}.txt"
        };

        if (dialog.ShowDialog(this) != DialogResult.OK)
            return;

        var export = new StringBuilder();
        export.AppendLine("Ollama GGUF Importer — Session Log");
        export.AppendLine($"Version: {GetProductVersion()}");
        export.AppendLine($"Exported at (local): {DateTimeOffset.Now:O}");
        export.AppendLine($"Status: {_status.Text}");
        export.AppendLine($"Ollama CLI: {_ollamaStatus.Text}");
        export.AppendLine();
        export.AppendLine("Activity log");
        export.Append(_log.Text);
        export.AppendLine();
        export.AppendLine("Current import queue");
        export.AppendLine("GGUF file\tSuggested Ollama name\tImport status");
        foreach (ListViewItem row in _files.Items)
        {
            string name = row.SubItems.Count > 0 ? row.SubItems[0].Text : "";
            string model = row.SubItems.Count > 1 ? row.SubItems[1].Text : "";
            string state = row.SubItems.Count > 2 ? row.SubItems[2].Text : "";
            export.Append(name.Replace('\t', ' ').Replace('\r', ' ').Replace('\n', ' ')).Append('\t')
                .Append(model.Replace('\t', ' ').Replace('\r', ' ').Replace('\n', ' ')).Append('\t')
                .AppendLine(state.Replace('\t', ' ').Replace('\r', ' ').Replace('\n', ' '));
        }

        try
        {
            await File.WriteAllTextAsync(dialog.FileName, export.ToString(), new UTF8Encoding(false));
            _status.Text = $"Logs saved: {Path.GetFileName(dialog.FileName)}";
            Log($"Saved logs to {dialog.FileName}");
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException
            or ArgumentException or NotSupportedException or System.Security.SecurityException)
        {
            _status.Text = "Could not save logs.";
            Log("ERROR: Unable to save logs: " + ex.Message);
            MessageBox.Show(this, $"Could not save logs.\r\n\r\n{ex.Message}",
                "Save Logs", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private static string GetProductVersion()
    {
        string? version = typeof(ImporterForm).Assembly
            .GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion;
        return string.IsNullOrWhiteSpace(version) ? "1.0.0" : version.Split('+', 2)[0];
    }

    private static Button MakeButton(string text, Color fill, Color foreground, int width)
    {
        var button = new Button
        {
            Text = text, Width = width, Height = 34, Margin = new Padding(0, 0, 9, 0),
            FlatStyle = FlatStyle.Flat, BackColor = fill, ForeColor = foreground,
            Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold), Cursor = Cursors.Hand,
            UseVisualStyleBackColor = false
        };
        button.FlatAppearance.BorderColor = fill == Accent ? AccentHover : Color.FromArgb(67, 78, 96);
        button.FlatAppearance.BorderSize = 1;
        button.FlatAppearance.MouseOverBackColor = fill == Accent ? AccentHover : Color.FromArgb(47, 56, 70);
        return button;
    }

    private void LocateFiles()
    {
        using var picker = new OpenFileDialog
        {
            Title = "Locate GGUF model files",
            Filter = "GGUF model files (*.gguf)|*.gguf|All files (*.*)|*.*",
            Multiselect = true, CheckFileExists = true, CheckPathExists = true, RestoreDirectory = true
        };
        if (picker.ShowDialog(this) != DialogResult.OK) return;

        int added = 0;
        foreach (string path in picker.FileNames)
        {
            string fullPath = Path.GetFullPath(path);
            if (_files.Items.Cast<ListViewItem>().Any(x =>
                string.Equals(x.Tag as string, fullPath, StringComparison.OrdinalIgnoreCase)))
                continue;

            var row = new ListViewItem(Path.GetFileName(fullPath)) { Tag = fullPath };
            row.SubItems.Add(SuggestedModelName(fullPath));
            row.SubItems.Add("Ready");
            row.ToolTipText = fullPath;
            _files.Items.Add(row);
            added++;
        }

        Log($"Selected {added} new GGUF file(s).");
        RefreshButtons();
    }

    private void RemoveSelected()
    {
        foreach (ListViewItem item in _files.SelectedItems.Cast<ListViewItem>().ToArray())
            _files.Items.Remove(item);
        RefreshButtons();
    }

    private void ClearFiles()
    {
        _files.Items.Clear();
        _progress.Value = 0;
        _status.Text = "Idle";
        RefreshButtons();
    }

    private void RefreshButtons()
    {
        bool busy = _importCancellation is not null;
        _locateButton.Enabled = !busy;
        _removeButton.Enabled = !busy && _files.SelectedItems.Count > 0;
        _clearButton.Enabled = !busy && _files.Items.Count > 0;
        _importButton.Enabled = !busy && _ollamaExecutable is not null && _files.Items.Count > 0;
    }

    private async Task ImportSelectedAsync()
    {
        if (_ollamaExecutable is null || _importCancellation is not null || _files.Items.Count == 0)
            return;

        var rows = _files.Items.Cast<ListViewItem>().ToArray();
        List<ImportJob> jobs = BuildJobs(rows);
        if (jobs.Count == 0)
        {
            MessageBox.Show(this, "No importable GGUF files were selected.", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        using var cancellation = new CancellationTokenSource();
        _importCancellation = cancellation;
        _cancelButton.Enabled = true;
        RefreshButtons();
        _progress.Value = 0;
        int succeeded = 0, failed = 0;
        try
        {
            for (int i = 0; i < jobs.Count; i++)
            {
                cancellation.Token.ThrowIfCancellationRequested();
                ImportJob job = jobs[i];
                UpdateRowStates(job, "Importing…");
                _status.Text = $"Importing {i + 1} of {jobs.Count}: {job.ModelName}";
                Log($"[{i + 1}/{jobs.Count}] Importing '{job.ModelName}' from {job.SourceDisplay}");

                ImportResult result = await RunImportAsync(job, cancellation.Token);
                UpdateRowStates(job, result.Success ? "Imported" : (result.ExitCode is null ? "Cancelled" : "Failed"));
                if (result.Success)
                {
                    succeeded++;
                    Log($"SUCCESS: ollama create completed for '{job.ModelName}'.");
                    if (!string.IsNullOrWhiteSpace(result.Output)) Log(result.Output.Trim());
                }
                else if (result.ExitCode is null && cancellation.IsCancellationRequested)
                {
                    Log($"CANCELLED: {job.ModelName}.");
                }
                else
                {
                    failed++;
                    Log($"FAILED: {job.ModelName} (exit code {result.ExitCode?.ToString() ?? "unknown"}).");
                    if (!string.IsNullOrWhiteSpace(result.Error)) Log(result.Error.Trim());
                    else if (!string.IsNullOrWhiteSpace(result.Output)) Log(result.Output.Trim());
                }

                _progress.Value = Math.Min(100, (i + 1) * 100 / jobs.Count);
            }

            _status.Text = $"Finished — {succeeded} imported, {failed} failed.";
            Log($"Finished. Imported: {succeeded}; failed: {failed}.");
        }
        catch (OperationCanceledException)
        {
            _status.Text = $"Cancelled — {succeeded} imported, {failed} failed before cancellation.";
            Log("Cancellation requested. No new imports will be started.");
        }
        catch (Exception ex)
        {
            _status.Text = "Import run failed.";
            Log("ERROR: " + ex.Message);
        }
        finally
        {
            _importCancellation = null;
            _cancelButton.Enabled = false;
            RefreshButtons();
        }
    }

    private List<ImportJob> BuildJobs(IEnumerable<ListViewItem> rows)
    {
        var seenSources = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        var usedNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        var jobs = new List<ImportJob>();

        foreach (ListViewItem row in rows)
        {
            string path = row.Tag as string ?? string.Empty;
            if (!File.Exists(path))
            {
                row.SubItems[2].Text = "Missing file";
                continue;
            }

            (string source, bool splitSet) = GetOllamaSource(path);
            if (!seenSources.Add(source)) continue;

            string baseName = SuggestedModelName(path);
            if (splitSet)
                baseName = SuggestedModelName(Regex.Replace(Path.GetFileName(path), @"-\d{5}-of-\d{5}(?=\.gguf$)", "", RegexOptions.IgnoreCase));
            string modelName = baseName;
            int suffix = 2;
            while (!usedNames.Add(modelName)) modelName = $"{baseName}-{suffix++}";

            ListViewItem[] relatedRows = rows.Where(r =>
            {
                string rowPath = r.Tag as string ?? string.Empty;
                return rowPath.Length > 0 && File.Exists(rowPath) &&
                    string.Equals(GetOllamaSource(rowPath).Source, source, StringComparison.OrdinalIgnoreCase);
            }).ToArray();
            jobs.Add(new ImportJob(source, Path.GetFileName(path), modelName, relatedRows));
        }
        return jobs;
    }

    private async Task<ImportResult> RunImportAsync(ImportJob job, CancellationToken cancellationToken)
    {
        if (_ollamaExecutable is null)
            return new ImportResult(false, null, "", "The Ollama command-line tool is not installed.");

        string sourceDirectory = Path.GetDirectoryName(job.Source) ?? AppContext.BaseDirectory;
        string sourceLeaf = Path.GetFileName(job.Source);
        string tempDirectory = Path.Combine(Path.GetTempPath(), "OllamaGgufImporter", Guid.NewGuid().ToString("N"));
        string? modelfile = null;
        bool localModelfile = false;
        Directory.CreateDirectory(tempDirectory);

        try
        {
            // Ollama resolves relative FROM paths against the Modelfile directory.
            // A sibling Modelfile avoids fragile absolute Windows paths and path parsing bugs.
            string localCandidate = Path.Combine(sourceDirectory, $".ollama-import-{Guid.NewGuid():N}.modelfile");
            string localDirective = $"FROM \"./{sourceLeaf.Replace("\\", "/", StringComparison.Ordinal)}\"{Environment.NewLine}";
            try
            {
                await File.WriteAllTextAsync(localCandidate, localDirective, new UTF8Encoding(false), cancellationToken);
                modelfile = localCandidate;
                localModelfile = true;
            }
            catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
            {
                try { File.Delete(localCandidate); } catch { }
                modelfile = Path.Combine(tempDirectory, "Modelfile");
                string absoluteSource = job.Source.Replace("\\", "/", StringComparison.Ordinal);
                await File.WriteAllTextAsync(modelfile, $"FROM \"{absoluteSource}\"{Environment.NewLine}", new UTF8Encoding(false), cancellationToken);
            }

            Log(localModelfile ? "Using a sibling Modelfile and relative GGUF path." :
                "Source folder is read-only; using a temporary Modelfile with an absolute GGUF path.");

            var startInfo = new ProcessStartInfo
            {
                FileName = _ollamaExecutable,
                WorkingDirectory = Path.GetDirectoryName(modelfile) ?? sourceDirectory,
                UseShellExecute = false, RedirectStandardOutput = true, RedirectStandardError = true,
                CreateNoWindow = true, StandardOutputEncoding = Encoding.UTF8, StandardErrorEncoding = Encoding.UTF8
            };
            startInfo.ArgumentList.Add("create");
            startInfo.ArgumentList.Add(job.ModelName);
            startInfo.ArgumentList.Add("-f");
            startInfo.ArgumentList.Add(modelfile);

            using var process = new Process { StartInfo = startInfo };
            if (!process.Start())
                return new ImportResult(false, null, "", "Windows could not start Ollama. Confirm that it is installed and running.");

            using CancellationTokenRegistration registration = cancellationToken.Register(() =>
            {
                try { if (!process.HasExited) process.Kill(entireProcessTree: true); }
                catch (InvalidOperationException) { }
                catch (System.ComponentModel.Win32Exception) { }
            });

            Task<string> outputTask = process.StandardOutput.ReadToEndAsync();
            Task<string> errorTask = process.StandardError.ReadToEndAsync();
            try { await process.WaitForExitAsync(cancellationToken); }
            catch (OperationCanceledException)
            {
                try { if (!process.HasExited) process.Kill(entireProcessTree: true); }
                catch (InvalidOperationException) { }
                await process.WaitForExitAsync();
                _ = await outputTask;
                _ = await errorTask;
                return new ImportResult(false, null, "", "Cancelled by user.");
            }

            string output = await outputTask;
            string error = await errorTask;
            if (process.ExitCode != 0)
            {
                string combined = (error + Environment.NewLine + output).Trim();
                if (combined.Contains("connect", StringComparison.OrdinalIgnoreCase) ||
                    (combined.Contains("running", StringComparison.OrdinalIgnoreCase) && combined.Contains("ollama", StringComparison.OrdinalIgnoreCase)))
                    error = combined + Environment.NewLine + "Troubleshooting: launch Ollama from the Windows Start menu and wait for startup. Check OLLAMA_HOST if using a custom endpoint.";
            }
            return new ImportResult(process.ExitCode == 0, process.ExitCode, output, error);
        }
        catch (OperationCanceledException) { throw; }
        catch (Exception ex) { return new ImportResult(false, null, "", ex.Message); }
        finally
        {
            if (localModelfile && modelfile is not null)
            {
                try { File.Delete(modelfile); } catch (IOException) { } catch (UnauthorizedAccessException) { }
            }
            try { Directory.Delete(tempDirectory, recursive: true); } catch (IOException) { } catch (UnauthorizedAccessException) { }
        }
    }

    private static (string Source, bool SplitSet) GetOllamaSource(string path)
    {
        string full = Path.GetFullPath(path);
        string name = Path.GetFileName(full);
        Match split = Regex.Match(name, @"^(?<prefix>.+)-\d{5}-of-\d{5}(?<ext>\.gguf)$", RegexOptions.IgnoreCase);
        if (!split.Success) return (full, false);
        string patternName = split.Groups["prefix"].Value + "-*" + split.Groups["ext"].Value;
        return (Path.Combine(Path.GetDirectoryName(full) ?? "", patternName), true);
    }

    private static string SuggestedModelName(string pathOrName)
    {
        string name = Path.GetFileNameWithoutExtension(pathOrName);
        name = Regex.Replace(name, @"-\d{5}-of-\d{5}$", "", RegexOptions.IgnoreCase).ToLowerInvariant();
        name = Regex.Replace(name, @"[^a-z0-9._-]+", "-").Trim('-', '.', '_');
        if (string.IsNullOrWhiteSpace(name)) name = "imported-gguf";
        if (!char.IsLetterOrDigit(name[0])) name = "model-" + name;
        return name.Length > 64 ? name[..64].TrimEnd('-', '.', '_') : name;
    }

    private static string? FindOllamaExecutable()
    {
        string? fromPath = FindOnPath("ollama.exe") ?? FindOnPath("ollama");
        if (fromPath is not null) return fromPath;
        string local = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        string programFiles = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
        string[] candidates =
        [
            Path.Combine(local, "Programs", "Ollama", "ollama.exe"),
            Path.Combine(programFiles, "Ollama", "ollama.exe"),
            Path.Combine(local, "Ollama", "ollama.exe")
        ];
        return candidates.FirstOrDefault(File.Exists);
    }

    private static string? FindOnPath(string executableName)
    {
        string? path = Environment.GetEnvironmentVariable("PATH");
        if (string.IsNullOrWhiteSpace(path)) return null;
        foreach (string part in path.Split(Path.PathSeparator, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
        {
            string candidate = Path.Combine(part.Trim('"'), executableName);
            if (File.Exists(candidate)) return candidate;
        }
        return null;
    }

    private void UpdateRowStates(ImportJob job, string state)
    {
        foreach (ListViewItem row in job.Rows) row.SubItems[2].Text = state;
    }

    private void Log(string message) => _log.AppendText($"[{DateTime.Now:HH:mm:ss}] {message}{Environment.NewLine}");

    private sealed record ImportJob(string Source, string SourceDisplay, string ModelName, ListViewItem[] Rows);
    private sealed record ImportResult(bool Success, int? ExitCode, string Output, string Error);
}
