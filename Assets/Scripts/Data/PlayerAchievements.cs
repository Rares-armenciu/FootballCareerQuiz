using System.Collections.Generic;
using System.Linq;

public class PlayerAchievements
{
    private readonly HashSet<string> _unlocked = new();

    public bool IsUnlocked(string id)
    {
        return _unlocked.Contains(id);
    }

    public bool Unlock(string id)
    {
        return _unlocked.Add(id);
    }

    public IReadOnlyCollection<string> UnlockedAchievements => _unlocked;

    public PlayerAchievementsSaveData ToSaveData()
    {
        return new PlayerAchievementsSaveData
        {
            UnlockedAchievements = _unlocked.ToList()
        };
    }

    public void Load(PlayerAchievementsSaveData data)
    {
        _unlocked.Clear();

        if (data == null)
            return;

        foreach (var id in data.UnlockedAchievements)
            _unlocked.Add(id);
    }
}