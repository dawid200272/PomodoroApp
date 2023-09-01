using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PomodoroApp.Avalonia.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PomodoroApp.Avalonia.ViewModels;
public partial class PomodoroViewModel : ViewModelBase, IDisposable
{
    private readonly PomodoroService _pomodoroService;

    [ObservableProperty]
    private string _timeLeftDisplay;

    [ObservableProperty]
    private string _pomodoroCount;

    [ObservableProperty]
    private string _pomodoroStateDisplay;

    private int _minutesLeft;
    private int _secondsLeft;

    public PomodoroViewModel(PomodoroService pomodoroService)
    {
        _pomodoroService = pomodoroService;

        PomodoroService_TimerReseted();

        _pomodoroService.TimeLeftChanged += PomodoroService_TimeLeftChanged;
        _pomodoroService.PomodoroCountChanged += PomodoroService_PomodoroCountChanged;
        _pomodoroService.CurrentStateChanged += PomodoroService_CurrentStateChanged;
        _pomodoroService.TimerReseted += PomodoroService_TimerReseted;
    }

    /// <summary>
    /// Design time only constructor
    /// </summary>
    public PomodoroViewModel()
    {
        ICountdownService countdownService = new CountdownService();
        _pomodoroService = new PomodoroService(countdownService);
    }

    public void Dispose()
    {
        _pomodoroService.TimeLeftChanged -= PomodoroService_TimeLeftChanged;
        _pomodoroService.PomodoroCountChanged -= PomodoroService_PomodoroCountChanged;
        _pomodoroService.CurrentStateChanged -= PomodoroService_CurrentStateChanged;
        _pomodoroService.TimerReseted -= PomodoroService_TimerReseted;

        _pomodoroService.Dispose();
    }

    [RelayCommand]
    private void StartPomodoroTimer() => _pomodoroService.Start();

    [RelayCommand]
    private void StopPomodoroTimer() => _pomodoroService.Stop();

    [RelayCommand]
    private void ResetPomodoroTimer() => _pomodoroService.Reset();

    private void PomodoroService_TimeLeftChanged()
    {
        UpdateTimeLeftDisplay();
    }

    private void PomodoroService_PomodoroCountChanged()
    {
        UpdatePomodoroCount();
    }

    private void PomodoroService_CurrentStateChanged()
    {
        string pomodoroStateDisplay = string.Empty;

        switch (_pomodoroService.CurrentState)
        {
            case PomodoroState.Pomodoro:
                pomodoroStateDisplay = "Pomodoro";
                break;
            case PomodoroState.ShortBreak:
                pomodoroStateDisplay = "Short Break";
                break;
            case PomodoroState.LongBreak:
                pomodoroStateDisplay = "Long Break";
                break;
        }

        PomodoroStateDisplay = pomodoroStateDisplay;
    }

    private void PomodoroService_TimerReseted()
    {
        UpdateTimeLeftDisplay();

        UpdatePomodoroCount();

        PomodoroStateDisplay = _pomodoroService.CurrentState.ToString();
    }

    private void UpdateTimeLeftDisplay()
    {
        _minutesLeft = _pomodoroService.MinutesLeft;
        _secondsLeft = _pomodoroService.SecondsLeft;

        TimeLeftDisplay = $"{_minutesLeft:00}:{_secondsLeft:00}";
    }

    private void UpdatePomodoroCount()
    {
        PomodoroCount = _pomodoroService.PomodoroCount.ToString();
    }
}
