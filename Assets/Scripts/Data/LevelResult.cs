public class LevelResult
{
    public int Level { get; }
    public int TotalQuestions { get; }

    public int CorrectAnswers { get; }
    public int WrongAnswers { get; }
    public int HintsUsed { get; }

    public int BaseReward { get; }
    public int WrongAnswerPenalty { get; }
    public int HintPenalty { get; }
    public int FlawlessBonus { get; }

    public int FinalReward { get; }

    public bool IsFlawless =>
        WrongAnswers == 0 && HintsUsed == 0;

    public LevelResult(
        int level,
        int totalQuestions,
        int correctAnswers,
        int wrongAnswers,
        int hintsUsed,
        int baseReward,
        int wrongAnswerPenalty,
        int hintPenalty,
        int flawlessBonus,
        int finalReward)
    {
        Level = level;
        TotalQuestions = totalQuestions;

        CorrectAnswers = correctAnswers;
        WrongAnswers = wrongAnswers;
        HintsUsed = hintsUsed;

        BaseReward = baseReward;
        WrongAnswerPenalty = wrongAnswerPenalty;
        HintPenalty = hintPenalty;
        FlawlessBonus = flawlessBonus;

        FinalReward = finalReward;
    }
}