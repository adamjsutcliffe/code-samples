﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Peak.QuixelLogic.Scripts.Autogenerated;
using TMPro;
using System;
using JetBrains.Annotations;
using Peak.QuixelLogic.Scripts.Common;
using Peak.QuixelLogic.Scripts.Common.Extensions;

namespace Peak.QuixelLogic.Scripts.ScenesLogic
{
    public class TopBarUIActivator : SceneActivationBehaviour<TopBarUIActivator>
    {
        [SerializeField]
        private ButtonController pauseButton;

        [SerializeField]
        private TextMeshProUGUI levelText;

        [SerializeField]
        private Animator topBarAnimator;

        [SerializeField]
        private RectTransform topBarHolder;

        private Vector2 topBarOriginalTransform;

        public event Action GamePaused;

        public override void Initialize()
        {
            base.Initialize();
            topBarOriginalTransform = topBarHolder.anchoredPosition;
        }

        public override void Show()
        {
            base.Show();
            pauseButton.SetInteractability(SceneActivationBehaviour<GameLogicActivator>.Instance.GameController.Player.FtuePassed);
        }

        public override void Hide()
        {
            base.Hide();
        }

        public void ExitTopbar(bool exit = true)
        {
            if (exit)
            {
                topBarHolder.SetParent(topBarAnimator.transform.GetChild(0));
                topBarAnimator.SetTrigger("Exit");
            }
            else
            {
                ResetTopBar();
                topBarAnimator.SetTrigger("Idle");
            }
        }

        public override void SetButtons(bool enabled)
        {
            base.SetButtons(enabled);
        }

        public void SetLevelText(string level)
        {
            levelText.text = $"{GameConstants.MainGame.Level} {level}"; //    "LEVEL " + level;
        }

        private void ResetTopBar()
        {
            topBarHolder.SetParent(canvasRoot.gameObject.transform);
            topBarHolder.anchoredPosition = topBarOriginalTransform;
        }

        [UsedImplicitly] // by pause button
        public void PauseGameButtonHandler()
        {
            InterfaceController.Instance.Show(GameWindow.BoardMenu);
            InterfaceController.Instance.Show(GameWindow.BoardBlur);

            GamePaused?.Invoke();
        }
    }
}
