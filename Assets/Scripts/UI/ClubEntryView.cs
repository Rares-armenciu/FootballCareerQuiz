using TMPro;
using UnityEngine;

public class ClubEntryView : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI clubName;

    [SerializeField]
    private TextMeshProUGUI clubPeriod;

    public void Show(CareerClub club, bool revealed)
    {
        clubName.text = revealed ? club.Name : "???";
        clubPeriod.text = revealed ? $"{club.StartYear} - {club.EndYear}" + (club.IsLoan ? " (Loan)" : "") : "";
    }
}
