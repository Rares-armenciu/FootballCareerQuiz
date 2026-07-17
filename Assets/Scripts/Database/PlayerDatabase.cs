using System.Collections.Generic;
using UnityEngine;

public class PlayerDatabase
{
    private readonly List<FootballPlayer> players =
        new List<FootballPlayer>();

    public IReadOnlyList<FootballPlayer> Players => players;

    public PlayerDatabase()
    {
        LoadPlayers();
    }

    private void LoadPlayers()
    {
        players.Add(new FootballPlayer
        {
            Name = "Cristiano Ronaldo",

            Clubs = new List<CareerClub>
            {
                new CareerClub { Name = "Sporting CP" },
                new CareerClub { Name = "Manchester United" },
                new CareerClub { Name = "Real Madrid" },
                new CareerClub { Name = "Juventus" },
                new CareerClub { Name = "Manchester United" },
                new CareerClub { Name = "Al Nassr" }
            }
        });

        players.Add(new FootballPlayer
        {
            Name = "Lionel Messi",

            Clubs = new List<CareerClub>
            {
                new CareerClub { Name = "Barcelona" },
                new CareerClub { Name = "Paris Saint-Germain" },
                new CareerClub { Name = "Inter Miami" }
            }
        });

        players.Add(new FootballPlayer
        {
            Name = "Erling Haaland",

            Clubs = new List<CareerClub>
            {
                new CareerClub { Name = "Bryne FK" },
                new CareerClub { Name = "Molde FK" },
                new CareerClub { Name = "RB Salzburg" },
                new CareerClub { Name = "Borussia Dortmund" },
                new CareerClub { Name = "Manchester City" }
            }
        });

        players.Add(new FootballPlayer
        {
            Name = "Kylian Mbappe",

            Clubs = new List<CareerClub>
            {
                new CareerClub { Name = "AS Monaco" },
                new CareerClub { Name = "Paris Saint-Germain" },
                new CareerClub { Name = "Real Madrid" }
            }
        });

        players.Add(new FootballPlayer
        {
            Name = "John Stones",

            Clubs = new List<CareerClub>
            {
                new CareerClub { Name = "Barnsley" },
                new CareerClub { Name = "Everton" },
                new CareerClub { Name = "Manchester City" },
            }
        });

        players.Add(new FootballPlayer
        {
            Name = "Jeremy Doku",

            Clubs = new List<CareerClub>
            {
                new CareerClub { Name = "RSC Anderlecht" },
                new CareerClub { Name = "Stade Rennais" },
                new CareerClub { Name = "Manchester City" },
            }
        });

        players.Add(new FootballPlayer
        {
            Name = "Karim Benzema",

            Clubs = new List<CareerClub>
            {
                new CareerClub { Name = "Lyon" },
                new CareerClub { Name = "Real Madrid" },
                new CareerClub { Name = "Al-Ittihad" },
                new CareerClub { Name = "Al-Hilal" }
            }
        });

        players.Add(new FootballPlayer
        {
            Name = "Alvaro Morata",

            Clubs = new List<CareerClub>
            {
                new CareerClub { Name = "Real Madrid" },
                new CareerClub { Name = "Chelsea" },
                new CareerClub { Name = "Atletico Madrid" },
                new CareerClub { Name = "Chelsea" },
                new CareerClub { Name = "Atletico Madrid" },
                new CareerClub { Name = "Juventus" },
                new CareerClub { Name = "AC Milan" },
                new CareerClub { Name = "Galatasaray" },
                new CareerClub { Name = "AC Milan" },
                new CareerClub { Name = "Como" },


            }
        });
    }

    public FootballPlayer GetRandomPlayer()
    {
        return players[Random.Range(0, players.Count)];
    }

    public FootballPlayer GetPlayer(string name)
    {
        return players.Find(player => player.Name == name);
    }

    public int Count => players.Count;
}