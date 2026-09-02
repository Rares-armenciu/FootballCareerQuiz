using System;
using System.Collections.Generic;
using System.Linq;

namespace PlayerDatabaseValidator;

public class PlayerDatabaseFile
{
    public List<FootballPlayer> Players { get; set; } = new();
}

public class ClubHistoryEntry
{
    public int Order { get; }

    public string? Name { get; }

    public int StartYear { get; }

    public int EndYear { get; }

    public bool IsLoan { get; }

    public string IsLoanDisplay => IsLoan ? "Yes" : "No";

    public ClubHistoryEntry(int order, CareerClub club)
    {
        Order = order;
        Name = club.Name;
        StartYear = club.StartYear;
        EndYear = club.EndYear;
        IsLoan = club.IsLoan;
    }
}

public class FootballPlayer
{
    public string? Name { get; set; }

    public string? Nationality { get; set; }

    public string? Position { get; set; }

    public int BirthYear { get; set; }

    public bool Retired { get; set; }

    public int Difficulty { get; set; }

    public List<CareerClub> Clubs { get; set; } = new();
}

public class CareerClub
{
    public string? Name { get; set; }

    public int StartYear { get; set; }

    public int EndYear { get; set; }

    public bool IsLoan { get; set; }
}

public class PlayerValidationResult
{
    private static readonly string[] ValidPositions = { "GK", "DF", "MF", "FW" };

    public FootballPlayer Player { get; }

    public List<string> Errors { get; } = new();

    public List<string> Warnings { get; } = new();

    public bool IsValid => Errors.Count == 0;

    public string Status => Errors.Count > 0
        ? "Error"
        : Warnings.Count > 0
            ? "Warning"
            : "OK";

    public string IssuesSummary =>
        string.Join("; ", Errors.Concat(Warnings));

    public PlayerValidationResult(FootballPlayer player, IEnumerable<FootballPlayer> allPlayers)
    {
        Player = player;

        if (string.IsNullOrWhiteSpace(player.Name))
            Errors.Add("Missing Name");

        if (string.IsNullOrWhiteSpace(player.Nationality))
            Errors.Add("Missing Nationality");

        if (string.IsNullOrWhiteSpace(player.Position))
        {
            Errors.Add("Missing Position");
        }
        else if (!ValidPositions.Contains(player.Position.Trim().ToUpperInvariant()))
        {
            Errors.Add($"Invalid Position '{player.Position}' (expected GK/DF/MF/FW)");
        }

        int currentYear = DateTime.UtcNow.Year;
        if (player.BirthYear < 1950 || player.BirthYear > currentYear)
            Errors.Add($"Implausible BirthYear ({player.BirthYear})");

        if (player.Clubs == null || player.Clubs.Count == 0)
        {
            Errors.Add("No clubs listed");
        }
        else
        {
            for (int i = 0; i < player.Clubs.Count; i++)
            {
                CareerClub club = player.Clubs[i];

                if (string.IsNullOrWhiteSpace(club.Name))
                    Errors.Add($"Club #{i + 1} missing Name");

                if (club.StartYear > club.EndYear)
                    Errors.Add($"Club '{club.Name}' has StartYear ({club.StartYear}) after EndYear ({club.EndYear})");
            }
        }

        if (!string.IsNullOrWhiteSpace(player.Name))
        {
            int duplicateCount = allPlayers.Count(p =>
                string.Equals(p.Name?.Trim(), player.Name.Trim(), StringComparison.OrdinalIgnoreCase));

            if (duplicateCount > 1)
                Warnings.Add("Duplicate player name");
        }
    }
}
