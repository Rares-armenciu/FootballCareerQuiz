using UnityEngine;

[CreateAssetMenu(menuName = "Football Quiz/Achievement Icon Set")]
public class AchievementIconSet : ScriptableObject
{
    [SerializeField] private Sprite football;
    [SerializeField] private Sprite trophy;
    [SerializeField] private Sprite book;
    [SerializeField] private Sprite fire;
    [SerializeField] private Sprite coins;
    [SerializeField] private Sprite lightning;
    [SerializeField] private Sprite shield;

    public Sprite Get(AchievementIcon icon)
    {
        return icon switch
        {
            AchievementIcon.Football => football,
            AchievementIcon.Trophy => trophy,
            AchievementIcon.Book => book,
            AchievementIcon.Fire => fire,
            AchievementIcon.Coins => coins,
            AchievementIcon.Lightning => lightning,
            AchievementIcon.Shield => shield,

            _ => football
        };
    }
}
