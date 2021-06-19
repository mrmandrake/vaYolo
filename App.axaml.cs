using System;
using System.Reflection;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using vaYolo.ViewModels;
using vaYolo.Views;

namespace vaYolo
{
    public class App : Application
    {
        public static Version Version => Assembly.GetExecutingAssembly().GetName().Version;
        public static string VersionStr => Version.ToString(3);


        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
            this.Name = "vaYolo";
        }
        
        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
                desktop.MainWindow = new MainWindow {
                    DataContext = new MainWindowViewModel(),
                };
                
            base.OnFrameworkInitializationCompleted();
        }
    }
}
