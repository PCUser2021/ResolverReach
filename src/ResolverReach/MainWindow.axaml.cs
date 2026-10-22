using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using FluentAvalonia.UI.Windowing;

namespace ResolverReach;

public partial class MainWindow : AppWindow
{
    private TesterWindow? _tester;

    public MainWindow()
    {
        InitializeComponent();
        TitleBar.ExtendsContentIntoTitleBar = true;
        TitleBar.TitleBarHitTestType = TitleBarHitTestType.Complex;
        TransparencyLevelHint = [WindowTransparencyLevel.Mica, WindowTransparencyLevel.AcrylicBlur];
        SetStatusRibbon(
            "Resolver diagnostics ready",
            "No test run yet · HTTPS DNS exchange validation",
            "#0E355D");
    }

    private void SetStatusRibbon(string title, string detail, string fill)
    {
        StatusRibbonTitle.Text = title;
        StatusRibbonText.Text = detail;
        StatusRibbon.Background = new SolidColorBrush(Color.Parse(fill));
    }

    private async void OpenTester_Click(object? sender, RoutedEventArgs e)
    {
        if (_tester is not null)
        {
            _tester.Activate();
            return;
        }

        var tester = new TesterWindow(SetStatusRibbon);
        _tester = tester;
        tester.Closed += (_, _) => _tester = null;
        await tester.ShowDialog(this);
    }
}
