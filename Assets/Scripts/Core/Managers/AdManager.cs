using System;
using UnityEngine;

public class AdManager : MonoBehaviour
{
    public static AdManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public bool IsRewardedAdAvailable()
    {
        // Later: ask LevelPlay/AdMob
        return true;
    }

    public void ShowRewardedAd(Action onRewarded)
    {
        ShowRewardedAd(onRewarded, null);
    }

    public void ShowRewardedAd(Action onRewarded, Action onFailed)
    {
        // TEMPORARY while developing:
        Debug.Log("Fake rewarded ad completed");
        onRewarded?.Invoke();

        // Later: wire onFailed to the ad SDK's failed-to-show/skipped/no-fill
        // callbacks so callers can restore UI state instead of hanging.
    }
}