using UnityEngine;

public class AchievementIconProvider : MonoBehaviour
{
    [SerializeField] private static Sprite football;
    [SerializeField] private static Sprite trophy;
    [SerializeField] private static Sprite book;
    [SerializeField] private static Sprite fire;

    public static Sprite Get(AchievementIcon icon)
    {
        return icon switch
        {
            AchievementIcon.Football => football,
            AchievementIcon.Trophy => trophy,
            AchievementIcon.Book => book,
            AchievementIcon.Fire => fire,

            _ => football
        };
    }
}