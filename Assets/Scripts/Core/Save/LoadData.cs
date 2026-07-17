using System;

public class LoadData
{
    public PlayerProgress Progress;
    public PlayerStatistics Statistics;
    public PlayerAchievements Achievements;

    public LoadData(PlayerProgress progress, PlayerStatistics statistics, PlayerAchievements achievements)
    {
        Progress = progress;
        Statistics = statistics;
        Achievements = achievements;
    }
}
