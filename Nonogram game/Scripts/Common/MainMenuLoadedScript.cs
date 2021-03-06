﻿using System.Collections;
using System.Collections.Generic;
using Peak.QuixelLogic.Scripts.Settings.Autogenerated;
using UnityEngine;
using Peak.QuixelLogic.Scripts.ScenesLogic;

namespace Peak.QuixelLogic.Scripts.Common
{
    public sealed class MainMenuLoadedScript : MonoBehaviour
    {
        [SerializeField]
        private Animator mainMenuAnimator;

        public void PlayMainMenuSound()
        {
            SoundController.Instance.PlayAudioClip(SoundSettingsKey.Mainmenuload);
        }

        public void HideCollectionAfterLoad()
        {
            InterfaceController.Instance.Hide(Autogenerated.GameWindow.CollectionScreen);

            // trigger film and coin fall down animations
            SceneActivationBehaviour<UIFilmCounterActivator>.Instance.GameStartAnimation("FilmEnter");
            SceneActivationBehaviour<UICoinCounterActivator>.Instance.GameStartAnimation("CoinEnter");
        }

        public void DeactivateAnimation()
        {
            mainMenuAnimator.enabled = false;
            SoundController.Instance.MuteMusic(false);
        }
    }
}