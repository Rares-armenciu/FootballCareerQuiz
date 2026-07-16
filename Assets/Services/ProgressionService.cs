using UnityEngine;

public class ProgressionService
{
    private readonly PlayerProgress _playerProgress;

    public ProgressionService(PlayerProgress playerProgress)
    {
        _playerProgress = playerProgress;
    }

    public void LevelUp()
    {
        _playerProgress.CurrentLevel++;
    }
}
