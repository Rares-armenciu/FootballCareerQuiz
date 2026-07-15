
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuizController : MonoBehaviour
{
    [SerializeField]
    private CareerPathView careerPathView;

    [SerializeField]
    private AnswerButtonView[] answerViews;

    [SerializeField]
    private Button hintButton;

    [SerializeField] 
    private HeaderView headerView;

    private QuizQuestion _currentQuestion;
    private QuizSession _quizSession;

    private void Start()
    {
        _quizSession = new QuizSession(GameManager.Instance.PlayerDatabase, headerView);
        RefreshUI();
        RefreshHeader();
        hintButton.onClick.AddListener(OnHintClicked);
    }

    private void RefreshUI()
    {
        _currentQuestion = _quizSession.NextQuestion();

        for (int i = 0; i < _currentQuestion.Options.Count; i++)
        {
            answerViews[i].SetInteractable(true);
            answerViews[i].ResetColor();
            
            answerViews[i].Clicked -= OnAnswerClicked;
            answerViews[i].Clicked += OnAnswerClicked;

            answerViews[i].Show(_currentQuestion.Options[i]);
        }

        // Use the serialized field so the compiler doesn't warn it's unused
        if (careerPathView != null)
        {
            careerPathView.ShowQuestion(_currentQuestion);
        }

        
    }

    private void OnAnswerClicked(AnswerButtonView button)
    {
        bool correct = _quizSession.SubmitAnswer(button.Player);

        if(correct)
        {
            button.SetCorrect();
        }
        else
        {
            button.SetWrong();
            answerViews[_currentQuestion.CorrectIndex].SetCorrect();
        }

        RefreshHeader();

        DisableAnswers();

        Invoke(nameof(RefreshUI), 1.5f);
    }

    private void DisableAnswers()
    {
        foreach (var answer in answerViews)
            answer.SetInteractable(false);
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
    }
}