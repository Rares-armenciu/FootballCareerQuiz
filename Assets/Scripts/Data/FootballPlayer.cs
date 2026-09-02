using System.Collections.Generic;

[System.Serializable]
public class FootballPlayer
{
    public string Name;

    public string Nationality;

    public string Position;

    public int BirthYear;

    public bool Retired;

    public List<CareerClub> Clubs = new();

    public int Difficulty;
}