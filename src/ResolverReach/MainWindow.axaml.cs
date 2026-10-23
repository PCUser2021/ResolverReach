using Avalonia;
using Avalonia.Controls;
using FluentAvalonia.UI.Windowing;

namespace ResolverReach;

public partial class MainWindow : FAAppWindow
{
    public MainWindow()
    {
        InitializeComponent();

        // FluentAvalonia's extended title bar and Mica/Acrylic material are Windows-specific.
        // Keep the same single-window app and let macOS use its native, opaque window chrome.
        if (OperatingSystem.IsWindows())
        {
            TitleBar.ExtendsContentIntoTitleBar = true;
            TransparencyLevelHint = [WindowTransparencyLevel.Mica, WindowTransparencyLevel.AcrylicBlur];
        }
        else
        {
            TransparencyLevelHint = [WindowTransparencyLevel.None];
        }

        WindowState = WindowState.Maximized;
        MainContent.Content = new MainView();
    }
}
