
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
    private GameOverView gameOverView;

    private QuizQuestion _currentQuestion;
    private QuizSession _quizSession;

    private void Start()
    {
        _quizSession = new QuizSession(GameManager.Instance.PlayerDatabase, headerView);
        RefreshUI();
        hintButton.onClick.AddListener(OnHintClicked);
        gameOverView.Hide();
    }

    private void RefreshUI()
    {
        _currentQuestion = _quizSession.NextQuestion();

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

        answerPanelView.ShowAnswerResult(button, correct, _currentQuestion.CorrectIndex);

        RefreshHeader();

        DisableAnswers();

        Invoke(nameof(RefreshUI), 1.5f);
    }

    private void DisableAnswers()
    {
        answerPanelView.SetInteractable(false);
    }

    private void OnHintClicked()
    {
        _quizSession.RevealHint();

        careerPathView.ShowQuestion(_currentQuestion);

        RefreshHeader();
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

            gameOverView.Show(GameManager.Instance.Progress);
        }
    }
}