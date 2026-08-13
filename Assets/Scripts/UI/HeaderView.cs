using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HeaderView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coinsText;
    [SerializeField] private TextMeshProUGUI livesText;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private Button dailyRewardButton;
    [SerializeField] private MainMenuController mainMenuController;

    private void Awake()
    {
        dailyRewardButton.onClick.AddListener(ShowDailyReward);
    }

    private void OnDestroy()
    {
        dailyRewardButton.onClick.RemoveListener(ShowDailyReward);
    }

    public void Show(PlayerProgress session)
    {
        coinsText.text = session.Coins.ToString();
        livesText.text = session.Lives.ToString();

        // Show the active level (replay override if set) so the header reflects the level being played.
        if (levelText != null)
        {
            int activeLevel = GameManager.Instance.ProgressionService.ActiveLevel;
            levelText.text = $"Level {activeLevel}";
        }
    }

    public void ShowDailyReward()
    {
        if(mainMenuController == null)
        {
            return;
        }

        mainMenuController.OpenDailyReward();
    }
}