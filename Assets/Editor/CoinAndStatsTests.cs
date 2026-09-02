using NUnit.Framework;
using System.Reflection;
using UnityEngine;

public class CoinAndStatsTests
{
    [Test]
    public void ProgressionService_CalculateCoinsToAward_ReturnsDelta()
    {
        var playerProgress = new PlayerProgress();

        // Ensure a previous best exists
        var levelProg = new LevelProgress { Level = 2, BestReward = 50 };
        playerProgress.Levels.Add(levelProg);

        var service = new ProgressionService(playerProgress);

        var result = new LevelResult(
            level: 2,
            totalQuestions: 5,
            correctAnswers: 5,
            wrongAnswers: 0,
            hintsUsed: 0,
            baseReward: 100,
            wrongAnswerPenalty: 0,
            hintPenalty: 0,
            flawlessBonus: 0,
            finalReward: 80,
            stars: 5);

        int delta = service.CalculateCoinsToAward(result);

        Assert.AreEqual(30, delta);
    }

    [Test]
    public void ProgressionService_SaveLevelProgress_UpdatesBest()
    {
        var playerProgress = new PlayerProgress();

        var service = new ProgressionService(playerProgress);

        var result = new LevelResult(
            level: 3,
            totalQuestions: 5,
            correctAnswers: 4,
            wrongAnswers: 1,
            hintsUsed: 0,
            baseReward: 100,
            wrongAnswerPenalty: 10,
            hintPenalty: 0,
            flawlessBonus: 0,
            finalReward: 90,
            stars: 4);

        // Initially no LevelProgress exists
        service.SaveLevelProgress(result);

        var progress = playerProgress.GetLevelProgress(3);

        Assert.AreEqual(4, progress.BestStars);
        Assert.AreEqual(90, progress.BestReward);
    }

    [Test]
    public void CoinsService_GrantAndSpend_Works()
    {
        var playerProgress = new PlayerProgress();
        var coinsService = new CoinsService(playerProgress);

        coinsService.GrantCoins(50);
        Assert.AreEqual(50, playerProgress.Coins);

        bool canAfford = coinsService.CanAfford(20);
        Assert.IsTrue(canAfford);

        bool spent = coinsService.SpendCoins(20);
        Assert.IsTrue(spent);
        Assert.AreEqual(30, playerProgress.Coins);

        // Can't spend more than available
        Assert.IsFalse(coinsService.SpendCoins(100));
    }

    [Test]
    public void StatisticsService_RecordCorrectWrongHint_UpdatesValues()
    {
        var stats = new PlayerStatistics();
        var playerAchievements = new PlayerAchievements();
        var achievementService = new AchievementService(playerAchievements, new PlayerProgress(), stats, new CoinsService(new PlayerProgress()), ScriptableObject.CreateInstance<AchievementDatabase>());
        var statisticsService = new StatisticsService(stats, achievementService);

        statisticsService.RecordCorrectAnswer();
        Assert.AreEqual(1, stats.CorrectAnswers);
        Assert.AreEqual(1, stats.QuestionsAnswered);

        statisticsService.RecordWrongAnswer();
        Assert.AreEqual(1, stats.WrongAnswers);
        Assert.AreEqual(2, stats.QuestionsAnswered);

        statisticsService.UseHint();
        Assert.AreEqual(1, stats.HintsUsed);
    }

    [Test]
    public void PlayerStatistics_RecordLevelCompleted_UpdatesStarsAndPerfect()
    {
        var stats = new PlayerStatistics();

        var previous = new LevelProgress { Level = 1, BestStars = 3 };

        var result = new LevelResult(
            level: 1,
            totalQuestions: 5,
            correctAnswers: 5,
            wrongAnswers: 0,
            hintsUsed: 0,
            baseReward: 100,
            wrongAnswerPenalty: 0,
            hintPenalty: 0,
            flawlessBonus: 0,
            finalReward: 100,
            stars: 5);

        // First completion should increment LevelsCompleted and PerfectLevelsCompleted
        stats.RecordLevelCompleted(previous, result, isBossLevel: false, firstCompletion: true);

        Assert.AreEqual(2, stats.StarsEarned); // gainedStars = 5 - 3
        Assert.AreEqual(1, stats.PerfectLevelsCompleted);
    }

