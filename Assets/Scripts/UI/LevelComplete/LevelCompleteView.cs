using Assets.Scripts.UI;
using Assets.Scripts.UI.LevelComplete;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.LevelComplete
{
    public class LevelCompleteView : MonoBehaviour
    {
        [Header("Header")]
        [SerializeField] private TMP_Text title;

        [Header("Continue")]
        [SerializeField] private Button continueButton;

        [Header("Views")]
        [SerializeField]
        private RewardBreakdownView breakdown;
        [SerializeField]
        private StarRatingView stars;
        [SerializeField]
        private RewardView reward;

        public Button ContinueButton => continueButton;

        public void Show(LevelResult result)
        {
            breakdown.HideRows();
            stars.gameObject.SetActive(false);
            reward.gameObject.SetActive(false);

            continueButton.gameObject.SetActive(false);
            continueButton.interactable = false;

            gameObject.SetActive(true);

            title.text = $"LEVEL {result.Level} COMPLETE";

            StartCoroutine(ShowSequence(result));
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        IEnumerator ShowSequence(LevelResult result)
        {
            yield return breakdown.Play(result);

            stars.gameObject.SetActive(true);
            yield return stars.Play(result.Stars);

            reward.gameObject.SetActive(true);

            // Display both the level's final reward and the actual coins that will be awarded
            // (the delta between this run's final reward and the previous best).
            int awardedCoins = GameManager.Instance.ProgressionService.CalculateCoinsToAward(result);
            yield return reward.Play(result.FinalReward, awardedCoins);

            continueButton.gameObject.SetActive(true);
            continueButton.interactable = true;
        }

    }
}