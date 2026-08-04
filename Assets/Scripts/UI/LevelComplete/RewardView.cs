using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.UI.LevelComplete
{
    internal class RewardView :MonoBehaviour
    {
        [SerializeField]
        private RectTransform rewardContainer;

        [SerializeField] 
        private TMP_Text rewardText;

        [SerializeField]
        private RectTransform coinIcon;

        // Now shows both the level's final reward and the actual awarded coins.
        // finalReward: the computed final reward for the run
        // awardedCoins: the delta that will be granted to the player (may be zero)
        public IEnumerator Play(int finalReward, int awardedCoins)
        {
            // Animate the final reward value
            float duration = .7f;

            float t = 0;

            while (t < duration)
            {
                t += Time.deltaTime;

                float p = Mathf.SmoothStep(0f, 1f, t / duration);

                int value = Mathf.RoundToInt(Mathf.Lerp(0, finalReward, p));

                rewardText.text = $"Final Reward: {value} Coins\nAwarded: {FormatAward(awardedCoins)}";

                yield return null;
            }

            // Ensure final state
            rewardText.text = $"Final Reward: {finalReward} Coins\nAwarded: {FormatAward(awardedCoins)}";

            yield return StartCoroutine(CelebrateReward());

            yield return new WaitForSeconds(.25f);
        }

        private string FormatAward(int awardedCoins)
        {
            if (awardedCoins <= 0)
                return "0 Coins";

            return $"+{awardedCoins} Coins";
        }

        private IEnumerator CelebrateReward()
        {
            coinIcon.localRotation = Quaternion.Euler(0, 0, 10);
            Vector3 original = rewardContainer.localScale;

            Vector3 target = original * 1.05f;

            float t = 0f;

            while (t < 0.12f)
            {
                t += Time.deltaTime;

                rewardContainer.localScale =
                    Vector3.Lerp(original, target, t / 0.12f);

                yield return null;
            }

            t = 0f;

            while (t < 0.12f)
            {
                t += Time.deltaTime;

                rewardContainer.localScale =
                    Vector3.Lerp(target, original, t / 0.12f);

                yield return null;
            }

            coinIcon.localRotation = Quaternion.identity;
            rewardContainer.localScale = original;
        }
    }
}
