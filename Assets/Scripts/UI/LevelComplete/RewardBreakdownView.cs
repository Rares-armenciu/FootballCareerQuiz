using System.Collections;
using TMPro;
using UnityEngine;

public class RewardBreakdownView : MonoBehaviour
{
    [SerializeField] private GameObject baseRewardRow;
    [SerializeField] private GameObject wrongRow;
    [SerializeField] private GameObject hintRow;
    [SerializeField] private GameObject flawlessRow;

    [SerializeField] private TMP_Text baseReward;
    [SerializeField] private TMP_Text wrongPenalty;
    [SerializeField] private TMP_Text hintPenalty;
    [SerializeField] private TMP_Text flawlessBonus;

    public IEnumerator Play(LevelResult result)
    {
        Debug.Log($"RewardBreakdownView.Play: {result}");
        HideRows();

        yield return ShowRow(baseRewardRow);

        baseReward.text = $"+{result.BaseReward}";
        baseReward.color = new Color32(124, 210, 61, 255);

        if (result.WrongAnswerPenalty > 0)
        {
            wrongPenalty.color = new Color32(230, 80, 80, 255);
            wrongPenalty.text = $"-{result.WrongAnswerPenalty}";
            yield return ShowRow(wrongRow);
        }

        if (result.HintPenalty > 0)
        {
            hintPenalty.color = new Color32(255, 176, 46, 255);
            hintPenalty.text = $"-{result.HintPenalty}";
            yield return ShowRow(hintRow);
        }

        if (result.FlawlessBonus > 0)
        {
            flawlessBonus.color = new Color32(255, 215, 70, 255);
            flawlessBonus.text = $"+{result.FlawlessBonus}";
            yield return ShowRow(flawlessRow);
        }
    }

    public void HideRows()
    {
        Hide(baseRewardRow);
        Hide(wrongRow);
        Hide(hintRow);
        Hide(flawlessRow);
    }

    private void Hide(GameObject row)
    {
        row.SetActive(false);
    }

    private IEnumerator ShowRow(GameObject row)
    {
        row.SetActive(true);

        CanvasGroup canvas = row.GetComponent<CanvasGroup>();

        if (canvas == null)
            canvas = row.AddComponent<CanvasGroup>();

        canvas.alpha = 0;

        row.transform.localScale = Vector3.one * .95f;

        float t = 0;

        while (t < .25f)
        {
            t += Time.deltaTime;

            float p = t / .25f;

            canvas.alpha = p;

            row.transform.localScale =
                Vector3.Lerp(
                    Vector3.one * .95f,
                    Vector3.one,
                    p);

            yield return null;
        }

        yield return new WaitForSeconds(.08f);
    }
}
