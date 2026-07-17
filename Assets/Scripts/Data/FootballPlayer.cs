using System.Collections.Generic;

[System.Serializable]
public class FootballPlayer
{
    public string Name;

    public List<CareerClub> Clubs = new();

    public int Difficulty;
}