using UnityEngine;
using UnityEngine.UI;

public class HintController : MonoBehaviour
{
    [SerializeField] private QuizController quizController;
    [SerializeField] private AdManager adManager;
    [SerializeField] private Button hintButton;
    [SerializeField] private HintPanelView hintPanel;

    private const int HintPrice = 100;

    private void Awake()
    {
        hintButton.onClick.AddListener(OpenHintPanel);

        hintPanel.SpendCoinsClicked += BuyHint;
        hintPanel.WatchAdClicked += WatchAdForHint;
    }

    private void OpenHintPanel()
    {
        bool canAfford = GameManager.Instance.CoinsService.CanAfford(HintPrice);

        hintPanel.Show(HintPrice, canAfford);
    }

    private void BuyHint()
    {
        if(!GameManager.Instance.CoinsService.CanAfford(HintPrice))
        {
            return;
        }

        GameManager.Instance.CoinsService.SpendCoins(HintPrice);
        quizController.RevealHint();
        hintPanel.Hide();
    }

    private void WatchAdForHint()
    {
        adManager.ShowRewardedAd(() =>
        {
            quizController.RevealHint();
            hintPanel.Hide();

            // Reward the player with a hint after watching the ad.
            // We'll connect this to your existing hint system.
        });

        // We'll connect this to AdManager.
    }
}