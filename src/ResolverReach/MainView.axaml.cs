using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using ReactiveUI;
using ReactiveUI.Avalonia;

namespace ResolverReach;

public partial class MainView : ReactiveUserControl<ResolverReachViewModel>
{
    public MainView()
    {
        InitializeComponent();
        ViewModel = new ResolverReachViewModel();
    }

    private async void SaveLogsButton_Click(object? sender, RoutedEventArgs e)
    {
        if (ViewModel is null)
            return;

        TopLevel? topLevel = TopLevel.GetTopLevel(this);
        if (topLevel is null)
        {
            ViewModel.SetStatusMessage("Could not open the ZIP save dialog.");
            return;
        }

        ViewModel.RecordUiActivity("Save Logs selected; waiting for a ZIP destination.");
        try
        {
            IStorageFile? destination = await topLevel.StorageProvider.SaveFilePickerAsync(
                new FilePickerSaveOptions
                {
                    Title = "Save Resolver Reach diagnostic logs",
                    SuggestedFileName = $"ResolverReach-Logs-{DateTime.Now:yyyyMMdd-HHmmss}.zip",
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
                ViewModel.SetStatusMessage("Save Logs cancelled.");
                return;
            }

            await using (Stream output = await destination.OpenWriteAsync())
            {
                await ViewModel.WriteLogsZipAsync(output);
            }

            ViewModel.RecordUiActivity($"ZIP diagnostic archive saved as {destination.Name}.");
            ViewModel.SetStatusMessage($"Saved full diagnostic logs to {destination.Name}");
        }
        catch (Exception ex)
        {
            ViewModel.RecordUiActivity($"Save Logs failed: {ex}");
            ViewModel.SetStatusMessage($"Save Logs failed: {ex.Message}");
        }
    }

    private void MainView_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.Key != Key.Escape || ViewModel?.IsBusy != true)
            return;

        ViewModel.CancelCommand.Execute().Subscribe();
        e.Handled = true;
    }
}
