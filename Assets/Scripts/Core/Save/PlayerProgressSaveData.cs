using System;

[Serializable]
public class PlayerProgressSaveData
{
    public int Coins;
    public int Lives;
    public int CurrentLevel;
    public int CurrentQuestion;
    public string NextLifeTime;
    public int CorrectAnswersThisLevel;
    public int WrongAnswersThisLevel;
    public int HintsUsedThisLevel;
}