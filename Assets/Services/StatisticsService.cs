public class StatisticsService
{
    private readonly PlayerStatistics _statistics;

    public StatisticsService(PlayerStatistics statistics)
    {
        _statistics = statistics;
    }

    public void RecordCorrectAnswer()
    {
        _statistics.RecordCorrectAnswer();
    }

    public void RecordWrongAnswer()
    {
        _statistics.RecordWrongAnswer();
    }

    public void UseHint()
    {
        _statistics.UseHint();
    }
}