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

        // Temporary
        if(levelText != null)
        {
            levelText.text = "Level 1";
        }
    }
}