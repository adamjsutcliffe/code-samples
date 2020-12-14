using System.Collections;
using System.Collections.Generic;
using Peak.QuixelLogic.Scripts.Common.Localisation;
using Peak.QuixelLogic.Scripts.ScenesLogic;
using TMPro;
using UnityEngine;

namespace Peak.QuixelLogic.Scripts.Common
{
    public sealed class OutOfFilmDetailsScript : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI freeFilmTimeText;

        private void LateUpdate()
        {
            if (SceneActivationBehaviour<UIFilmCounterActivator>.Instance.TimeSpanToNextFilm.TotalSeconds > 0)
            {
                if (LocalisationSystem.GetSystemLanguage() == SystemLanguage.Korean)
                {
                    freeFilmTimeText.text = $"{SceneActivationBehaviour<UIFilmCounterActivator>.Instance.TimeCounterText.text}  {GameConstants.MainGame.FeatureMessages.FreeFilmIn} ";
                }
                else freeFilmTimeText.text = $"{GameConstants.MainGame.FeatureMessages.FreeFilmIn}  {SceneActivationBehaviour<UIFilmCounterActivator>.Instance.TimeCounterText.text} ";
            }
            else
            {
                SceneActivationBehaviour<PopUpFilmActivator>.Instance.ExitPanel();
            }
        }
    }
}