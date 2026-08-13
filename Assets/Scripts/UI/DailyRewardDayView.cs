using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DailyRewardDayView : MonoBehaviour
{
    [SerializeField] private TMP_Text dayText;
    [SerializeField] private TMP_Text rewardText;

    [SerializeField] private GameObject lockedOverlay;
    [SerializeField] private GameObject lockIcon;
    [SerializeField] private GameObject currentState;
    [SerializeField] private GameObject claimedState;

    [Header("Background")]
    [SerializeField] private Image background;
    [SerializeField] private Sprite normalSprite;
    [SerializeField] private Sprite currentSprite;
    [SerializeField] private Sprite disabledSprite;


    [Header("Claim Celebration")]
    [SerializeField] private RectTransform coinIcon;

    public void Show(
        int day,
        int reward,
        bool isClaimed,
        bool isCurrent)
    {
        dayText.text = $"DAY {day}";
        rewardText.text = reward.ToString("N0");

        background.sprite = isCurrent ? currentSprite : (isClaimed ? normalSprite : disabledSprite);
        bool isLocked = !isClaimed && !isCurrent;

        lockedOverlay.SetActive(isLocked);
        lockIcon.SetActive(isLocked);

        currentState.SetActive(isCurrent);
        claimedState.SetActive(isClaimed);
    }

    public void PlayClaimCelebration()
    {
        if (coinIcon == null)
            return;

        StartCoroutine(CelebrateCoroutine());
    }

    private IEnumerator CelebrateCoroutine()
    {
        Vector3 original = transform.localScale;
        Vector3 target = original * 1.1f;

        Quaternion originalCoinRotation = coinIcon.localRotation;
        coinIcon.localRotation = Quaternion.Euler(0, 0, 12);

        float t = 0f;

        while (t < 0.15f)
        {
            t += Time.deltaTime;

            transform.localScale = Vector3.Lerp(original, target, t / 0.15f);

            yield return null;
        }

        t = 0f;

        while (t < 0.15f)
        {
            t += Time.deltaTime;

            transform.localScale = Vector3.Lerp(target, original, t / 0.15f);

            yield return null;
        }

        transform.localScale = original;
        coinIcon.localRotation = originalCoinRotation;
    }
}
