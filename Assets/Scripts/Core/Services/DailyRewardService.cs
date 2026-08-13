using System;
using UnityEngine;

public class DailyRewardService
{
    private static readonly int[] RewardSchedule = { 25, 35, 50, 65, 75, 100, 200 };

    private readonly StatisticsService _statisticsService;
    private readonly CoinsService _coinsService;
    private readonly DailyRewardProgress _progress;

    public DailyRewardService(
        CoinsService coinsService,
        DailyRewardProgress progress,
        StatisticsService statisticsService)
    {
        _coinsService = coinsService;
        _progress = progress ?? new DailyRewardProgress();
        _statisticsService = statisticsService;
    }

    public DailyRewardProgress Progress => _progress;

    public int CurrentStreak => _progress.CurrentStreak;

    // Streak used for display purposes only. If the last claim was not
    // yesterday or today, the streak is treated as broken (0) even though
    // the persisted CurrentStreak has not been reset yet (it only resets
    // on the next successful Claim()).
    public int GetDisplayStreak()
    {
        return GetDisplayStreak(DateTime.UtcNow);
    }

    public int GetDisplayStreak(DateTime utcNow)
    {
        utcNow = utcNow.ToUniversalTime();

        if (string.IsNullOrEmpty(_progress.LastClaimDateUtc))
            return 0;

        string today = utcNow.Date.ToString("yyyy-MM-dd");

        if (_progress.LastClaimDateUtc == today)
            return _progress.CurrentStreak;

        DateTime yesterday = utcNow.Date.AddDays(-1);
        bool isYesterday = DateTime.TryParseExact(
            _progress.LastClaimDateUtc,
            "yyyy-MM-dd",
            null,
            System.Globalization.DateTimeStyles.None,
            out DateTime lastClaimDate)
            && lastClaimDate.Date == yesterday;

        return isYesterday ? _progress.CurrentStreak : 0;
    }

    public bool CanClaim()
    {
        return CanClaim(DateTime.UtcNow);
    }

    public bool CanClaim(DateTime utcNow)
    {
        string today = utcNow.ToUniversalTime().Date.ToString("yyyy-MM-dd");
        return _progress.LastClaimDateUtc != today;
    }

    public int GetRewardForDay(int rewardDay)
    {
        if (rewardDay < 1 || rewardDay > RewardSchedule.Length)
            throw new ArgumentOutOfRangeException(nameof(rewardDay));

        return RewardSchedule[rewardDay - 1];
    }

    public DailyRewardClaim Claim()
    {
        return Claim(DateTime.UtcNow);
    }

    public DailyRewardClaim Claim(DateTime utcNow)
    {
        return ClaimInternal(utcNow, multiplier: 1);
    }

    // Alternative to Claim() that awards double the day's coins in exchange
    // for watching a rewarded ad. Shares the same one-claim-per-day gate as
    // Claim(), so once either is used today the other becomes unavailable.
    public bool CanClaimAdBonus()
    {
        return CanClaim(DateTime.UtcNow);
    }

    public bool CanClaimAdBonus(DateTime utcNow)
    {
        return CanClaim(utcNow);
    }

    public DailyRewardClaim ClaimAdBonus()
    {
        return ClaimAdBonus(DateTime.UtcNow);
    }

    public DailyRewardClaim ClaimAdBonus(DateTime utcNow)
    {
        return ClaimInternal(utcNow, multiplier: 2);
    }

    private DailyRewardClaim ClaimInternal(DateTime utcNow, int multiplier)
    {
        utcNow = utcNow.ToUniversalTime();
        string today = utcNow.Date.ToString("yyyy-MM-dd");

        if (_progress.LastClaimDateUtc == today)
            return null;

        DateTime yesterday = utcNow.Date.AddDays(-1);
        bool continuesStreak = DateTime.TryParseExact(
            _progress.LastClaimDateUtc,
            "yyyy-MM-dd",
            null,
            System.Globalization.DateTimeStyles.None,
            out DateTime lastClaimDate)
            && lastClaimDate.Date == yesterday;

        _progress.CurrentStreak = continuesStreak
            ? Mathf.Clamp(_progress.CurrentStreak + 1, 1, RewardSchedule.Length)
            : 1;
        _progress.LastClaimDateUtc = today;

        int rewardDay = _progress.CurrentStreak;
        int coinsAwarded = GetRewardForDay(rewardDay) * multiplier;

        _coinsService.GrantCoins(coinsAwarded);
        _statisticsService.RecordCoinsEarned(coinsAwarded);

        return new DailyRewardClaim(rewardDay, coinsAwarded, _progress.CurrentStreak);
    }
}
