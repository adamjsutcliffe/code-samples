using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Peak.QuixelLogic.Scripts.Common
{
    public class HideAfterTime : MonoBehaviour
    {
        [SerializeField]
        private float HideAfterSeconds;

        public void OnEnable()
        {
            Invoke(nameof(Hide), HideAfterSeconds);
        }

        private void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}