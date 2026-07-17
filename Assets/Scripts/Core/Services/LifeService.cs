
using System;
using System.Collections;
using UnityEngine;

public class LifeService
{
    private readonly PlayerProgress _playerProgress;
    private const int MinutesPerLife = 1;
    public event Action LivesChanged;

    public LifeService(PlayerProgress playerProgress)
    {
        _playerProgress = playerProgress;
    }

    public bool SpendLife()
    {
        if(_playerProgress.Lives <= 0)
        {
            return false;
        }

        _playerProgress.Lives--;

        LivesChanged?.Invoke();

        if(_playerProgress.Lives == PlayerProgress.MaxLives - 1)
        {
            _playerProgress.NextLifeTime = DateTime.Now.AddMinutes(MinutesPerLife);
        }

        return true;
    }

    public void AddLife()
    {
        if(_playerProgress.Lives < PlayerProgress.MaxLives)
        {
            _playerProgress.Lives++;

            LivesChanged?.Invoke();
        }
    }

    public void AddLives(int amount)
    {
        _playerProgress.Lives = Mathf.Min(
            PlayerProgress.MaxLives,
            _playerProgress.Lives + amount);

        LivesChanged?.Invoke();
    }

    public bool CanPlay()
    {
        return _playerProgress.Lives > 0;
    }

    public TimeSpan GetTimeUntilNextLife()
    {
        return _playerProgress.NextLifeTime - DateTime.Now;
    }

    public void RefreshLives()
    {
        if (_playerProgress.Lives >= PlayerProgress.MaxLives)
        {
            return;
        }

        while(_playerProgress.Lives < PlayerProgress.MaxLives && DateTime.Now >= _playerProgress.NextLifeTime)
        {
            AddLife();

            _playerProgress.NextLifeTime = _playerProgress.NextLifeTime.AddMinutes(MinutesPerLife);
        }

        LivesChanged?.Invoke();
    }
}
