using System;

[Serializable]
public class PlayerStatistics
{
    public int QuestionsAnswered { get; private set; }
    public int CorrectAnswers { get; private set; }
    public int WrongAnswers { get; private set; }
    public int HintsUsed { get; private set; }
    public int CurrentStreak { get; private set; }
    public int LongestStreak { get; private set; }
    public int CoinsEarned { get; private set; }
    public int LevelsCompleted { get; private set; }
    public int StarsEarned { get; private set; }
    public int PerfectLevelsCompleted { get; private set; }
    public int BossLevelsCompleted { get; private set; }
    public float AccuracyPercentage => QuestionsAnswered == 0 ? 0 : CorrectAnswers * 100f / QuestionsAnswered;
    public float PerfectLevelPercentage => LevelsCompleted == 0 ? 0 : PerfectLevelsCompleted * 100f / LevelsCompleted;

    public void Restore(PlayerStatisticsSaveData saveData)
    {
        QuestionsAnswered = saveData.QuestionsAnswered;
        CorrectAnswers = saveData.CorrectAnswers;
        WrongAnswers = saveData.WrongAnswers;
        HintsUsed = saveData.HintsUsed;
        CurrentStreak = 0;
        LongestStreak = saveData.LongestStreak;
        CoinsEarned = saveData.CoinsEarned;
        LevelsCompleted = saveData.LevelsCompleted;
        StarsEarned = saveData.StarsEarned;
        PerfectLevelsCompleted = saveData.PerfectLevelsCompleted;
        BossLevelsCompleted = saveData.BossLevelsCompleted;
    }

    public void RecordCorrectAnswer()
    {
        QuestionsAnswered++;
        CorrectAnswers++;
        CurrentStreak++;

        if (CurrentStreak > LongestStreak)
            LongestStreak = CurrentStreak;
    }

    public void RecordWrongAnswer()
    {
        QuestionsAnswered++;
        WrongAnswers++;
        CurrentStreak = 0;
    }

    public void UseHint()
    {
        HintsUsed++;
    }

    public void RecordCoinsEarned(int amount)
    {
        if (amount <= 0)
            return;

        CoinsEarned += amount;
    }

    public void RecordLevelCompleted(LevelProgress previous, LevelResult result, bool isBossLevel, bool firstCompletion)
    {
        if(firstCompletion)
        {
            LevelsCompleted++;

            if (isBossLevel)
            {
                BossLevelsCompleted++;
            }
        }

        int gainedStars = Math.Max(0, result.Stars - previous.BestStars);
        StarsEarned += gainedStars;

        if (previous.BestStars < 5 && result.Stars == 5)
        {
            PerfectLevelsCompleted++;
        }
    }

    public PlayerStatisticsSaveData ToSaveData()
    {
        return new PlayerStatisticsSaveData
        {
            QuestionsAnswered = QuestionsAnswered,
            CorrectAnswers = CorrectAnswers,
            WrongAnswers = WrongAnswers,
            HintsUsed = HintsUsed,
            LongestStreak = LongestStreak,
            CoinsEarned = CoinsEarned,
            LevelsCompleted = LevelsCompleted,
            StarsEarned = StarsEarned,
            PerfectLevelsCompleted = PerfectLevelsCompleted,
            BossLevelsCompleted = BossLevelsCompleted
        };
    }
}