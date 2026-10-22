using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Input.Platform;
using Avalonia.Platform.Storage;
using Avalonia.Styling;
using ReactiveUI;
using ReactiveUI.Avalonia;
using System.Reactive.Disposables;
using System.Text;

namespace ResolverReach;

public partial class MainView : ReactiveUserControl<ResolverReachViewModel>
{
    public MainView()
    {
        InitializeComponent();
        ViewModel = new ResolverReachViewModel();
        ThemeSelector.SelectedIndex = 0;

        // Platform-backed clipboard and save-file dialogs are registered only while
        // this view is active. Application state remains in the ReactiveUI ViewModel.
        this.WhenActivated(disposables =>
        {
            disposables.Add(ViewModel.ClipboardTextInteraction.RegisterHandler(async interaction =>
            {
                var clipboard = TopLevel.GetTopLevel(this)?.Clipboard;
                string? text = clipboard is null ? null : await clipboard.TryGetTextAsync();
                interaction.SetOutput(text);
            }));

            disposables.Add(ViewModel.SaveLogInteraction.RegisterHandler(async interaction =>
            {
                var storageProvider = TopLevel.GetTopLevel(this)?.StorageProvider;
                if (storageProvider is null || !storageProvider.CanSave)
                {
                    interaction.SetOutput(LogSaveResult.Failed("The platform does not provide a save-file dialog."));
                    return;
                }

                try
                {
                    var file = await storageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
                    {
                        Title = "Save Resolver Reach Logs",
                        SuggestedFileName = $"ResolverReach-Logs-{DateTime.Now:yyyyMMdd-HHmmss}.txt",
                        DefaultExtension = "txt",
                        ShowOverwritePrompt = true,
                        FileTypeChoices =
                        [
                            new FilePickerFileType("Text files (*.txt)") { Patterns = ["*.txt"] },
                            new FilePickerFileType("All files") { Patterns = ["*.*"] }
                        ]
                    });

                    if (file is null)
                    {
                        interaction.SetOutput(LogSaveResult.UserCancelled());
                        return;
                    }

                    await using var stream = await file.OpenWriteAsync();
                    await using var writer = new StreamWriter(stream, new UTF8Encoding(false), 1024, leaveOpen: true);
                    await writer.WriteAsync(interaction.Input);
                    await writer.FlushAsync();
                    interaction.SetOutput(LogSaveResult.Saved(file.Name));
                }
                catch (Exception ex)
                {
                    interaction.SetOutput(LogSaveResult.Failed(ex.Message));
                }
            }));
        });
    }

    private void ThemeSelector_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (ThemeSelector.SelectedItem is not ComboBoxItem item || Application.Current is null)
            return;

        string theme = item.Tag?.ToString() ?? "Default";
        Application.Current.RequestedThemeVariant = theme switch
        {
            "Light" => ThemeVariant.Light,
            "Dark" => ThemeVariant.Dark,
            _ => ThemeVariant.Default
        };
    }

    private void MainView_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.Key != Key.Escape || ViewModel?.IsBusy != true)
            return;

        ViewModel.CancelCommand.Execute().Subscribe();
        e.Handled = true;
    }
}