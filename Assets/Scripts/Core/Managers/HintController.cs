using UnityEngine;
using UnityEngine.UI;

public class HintController : MonoBehaviour
{
    [SerializeField] private QuizController quizController;
    [SerializeField] private AdManager adManager;
    [SerializeField] private Button hintButton;
    [SerializeField] private HintPanelView hintPanel;

    private const int OriginalHintPrice = 100;
    private const int PriceIncreasePerHint = 50;
    private int CurrentHintPrice;

    private void Start()
    {
        hintPanel.Hide();
        CurrentHintPrice = OriginalHintPrice;
        quizController.UIRefreshed += () => RefreshButton();
        quizController.NewQuestionShown += () => CurrentHintPrice = OriginalHintPrice;
    }
    private void Awake()
    {
        hintButton.onClick.AddListener(OpenHintPanel);

        hintPanel.SpendCoinsClicked += BuyHint;
        hintPanel.WatchAdClicked += WatchAdForHint;
    }

    private void OpenHintPanel()
    {
        bool canAfford = GameManager.Instance.CoinsService.CanAfford(CurrentHintPrice);

        hintPanel.Show(CurrentHintPrice, canAfford);
    }

    private void BuyHint()
    {
        if(!GameManager.Instance.CoinsService.CanAfford(CurrentHintPrice))
        {
            return;
        }

        GameManager.Instance.CoinsService.SpendCoins(CurrentHintPrice);
        CurrentHintPrice += PriceIncreasePerHint;
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

    private void RefreshButton()
    {
        hintButton.interactable = GameManager.Instance.CoinsService.CanAfford(CurrentHintPrice) || adManager.IsRewardedAdAvailable();
    }
}