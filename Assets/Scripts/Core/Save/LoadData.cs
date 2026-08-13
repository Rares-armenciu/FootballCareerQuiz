using System;

public class LoadData
{
    public PlayerProgress Progress;
    public PlayerStatistics Statistics;
    public PlayerAchievements Achievements;
    public DailyRewardProgress DailyReward;

    public LoadData(
        PlayerProgress progress,
        PlayerStatistics statistics,
        PlayerAchievements achievements,
        DailyRewardProgress dailyReward)
    {
        Progress = progress;
        Statistics = statistics;
        Achievements = achievements;
        DailyReward = dailyReward;
    }
}
