using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AnswerButtonView : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI label;
    
    [SerializeField] 
    private Button button;
    
    [SerializeField] 
    private Image background;

    public event Action<AnswerButtonView> Clicked;
    private FootballPlayer player;

    public void Show(FootballPlayer footballPlayer)
    {
        player = footballPlayer;
        label.text = footballPlayer.Name;
    }

    private void Awake()
    {
        button.onClick.AddListener(OnClicked);
    }

    private void OnClicked()
    {
        Clicked?.Invoke(this);
    }

    public FootballPlayer Player => player;

    public void SetCorrect()
    {
        background.color = Color.green;
    }

    public void SetWrong()
    {
        background.color = Color.red;
    }

    public void ResetColor()
    {
        background.color = Color.white;
    }

    public void SetInteractable(bool interactable)
    {
        button.interactable = interactable;
    }
}
