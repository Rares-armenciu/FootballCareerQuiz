using UnityEngine;

public class ProgressionService
{
    private const int QuestionsPerLevel = 5;

    private readonly PlayerProgress _playerProgress;

    private readonly LevelRewardCalculator _rewardCalculator = new();

    public bool IsCurrentLevelCompleted => _playerProgress.CurrentQuestion >= QuestionsInCurrentLevel;

    public ProgressionService(PlayerProgress playerProgress)
    {
        _playerProgress = playerProgress;

        GetCurrentLevelResult();
    }

    public int CurrentLevel => _playerProgress.CurrentLevel;

    public int CurrentQuestion => _playerProgress.CurrentQuestion;

    public int QuestionsInCurrentLevel => QuestionsPerLevel;

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

    public LevelResult GetCurrentLevelResult()
    {
        return _rewardCalculator.Calculate(
            _playerProgress.CurrentLevel,
            QuestionsInCurrentLevel,
            _playerProgress.CorrectAnswersThisLevel,
            _playerProgress.WrongAnswersThisLevel,
            _playerProgress.HintsUsedThisLevel);
    }

    public void SaveLevelResult(LevelResult result)
    {
        LevelProgress progress =
            _playerProgress.GetLevelProgress(result.Level);

        progress.BestStars =
            Mathf.Max(progress.BestStars, result.Stars);

        progress.BestReward =
            Mathf.Max(progress.BestReward, result.FinalReward);
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
            .Completed;
    }
}