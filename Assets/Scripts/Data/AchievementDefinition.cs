using System;

[Serializable]
public class AchievementDefinition
{
    public string Id;
    public string Name;
    public string Description;
    public AchievementType Type;
    public int Target;
    public int RewardCoins;
    public AchievementIcon Icon;
}