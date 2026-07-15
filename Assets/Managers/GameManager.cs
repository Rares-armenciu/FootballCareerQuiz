using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public int Coins { get; private set; } = 500;
    public int Lives { get; private set; } = 3;

    public PlayerDatabase PlayerDatabase { get; private set; }

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

        Debug.Log("GameManager created");
    }

    public bool SpendCoins(int amount)
    {
        if (Coins < amount)
            return false;

        Coins -= amount;
        return true;
    }

    public bool WinCoins(int amount)
    {
        if (amount < 0)
            return false;
        Coins += amount;
        return true;
    }

    public bool LoseLife()
    {
        if (Lives > 0)
        {
            Lives--;

            return true;
        }

        return false;
    }
}