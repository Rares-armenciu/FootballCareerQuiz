using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class AchievementsView : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private Transform content;
    [SerializeField] private AchievementCardView cardPrefab;
    [SerializeField] private TMP_Text completionText;
    [SerializeField] private AchievementIconSet iconSet;

    public void Show(AchievementService achievementService, PlayerAchievements playerAchievements)
    {
        panel.SetActive(true);

        Populate(achievementService, playerAchievements);
    }

    public void Hide()
    {
        panel.SetActive(false);
    }

    private void Populate(AchievementService achievementService, PlayerAchievements playerAchievements)
    {
        foreach (Transform child in content)
            Destroy(child.gameObject);

        int unlockedCount = 0;
        int totalCount = 0;

        foreach (AchievementDefinition achievement in achievementService.GetAchievements())
        {
            AchievementCardView card =
                Instantiate(cardPrefab, content);

            card.Setup(
                title: achievement.Name,
                descriptionText: achievement.Description,
                rewardCoins: achievement.RewardCoins,
                currentProgress: achievementService.GetCurrentProgress(achievement),
                targetProgress: achievement.Target,
                icon: iconSet != null ? iconSet.Get(achievement.Icon) : null
            );

            totalCount++;

            if (playerAchievements.IsUnlocked(achievement.Id))
                unlockedCount++;
        }

        completionText.text = $"{unlockedCount}/{totalCount}";
    }
}