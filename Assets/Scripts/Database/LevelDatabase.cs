using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Football Quiz/Level Database")]
public class LevelDatabase : ScriptableObject
{
    [SerializeField]
    private List<LevelDefinition> levels = new();

    public LevelDefinition Get(int level)
    {
        return levels.Find(l => l.Level == level);
    }

    public IReadOnlyList<LevelDefinition> Levels => levels;
}