using Avalonia;
using FluentAvalonia.UI.Windowing;

namespace ResolverReach;

public partial class MainWindow : AppWindow
{
    public MainWindow()
    {
        InitializeComponent();
        TitleBar.ExtendsContentIntoTitleBar = true;
        TitleBar.TitleBarHitTestType = TitleBarHitTestType.Complex;
        TransparencyLevelHint = [WindowTransparencyLevel.Mica, WindowTransparencyLevel.AcrylicBlur];
        WindowState = Avalonia.Controls.WindowState.Maximized;
        MainContent.Content = new MainView();
    }
}