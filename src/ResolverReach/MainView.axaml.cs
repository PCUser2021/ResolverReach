using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
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

        this.WhenActivated(disposables =>
        {
            this.Bind(ViewModel, vm => vm.InputLinks, view => view.EndpointBox.Text).DisposeWith(disposables);
            this.OneWayBind(ViewModel, vm => vm.Results, view => view.ResultsList.ItemsSource).DisposeWith(disposables);
            this.OneWayBind(ViewModel, vm => vm.DashboardState.Title, view => view.StatusRibbonTitle.Text).DisposeWith(disposables);
            this.OneWayBind(ViewModel, vm => vm.DashboardState.Detail, view => view.StatusRibbonText.Text).DisposeWith(disposables);
            this.OneWayBind(ViewModel, vm => vm.DashboardState.StatusText, view => view.StatusText.Text).DisposeWith(disposables);
            this.OneWayBind(ViewModel, vm => vm.DashboardState.ProgressValue, view => view.TestProgress.Value).DisposeWith(disposables);
            this.OneWayBind(ViewModel, vm => vm.DashboardState.ProgressLabel, view => view.ProgressText.Text).DisposeWith(disposables);
            this.OneWayBind(ViewModel, vm => vm.DashboardState.PastePanelVisible, view => view.PasteLinksPanel.IsVisible).DisposeWith(disposables);
            this.OneWayBind(ViewModel, vm => vm.HasResults, view => view.ResultsList.IsVisible).DisposeWith(disposables);
            this.OneWayBind(ViewModel, vm => vm.IsEmptyResults, view => view.EmptyState.IsVisible).DisposeWith(disposables);

            this.BindCommand(ViewModel, vm => vm.CheckCatalogCommand, view => view.CheckNowButton).DisposeWith(disposables);
            this.BindCommand(ViewModel, vm => vm.CheckLinksCommand, view => view.CheckPasteButton).DisposeWith(disposables);
            this.BindCommand(ViewModel, vm => vm.PasteLinksCommand, view => view.PasteMenuItem).DisposeWith(disposables);

            ViewModel.ClipboardTextInteraction.RegisterHandler(async interaction =>
            {
                var clipboard = TopLevel.GetTopLevel(this)?.Clipboard;
                string? text = clipboard is null ? null : await clipboard.GetTextAsync();
                interaction.SetOutput(text);
            }).DisposeWith(disposables);
        });
    }

    private void ThemeSelector_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (ThemeSelector.SelectedItem is not ComboBoxItem item || Application.Current is null) return;
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
        if (e.Key != Key.Escape || ViewModel?.IsBusy != true) return;
        ViewModel.CancelCommand.Execute().Subscribe();
        e.Handled = true;
    }
}