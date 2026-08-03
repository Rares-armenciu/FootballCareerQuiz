using Assets.Scripts.Data;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelEntryView : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private TMP_Text subtitleText;
    [SerializeField] private Image lockIcon;
    [SerializeField] private Image chevronIcon;
    [SerializeField] private GameObject currentBadge;
    [SerializeField] private Image[] stars;

    [SerializeField] private Image background;
    [SerializeField] private Sprite normalSprite;
    [SerializeField] private Sprite currentSprite;
    [SerializeField] private Sprite disabledSprite;

    public event Action<int> Clicked;

    private int level;

    private void Awake()
    {
        button.onClick.AddListener(OnClicked);
    }

    public void Show(LevelInfo data)
    {
        Debug.Log($"Level {data.Level}  Stars: {data.BestStars}  Unlocked: {data.IsUnlocked}");

        SetBackground(data);
        level = data.Level;
        levelText.text = $"Level {data.Level}";
        currentBadge.SetActive(data.IsCurrent);
        chevronIcon.gameObject.SetActive(data.IsUnlocked && !data.IsCurrent);
        lockIcon.gameObject.SetActive(!data.IsUnlocked);
        button.interactable = data.IsUnlocked && !data.IsCurrent;
        SetSubtitle(data);

        for (int i = 0; i < stars.Length; i++)
        {
            stars[i].gameObject.SetActive(i < data.BestStars);
        }
    }

    private void OnClicked()
    {
        Clicked?.Invoke(level);
    }

    private void SetBackground(LevelInfo data)
    {
        if(data.IsCurrent)
        {
            background.sprite = currentSprite;
        }
        else if (data.IsUnlocked)
        {
            background.sprite = normalSprite;
        }
        else
        {
            background.sprite = disabledSprite;
        }
    }

    private void SetSubtitle(LevelInfo data)
    {
        int currentQuestion = GameManager.Instance.ProgressionService.CurrentQuestion;

        if (data.IsCompleted)
        {
            subtitleText.text =
                $"{data.BestCorrectAnswers}/{data.QuestionCount} Correct";
        }
        else if (data.IsCurrent)
        {
            if (currentQuestion == 0)
                subtitleText.text = "Ready to Play";
            else
                subtitleText.text =
                    $"Question {currentQuestion + 1}/{data.QuestionCount}";
        }
        else
        {
            subtitleText.text =
                $"Complete Level {data.Level - 1}";
        }
    }
}