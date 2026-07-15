public class PlayerProgress
{
    public int Coins { get; private set; } = 500;

    public int Lives { get; private set; } = 5;

    public int CurrentLevel { get; private set; } = 1;

    public void AddCoins(int amount)
    {
        Coins += amount;
    }

    public bool SpendCoins(int amount)
    {
        if (Coins < amount)
            return false;

        Coins -= amount;
        return true;
    }

    public void LoseLife()
    {
        Lives--;
    }
}