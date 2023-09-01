using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace PomodoroApp.Avalonia.Services;
public class CountdownService : ICountdownService
{
    private readonly Timer _timer;

    private const int INTERVAL_IN_MILISECONDS = 1000;
    private const int SECONDS_PER_MINUTE = 60;

    private int _timeLeftInSeconds;

    public CountdownService()
    {
        _timer = new Timer(interval: INTERVAL_IN_MILISECONDS);

        _timer.Elapsed += Timer_Elapsed;
    }

    public int MinutesLeft => _timeLeftInSeconds / SECONDS_PER_MINUTE;
    public int SecondsLeft => _timeLeftInSeconds % SECONDS_PER_MINUTE;

    public bool IsTimeUp => _timeLeftInSeconds == 0;

    public event Action? Tick;

    public void SetTime(int durationInMinutes)
    {
        _timeLeftInSeconds = durationInMinutes * SECONDS_PER_MINUTE;
    }

    public void StartTimer() => _timer.Start();

    public void StopTimer() => _timer.Stop();

    private void Timer_Elapsed(object? sender, ElapsedEventArgs e)
    {
        _timeLeftInSeconds--;

        Tick?.Invoke();
    }

    public void Dispose()
    {
        _timer.Elapsed -= Timer_Elapsed;

        _timer.Dispose();
    }
}
