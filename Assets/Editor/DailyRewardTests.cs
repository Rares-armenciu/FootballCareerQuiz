using System;
using NUnit.Framework;

public class DailyRewardTests
{
    [Test]
    public void DailyReward_CanClaimOnlyOncePerUtcDay()
    {
        var progress = new PlayerProgress();
        var statistics = new PlayerStatistics();
        var achievements = new PlayerAchievements();
        var coins = new CoinsService(progress);
        var achievementService = new AchievementService(achievements, progress, statistics, coins);
        var statisticsService = new StatisticsService(statistics, achievementService);
        var service = new DailyRewardService(coins, new DailyRewardProgress(), statisticsService);
        var claimDate = new DateTime(2026, 1, 10, 12, 0, 0, DateTimeKind.Utc);

        DailyRewardClaim first = service.Claim(claimDate);
        DailyRewardClaim second = service.Claim(claimDate.AddHours(4));

        Assert.IsNotNull(first);
        Assert.IsNull(second);
        Assert.AreEqual(25, progress.Coins);
        Assert.AreEqual(25, statistics.CoinsEarned);
    }

    [Test]
    public void DailyReward_DisplayStreakBreaksAfterMissedDayBeforeClaiming()
    {
        var progress = new PlayerProgress();
        var statistics = new PlayerStatistics();
        var achievements = new PlayerAchievements();
        var coins = new CoinsService(progress);
        var achievementService = new AchievementService(achievements, progress, statistics, coins);
        var statisticsService = new StatisticsService(statistics, achievementService);
        var service = new DailyRewardService(coins, new DailyRewardProgress(), statisticsService);
        var firstDate = new DateTime(2026, 4, 1, 12, 0, 0, DateTimeKind.Utc);
        var afterTwoDays = firstDate.AddDays(2);

        service.Claim(firstDate);

        // Before claiming again, the UI-facing streak must already reflect
        // the broken streak so Day 1 is shown as today's reward.
        int displayStreakBeforeClaim = service.GetDisplayStreak(afterTwoDays);
        Assert.AreEqual(0, displayStreakBeforeClaim);

        DailyRewardClaim claim = service.Claim(afterTwoDays);

        Assert.AreEqual(1, claim.CurrentStreak);
        Assert.AreEqual(25, claim.CoinsAwarded);
    }

    [Test]
    public void DailyReward_ConsecutiveClaimsAdvanceStreakAndReward()
    {
        var progress = new PlayerProgress();
        var statistics = new PlayerStatistics();
        var achievements = new PlayerAchievements();
        var coins = new CoinsService(progress);
        var achievementService = new AchievementService(achievements, progress, statistics, coins);
        var statisticsService = new StatisticsService(statistics, achievementService);
        var service = new DailyRewardService(coins, new DailyRewardProgress(), statisticsService);
        var firstDate = new DateTime(2026, 2, 1, 12, 0, 0, DateTimeKind.Utc);

        service.Claim(firstDate);
        DailyRewardClaim second = service.Claim(firstDate.AddDays(1));

        Assert.AreEqual(2, second.CurrentStreak);
        Assert.AreEqual(35, second.CoinsAwarded);
        Assert.AreEqual(60, progress.Coins);
        Assert.AreEqual(60, statistics.CoinsEarned);
    }

    [Test]
    public void DailyReward_MissedDayResetsStreak()
    {
        var progress = new PlayerProgress();
        var statistics = new PlayerStatistics();
        var achievements = new PlayerAchievements();
        var coins = new CoinsService(progress);
        var achievementService = new AchievementService(achievements, progress, statistics, coins);
        var statisticsService = new StatisticsService(statistics, achievementService);
        var service = new DailyRewardService(coins, new DailyRewardProgress(), statisticsService);
        var firstDate = new DateTime(2026, 3, 1, 12, 0, 0, DateTimeKind.Utc);

        service.Claim(firstDate);
        DailyRewardClaim nextClaim = service.Claim(firstDate.AddDays(2));

        Assert.AreEqual(1, nextClaim.CurrentStreak);
        Assert.AreEqual(25, nextClaim.CoinsAwarded);
        Assert.AreEqual(50, progress.Coins);
    }

