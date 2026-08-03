using UnityEngine;

public class LevelRewardCalculator
{
    private const int MinimumReward = 25;

    public LevelResult Calculate(
        LevelDefinition levelDefinition,
        int correctAnswers,
        int wrongAnswers,
        int hintsUsed)
    {
        int wrongPenalty = wrongAnswers * levelDefinition.WrongAnswerPenalty;

        int hintPenalty = hintsUsed * levelDefinition.HintPenalty;

        bool flawless =
            wrongAnswers == 0 &&
            hintsUsed == 0;

        int flawlessBonus =
            flawless ? levelDefinition.FlawlessBonus : 0;

        int reward =
            levelDefinition.BaseReward
            - wrongPenalty
            - hintPenalty
            + flawlessBonus;

        reward = Mathf.Max(reward, MinimumReward);
        var stars = CalculateStars(wrongAnswers, hintsUsed);

        return new LevelResult(
            levelDefinition.Level,
            levelDefinition.QuestionCount,
            correctAnswers,
            wrongAnswers,
            hintsUsed,
            levelDefinition.BaseReward,
            wrongPenalty,
            hintPenalty,
            flawlessBonus,
            reward,
            stars);
    }

    private int CalculateStars(
        int wrongAnswers,
        int hintsUsed)
    {
        if (wrongAnswers == 0 && hintsUsed == 0)
            return 5;

        if (wrongAnswers == 0 && hintsUsed >= 1)
            return 4;

        if (wrongAnswers <= 1)
            return 3;

        if (wrongAnswers <= 2)
            return 2;

        return 1;
    }
}