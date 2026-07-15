using System;
using UnityEngine;

public class AnswerPanelView : MonoBehaviour
{
    [SerializeField]
    private AnswerButtonView[] answerViews;

    public AnswerButtonView[] AnswerViews => answerViews;

    public void Show(QuizQuestion question)
    {
        for (int i = 0; i < AnswerViews.Length; i++)
        {
            AnswerViews[i].Show(question.Options[i]);
        }
    }

    public void SetInteractable(bool interactable)
    {
        foreach (var answer in AnswerViews)
        {
            answer.SetInteractable(interactable);
            answer.ResetColor();
        }
    }

    public void Subscribe(Action<AnswerButtonView> callback)
    {
        foreach (var answer in AnswerViews)
        {
            answer.Clicked -= callback;
            answer.Clicked += callback;
        }
    }
}