    [Test]
    public void DailyReward_AdBonus_AvailableBeforeBaseClaim()
    {
        var progress = new PlayerProgress();
        var statistics = new PlayerStatistics();
        var achievements = new PlayerAchievements();
        var coins = new CoinsService(progress);
        var achievementService = new AchievementService(achievements, progress, statistics, coins);
        var statisticsService = new StatisticsService(statistics, achievementService);
        var service = new DailyRewardService(coins, new DailyRewardProgress(), statisticsService);
        var claimDate = new DateTime(2026, 5, 1, 12, 0, 0, DateTimeKind.Utc);

        Assert.IsTrue(service.CanClaimAdBonus(claimDate));
    }

    [Test]
    public void DailyReward_AdBonus_DoublesTodaysReward()
    {
        var progress = new PlayerProgress();
        var statistics = new PlayerStatistics();
        var achievements = new PlayerAchievements();
        var coins = new CoinsService(progress);
        var achievementService = new AchievementService(achievements, progress, statistics, coins);
        var statisticsService = new StatisticsService(statistics, achievementService);
        var service = new DailyRewardService(coins, new DailyRewardProgress(), statisticsService);
        var claimDate = new DateTime(2026, 5, 1, 12, 0, 0, DateTimeKind.Utc);

        int baseReward = service.GetRewardForDay(1);
        DailyRewardClaim bonusClaim = service.ClaimAdBonus(claimDate);

        Assert.IsNotNull(bonusClaim);
        Assert.AreEqual(baseReward * 2, bonusClaim.CoinsAwarded);
        Assert.AreEqual(baseReward * 2, progress.Coins);
        Assert.AreEqual(baseReward * 2, statistics.CoinsEarned);
    }

    [Test]
    public void DailyReward_AdBonus_ClaimingBonusBlocksBaseClaimSameDay()
    {
        var progress = new PlayerProgress();
        var statistics = new PlayerStatistics();
        var achievements = new PlayerAchievements();
        var coins = new CoinsService(progress);
        var achievementService = new AchievementService(achievements, progress, statistics, coins);
        var statisticsService = new StatisticsService(statistics, achievementService);
        var service = new DailyRewardService(coins, new DailyRewardProgress(), statisticsService);
        var claimDate = new DateTime(2026, 5, 1, 12, 0, 0, DateTimeKind.Utc);

        service.ClaimAdBonus(claimDate);

        Assert.IsFalse(service.CanClaim(claimDate.AddMinutes(1)));
        Assert.IsNull(service.Claim(claimDate.AddMinutes(1)));
    }

    [Test]
    public void DailyReward_AdBonus_ClaimingBaseBlocksAdBonusSameDay()
    {
        var progress = new PlayerProgress();
        var statistics = new PlayerStatistics();
        var achievements = new PlayerAchievements();
        var coins = new CoinsService(progress);
        var achievementService = new AchievementService(achievements, progress, statistics, coins);
        var statisticsService = new StatisticsService(statistics, achievementService);
        var service = new DailyRewardService(coins, new DailyRewardProgress(), statisticsService);
        var claimDate = new DateTime(2026, 5, 1, 12, 0, 0, DateTimeKind.Utc);

        service.Claim(claimDate);

        Assert.IsFalse(service.CanClaimAdBonus(claimDate.AddMinutes(1)));
        Assert.IsNull(service.ClaimAdBonus(claimDate.AddMinutes(1)));
    }

    [Test]
    public void DailyReward_AdBonus_AvailableAgainNextDay()
    {
        var progress = new PlayerProgress();
        var statistics = new PlayerStatistics();
        var achievements = new PlayerAchievements();
        var coins = new CoinsService(progress);
        var achievementService = new AchievementService(achievements, progress, statistics, coins);
        var statisticsService = new StatisticsService(statistics, achievementService);
        var service = new DailyRewardService(coins, new DailyRewardProgress(), statisticsService);
        var firstDate = new DateTime(2026, 5, 1, 12, 0, 0, DateTimeKind.Utc);

        service.Claim(firstDate);
        Assert.IsTrue(service.CanClaimAdBonus(firstDate.AddDays(1)));
    }
}
