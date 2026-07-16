using System;
using UnityEngine;

public class SaveService
{
    private const string PlayerProgressKey = "PlayerProgress";

    public void Save(PlayerProgress progress)
    {
        PlayerProgressSaveData data =
            new PlayerProgressSaveData
            {
                Coins = progress.Coins,
                Lives = progress.Lives,
                CurrentLevel = progress.CurrentLevel,
                NextLifeTime = progress.NextLifeTime.ToString("O")
            };

        string json = JsonUtility.ToJson(data);

        PlayerPrefs.SetString(PlayerProgressKey, json);
        PlayerPrefs.Save();
    }

    public PlayerProgress Load()
    {
        if (!PlayerPrefs.HasKey(PlayerProgressKey))
        {
            return new PlayerProgress();
        }

        string json = PlayerPrefs.GetString(PlayerProgressKey);

        PlayerProgressSaveData data =
            JsonUtility.FromJson<PlayerProgressSaveData>(json);

        PlayerProgress progress = new PlayerProgress();

        progress.Restore(
            data.Coins,
            data.Lives,
            data.CurrentLevel,
            DateTime.Parse(data.NextLifeTime));

        return progress;
    }
}