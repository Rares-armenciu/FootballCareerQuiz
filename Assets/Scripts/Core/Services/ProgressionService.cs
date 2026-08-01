using System.Collections.Generic;
using UnityEngine;

public class ProgressionService
{
    private const int QuestionsPerLevel = 5;

    private readonly PlayerProgress _playerProgress;

    private readonly LevelRewardCalculator _rewardCalculator = new();

    private int? replayLevel;

    public bool IsCurrentLevelCompleted => _playerProgress.CurrentQuestion >= QuestionsInCurrentLevel;

    public ProgressionService(PlayerProgress playerProgress)
    {
        _playerProgress = playerProgress;
        replayLevel = null;
        GetCurrentLevelResult();
    }

    public int CurrentQuestion => _playerProgress.CurrentQuestion;

    public int QuestionsInCurrentLevel => QuestionsPerLevel;

    public bool IsReplay => replayLevel.HasValue;

    public int ActiveLevel => replayLevel ?? _playerProgress.CurrentLevel;

    public int HighestUnlockedLevel =>
    _playerProgress.CurrentLevel + 1;

    public bool AdvanceQuestion()
    {
        _playerProgress.CurrentQuestion++;

        return _playerProgress.CurrentQuestion >= QuestionsPerLevel;
    }

    public void CompleteLevel()
    {
        _playerProgress.CurrentLevel++;
        _playerProgress.CurrentQuestion = 0;

        _playerProgress.CorrectAnswersThisLevel = 0;
        _playerProgress.WrongAnswersThisLevel = 0;
        _playerProgress.HintsUsedThisLevel = 0;
    }

    public void RecordCorrectAnswer()
    {
        _playerProgress.CorrectAnswersThisLevel++;
    }

    public void RecordWrongAnswer()
    {
        _playerProgress.WrongAnswersThisLevel++;
    }

    public void RecordHintUsed()
    {
        _playerProgress.HintsUsedThisLevel++;
    }

    public void StartReplay(int level)
    {
        replayLevel = level;
    }

    public void FinishReplay()
    {
        replayLevel = null;
    }

    public LevelResult GetCurrentLevelResult()
    {
        return _rewardCalculator.Calculate(
            ActiveLevel,
            QuestionsInCurrentLevel,
            _playerProgress.CorrectAnswersThisLevel,
            _playerProgress.WrongAnswersThisLevel,
            _playerProgress.HintsUsedThisLevel);
    }

    public int GetBestStars(int level)
    {
        return _playerProgress
            .GetLevelProgress(level)
            .BestStars;
    }

    public bool IsLevelCompleted(int level)
    {
        return _playerProgress
            .GetLevelProgress(level)
            .IsUnlocked;
    }

    public IReadOnlyList<LevelProgress> GetLevels()
    {
        List<LevelProgress> levels = new();

        for (int i = 0; i <= HighestUnlockedLevel; i++)
        {
            LevelProgress progress =
                _playerProgress.GetLevelProgress(i);

            progress.IsUnlocked = i <= HighestUnlockedLevel;

            levels.Add(progress);
        }

        return levels;
    }

    public void SaveLevelProgress(LevelResult result)
    {
        LevelProgress progress =
            _playerProgress.GetLevelProgress(result.Level);

        progress.BestStars =
            Mathf.Max(progress.BestStars, result.Stars);
        progress.TotalQuestions = result.TotalQuestions;
        progress.CorrectAnswers = result.CorrectAnswers;
        progress.BestReward = Mathf.Max(progress.BestReward, result.FinalReward);
    }
}