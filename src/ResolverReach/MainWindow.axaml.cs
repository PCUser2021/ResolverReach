using Avalonia;
using Avalonia.Controls;
using FluentAvalonia.UI.Windowing;

namespace ResolverReach;

public partial class MainWindow : FAAppWindow
{
    public MainWindow()
    {
        InitializeComponent();
        TitleBar.ExtendsContentIntoTitleBar = true;
        TransparencyLevelHint = [WindowTransparencyLevel.Mica, WindowTransparencyLevel.AcrylicBlur];
        WindowState = Avalonia.Controls.WindowState.Maximized;
        MainContent.Content = new MainView();
    }
}
