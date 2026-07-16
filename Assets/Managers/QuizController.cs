
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
    private Button hintButton;

    [SerializeField] 
    private HeaderView headerView;

    [SerializeField]
    private NoLivesView gameOverView;

    private QuizQuestion _currentQuestion;
    private QuizSession _quizSession;

    private void Start()
    {
        _quizSession = new QuizSession(new QuizGenerator(GameManager.Instance.PlayerDatabase), 
            GameManager.Instance.LifeService, 
            GameManager.Instance.CoinsService, 
            GameManager.Instance.ProgressionService,
            GameManager.Instance.StatisticsService);

        NextQuestion();
        hintButton.onClick.AddListener(OnHintClicked);
        gameOverView.Hide();
    }

    private void NextQuestion()
    {
        _currentQuestion = _quizSession.NextQuestion();

        RefreshUI();
    }

    private void RefreshUI()
    {
        RefreshAnswers();
        ShowQuestion();
        RefreshHeader();
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
        GameManager.Instance.SaveService.Save(GameManager.Instance.Progress);
        answerPanelView.ShowAnswerResult(button, correct, _currentQuestion.CorrectIndex);

        RefreshHeader();

        DisableAnswers();

        Invoke(nameof(NextQuestion), 1.5f);
    }

    private void DisableAnswers()
    {
        answerPanelView.SetInteractable(false);
    }

    private void OnHintClicked()
    {
        if (_quizSession.RevealHint())
        {
            RefreshHeader();
            careerPathView.ShowQuestion(_currentQuestion);
        }
    }

    private void RefreshHeader()
    {
        headerView.Show(GameManager.Instance.Progress);
        CheckGameOver();
    }

    private void CheckGameOver()
    {
        if (GameManager.Instance.Progress.Lives <= 0)
        {
            DisableAnswers();
            hintButton.interactable = false;

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
    }
}