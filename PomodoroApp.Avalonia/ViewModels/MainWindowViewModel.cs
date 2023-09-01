using CommunityToolkit.Mvvm.ComponentModel;

namespace PomodoroApp.Avalonia.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    public PomodoroViewModel PromodoroViewModel { get; }

    public MainWindowViewModel(PomodoroViewModel promodoroViewModel)
    {
        PromodoroViewModel = promodoroViewModel;
    }

    /// <summary>
    /// Design time only constructor
    /// </summary>
    public MainWindowViewModel()
    {
        PromodoroViewModel = new PomodoroViewModel();
    }
}