    [Test]
    public void PlayerStatistics_ReplayDoesNotIncrementFirstCompletionCounters()
    {
        var stats = new PlayerStatistics();
        var previous = new LevelProgress { Level = 4, BestStars = 5 };
        var result = new LevelResult(
            level: 4,
            totalQuestions: 5,
            correctAnswers: 5,
            wrongAnswers: 0,
            hintsUsed: 0,
            baseReward: 100,
            wrongAnswerPenalty: 0,
            hintPenalty: 0,
            flawlessBonus: 0,
            finalReward: 100,
            stars: 5);

        stats.RecordLevelCompleted(previous, result, isBossLevel: true, firstCompletion: false);

        Assert.AreEqual(0, stats.LevelsCompleted);
        Assert.AreEqual(0, stats.BossLevelsCompleted);
        Assert.AreEqual(0, stats.PerfectLevelsCompleted);
        Assert.AreEqual(0, stats.StarsEarned);
    }

    [Test]
    public void PlayerStatistics_RecordCoinsEarned_IgnoresNonPositiveAmounts()
    {
        var stats = new PlayerStatistics();

        stats.RecordCoinsEarned(50);
        stats.RecordCoinsEarned(0);
        stats.RecordCoinsEarned(-10);

        Assert.AreEqual(50, stats.CoinsEarned);
    }

    [Test]
    public void AchievementService_Unlock_GrantsCoinsAndRecordsInStatistics()
    {
        // Arrange
        var playerProgress = new PlayerProgress();
        var playerStats = new PlayerStatistics();
        var playerAchievements = new PlayerAchievements();
        var coinsService = new CoinsService(playerProgress);

        var achievementService = new AchievementService(playerAchievements, playerProgress, playerStats, coinsService, ScriptableObject.CreateInstance<AchievementDatabase>());
        var statisticsService = new StatisticsService(playerStats, achievementService);

        // Create a GameManager object and set it as the static Instance via reflection
        var gmGo = new GameObject("GM_Test");
        var gm = gmGo.AddComponent<GameManager>();

        // Set GameManager.Instance = gm (private setter) via reflection
        var gmType = typeof(GameManager);
        var instanceProp = gmType.GetProperty("Instance", BindingFlags.Public | BindingFlags.Static);
        var setInstance = instanceProp.GetSetMethod(true);
        setInstance.Invoke(null, new object[] { gm });

        // Set the GameManager.Progress and GameManager.StatisticsService properties via non-public setters
        var progressProp = gmType.GetProperty("Progress", BindingFlags.Public | BindingFlags.Instance);
        var setProgress = progressProp.GetSetMethod(true);
        setProgress.Invoke(gm, new object[] { playerProgress });

        var statsProp = gmType.GetProperty("StatisticsService", BindingFlags.Public | BindingFlags.Instance);
        var setStats = statsProp.GetSetMethod(true);
        setStats.Invoke(gm, new object[] { statisticsService });

        // Act - unlock an achievement via reflection (Unlock is private)
        var achievement = new AchievementDefinition
        {
            Id = "test.unlock",
            Name = "Test Unlock",
            Description = "Grants coins",
            Type = AchievementType.CoinsEarned,
            Target = 10,
            RewardCoins = 123,
            Icon = AchievementIcon.Coins
        };

        var unlockMethod = typeof(AchievementService).GetMethod("Unlock", BindingFlags.NonPublic | BindingFlags.Instance);
        unlockMethod.Invoke(achievementService, new object[] { achievement });

        // Assert
        Assert.AreEqual(123, playerProgress.Coins, "PlayerProgress.Coins should be increased by achievement reward");
        Assert.AreEqual(123, playerStats.CoinsEarned, "PlayerStatistics.CoinsEarned should include achievement reward");

        // Cleanup
        Object.DestroyImmediate(gmGo);
    }
}
