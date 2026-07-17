public class StatisticsService
{
    private readonly PlayerStatistics _statistics;
    private readonly AchievementService _achievementService;

    public StatisticsService(PlayerStatistics statistics, AchievementService achievementService)
    {
        _statistics = statistics;
        _achievementService = achievementService;
    }

    public void RecordCorrectAnswer()
    {
        _statistics.RecordCorrectAnswer();

        _achievementService.CheckAchievements(AchievementType.CorrectAnswers);
    }

    public void RecordWrongAnswer()
    {
        _statistics.RecordWrongAnswer();

        _achievementService.CheckAchievements();
    }

    public void UseHint()
    {
        _statistics.UseHint();

        _achievementService.CheckAchievements();
    }
}