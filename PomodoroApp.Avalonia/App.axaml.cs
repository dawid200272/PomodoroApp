using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using PomodoroApp.Avalonia.Services;
using PomodoroApp.Avalonia.ViewModels;
using PomodoroApp.Avalonia.Views;

namespace PomodoroApp.Avalonia;
public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            // Line below is needed to remove Avalonia data validation.
            // Without this line you will get duplicate validations from both Avalonia and CT
            BindingPlugins.DataValidators.RemoveAt(0);

            ICountdownService countdownService = new CountdownService();

            PomodoroService pomodoroService = new PomodoroService(countdownService);

            PomodoroViewModel pomodoroViewModel = new PomodoroViewModel(pomodoroService);

            desktop.MainWindow = new MainWindow
            {
                DataContext = new MainWindowViewModel(pomodoroViewModel)
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
}