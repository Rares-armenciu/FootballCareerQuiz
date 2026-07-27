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
                new CareerClub { Name = "Sporting CP", StartYear = 2002, EndYear = 2003 },
                new CareerClub { Name = "Manchester United", StartYear = 2003, EndYear = 2009 },
                new CareerClub { Name = "Real Madrid", StartYear = 2009, EndYear = 2014 },
                new CareerClub { Name = "Juventus", StartYear = 2014, EndYear = 2017 },
                new CareerClub { Name = "Manchester United", StartYear = 2017, EndYear = 2018 },
                new CareerClub { Name = "Al Nassr", StartYear = 2018, EndYear = 2023 }
            }
        });

        players.Add(new FootballPlayer
        {
            Name = "Lionel Messi",

            Clubs = new List<CareerClub>
            {
                new CareerClub { Name = "Barcelona", StartYear = 2003, EndYear = 2021 },
                new CareerClub { Name = "Paris Saint-Germain", StartYear = 2021, EndYear = 2023 },
                new CareerClub { Name = "Inter Miami", StartYear = 2023, EndYear = 2023 }
            }
        });

        players.Add(new FootballPlayer
        {
            Name = "Erling Haaland",

            Clubs = new List<CareerClub>
            {
                new CareerClub { Name = "Bryne FK", StartYear = 2017, EndYear = 2019 },
                new CareerClub { Name = "Molde FK", StartYear = 2019, EndYear = 2020 },
                new CareerClub { Name = "RB Salzburg", StartYear = 2020, EndYear = 2021 },
                new CareerClub { Name = "Borussia Dortmund", StartYear = 2021, EndYear = 2022 },
                new CareerClub { Name = "Manchester City", StartYear = 2022, EndYear = 2023 }
            }
        });

        players.Add(new FootballPlayer
        {
            Name = "Kylian Mbappe",

            Clubs = new List<CareerClub>
            {
                new CareerClub { Name = "AS Monaco", StartYear = 2015, EndYear = 2017 },
                new CareerClub { Name = "Paris Saint-Germain", StartYear = 2017, EndYear = 2023 },
                new CareerClub { Name = "Real Madrid", StartYear = 2023, EndYear = 2023 }
            }
        });

        players.Add(new FootballPlayer
        {
            Name = "John Stones",

            Clubs = new List<CareerClub>
            {
                new CareerClub { Name = "Barnsley", StartYear = 2011, EndYear = 2013 },
                new CareerClub { Name = "Everton", StartYear = 2013, EndYear = 2015 },
                new CareerClub { Name = "Manchester City", StartYear = 2015, EndYear = 2023 },
            }
        });

        players.Add(new FootballPlayer
        {
            Name = "Jeremy Doku",

            Clubs = new List<CareerClub>
            {
                new CareerClub { Name = "RSC Anderlecht", StartYear = 2015, EndYear = 2017 },
                new CareerClub { Name = "Stade Rennais", StartYear = 2017, EndYear = 2023 },
                new CareerClub { Name = "Manchester City", StartYear = 2023, EndYear = 2023 },
            }
        });

        players.Add(new FootballPlayer
        {
            Name = "Karim Benzema",

            Clubs = new List<CareerClub>
            {
                new CareerClub { Name = "Lyon", StartYear = 2002, EndYear = 2003 },
                new CareerClub { Name = "Real Madrid", StartYear = 2003, EndYear = 2014 },
                new CareerClub { Name = "Al-Ittihad", StartYear = 2014, EndYear = 2017 },
                new CareerClub { Name = "Al-Hilal", StartYear = 2017, EndYear = 2023 }
            }
        });

        players.Add(new FootballPlayer
        {
            Name = "Alvaro Morata",

            Clubs = new List<CareerClub>
            {
                new CareerClub { Name = "Real Madrid", StartYear = 2012, EndYear = 2014 },
                new CareerClub { Name = "Chelsea", StartYear = 2014, EndYear = 2015 },
                new CareerClub { Name = "Atletico Madrid", StartYear = 2015, EndYear = 2016, IsLoan = true },
                new CareerClub { Name = "Chelsea", StartYear = 2016, EndYear = 2017 },
                new CareerClub { Name = "Atletico Madrid", StartYear = 2017, EndYear = 2018, IsLoan = true },
                new CareerClub { Name = "Juventus", StartYear = 2018, EndYear = 2019 },
                new CareerClub { Name = "AC Milan", StartYear = 2019, EndYear = 2020 },
                new CareerClub { Name = "Galatasaray", StartYear = 2020, EndYear = 2021 },
                new CareerClub { Name = "AC Milan", StartYear = 2021, EndYear = 2022 },
                new CareerClub { Name = "Como", StartYear = 2022, EndYear = 2023 },


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