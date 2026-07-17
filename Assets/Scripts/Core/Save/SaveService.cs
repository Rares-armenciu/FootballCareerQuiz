using System;
using UnityEngine;

public class SaveService
{
    private const string PlayerProgressKey = "SaveData";

    public void Save(PlayerProgress progress, PlayerStatistics statistics, PlayerAchievements achievements)
    {
        PlayerProgressSaveData progressData =
            new PlayerProgressSaveData
            {
                Coins = progress.Coins,
                Lives = progress.Lives,
                CurrentLevel = progress.CurrentLevel,
                NextLifeTime = progress.NextLifeTime.ToString("O")
            };

        PlayerStatisticsSaveData statsData =
            new PlayerStatisticsSaveData
            {
                QuestionsAnswered = statistics.QuestionsAnswered,
                CorrectAnswers = statistics.CorrectAnswers,
                WrongAnswers = statistics.WrongAnswers,
                HintsUsed = statistics.HintsUsed,
                LongestStreak = statistics.LongestStreak
            };

        SaveData saveData = new SaveData
        {
            Progress = progressData,
            Statistics = statsData,
            Achievements = achievements.ToSaveData()
        };

        string json = JsonUtility.ToJson(saveData);

        PlayerPrefs.SetString(PlayerProgressKey, json);
        PlayerPrefs.Save();
    }

    public LoadData Load()
    {
        if (!PlayerPrefs.HasKey(PlayerProgressKey))
        {
            return new LoadData(new PlayerProgress(), new PlayerStatistics(), new PlayerAchievements());
        }

        string json = PlayerPrefs.GetString(PlayerProgressKey);

        SaveData data =
            JsonUtility.FromJson<SaveData>(json);

        PlayerProgress progress = new PlayerProgress();

        progress.Restore(
            data.Progress.Coins,
            data.Progress.Lives,
            data.Progress.CurrentLevel,
            DateTime.Parse(data.Progress.NextLifeTime));

        PlayerStatistics statistics = new PlayerStatistics();

        statistics.Restore(
            data.Statistics.QuestionsAnswered,
            data.Statistics.CorrectAnswers,
            data.Statistics.WrongAnswers,
            data.Statistics.HintsUsed,
            data.Statistics.LongestStreak);

        PlayerAchievements achievements = new PlayerAchievements();
        achievements.Load(data.Achievements);

        return new LoadData(progress, statistics, achievements);
    }
}