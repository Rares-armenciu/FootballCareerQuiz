using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class AchievementPopupView : MonoBehaviour
{
    [SerializeField] private TMP_Text _achievementName;
    [SerializeField] private TMP_Text _description;
    [SerializeField] private TMP_Text _reward;

    [SerializeField] private RectTransform _card;
    [SerializeField] private CanvasGroup _canvasGroup;

    [SerializeField] private float _showDuration = .3f;
    [SerializeField] private float _hideDuration = .3f;
    [SerializeField] private float _visibleTime = 2.5f;

    [SerializeField] private float _hiddenY = 120f;  //_card.rect.height + 20f
    [SerializeField] private float _visibleY = -80f;

    private Coroutine _hideRoutine;
    private Queue<AchievementDefinition> _achievementsQueue = new();
    private bool _isShowing;

    private void Awake()
    {
        _canvasGroup.alpha = 0;

        Vector2 pos = _card.anchoredPosition;
        pos.y = _hiddenY;
        _card.anchoredPosition = pos;

        gameObject.SetActive(false);
    }

    private void Display(AchievementDefinition achievement)
    {
        _achievementName.text = achievement.Name;
        _description.text = achievement.Description;
        _reward.text = $"+{achievement.RewardCoins} Coins";
    }

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

        Display(achievement);

        gameObject.SetActive(true);

        if (_hideRoutine != null)
        {
            StopCoroutine(_hideRoutine);
        }

        _hideRoutine = StartCoroutine(ShowRoutine());
    }

    private IEnumerator HideAfterSeconds()
    {
        yield return new WaitForSeconds(3f);

        gameObject.SetActive(false);

        ShowNext();
    }

    private IEnumerator ShowRoutine()
    {
        yield return Animate(
            _hiddenY,
            _visibleY,
            0,
            1,
            _showDuration);

        yield return new WaitForSeconds(_visibleTime);

        yield return Animate(
            _visibleY,
            _hiddenY,
            1,
            0,
            _hideDuration);

        ShowNext();
    }

    private IEnumerator Animate(
    float startY,
    float endY,
    float startAlpha,
    float endAlpha,
    float duration)
    {
        float elapsed = 0;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            float t = Mathf.Clamp01(elapsed / duration);

            float y = Mathf.Lerp(startY, endY, t);
            float alpha = Mathf.Lerp(startAlpha, endAlpha, t);

            Vector2 pos = _card.anchoredPosition;
            pos.y = y;
            _card.anchoredPosition = pos;

            _canvasGroup.alpha = alpha;

            yield return null;
        }

        Vector2 finalPos = _card.anchoredPosition;
        finalPos.y = endY;
        _card.anchoredPosition = finalPos;

        _canvasGroup.alpha = endAlpha;
    }
}