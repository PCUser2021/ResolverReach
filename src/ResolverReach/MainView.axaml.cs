using Avalonia.Input;
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

    private void MainView_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.Key != Key.Escape || ViewModel?.IsBusy != true)
            return;

        ViewModel.CancelCommand.Execute().Subscribe();
        e.Handled = true;
    }
}
