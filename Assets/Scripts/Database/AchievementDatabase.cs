using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Football Quiz/Achievement Database")]
public class AchievementDatabase : ScriptableObject
{
    [SerializeField]
    private List<AchievementDefinition> achievements = new();

    public IReadOnlyList<AchievementDefinition> AllAchievements => achievements;

    public AchievementDefinition Get(string id)
    {
        AchievementDefinition definition =
            achievements.FirstOrDefault(a => a.Id == id);

        if (definition == null)
            Debug.LogError($"Achievement '{id}' not found in AchievementDatabase.");

        return definition;
    }
}
