using UnityEngine;

public class CoinsService
{
    private readonly PlayerProgress _playerProgress;

    public CoinsService(PlayerProgress playerProgress)
    {
        _playerProgress = playerProgress;
    }

    public void GrantCoins(int amount)
    {
        _playerProgress.Coins += amount;
    }

    public bool SpendCoins(int amount)
    {
        if (!CanAfford(amount))
        {
            return false;
        }

        _playerProgress.Coins -= amount;
        return true;
    }

    public bool CanAfford(int amount)
    {
        return _playerProgress.Coins >= amount;
    }
}
