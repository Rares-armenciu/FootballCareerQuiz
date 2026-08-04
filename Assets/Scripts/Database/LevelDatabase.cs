using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Football Quiz/Level Database")]
public class LevelDatabase : ScriptableObject
{
    [SerializeField]
    private List<LevelDefinition> levels = new();

    public LevelDefinition Get(int level)
    {
        LevelDefinition definition =
            levels.Find(l => l.Level == level);

        if (definition == null)
            Debug.LogError($"Level {level} not found in LevelDatabase.");

        return definition;
    }

    public IReadOnlyList<LevelDefinition> AllLevels => levels;

    public int LevelCount => levels.Count;

    public int BossLevelCount => levels.Count(l => l.IsBossLevel);
}