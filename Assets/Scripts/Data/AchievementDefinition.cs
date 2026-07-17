public class AchievementDefinition
{
    public string Id { get; }
    public string Name { get; }
    public string Description { get; }
    public AchievementType Type { get; }
    public int Target { get; }
    public int RewardCoins { get; }

    public AchievementDefinition(
        string id,
        string name,
        string description,
        AchievementType type,
        int target,
        int rewardCoins)
    {
        Id = id;
        Name = name;
        Description = description;
        Type = type;
        Target = target;
        RewardCoins = rewardCoins;
    }
}