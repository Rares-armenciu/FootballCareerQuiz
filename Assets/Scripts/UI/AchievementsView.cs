using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class AchievementsView : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private Transform content;
    [SerializeField] private AchievementCardView cardPrefab;

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

        foreach (AchievementDefinition achievement in achievementService.GetAchievements())
        {
            AchievementCardView card =
                Instantiate(cardPrefab, content);

            card.Setup(
                title: achievement.Name,
                descriptionText: achievement.Description,
                rewardCoins: achievement.RewardCoins,
                currentProgress: achievementService.GetCurrentProgress(achievement),
                targetProgress: achievement.Target
            );
        }
    }
}