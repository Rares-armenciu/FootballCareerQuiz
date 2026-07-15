using TMPro;
using UnityEngine;

public class HeaderView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coinsText;
    [SerializeField] private TextMeshProUGUI livesText;
    [SerializeField] private TextMeshProUGUI levelText;

    public void Show(PlayerProgress session)
    {
        coinsText.text = GameManager.Instance.Progress.Coins.ToString();
        livesText.text = GameManager.Instance.Progress.Lives.ToString();

        // Temporary
        levelText.text = "Level 1";
    }
}