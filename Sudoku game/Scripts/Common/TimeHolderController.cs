using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Peak.Speedoku.Scripts.Common
{
    public class TimeHolderController : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI timeText;

        //private void Start()
        //{
        //    SetupTimerText(83);
        //}

        public void SetupTimerText(int seconds)
        {
            int minutes = seconds / 60;
            int remainingSeconds = seconds % 60;
            timeText.text = string.Format("{0,2:D2}:{1,2:D2}", minutes, remainingSeconds);
        }
    }
}
