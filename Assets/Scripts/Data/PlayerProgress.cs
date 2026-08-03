using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProgress
{
    public const int MaxLives = 5;
    public int Coins { get; set; }
    public int Lives { get; set; } =  MaxLives;
    public int CurrentLevel { get; set; } = 1;
    public int CurrentQuestion { get; set; } = 0;
    public DateTime NextLifeTime { get; set; }
    public int CorrectAnswersThisLevel { get; set; }
    public int WrongAnswersThisLevel { get; set; }
    public int HintsUsedThisLevel { get; set; }
    public List<LevelProgress> Levels { get; set; } = new();

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
        
        if(saveData.Levels != null)
        {
            Levels.Clear();
            Levels.AddRange(saveData.Levels);
        }
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
            HintsUsedThisLevel = HintsUsedThisLevel,
            Levels = Levels
        };
    }

    public LevelProgress GetLevelProgress(int level)
    {
        LevelProgress progress = Levels.Find(l => l.Level == level);

        if (progress != null)
            return progress;

        progress = new LevelProgress
        {
            Level = level
        };

        Levels.Add(progress);

        return progress;
    }
}