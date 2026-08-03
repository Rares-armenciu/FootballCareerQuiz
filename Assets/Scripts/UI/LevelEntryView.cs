using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelEntryView : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private TMP_Text completedText;
    [SerializeField] private Image lockIcon;

    [SerializeField] private Image[] stars;

    public event Action<int> Clicked;

    private int level;

    private void Awake()
    {
        button.onClick.AddListener(OnClicked);
    }

    public void Show(LevelProgress progress)
    {
        Debug.Log(
    $"Level {progress.Level}  Stars: {progress.BestStars}  Unlocked: {progress.IsUnlocked}");
        level = progress.Level;

        levelText.text = $"Level {progress.Level + 1}";

        //button.interactable = progress.IsUnlocked;
        levelText.gameObject.SetActive(progress.IsUnlocked);
        completedText.gameObject.SetActive(progress.IsUnlocked);
        completedText.text = $"{progress.CorrectAnswers} / {progress.TotalQuestions} Correct";

        for (int i = 0; i < stars.Length; i++)
        {
            stars[i].gameObject.SetActive(i < progress.BestStars);
        }
    }

    private void OnClicked()
    {
        Clicked?.Invoke(level);
    }
}