using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HintPanelView : MonoBehaviour
{
    [SerializeField] private Button spendCoinsButton;
    [SerializeField] private Button watchAdButton;
    [SerializeField] private Button backButton;
    [SerializeField] private TextMeshProUGUI spendCoinsText;

    public event Action SpendCoinsClicked;
    public event Action WatchAdClicked;

    private void Awake()
    {
        spendCoinsButton.onClick.AddListener(
            () => SpendCoinsClicked?.Invoke());

        watchAdButton.onClick.AddListener(
            () => WatchAdClicked?.Invoke());

        backButton.onClick.AddListener(Hide);
    }

    public void Show(int price, bool canAfford)
    {
        gameObject.SetActive(true);

        spendCoinsText.text = $"{price} COINS";
        spendCoinsButton.interactable = canAfford;
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}