public class DailyRewardClaim
{
    public int RewardDay { get; }
    public int CoinsAwarded { get; }
    public int CurrentStreak { get; }

    public DailyRewardClaim(int rewardDay, int coinsAwarded, int currentStreak)
    {
        RewardDay = rewardDay;
        CoinsAwarded = coinsAwarded;
        CurrentStreak = currentStreak;
    }
}
