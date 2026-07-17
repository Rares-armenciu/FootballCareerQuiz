using System.Collections.Generic;

public class QuizQuestion
{
    public FootballPlayer CorrectPlayer { get; set; }

    public List<FootballPlayer> Options { get; set; }

    public List<int> RevealedClubIndexes { get; } = new();

    public int CoinsReward { get; set; }

    public int LivesPenalty { get; set; }

    public int CorrectIndex { get; set; }

    public bool CanRevealClub =>
    RevealedClubIndexes.Count < CorrectPlayer.Clubs.Count;

    public bool RevealRandomClub()
    {
        List<int> unrevealedIndexes = new List<int>();
        for (int i = 0; i < CorrectPlayer.Clubs.Count; i++)
        {
            if (!RevealedClubIndexes.Contains(i))
            {
                unrevealedIndexes.Add(i);
            }
        }
        if (unrevealedIndexes.Count == 0)
        {
            return false;
        }

        int randomIndex = UnityEngine.Random.Range(0, unrevealedIndexes.Count);
        int clubIndexToReveal = unrevealedIndexes[randomIndex];
        RevealedClubIndexes.Add(clubIndexToReveal);
        
        return true;
    }
}