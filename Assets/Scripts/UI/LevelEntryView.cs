using Assets.Scripts.Data;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.AdaptivePerformance.Provider.AdaptivePerformanceSubsystemDescriptor;

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
    public bool IsCurrent { get; private set; }

    private int level;

    private void Awake()
    {
        button.onClick.AddListener(OnClicked);
    }

    public void Show(LevelInfo levelInfo)
    {
        Debug.Log($"Level {levelInfo.Level}  Stars: {levelInfo.BestStars}  Unlocked: {levelInfo.IsUnlocked}");

        SetBackground(levelInfo);
        level = levelInfo.Level;
        levelText.text = $"Level {levelInfo.Level}";
        currentBadge.SetActive(levelInfo.IsCurrent);
        chevronIcon.gameObject.SetActive(levelInfo.IsUnlocked && !levelInfo.IsCurrent);
        lockIcon.gameObject.SetActive(!levelInfo.IsUnlocked);
        button.interactable = levelInfo.IsUnlocked && !levelInfo.IsCurrent;
        SetSubtitle(levelInfo);

        for (int i = 0; i < stars.Length; i++)
        {
            stars[i].gameObject.SetActive(i < levelInfo.BestStars);
        }

        IsCurrent = levelInfo.IsCurrent;
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