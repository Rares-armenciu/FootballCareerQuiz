using UnityEngine;

public class LevelRewardCalculator
{
    private const int BaseReward = 100;
    private const int WrongAnswerPenalty = 10;

    private const int YellowCardPenalty = 10;
    private const int RedCardPenalty = 20;

    private const int FlawlessBonus = 25;
    private const int MinimumReward = 25;

    public LevelResult Calculate(
        int level,
        int totalQuestions,
        int correctAnswers,
        int wrongAnswers,
        int hintsUsed)
    {
        int wrongPenalty =
            wrongAnswers * WrongAnswerPenalty;

        int hintPenalty = CalculateHintPenalty(hintsUsed);

        bool flawless =
            wrongAnswers == 0 &&
            hintsUsed == 0;

        int flawlessBonus =
            flawless ? FlawlessBonus : 0;

        int reward =
            BaseReward
            - wrongPenalty
            - hintPenalty
            + flawlessBonus;

        reward = Mathf.Max(reward, MinimumReward);

        return new LevelResult(
            level,
            totalQuestions,
            correctAnswers,
            wrongAnswers,
            hintsUsed,
            BaseReward,
            wrongPenalty,
            hintPenalty,
            flawlessBonus,
            reward);
    }

    private int CalculateHintPenalty(int hintsUsed)
    {
        if (hintsUsed >= 2)
            return RedCardPenalty;

        if (hintsUsed == 1)
            return YellowCardPenalty;

        return 0;
    }
}