using System;
using UnityEngine;

public class PlayerProgress
{
    public const int MaxLives = 5;
    public int Coins { get; set; }
    public int Lives { get; set; } =  MaxLives;
    public int CurrentLevel { get; set; }
    public int CurrentQuestion { get; set; } = 0;
    public DateTime NextLifeTime { get; set; }
    public int CorrectAnswersThisLevel { get; set; }
    public int WrongAnswersThisLevel { get; set; }
    public int HintsUsedThisLevel { get; set; }

    public void Restore(PlayerProgressSaveData saveData)
    {
        int currentLevel = Mathf.Max(1, saveData.CurrentLevel);

        Coins = saveData.Coins;
        Lives = saveData.Lives;
        CurrentLevel = currentLevel;
        CurrentQuestion = saveData.CurrentQuestion;
        NextLifeTime = DateTime.Parse(saveData.NextLifeTime);
        CorrectAnswersThisLevel = saveData.CorrectAnswersThisLevel;
        WrongAnswersThisLevel = saveData.WrongAnswersThisLevel;
        HintsUsedThisLevel = saveData.HintsUsedThisLevel;
    }

    public PlayerProgressSaveData ToSaveData()
    {
        return new PlayerProgressSaveData
        {
            Coins = Coins,
            Lives = Lives,
            CurrentLevel = CurrentLevel,
            CurrentQuestion = CurrentQuestion,
            NextLifeTime = NextLifeTime.ToString("O"),
            CorrectAnswersThisLevel = CorrectAnswersThisLevel,
            WrongAnswersThisLevel = WrongAnswersThisLevel,
            HintsUsedThisLevel = HintsUsedThisLevel
        };
    }
}