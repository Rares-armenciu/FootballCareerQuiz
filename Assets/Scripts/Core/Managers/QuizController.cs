
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuizController : MonoBehaviour
{
    [SerializeField]
    private CareerPathView careerPathView;

    [SerializeField]
    private AnswerPanelView answerPanelView;

    [SerializeField] 
    private HeaderView headerView;

    [SerializeField]
    private NoLivesView gameOverView;

    [SerializeField]
    private LevelCompleteView levelCompleteView;

    [SerializeField] 
    private AchievementPopupView _achievementPopup;

    private QuizQuestion _currentQuestion;
    private QuizSession _quizSession;
    public event Action UIRefreshed;
    public event Action NewQuestionShown;

    private void Start()
    {
        _quizSession = new QuizSession(new QuizGenerator(GameManager.Instance.PlayerDatabase), 
            GameManager.Instance.LifeService, 
            GameManager.Instance.CoinsService, 
            GameManager.Instance.ProgressionService,
            GameManager.Instance.StatisticsService,
            GameManager.Instance.AchievementService);

        if (GameManager.Instance.ProgressionService.IsCurrentLevelCompleted)
        {
            ShowLevelComplete();
            return;
        }

        NextQuestion();
        gameOverView.Hide();
        GameManager.Instance.AchievementService.AchievementUnlocked += _achievementPopup.Enqueue;
        levelCompleteView.ContinueButton.onClick.AddListener(CompleteLevel);
    }

    private void ShowLevelComplete()
    {
        levelCompleteView.Show(GameManager.Instance.ProgressionService.GetCurrentLevelResult());
    }

    public void RevealHint()
    {
        if(_quizSession.RevealHint())
        {
            RefreshHeader();
            careerPathView.ShowQuestion(_currentQuestion);
            UIRefreshed?.Invoke();

            GameManager.Instance.SaveService.Save(GameManager.Instance.Progress, GameManager.Instance.Statistics, GameManager.Instance.Achievements);
        }
    }

    public bool CanRevealHint()
    {
        return _currentQuestion != null && _currentQuestion.CanRevealClub;
    }

    private void NextQuestion()
    {
        _currentQuestion = _quizSession.NextQuestion();

        NewQuestionShown?.Invoke();
        RefreshUI();
    }

    private void RefreshUI()
    {
        RefreshAnswers();
        ShowQuestion();
        RefreshHeader();
        UIRefreshed?.Invoke();
    }

    private void ShowQuestion()
    {
        // Use the serialized field so the compiler doesn't warn it's unused
        if (careerPathView != null)
        {
            careerPathView.ShowQuestion(_currentQuestion);
        }
    }

    private void RefreshAnswers()
    {
        answerPanelView.Show(_currentQuestion);
        answerPanelView.SetInteractable(true);
        answerPanelView.Subscribe(OnAnswerClicked);
    }

    private void OnAnswerClicked(AnswerButtonView button)
    {
        bool correct = _quizSession.SubmitAnswer(button.Player);

        answerPanelView.ShowAnswerResult(
            button,
            correct,
            _currentQuestion.CorrectIndex);

        RefreshHeader();
        DisableAnswers(button);

        bool levelCompleted =
            GameManager.Instance.ProgressionService.AdvanceQuestion();

        GameManager.Instance.SaveService.Save(
            GameManager.Instance.Progress,
            GameManager.Instance.Statistics,
            GameManager.Instance.Achievements);

        // Player answered the final question of the level.
        if (levelCompleted)
        {
            Invoke(nameof(ShowLevelComplete), 1.5f);
            return;
        }

        // Player ran out of lives.
        if (GameManager.Instance.Progress.Lives <= 0)
        {
            CheckGameOver();
            return;
        }

        // Normal next question.
        Invoke(nameof(NextQuestion), 1.5f);
    }

    private void CompleteLevel()
    {
        GameManager.Instance.ProgressionService.CompleteLevel();
        
        GameManager.Instance.CoinsService.GrantCoins(GameManager.Instance.ProgressionService.GetCurrentLevelResult().FinalReward);

        GameManager.Instance.SaveService.Save(
            GameManager.Instance.Progress,
            GameManager.Instance.Statistics,
            GameManager.Instance.Achievements);

        levelCompleteView.Hide();

        NextQuestion();
    }

    private void DisableAnswers(AnswerButtonView button = null)
    {
        answerPanelView.SetInteractable(false, button);
    }

    private void RefreshHeader()
    {
        headerView.Show(GameManager.Instance.Progress);
    }

    private void CheckGameOver()
    {
        if (GameManager.Instance.Progress.Lives <= 0)
        {
            DisableAnswers();

            gameOverView.Show(GameManager.Instance.Progress, GameManager.Instance.LifeService);
        }
    }

    public void OnEnable()
    {
        GameManager.Instance.LifeService.LivesChanged += RefreshHeader;
    }

    private void OnDisable()
    {
        GameManager.Instance.LifeService.LivesChanged -= RefreshHeader;
        if (GameManager.Instance.AchievementService != null)
            GameManager.Instance.AchievementService.AchievementUnlocked -= _achievementPopup.Enqueue;
    }
}