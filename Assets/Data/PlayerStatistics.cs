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

    public float Accuracy =>
        QuestionsAnswered == 0
            ? 0
            : (float)CorrectAnswers / QuestionsAnswered;

    public void Restore(
        int questionsAnswered,
        int correctAnswers,
        int wrongAnswers,
        int hintsUsed,
        int longestStreak)
    {
        QuestionsAnswered = questionsAnswered;
        CorrectAnswers = correctAnswers;
        WrongAnswers = wrongAnswers;
        HintsUsed = hintsUsed;
        CurrentStreak = 0;
        LongestStreak = longestStreak;
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

    public int AccuracyPercentage()
    {
        return (int)(Accuracy * 100);
    }
}