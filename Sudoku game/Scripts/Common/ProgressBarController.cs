using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

namespace Peak.Speedoku.Scripts.Common
{
    public class ProgressBarController : MonoBehaviour
    {
        [SerializeField] RectTransform progressBar;
        [SerializeField] TextMeshProUGUI titleText;

        private string locationName;
        private int startValue;
        private int endValue;
        private float totalWidth;


        public void SetupProgressBar(int start, int end, string location)
        {
            totalWidth = progressBar.rect.width;
            startValue = start;
            endValue = end;
            locationName = location;
            progressBar.anchoredPosition = new Vector2(-totalWidth, 0);
            float startPercentage = start / (float)end;
            UpdateBarPercentage(startPercentage);
            UpdateTitle();
        }

        public void IncrementProgress(Action completion = null)
        {
            float startPercentage = startValue / (float)endValue;
            startValue += 1;
            float endPercentage = startValue / (float)endValue;
            Action completionHandler = startValue == endValue ? completion : null;
            StartCoroutine(UpdateProgress(startPercentage, endPercentage, completionHandler));
        }

        private IEnumerator UpdateProgress(float start, float end, Action completion = null)
        {
            yield return new WaitForSeconds(0.3f);
            float diff = end - start;
            float increment = diff / 10.0f;
            for (int i = 0; i < 10; i++)
            {
                yield return new WaitForSeconds(0.01f);
                UpdateBarPercentage(start += increment);
            }
            UpdateBarPercentage(end);
            UpdateTitle();
#if PLATFORM_IOS
            iOSHapticFeedbackHelper.OnSelection();
#endif
            completion?.Invoke();
        }

        private void UpdateBarPercentage(float percentage) //between 0, and 1
        {
            float newPosition = Mathf.Min(-totalWidth + (totalWidth * percentage), 0);
            progressBar.anchoredPosition = new Vector2(newPosition, 0);
        }

        private void UpdateTitle()
        {
            titleText.text = $"{locationName} - {startValue}/{endValue}";
        }
    }
}
