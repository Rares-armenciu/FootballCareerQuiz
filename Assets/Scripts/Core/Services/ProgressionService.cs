using Assets.Scripts.Data;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ProgressionService
{
    private readonly PlayerProgress _playerProgress;

    private readonly LevelRewardCalculator _rewardCalculator = new();

    private int? replayLevel;

    public ProgressionService(PlayerProgress playerProgress)
    {
        _playerProgress = playerProgress;
        replayLevel = null;
        GetCurrentLevelResult();
    }

    public bool IsCurrentLevelCompleted => _playerProgress.CurrentQuestion >= QuestionsInCurrentLevel;

    public LevelDefinition CurrentLevelDefinition => GameManager.Instance.LevelDatabase.Get(ActiveLevel);

    public int CurrentQuestion => _playerProgress.CurrentQuestion;

    public int QuestionsInCurrentLevel => CurrentLevelDefinition.QuestionCount;

    public bool IsReplay => replayLevel.HasValue;

    public int ActiveLevel => replayLevel ?? _playerProgress.CurrentLevel;

    public int HighestUnlockedLevel => _playerProgress.CurrentLevel;

    public bool AdvanceQuestion()
    {
        _playerProgress.CurrentQuestion++;

        return _playerProgress.CurrentQuestion >= QuestionsInCurrentLevel;
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
            CurrentLevelDefinition,
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
        return _playerProgress.GetLevelProgress(level).Level <= HighestUnlockedLevel;
    }

    public IEnumerable<LevelInfo> GetLevels()
    {
        List<LevelProgress> levels = new();

        foreach (LevelDefinition definition in GameManager.Instance.LevelDatabase.AllLevels)
        {
            LevelProgress progress =
                    _playerProgress.GetLevelProgress(
                        definition.Level);

            yield return new LevelInfo
            {
                Level = definition.Level,
                IsBossLevel = definition.IsBossLevel,
                QuestionCount = definition.QuestionCount,
                BestStars = progress.BestStars,
                BestReward = progress.BestReward,
                IsUnlocked = definition.Level <= HighestUnlockedLevel,
                IsCurrent = definition.Level == ActiveLevel,
                BestCorrectAnswers = progress.BestCorrectAnswers,
                
            };
        }
    }

    public void SaveLevelProgress(LevelResult result)
    {
        LevelProgress progress =
            _playerProgress.GetLevelProgress(result.Level);

        progress.BestStars =
            Mathf.Max(progress.BestStars, result.Stars);
        progress.BestCorrectAnswers = result.CorrectAnswers;
        progress.BestReward = Mathf.Max(progress.BestReward, result.FinalReward);
    }
}