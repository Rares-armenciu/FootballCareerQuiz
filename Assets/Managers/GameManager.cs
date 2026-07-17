using System;
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public PlayerDatabase PlayerDatabase { get; private set; }

    public PlayerProgress Progress { get; private set; }

    public LifeService LifeService { get; private set; }

    public CoinsService CoinsService { get; private set; }

    public ProgressionService ProgressionService { get; private set; }

    public SaveService SaveService { get; private set; }

    public PlayerStatistics Statistics { get; private set; }

    public StatisticsService StatisticsService { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);

        PlayerDatabase = new PlayerDatabase();
        SaveService = new SaveService();
        var loadData = SaveService.Load();
        Progress = loadData.Item1;
        Statistics = loadData.Item2;
        LifeService = new LifeService(Progress);
        CoinsService = new CoinsService(Progress);
        ProgressionService = new ProgressionService(Progress);
        StatisticsService = new StatisticsService(Statistics);
        StartCoroutine(RefreshLoop());
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        if (hasFocus)
        {
            LifeService.RefreshLives();
        }
    }

    private IEnumerator RefreshLoop()
    {
        while (true)
        {
            LifeService.RefreshLives();

            yield return new WaitForSeconds(1f);
        }
    }
}