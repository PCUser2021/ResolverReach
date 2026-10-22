using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Input.Platform;
using Avalonia.Styling;
using ReactiveUI;
using ReactiveUI.Avalonia;
using System.Reactive.Disposables;

namespace ResolverReach;

public partial class MainView : ReactiveUserControl<ResolverReachViewModel>
{
    public MainView()
    {
        InitializeComponent();
        ViewModel = new ResolverReachViewModel();
        ThemeSelector.SelectedIndex = 0;

        // Only the clipboard bridge is imperative; its handler is scoped to the view
        // activation lifetime. All UI state and commands are driven by compiled XAML bindings.
        this.WhenActivated(disposables =>
        {
            disposables.Add(ViewModel.ClipboardTextInteraction.RegisterHandler(async interaction =>
            {
                var clipboard = TopLevel.GetTopLevel(this)?.Clipboard;
                string? text = clipboard is null ? null : await clipboard.TryGetTextAsync();
                interaction.SetOutput(text);
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