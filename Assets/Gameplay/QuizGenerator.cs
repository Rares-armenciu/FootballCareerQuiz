using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class QuizGenerator
{
    private readonly PlayerDatabase database;

    public QuizGenerator(PlayerDatabase database)
    {
        this.database = database;
    }

    public QuizQuestion Generate()
    {
        FootballPlayer correct =
            database.GetRandomPlayer();

        List<FootballPlayer> options = GenerateAnswerOptions(correct);

        var question = new QuizQuestion
        {
            CorrectPlayer = correct,
            Options = options,
            CoinsReward = 25,
            LivesPenalty = 1,
            CorrectIndex = options.IndexOf(correct),
        };

        RevealInitialClubs(question);

        return question;
    }

    private List<FootballPlayer> GenerateAnswerOptions(FootballPlayer correct)
    {
        List<FootballPlayer> options =
            new List<FootballPlayer>();
        options.Add(correct);
        while (options.Count < 4)
        {
            FootballPlayer random =
                database.GetRandomPlayer();
            if (!options.Contains(random))
            {
                options.Add(random);
            }
        }
        return options.OrderBy(x => Random.value).ToList();
    }

    private void RevealInitialClubs(QuizQuestion question)
    {
        int clubsToReveal = 2;

        if (question.CorrectPlayer.Clubs.Count > 8)
            clubsToReveal = 3;

        while (question.RevealedClubIndexes.Count < clubsToReveal)
        {
            question.RevealRandomClub();
        }
    }
}