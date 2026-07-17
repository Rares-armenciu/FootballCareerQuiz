using System;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class AchievementService
{
    private readonly PlayerAchievements _playerAchievements;
    private readonly PlayerProgress _playerProgress;
    private readonly PlayerStatistics _playerStatistics;
    private readonly CoinsService _coinsService;

    public AchievementService(
        PlayerAchievements playerAchievements,
        PlayerProgress playerProgress,
        PlayerStatistics playerStatistics,
        CoinsService coinsService)
    {
        _playerAchievements = playerAchievements;
        _playerProgress = playerProgress;
        _playerStatistics = playerStatistics;
        _coinsService = coinsService;
    }

    public event Action<AchievementDefinition> AchievementUnlocked;

    public void CheckAchievements()
    {
        Debug.Log("Achievements checked 1");
        foreach (var achievement in AchievementDatabase.All)
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
        Debug.Log("Achievements checked 2");

        foreach (var achievement in AchievementDatabase.All)
        {
            Debug.Log(
    $"{achievement.Name} | Type: {achievement.Type} | Target: {achievement.Target}");
            Debug.Log("Correct Answers: " + _playerStatistics.CorrectAnswers);
            if (achievement.Type != type)
                continue;
            if (_playerAchievements.IsUnlocked(achievement.Id))
                continue;
            if (MeetsRequirement(achievement))
            {
                Debug.Log("Achievements meets requirement");

                Unlock(achievement);
            }
        }
    }

    private bool MeetsRequirement(AchievementDefinition achievement)
    {
        return achievement.Type switch
        {
            AchievementType.CorrectAnswers =>
                _playerStatistics.CorrectAnswers >= achievement.Target,

            AchievementType.QuestionsAnswered =>
                _playerStatistics.QuestionsAnswered >= achievement.Target,

            AchievementType.LongestStreak =>
                _playerStatistics.LongestStreak >= achievement.Target,

            AchievementType.CurrentLevel =>
                _playerProgress.CurrentLevel >= achievement.Target,

            AchievementType.CoinsEarned =>
                _playerProgress.Coins >= achievement.Target,

            AchievementType.HintsUsed =>
                _playerStatistics.HintsUsed >= achievement.Target,

            _ => false
        };
    }

    private void Unlock(AchievementDefinition achievement)
    {
        _playerAchievements.Unlock(achievement.Id);

        _coinsService.AddCoins(achievement.RewardCoins);

        AchievementUnlocked?.Invoke(achievement);

        Debug.Log($"Achievement unlocked: {achievement.Name}");
    }
}