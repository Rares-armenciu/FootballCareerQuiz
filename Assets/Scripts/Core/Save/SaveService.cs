using System;
using UnityEngine;

public class SaveService
{
    private const string PlayerProgressKey = "SaveData";

    public void Save(PlayerProgress progress, PlayerStatistics statistics, PlayerAchievements achievements)
    {
        SaveData saveData = new SaveData
        {
            Progress = progress.ToSaveData(),
            Statistics = statistics.ToSaveData(),
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

        progress.Restore(data.Progress);

        PlayerStatistics statistics = new PlayerStatistics();

        statistics.Restore(data.Statistics);

        PlayerAchievements achievements = new PlayerAchievements();
        achievements.Load(data.Achievements);

        return new LoadData(progress, statistics, achievements);
    }
}