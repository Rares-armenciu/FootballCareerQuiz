using System;
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private LevelDatabase levelDatabase;

    [SerializeField]
    private AchievementDatabase achievementDatabase;

    public LevelDatabase LevelDatabase => levelDatabase;

    public AchievementDatabase AchievementDatabase => achievementDatabase;

    public static GameManager Instance { get; private set; }

    public PlayerDatabase PlayerDatabase { get; private set; }

    public PlayerProgress Progress { get; private set; }

    public PlayerStatistics Statistics { get; private set; }

    public PlayerAchievements Achievements { get; private set; }

    public LifeService LifeService { get; private set; }

    public CoinsService CoinsService { get; private set; }

    public ProgressionService ProgressionService { get; private set; }

    public SaveService SaveService { get; private set; }

    public StatisticsService StatisticsService { get; private set; }

    public AchievementService AchievementService { get; private set; }

    public DailyRewardService DailyRewardService { get; private set; }

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
        Progress = loadData.Progress;
        Statistics = loadData.Statistics;
        Achievements = loadData.Achievements;
        LifeService = new LifeService(Progress);
        CoinsService = new CoinsService(Progress);
        ProgressionService = new ProgressionService(Progress);
        AchievementService = new AchievementService(Achievements, Progress, Statistics, CoinsService, achievementDatabase);
        StatisticsService = new StatisticsService(Statistics, AchievementService);
        DailyRewardService = new DailyRewardService(
            CoinsService,
            loadData.DailyReward,
            StatisticsService);
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