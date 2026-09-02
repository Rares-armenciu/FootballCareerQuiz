using System.Collections.Generic;
using UnityEngine;

public class PlayerDatabase
{
    private const string ResourcePath = "Players";

    private readonly List<FootballPlayer> players =
        new List<FootballPlayer>();

    public IReadOnlyList<FootballPlayer> Players => players;

    public PlayerDatabase()
    {
        LoadPlayers();
    }

    private void LoadPlayers()
    {
        TextAsset jsonAsset = Resources.Load<TextAsset>(ResourcePath);

        if (jsonAsset == null)
        {
            Debug.LogError($"Players.json not found at Resources/{ResourcePath}.");
            return;
        }

        PlayerDatabaseData data =
            JsonUtility.FromJson<PlayerDatabaseData>(jsonAsset.text);

        if (data?.Players == null)
        {
            Debug.LogError("Players.json could not be parsed or contains no players.");
            return;
        }

        players.AddRange(data.Players);
    }

    public FootballPlayer GetRandomPlayer()
    {
        if (players.Count == 0)
        {
            Debug.LogError("PlayerDatabase has no players loaded.");
            return null;
        }

        return players[Random.Range(0, players.Count)];
    }

    public FootballPlayer GetPlayer(string name)
    {
        return players.Find(player => player.Name == name);
    }

    public int Count => players.Count;

    [System.Serializable]
    private class PlayerDatabaseData
    {
        public List<FootballPlayer> Players;
    }
}