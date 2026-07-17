using System;
using UnityEngine;

public class SaveService
{
    private const string PlayerProgressKey = "PlayerProgress";

    public void Save(PlayerProgress progress, PlayerStatistics statistics)
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
        };

        string json = JsonUtility.ToJson(saveData);

        PlayerPrefs.SetString(PlayerProgressKey, json);
        PlayerPrefs.Save();
        Debug.Log("Questions Answered " + saveData.Statistics.QuestionsAnswered);
        Debug.Log("Correct Answers " + saveData.Statistics.CorrectAnswers);
        Debug.Log("Wrong Answers " + saveData.Statistics.WrongAnswers);
        Debug.Log("Hints Used " + saveData.Statistics.HintsUsed);
        Debug.Log("Longest Streak " + saveData.Statistics.LongestStreak);
        Debug.Log("Has Key " + PlayerPrefs.HasKey(PlayerProgressKey));
    }

    public (PlayerProgress, PlayerStatistics) Load()
    {
        if (!PlayerPrefs.HasKey(PlayerProgressKey))
        {
            return (new PlayerProgress(), new PlayerStatistics());
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


        return (progress, statistics);
    }
}