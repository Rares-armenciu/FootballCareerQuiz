using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.SocialPlatforms.Impl;

public class AchievementService
{
    private readonly PlayerAchievements _playerAchievements;
    private readonly PlayerProgress _playerProgress;
    private readonly PlayerStatistics _playerStatistics;
    private readonly CoinsService _coinsService;
    private readonly AchievementDatabase _achievementDatabase;

    public AchievementService(
        PlayerAchievements playerAchievements,
        PlayerProgress playerProgress,
        PlayerStatistics playerStatistics,
        CoinsService coinsService,
        AchievementDatabase achievementDatabase)
    {
        _playerAchievements = playerAchievements;
        _playerProgress = playerProgress;
        _playerStatistics = playerStatistics;
        _coinsService = coinsService;
        _achievementDatabase = achievementDatabase;
    }

    public event Action<AchievementDefinition> AchievementUnlocked;

    public void CheckAchievements()
    {
        foreach (var achievement in _achievementDatabase.AllAchievements)
        {
            if (_playerAchievements.IsUnlocked(achievement.Id))
                continue;

            if (MeetsRequirement(achievement))
            {
                Unlock(achievement);
            }
        }
    }

    public void CheckAchievements(AchievementType type)
    {
        foreach (var achievement in _achievementDatabase.AllAchievements)
        {
            if (achievement.Type != type)
                continue;
            if (_playerAchievements.IsUnlocked(achievement.Id))
                continue;
            if (MeetsRequirement(achievement))
            {
                Unlock(achievement);
            }
        }
    }

    public int GetCurrentProgress(AchievementDefinition achievement)
    {
        switch(achievement.Type)
        {
            case AchievementType.CorrectAnswers:
                return _playerStatistics.CorrectAnswers;
            case AchievementType.WrongAnswers:
                return _playerStatistics.WrongAnswers;
            case AchievementType.QuestionsAnswered:
                return _playerStatistics.QuestionsAnswered;
            case AchievementType.Streak:
                return _playerStatistics.LongestStreak;
            case AchievementType.CurrentLevel:
                return _playerProgress.CurrentLevel;
            case AchievementType.CoinsEarned:
                return _playerStatistics.CoinsEarned;
            case AchievementType.HintsUsed:
                return _playerStatistics.HintsUsed;
            case AchievementType.PerfectLevels:
                return _playerStatistics.PerfectLevelsCompleted;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public IReadOnlyList<AchievementDefinition> GetAchievements()
    {
        return _achievementDatabase.AllAchievements;
    }

    private bool MeetsRequirement(AchievementDefinition achievement)
    {
        return achievement.Type switch
        {
            AchievementType.CorrectAnswers =>
                _playerStatistics.CorrectAnswers >= achievement.Target,

            AchievementType.WrongAnswers =>
                _playerStatistics.WrongAnswers >= achievement.Target,

            AchievementType.QuestionsAnswered =>
                _playerStatistics.QuestionsAnswered >= achievement.Target,

            AchievementType.Streak =>
                _playerStatistics.LongestStreak >= achievement.Target,

            AchievementType.CurrentLevel =>
                _playerProgress.CurrentLevel >= achievement.Target,

            AchievementType.CoinsEarned =>
                _playerStatistics.CoinsEarned >= achievement.Target,

            AchievementType.HintsUsed =>
                _playerStatistics.HintsUsed >= achievement.Target,

            AchievementType.PerfectLevels =>
                _playerStatistics.PerfectLevelsCompleted >= achievement.Target,

            _ => false
        };
    }

    private void Unlock(AchievementDefinition achievement)
    {
        _playerAchievements.Unlock(achievement.Id);

        _coinsService.GrantCoins(achievement.RewardCoins);

        // Achievement rewards are lifetime earnings and must be included in
        // CoinsEarned even when the service is used outside a live GameManager.
        _playerStatistics.RecordCoinsEarned(achievement.RewardCoins);

        AchievementUnlocked?.Invoke(achievement);

        Debug.Log($"Achievement unlocked: {achievement.Name}");
    }
}