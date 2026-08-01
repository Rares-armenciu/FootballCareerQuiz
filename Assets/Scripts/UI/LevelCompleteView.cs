using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelCompleteView : MonoBehaviour
{
    [Header("Header")]
    [SerializeField] private TMP_Text title;
    [SerializeField] private TMP_Text rewardText;

    [Header("Reward Breakdown")]
    [SerializeField] private TMP_Text baseReward;
    [SerializeField] private TMP_Text wrongPenalty;
    [SerializeField] private TMP_Text hintsPenalty;
    [SerializeField] private TMP_Text flawlessBonus;

    [Header("Final Reward")]
    [SerializeField] private TMP_Text finalReward;

    [Header("Rows")]
    [SerializeField] private GameObject wrongRow;
    [SerializeField] private GameObject hintsRow;
    [SerializeField] private GameObject flawlessRow;

    [Header("Continue")]
    [SerializeField] private Button continueButton;

    public Button ContinueButton => continueButton;

    public void Show(LevelResult result)
    {
        gameObject.SetActive(true);

        title.text = $"LEVEL {result.Level} COMPLETE";

        rewardText.text = $"{result.CorrectAnswers}/{result.TotalQuestions} Correct";

        baseReward.text = $"+{result.BaseReward}";
        wrongPenalty.text = $"-{result.WrongAnswerPenalty}";
        hintsPenalty.text = $"-{result.HintPenalty}";
        flawlessBonus.text = $"+{result.FlawlessBonus}";

        wrongRow.SetActive(result.WrongAnswerPenalty > 0);
        hintsRow.SetActive(result.HintPenalty > 0);
        flawlessRow.SetActive(result.FlawlessBonus > 0);

        finalReward.text = $"{result.FinalReward} Coins";
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}