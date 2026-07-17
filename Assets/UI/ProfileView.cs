using UnityEngine;

public class ProfileView : MonoBehaviour
{
    [Header("Rows")]
    [SerializeField] private StatRowView _levelRow;
    [SerializeField] private StatRowView _coinsRow;
    [SerializeField] private StatRowView _questionsRow;
    [SerializeField] private StatRowView _correctRow;
    [SerializeField] private StatRowView _accuracyRow;
    [SerializeField] private StatRowView _longestStreakRow;

    public void Show(PlayerProgress progress, PlayerStatistics statistics)
    {
        gameObject.SetActive(true);

        _levelRow.Set("Level", progress.CurrentLevel.ToString());
        _coinsRow.Set("Coins", progress.Coins.ToString("N0"));

        _questionsRow.Set(
            "Questions",
            statistics.QuestionsAnswered.ToString());

        _correctRow.Set(
            "Correct",
            statistics.CorrectAnswers.ToString());

        _accuracyRow.Set(
            "Accuracy",
            $"{statistics.AccuracyPercentage().ToString():F1}%");

        _longestStreakRow.Set(
            "Longest Streak",
            statistics.LongestStreak.ToString());
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}