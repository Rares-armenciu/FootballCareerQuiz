using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class NoLivesView : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI coinsText;
    [SerializeField] private TextMeshProUGUI timeLeftText;

    private Coroutine countdownCoroutine;

    public void Show(PlayerProgress progress, LifeService lifeService)
    {
        levelText.text = $"Level {progress.CurrentLevel}";
        coinsText.text = $"{progress.Coins} Coins";

        panel.SetActive(true);

        if (countdownCoroutine != null)
        {
            StopCoroutine(countdownCoroutine);
        }

        countdownCoroutine = StartCoroutine(UpdateCountdown(lifeService));
    }

    public void Hide()
    {
        if (countdownCoroutine != null)
        {
            StopCoroutine(countdownCoroutine);
            countdownCoroutine = null;
        }

        panel.SetActive(false);
    }

    private IEnumerator UpdateCountdown(LifeService lifeService)
    {
        while (true)
        {
            TimeSpan remaining = lifeService.GetTimeUntilNextLife();

            timeLeftText.text =
                $"{remaining.Minutes:00}:{remaining.Seconds:00}";
            yield return new WaitForSeconds(1f);
        }
    }
}
