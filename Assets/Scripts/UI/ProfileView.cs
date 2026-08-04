using UnityEngine;

public class ProfileView : MonoBehaviour
{
    [Header("Rows")]
    [SerializeField] private ProfileRowView _levelRow;
    [SerializeField] private ProfileRowView _starsEarnedRow;
    [SerializeField] private ProfileRowView _bossLevelsRow;
    [SerializeField] private ProfileRowView _coinsRow;
    [SerializeField] private ProfileRowView _coinsEarnedRow;
    [SerializeField] private ProfileRowView _correctRow;
    [SerializeField] private ProfileRowView _accuracyRow;
    [SerializeField] private ProfileRowView _longestStreakRow;
    [SerializeField] private ProfileRowView _perfectLevelsRow;

    public void Show(PlayerProgress progress, PlayerStatistics statistics)
    {
        gameObject.SetActive(true);

        _levelRow.Set("Current Level", $"{progress.CurrentLevel}/{GameManager.Instance.LevelDatabase.LevelCount}");
        _starsEarnedRow.Set("Stars Earned", $"{statistics.StarsEarned}/{GameManager.Instance.LevelDatabase.LevelCount * 5}");
        _bossLevelsRow.Set("Boss Levels Cleared", $"{statistics.BossLevelsCompleted}/{GameManager.Instance.LevelDatabase.BossLevelCount}");
        _coinsRow.Set("Available Coins", progress.Coins.ToString("N0"));
        _coinsEarnedRow.Set("Coins Earned", statistics.CoinsEarned.ToString("N0"));
        _correctRow.Set("Correct Answers", $"{statistics.CorrectAnswers}/{statistics.QuestionsAnswered}");
        _accuracyRow.Set("Answer Accuracy", $"{statistics.AccuracyPercentage.ToString():F1}%");
        _longestStreakRow.Set("Longest Streak", statistics.LongestStreak.ToString());
        _perfectLevelsRow.Set("Perfect Levels", $"{statistics.PerfectLevelsCompleted} ({statistics.PerfectLevelPercentage.ToString():F1}%)");
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}