using UnityEngine;

public class CoinsService
{
    private readonly PlayerProgress _playerProgress;

    public CoinsService(PlayerProgress playerProgress)
    {
        _playerProgress = playerProgress;
    }

    public void AddCoins(int amount)
    {
        _playerProgress.Coins += amount;
    }

    public bool SpendCoins(int amount)
    {
        if (_playerProgress.Coins < amount)
        {
            return false;
        }

        _playerProgress.Coins -= amount;
        return true;
    }
}
