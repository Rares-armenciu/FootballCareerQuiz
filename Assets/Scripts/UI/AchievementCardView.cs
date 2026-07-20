using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class AchievementCardView : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text descriptionText;

    [SerializeField] private Image rewardIcon;
    [SerializeField] private TMP_Text rewardText;
    [SerializeField] private TMP_Text progressText;

    public void Show(
        string name,
        string description,
        int reward,
        int current,
        int target)
    {
        nameText.text = name;
        descriptionText.text = description;
        rewardText.text = reward.ToString();
        progressText.text = $"{current} / {target}";
    }

    public void Show(AchievementDefinition achievement, PlayerAchievements playerAchievements, int achievementProgress)
    {
        nameText.text = achievement.Name;
        descriptionText.text = achievement.Description;
        rewardText.text = achievement.RewardCoins.ToString();

        progressText.text = playerAchievements.IsUnlocked(achievement.Id)
            ? "Completed"
            : $"{achievementProgress} / {achievement.Target}";

        // We'll assign the correct sprite later.
        // icon.sprite = ...
        // rewardIcon.sprite = ...
    }
}