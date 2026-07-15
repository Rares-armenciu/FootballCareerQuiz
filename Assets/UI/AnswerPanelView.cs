using System;
using UnityEngine;

public class AnswerPanelView : MonoBehaviour
{
    [SerializeField]
    private AnswerButtonView[] answerViews;

    public void Show(QuizQuestion question)
    {
        for (int i = 0; i < answerViews.Length; i++)
        {
            answerViews[i].Show(question.Options[i]);
        }
    }

    public void SetInteractable(bool interactable)
    {
        foreach (var answer in answerViews)
        {
            answer.SetInteractable(interactable);
            answer.ResetColor();
        }
    }

    public void Subscribe(Action<AnswerButtonView> callback)
    {
        foreach (var answer in answerViews)
        {
            answer.Clicked -= callback;
            answer.Clicked += callback;
        }
    }

    public void ShowAnswerResult(AnswerButtonView button, bool correct, int correctIndex)
    {
        if (correct)
        {
            button.SetCorrect();
        }
        else
        {
            button.SetWrong();
            answerViews[correctIndex].SetCorrect();
        }
    }
}
