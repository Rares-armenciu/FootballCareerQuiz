using TMPro;
using UnityEngine;

public class ClubEntryView : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI clubName;

    public void Show(CareerClub club, bool revealed)
    {
        clubName.fontSize = 60;
        clubName.text = revealed ? club.Name : "???";
    }
}
