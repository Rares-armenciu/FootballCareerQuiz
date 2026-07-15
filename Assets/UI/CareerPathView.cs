using TMPro;
using UnityEngine;

public class CareerPathView : MonoBehaviour
{
    [SerializeField]
    private Transform container;

    [SerializeField]
    private ClubEntryView clubEntryPrefab;

    public void ShowQuestion(QuizQuestion question)
    {
        Debug.Log("ShowQuestion called");
        Debug.Log(question.CorrectPlayer.Clubs.Count);

        foreach (Transform child in container)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < question.CorrectPlayer.Clubs.Count; i++)
        {
            CareerClub club = question.CorrectPlayer.Clubs[i];

            Debug.Log("Creating entry for " + club.Name);
            
            ClubEntryView entry =
                Instantiate(clubEntryPrefab, container);

            bool revealed =
                question.RevealedClubIndexes.Contains(i);

            entry.Show(club, revealed);
        }
    }
}