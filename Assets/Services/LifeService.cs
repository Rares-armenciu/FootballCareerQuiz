public class LifeService
{
    private readonly PlayerProgress _playerProgress;

    public LifeService(PlayerProgress playerProgress)
    {
        _playerProgress = playerProgress;
    }

    public void SpendLife()
    {
        _playerProgress.Lives--;
    }

    public void AddLife()
    {
        _playerProgress.Lives++;
    }

    public void AddLives(int amount)
    {
        _playerProgress.Lives += amount;
    }

    public bool CanPlay()
    {
        return _playerProgress.Lives <= 0;
    }
}
