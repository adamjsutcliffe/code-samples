﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Peak.QuixelLogic.Scripts.Common;
using Peak.QuixelLogic.Scripts.Autogenerated;
using System;
using JetBrains.Annotations;
using TMPro;
//using DeadMosquito.IosGoodies;
#if UNITY_IOS
using UnityEngine.iOS;
#endif
namespace Peak.QuixelLogic.Scripts.ScenesLogic
{
    public class BoardMenuActivator : SceneActivationBehaviour<BoardMenuActivator>
    {
        [SerializeField]
        private GameObject musicOffIcon;

        [SerializeField]
        private GameObject sfxOffIcon;

        [SerializeField]
        private GameObject notificationOffIcon;

        [SerializeField]
        private GameObject howToPlayWindow;

        public event Action GameResumed;

        public event Action GameRestarted;

        public event Action GameQuit;

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Show()
        {
            base.Show();

            musicOffIcon.SetActive(!SoundController.Instance.IsMusicEnabled);
            sfxOffIcon.SetActive(!SoundController.Instance.IsSoundEnabled);
            notificationOffIcon.SetActive(!NotificationController.Instance.notificationsOn);
        }

        public override void Hide()
        {
            base.Hide();
        }

        [UsedImplicitly]
        public void ResumeGameButtonHandler()
        {
            SceneActivationBehaviour<TopBarUIActivator>.Instance.SetButtons(true);

            InterfaceController.Instance.Hide(GameWindow.BoardMenu);
            InterfaceController.Instance.Hide(GameWindow.BoardBlur);

            GameResumed?.Invoke();
        }

        [UsedImplicitly]
        public void RestartGameButtonHandler()
        {
            SceneActivationBehaviour<ToolbarUIActivator>.Instance.ToggleToPaint();
            GameRestarted?.Invoke();
        }

        [UsedImplicitly]
        public void HowToPlayButtonHandler()
        {
            howToPlayWindow.SetActive(true);
        }

        [UsedImplicitly]
        public void QuitGameButtonHandler()
        {
            InterfaceController.Instance.Show(GameWindow.CollectionScreen);
            InterfaceController.Instance.Hide(GameWindow.BoardBlur);
            InterfaceController.Instance.Hide(GameWindow.BoardMenu);
            InterfaceController.Instance.Hide(GameWindow.BoardUI);
            InterfaceController.Instance.Hide(GameWindow.BonusBarUI);
            InterfaceController.Instance.Hide(GameWindow.ToolbarUI);
            InterfaceController.Instance.Hide(GameWindow.TopBarUI);
            InterfaceController.Instance.Hide(GameWindow.PostGameScene);

            GameQuit?.Invoke();
        }

        [UsedImplicitly] // by Map/Settings button and Pause menu button
        public void ToggleMusic()
        {
            bool isEnabled = SoundController.Instance.ToggleMusic(true);
            musicOffIcon.SetActive(!isEnabled);
        }

        [UsedImplicitly] // by Map/Settings button and Pause menu button
        public void ToggleSfx()
        {
            bool isEnabled = SoundController.Instance.ToggleSfx(true);
            sfxOffIcon.SetActive(!isEnabled);
        }

        [UsedImplicitly] // by Map/Settings button and Pause menu button
        public void ToggleNotifications()
        {
            bool isEnabled = NotificationController.Instance.ToggleNotifications();
            notificationOffIcon.SetActive(!isEnabled);
#if UNITY_IOS
            bool permissionAllowed = UnityEngine.iOS.NotificationServices.enabledNotificationTypes != NotificationType.None;

            if (isEnabled)
            {
                StartCoroutine(NotificationController.Instance.AskForPermissionsCoroutine());
            }
#endif
        }
    }
}