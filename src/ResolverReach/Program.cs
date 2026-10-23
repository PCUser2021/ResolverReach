using Avalonia;
using ReactiveUI.Avalonia.Reactive;

namespace ResolverReach;

internal static class Program
{
    [STAThread]
    public static void Main(string[] args) =>
        BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);

    public static AppBuilder BuildAvaloniaApp() =>
        AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .UseReactiveUI(_ => { })
            .RegisterReactiveUIViewsFromEntryAssembly()
            .LogToTrace();
}
