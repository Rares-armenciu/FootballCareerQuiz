using Assets.Scripts.Data;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ProgressionService
{
    private readonly PlayerProgress _playerProgress;

    private readonly LevelRewardCalculator _rewardCalculator = new();

    private int? replayLevel;

    // Session-only counters used while replaying a level. These ensure PlayerProgress
    // is not mutated during replay sessions and the real saved progress remains intact.
    private int sessionCurrentQuestion;
    private int sessionCorrectAnswersThisLevel;
    private int sessionWrongAnswersThisLevel;
    private int sessionHintsUsedThisLevel;

    public ProgressionService(PlayerProgress playerProgress)
    {
        _playerProgress = playerProgress;
        replayLevel = null;
        sessionCurrentQuestion = 0;
        sessionCorrectAnswersThisLevel = 0;
        sessionWrongAnswersThisLevel = 0;
        sessionHintsUsedThisLevel = 0;
    }

    public bool IsCurrentLevelCompleted => CurrentQuestion >= QuestionsInCurrentLevel;

    public LevelDefinition CurrentLevelDefinition => GameManager.Instance.LevelDatabase.Get(ActiveLevel);

    public int CurrentQuestion => IsReplay ? sessionCurrentQuestion : _playerProgress.CurrentQuestion;

    public int QuestionsInCurrentLevel => CurrentLevelDefinition.QuestionCount;

    public bool IsReplay => replayLevel.HasValue;

    public int ActiveLevel => replayLevel ?? _playerProgress.CurrentLevel;

    public int HighestUnlockedLevel => _playerProgress.CurrentLevel;

    public bool AdvanceQuestion()
    {
        if (IsReplay)
        {
            sessionCurrentQuestion++;

            return sessionCurrentQuestion >= QuestionsInCurrentLevel;
        }

        _playerProgress.CurrentQuestion++;

        return _playerProgress.CurrentQuestion >= QuestionsInCurrentLevel;
    }

    public void AdvanceToNextLevel()
    {
        _playerProgress.CurrentLevel++;
        _playerProgress.CurrentQuestion = 0;

        _playerProgress.CorrectAnswersThisLevel = 0;
        _playerProgress.WrongAnswersThisLevel = 0;
        _playerProgress.HintsUsedThisLevel = 0;
    }

    public void RecordCorrectAnswer()
    {
        if (IsReplay)
            sessionCorrectAnswersThisLevel++;
        else
            _playerProgress.CorrectAnswersThisLevel++;
    }

    public void RecordWrongAnswer()
    {
        if (IsReplay)
            sessionWrongAnswersThisLevel++;
        else
            _playerProgress.WrongAnswersThisLevel++;
    }

    public void RecordHintUsed()
    {
        if (IsReplay)
            sessionHintsUsedThisLevel++;
        else
            _playerProgress.HintsUsedThisLevel++;
    }

    public void StartReplay(int level)
    {
        // Initialize session counters for the replay and set the active replay level.
        // Do NOT mutate PlayerProgress so saved progress remains unchanged while replaying.
        replayLevel = level;

        sessionCurrentQuestion = 0;
        sessionCorrectAnswersThisLevel = 0;
        sessionWrongAnswersThisLevel = 0;
        sessionHintsUsedThisLevel = 0;
    }

    public void FinishReplay()
    {
        replayLevel = null;

        // Clear session counters; PlayerProgress was never mutated for the replay.
        sessionCurrentQuestion = 0;
        sessionCorrectAnswersThisLevel = 0;
        sessionWrongAnswersThisLevel = 0;
        sessionHintsUsedThisLevel = 0;
    }

    public LevelResult GetCurrentLevelResult()
    {
        // Use session counters when replaying, otherwise use player's persistent counters.
        int correct = IsReplay ? sessionCorrectAnswersThisLevel : _playerProgress.CorrectAnswersThisLevel;
        int wrong = IsReplay ? sessionWrongAnswersThisLevel : _playerProgress.WrongAnswersThisLevel;
        int hints = IsReplay ? sessionHintsUsedThisLevel : _playerProgress.HintsUsedThisLevel;

        return _rewardCalculator.Calculate(
            CurrentLevelDefinition,
            correct,
            wrong,
            hints);
    }

    public int GetBestStars(int level)
    {
        return _playerProgress
            .GetLevelProgress(level)
            .BestStars;
    }

    public bool IsLevelCompleted(int level)
    {
        return _playerProgress.GetLevelProgress(level).BestStars > 0;
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
        LevelProgress progress = _playerProgress.GetLevelProgress(result.Level);

        progress.BestStars = Mathf.Max(progress.BestStars, result.Stars);
        progress.BestCorrectAnswers = Mathf.Max(progress.BestCorrectAnswers, result.CorrectAnswers);
        progress.BestReward = Mathf.Max(progress.BestReward, result.FinalReward);
    }

    public int CalculateCoinsToAward(LevelResult result)
    {
        LevelProgress progress = _playerProgress.GetLevelProgress(result.Level);

        return Mathf.Max(0, result.FinalReward - progress.BestReward);
    }

    public bool IsNewBestReward(LevelResult result)
    {
        return result.FinalReward >
               _playerProgress
                   .GetLevelProgress(result.Level)
                   .BestReward;
    }

    public LevelProgress GetLevelProgress(int level)
    {
        return _playerProgress.GetLevelProgress(level);
    }
}