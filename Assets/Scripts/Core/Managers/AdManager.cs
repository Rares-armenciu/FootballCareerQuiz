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
        // TEMPORARY while developing:
        Debug.Log("Fake rewarded ad completed");
        onRewarded?.Invoke();
    }
}