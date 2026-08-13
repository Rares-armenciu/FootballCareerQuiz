using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DailyRewardView : MonoBehaviour
{
    [Header("Reward Cards")]
    [SerializeField] private DailyRewardDayView rewardDayPrefab;
    [SerializeField] private Transform rewardGrid;
    [SerializeField] private GameObject gridSpacerPrefab;
    [SerializeField] private TMP_Text claimButtonText;

    [Header("UI")]
    [SerializeField] private MainMenuController mainMenuController;
    [SerializeField] private Button claimButton;
    [SerializeField] private Button closeButton;
    [SerializeField] private GameObject buttonsContainer;
    [SerializeField] private TMP_Text rewardLabelText;
    [SerializeField] private TMP_Text rewardText;
    [SerializeField] private TMP_Text statusText;

    [Header("Ad Bonus")]
    [SerializeField] private AdManager adManager;
    [SerializeField] private Button watchAdButton;
    [SerializeField] private TMP_Text watchAdButtonText;

    [Header("Claim Celebration")]
    [SerializeField] private RectTransform rewardCoinIcon;

    private readonly List<DailyRewardDayView> rewardDays = new();

    private void Awake()
    {
        claimButton.onClick.AddListener(ClaimReward);
        closeButton.onClick.AddListener(Hide);
        watchAdButton.onClick.AddListener(WatchAdForBonus);
    }

    private void OnDestroy()
    {
        claimButton.onClick.RemoveListener(ClaimReward);
        closeButton.onClick.RemoveListener(Hide);
        watchAdButton.onClick.RemoveListener(WatchAdForBonus);
    }

    private void OnEnable()
    {
        Refresh();
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Refresh()
    {
        RefreshRewardDays();

        bool canClaim = mainMenuController.CanClaimDailyReward();
        rewardLabelText.text = canClaim ? "TODAY'S REWARD" : "TOMORROW'S REWARD";
        rewardText.text = $"{mainMenuController.GetNextDailyReward():N0}";
        statusText.text = canClaim ? "Ready to claim" : "Come back tomorrow";
        claimButtonText.text = canClaim ? "CLAIM" : "CLAIMED";
        claimButton.interactable = canClaim;

        if (buttonsContainer != null)
            buttonsContainer.SetActive(canClaim);

        RefreshAdBonusButton();
    }

    private void RefreshAdBonusButton()
    {
        bool canClaimAdBonus = mainMenuController.CanClaimDailyRewardAdBonus();
        bool adAvailable = adManager != null && adManager.IsRewardedAdAvailable();

        // Keep the button visible at all times; its sprite swap (via the
        // Button component's disabled transition) communicates availability
        // instead of hiding/showing the object.
        watchAdButton.interactable = canClaimAdBonus && adAvailable;
        watchAdButtonText.text = $"WATCH AD: {mainMenuController.GetNextDailyReward() * 2:N0}";
    }

    private void RefreshRewardDays()
    {
        foreach (Transform child in rewardGrid)
        {
            Destroy(child.gameObject);
        }

        rewardDays.Clear();

        var service = GameManager.Instance.DailyRewardService;

        int currentStreak = service.GetDisplayStreak();
        bool canClaim = service.CanClaim();
        int currentDay = canClaim
            ? Mathf.Clamp(currentStreak + 1, 1, 7)
            : 0;

        for (int day = 1; day <= 7; day++)
        {
            if (day == 7)
                Instantiate(gridSpacerPrefab, rewardGrid);

            DailyRewardDayView view =
                Instantiate(rewardDayPrefab, rewardGrid);

            int reward = service.GetRewardForDay(day);

            bool isClaimed =
                day <= currentStreak;

            bool isCurrent =
                day == currentDay;

            view.Show(
                day,
                reward,
                isClaimed,
                isCurrent);

            rewardDays.Add(view);
        }

        Instantiate(gridSpacerPrefab, rewardGrid);
    }

    private void ClaimReward()
    {
        DailyRewardClaim claim = mainMenuController.ClaimDailyReward();

        if (claim == null)
            return;

        Refresh();

        int claimedIndex = claim.RewardDay - 1;

        if (claimedIndex >= 0 && claimedIndex < rewardDays.Count)
        {
            rewardDays[claimedIndex].PlayClaimCelebration();
        }

        if (rewardCoinIcon != null)
        {
            StartCoroutine(CelebrateRewardCoroutine());
        }
    }

    private void WatchAdForBonus()
    {
        if (!mainMenuController.CanClaimDailyRewardAdBonus())
            return;

        if (adManager == null || !adManager.IsRewardedAdAvailable())
            return;

        watchAdButton.interactable = false;
        claimButton.interactable = false;

        adManager.ShowRewardedAd(
            onRewarded: () =>
            {
                DailyRewardClaim bonusClaim = mainMenuController.ClaimDailyRewardAdBonus();

                if (bonusClaim == null)
                {
                    Refresh();
                    return;
                }

                Refresh();

                int claimedIndex = bonusClaim.RewardDay - 1;

                if (claimedIndex >= 0 && claimedIndex < rewardDays.Count)
                {
                    rewardDays[claimedIndex].PlayClaimCelebration();
                }

                if (rewardCoinIcon != null)
                {
                    StartCoroutine(CelebrateRewardCoroutine());
                }
            },
            onFailed: () =>
            {
                // Ad failed, was skipped, or was cancelled before completion.
                // No reward was granted, so just restore the buttons to their
                // correct state instead of leaving them disabled.
                Refresh();
            });
    }

    private IEnumerator CelebrateRewardCoroutine()
    {
        Vector3 original = rewardCoinIcon.localScale;
        Vector3 target = original * 1.2f;

        Quaternion originalRotation = rewardCoinIcon.localRotation;
        rewardCoinIcon.localRotation = Quaternion.Euler(0, 0, 12);

        float t = 0f;

        while (t < 0.15f)
        {
            t += Time.deltaTime;

            rewardCoinIcon.localScale = Vector3.Lerp(original, target, t / 0.15f);

            yield return null;
        }

        t = 0f;

        while (t < 0.15f)
        {
            t += Time.deltaTime;

            rewardCoinIcon.localScale = Vector3.Lerp(target, original, t / 0.15f);

            yield return null;
        }

        rewardCoinIcon.localScale = original;
        rewardCoinIcon.localRotation = originalRotation;
    }
}
