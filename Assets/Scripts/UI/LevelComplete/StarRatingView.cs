using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.LevelComplete
{
    internal class StarRatingView : MonoBehaviour
    {
        [Header("Stars")]
        [SerializeField]
        private Image[] stars;

        public IEnumerator Play(int starsCount)
        {
            foreach (var star in stars)
            {
                star.gameObject.SetActive(false);
                star.transform.localScale = Vector3.zero;
            }

            for (int i = 0; i < starsCount; i++)
            {
                yield return Pop(stars[i]);

                yield return new WaitForSeconds(.15f);
            }
        }

        private IEnumerator Pop(Image star)
        {
            star.gameObject.SetActive(true);

            float t = 0;

            while (t < .18f)
            {
                t += Time.deltaTime;

                float s = Mathf.Lerp(0, 1.2f, t / .18f);

                star.transform.localScale = Vector3.one * s;

                yield return null;
            }

            t = 0;

            while (t < .08f)
            {
                t += Time.deltaTime;

                float s = Mathf.Lerp(1.2f, 1f, t / .08f);

                star.transform.localScale = Vector3.one * s;

                yield return null;
            }
        }
    }
}
