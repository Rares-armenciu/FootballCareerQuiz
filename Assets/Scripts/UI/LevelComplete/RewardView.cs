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

        public IEnumerator Play(int reward)
        {
            rewardText.text = "0";

            float duration = .7f;

            float t = 0;

            while (t < duration)
            {
                t += Time.deltaTime;

                float p = Mathf.SmoothStep(0f, 1f, t / duration);

                int value =
                    Mathf.RoundToInt(
                        Mathf.Lerp(0,
                                   reward,
                                   p));

                rewardText.text = $"{value} Coins";

                yield return null;
            }

            yield return StartCoroutine(CelebrateReward());

            yield return new WaitForSeconds(.25f);
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
