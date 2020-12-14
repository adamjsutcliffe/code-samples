using System.Collections;
using System.Collections.Generic;
using Peak.QuixelLogic.Scripts.Settings;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Peak.QuixelLogic.Scripts.Game.CollectionScripts
{
    public sealed class ReplayPopupDetailsScript : MonoBehaviour
    {
        [SerializeField]
        private Image popupLevelImage;

        [SerializeField]
        private GameObject regularPoloroid;

        [SerializeField]
        private GameObject goldPoloroid;

        [SerializeField]
        private TextMeshProUGUI popupLevelNameText;

        [SerializeField]
        private TextMeshProUGUI purchaseMessage;

        [SerializeField]
        private List<GameObject> stars = new List<GameObject>();

        public void PopulateInformation(Sprite puzzleIcon, string puzzleName, string message, int starRecord, bool gold)
        {
            for (int i = 0; i < stars.Count; i++)
            {
                stars[i].SetActive(false);
            }

            regularPoloroid.SetActive(!gold);
            goldPoloroid.SetActive(gold);

            popupLevelImage.sprite = puzzleIcon;
            popupLevelNameText.text = puzzleName;
            purchaseMessage.text = message;

            for (int i = 0; i < starRecord; i++)
            {
                stars[i].SetActive(true);
            }
        }

    }
}