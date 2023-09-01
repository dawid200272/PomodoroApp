using System;

namespace PomodoroApp.Avalonia.Services;
public interface ICountdownService : IDisposable
{
    bool IsTimeUp { get; }
    int MinutesLeft { get; }
    int SecondsLeft { get; }

    event Action? Tick;

    void SetTime(int durationInMinutes);
    void StartTimer();
    void StopTimer();
}