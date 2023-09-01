using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PomodoroApp.Avalonia.Services;
public class PomodoroService : IDisposable
{
    private readonly ICountdownService _countdownService;

    private const int LONG_BREAK_AFTER_POMODOROS = 4;

    private const int POMODORO_TIME_MINUTES = 25;
    private const int SHORT_BREAK_TIME_MINUTES = 5;
    private const int LONG_BREAK_TIME_MINUTES = 15;

    public PomodoroState CurrentState { get; private set; }
    private PomodoroState _lastState;

    private bool _isRunning;
    private bool _isFirstTime;

    public PomodoroService(ICountdownService countdownService)
    {
        _countdownService = countdownService;
        
        ResetPomodoroTimer();

        _countdownService.Tick += CountdownService_Tick;
    }

    public int PomodoroCount { get; private set; }

    public int MinutesLeft => _countdownService.MinutesLeft;
    public int SecondsLeft => _countdownService.SecondsLeft;
    public bool IsTimeUp => _countdownService.IsTimeUp;

    public event Action? TimeLeftChanged;
    public event Action? PomodoroCountChanged;
    public event Action? CurrentStateChanged;
    public event Action? TimerReseted;

    public void Start()
    {
        if (_isRunning)
        {
            _countdownService.StartTimer();
            return;
        }

        //SetPomodoroTimer();

        _countdownService.StartTimer();
        _isRunning = true;
        _isFirstTime = false;
    }

    public void Stop() => _countdownService.StopTimer();

    public void Reset() => ResetPomodoroTimer();

    private void CountdownService_Tick()
    {
        OnTimeLeftChanged();

        if (!IsTimeUp)
        {
            return;
        }

        _countdownService.StopTimer();

        if (CurrentState is not PomodoroState.Pomodoro)
        {
            CurrentState = PomodoroState.Pomodoro;

            OnCurrentStateChanged();

            _isRunning = false;

            _countdownService.SetTime(POMODORO_TIME_MINUTES);

            OnTimerReseted();

            //Start();

            return;
        }

        PomodoroCount++;

        OnPomodoroCountChanged();

        if (PomodoroCount % LONG_BREAK_AFTER_POMODOROS == 0)
        {
            CurrentState = PomodoroState.LongBreak;

            OnCurrentStateChanged();

            _isRunning = false;

            _countdownService.SetTime(LONG_BREAK_TIME_MINUTES);

            OnTimerReseted();

            //Start();

            return;
        }

        CurrentState = PomodoroState.ShortBreak;

        OnCurrentStateChanged();

        _isRunning = false;

        _countdownService.SetTime(SHORT_BREAK_TIME_MINUTES);

        OnTimerReseted();

        //var t = Task.Run(async delegate
        //{
        //    await Task.Delay(1000);
        //});

        //Start();
    }

    private void OnPomodoroCountChanged()
    {
        PomodoroCountChanged?.Invoke();
    }

    private void OnTimeLeftChanged()
    {
        TimeLeftChanged?.Invoke();
    }

    private void OnCurrentStateChanged()
    {
        CurrentStateChanged?.Invoke();
    }

    private void OnTimerReseted()
    {
        TimerReseted?.Invoke();
    }

    private void ResetPomodoroTimer()
    {
        CurrentState = PomodoroState.Pomodoro;

        PomodoroCount = 0;

        _countdownService.SetTime(POMODORO_TIME_MINUTES);

        _isFirstTime = true;

        _isRunning = false;

        Stop();

        OnTimerReseted();
    }

    private void SetPomodoroTimer()
    {
        _lastState = CurrentState;

        switch (CurrentState)
        {
            case PomodoroState.Pomodoro:
                if (_isFirstTime)
                {
                    break;
                }

                _countdownService.SetTime(POMODORO_TIME_MINUTES);
                break;
            case PomodoroState.ShortBreak:
                _countdownService.SetTime(SHORT_BREAK_TIME_MINUTES);
                break;
            case PomodoroState.LongBreak:
                _countdownService.SetTime(LONG_BREAK_TIME_MINUTES);
                break;
            default:
                throw new Exception("Not supported state.");
        }
    }

    public void Dispose()
    {
        _countdownService.Tick -= CountdownService_Tick;

        _countdownService.Dispose();
    }
}

public enum PomodoroState
{
    Pomodoro,
    ShortBreak,
    LongBreak
}