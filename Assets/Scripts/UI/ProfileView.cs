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

        _levelRow.Set("Current Level", progress.CurrentLevel.ToString());
        _coinsRow.Set("Available Coins", progress.Coins.ToString("N0"));

        _questionsRow.Set(
            "Questions Answered",
            statistics.QuestionsAnswered.ToString());

        _correctRow.Set(
            "Correct Answers",
            statistics.CorrectAnswers.ToString());

        _accuracyRow.Set(
            "Answer Accuracy",
            $"{statistics.AccuracyPercentage.ToString():F1}%");

        _longestStreakRow.Set(
            "Longest Streak",
            statistics.LongestStreak.ToString());
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}