using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class AchievementPopupView : MonoBehaviour
{
    [SerializeField] private TMP_Text _title;
    [SerializeField] private TMP_Text _achievementName;
    [SerializeField] private TMP_Text _reward;
    private Coroutine _hideRoutine;
    private Queue<AchievementDefinition> _achievementsQueue = new();
    private bool _isShowing;

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Enqueue(AchievementDefinition achievement)
    {
        _achievementsQueue.Enqueue(achievement);

        if (!_isShowing)
            ShowNext();
    }

    private void ShowNext()
    {
        if (_achievementsQueue.Count == 0)
        {
            _isShowing = false;
            gameObject.SetActive(false);
            return;
        }

        _isShowing = true;

        AchievementDefinition achievement = _achievementsQueue.Dequeue();

        gameObject.SetActive(true);

        _title.text = "Achievement Unlocked!";
        _achievementName.text = achievement.Name;
        _reward.text = $"+{achievement.RewardCoins} Coins";

        if (_hideRoutine != null)
        {
            StopCoroutine(_hideRoutine);
        }

        _hideRoutine = StartCoroutine(HideAfterSeconds());
    }

    private IEnumerator HideAfterSeconds()
    {
        yield return new WaitForSeconds(3f);

        gameObject.SetActive(false);

        ShowNext();
    }
}