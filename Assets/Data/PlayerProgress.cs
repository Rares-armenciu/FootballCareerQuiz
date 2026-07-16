using System;

public class PlayerProgress
{
    public const int MaxLives = 5;
    public int Coins { get; set; }

    public int Lives { get; set; } =  MaxLives;

    public int CurrentLevel { get; set; }

    public DateTime NextLifeTime { get; set; }

    public void Restore(
        int coins,
        int lives,
        int currentLevel,
        DateTime nextLifeTime)
    {
        Coins = coins;
        Lives = lives;
        CurrentLevel = currentLevel;
        NextLifeTime = nextLifeTime;
    }
}