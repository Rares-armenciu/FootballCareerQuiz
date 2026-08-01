using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class AchievementCardView : MonoBehaviour
{
    [Header("Text")]
    [SerializeField] private TMP_Text achievementName;
    [SerializeField] private TMP_Text description;
    [SerializeField] private TMP_Text rewardText;
    [SerializeField] private TMP_Text statusText;

    [Header("Progress")]
    [SerializeField] private Image progressFill;

    [Header("Icons")]
    [SerializeField] private Image achievementIcon;
    [SerializeField] private Image rewardIcon;

    public void Setup(
            string title,
            string descriptionText,
            int rewardCoins,
            int currentProgress,
            int targetProgress,
            Sprite icon = null)
    {
        achievementName.text = title;
        description.text = descriptionText;
        rewardText.text = rewardCoins + " Coins";

        currentProgress = Mathf.Clamp(currentProgress, 0, targetProgress);

        if (targetProgress > 0)
        {
            float progress = (float)currentProgress / targetProgress;
            progressFill.fillAmount = progress;

            if(currentProgress >= targetProgress)
            {
                statusText.text = "Completed!";
            }
            else
            {
                statusText.text = currentProgress + " / " + targetProgress;
            }
        }
        else
        {
            progressFill.fillAmount = 0f;
            statusText.text = "";
        }

        if (icon != null)
            achievementIcon.sprite = icon;
    }
}