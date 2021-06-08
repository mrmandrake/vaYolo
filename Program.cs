using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.ReactiveUI;
using System;

namespace vaYolo
{
    class Program
    {
        public static void Main(string[] args) => BuildAvaloniaApp()
                    .StartWithClassicDesktopLifetime(args);
        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .LogToTrace()
                .UseReactiveUI();
    }
}
