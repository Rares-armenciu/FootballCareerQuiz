using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public PlayerDatabase PlayerDatabase { get; private set; }

    public PlayerProgress Progress { get; private set; }

    public LifeService LifeService { get; private set; }

    public CoinsService CoinsService { get; private set; }

    public ProgressionService ProgressionService { get; private set; }

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
        Progress = new PlayerProgress();
        LifeService = new LifeService(Progress);
        CoinsService = new CoinsService(Progress);
        ProgressionService = new ProgressionService(Progress);
    }
}