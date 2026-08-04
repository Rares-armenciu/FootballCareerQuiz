using System;
using TMPro;
using UnityEngine;

public class HeaderView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coinsText;
    [SerializeField] private TextMeshProUGUI livesText;
    [SerializeField] private TextMeshProUGUI levelText;

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
}