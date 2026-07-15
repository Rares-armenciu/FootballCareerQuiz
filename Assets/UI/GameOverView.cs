using TMPro;
using UnityEngine;

public class GameOverView : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI coinsText;

    public void Show(PlayerProgress progress)
    {
        levelText.text = $"Level {progress.CurrentLevel}";
        coinsText.text = $"{progress.Coins} Coins";

        panel.SetActive(true);
    }

    public void Hide()
    {
        panel.SetActive(false);
    }
}
