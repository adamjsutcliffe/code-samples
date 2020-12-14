using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using Peak.Speedoku.Scripts.Common.Extensions;
using Peak.UnityGameFramework.Scripts.Common;

namespace Peak.UnityGameFramework.Scripts.ScenesLogic
{
    public class OverlayUISceneActivator : SceneActivationBehaviour<OverlayUISceneActivator>
    {
        [SerializeField]
        private Canvas clickCanvasRoot;

        [SerializeField]
        private Button fullScreenButton;

        public Button FullScreenButton => fullScreenButton;

        [SerializeField]
        private GameObject backgroundBlur;

        [Header("Overlay - FTUE")]
        [SerializeField]
        private GameObject gameplayMessageObject;

        [SerializeField]
        private TextMeshProUGUI gameplayMessageText;

        [SerializeField]
        private GameObject notificationMessageObject;

        [SerializeField]
        private TextMeshProUGUI notificationMessageText;

        [Header("Overlay - HUD")]
        [SerializeField] private HUDController hudController;

        [Header("Overlay - Screen Transition")]
        [SerializeField] private Animator transitionAnimator;

        [Header("Overlay - Round Animation")]
        [SerializeField] private Animator roundAnimator;

        private List<GameObject> allVisibleObjects = new List<GameObject>();

        private Coroutine autoHideFtueCoroutine;

        public override void Initialize()
        {
            base.Initialize();

            allVisibleObjects.Add(fullScreenButton.gameObject);
            allVisibleObjects.Add(gameplayMessageObject);
        }

        public override void Show(bool animated = false)
        {
            base.Show(animated);
        }

        public override void Hide()
        {
            base.Hide();
        }

        public override void SetButtonsEnabled(bool isEnabled)
        {
            base.SetButtonsEnabled(isEnabled);
        }

        public void ShowFtueGameplayMessage(string message, int autoHideDelayInMs = 0)
        {
            gameplayMessageObject.SetActive(true);
            gameplayMessageText.text = message;

            if (autoHideDelayInMs > 0)
            {
                if (autoHideFtueCoroutine != null)
                {
                    StopCoroutine(autoHideFtueCoroutine);
                }

                autoHideFtueCoroutine = StartCoroutine(AutoHideFtue(autoHideDelayInMs));
            }
        }

        public void ShowNotificationMessage(string message, int autoHideDelayInMs = 0)
        {
            notificationMessageObject.SetActive(true);
            notificationMessageText.text = message;

            if (autoHideDelayInMs > 0)
            {
                if (autoHideFtueCoroutine != null)
                {
                    StopCoroutine(autoHideFtueCoroutine);
                }

                autoHideFtueCoroutine = StartCoroutine(AutoHideFtue(autoHideDelayInMs));
            }
        }

        public void HideFtueMessages()
        {
            gameplayMessageObject.SetActive(false);
            notificationMessageObject.SetActive(false);
        }

        private IEnumerator AutoHideFtue(int autoHideDelayInMs)
        {
            yield return new WaitForSeconds(autoHideDelayInMs / 1000f);
            HideFtueMessages();
        }

        public void SubscribeForFullScreenTap(Action handler)
        {
            clickCanvasRoot.gameObject.SetActive(true);
            FullScreenButton.SetInteractability(true);
            FullScreenButton.SetEnabled(true);

            FullScreenButton.onClick.RemoveAllListeners();
            FullScreenButton.onClick.AddListener(() =>
            {
                FullScreenButton.SetEnabled(false);
                clickCanvasRoot.gameObject.SetActive(false);

                handler?.Invoke();
            });
        }

        #region Screen transition

        private Action startCompletion;
        private Action endCompletion;

        public void StartScreenTransition(Action action)
        {
            startCompletion = action;
            transitionAnimator.SetTrigger(Constants.Animation.Overlay.StartTransition);
        }

        public void StartTransitionAnimationDone()
        {
            print("start animation done");
            startCompletion?.Invoke();
            startCompletion = null;
        }

        public void EndScreenTransition(Action action)
        {
            endCompletion = action;
            transitionAnimator.SetTrigger(Constants.Animation.Overlay.EndTransition);
        }

        public void EndTransitionAnimationDone()
        {
            print("End transition done");
            endCompletion?.Invoke();
            endCompletion = null;
        }

        #endregion

        #region Round result animation

        private Action correctCompletion;
        private Action wrongCompletion;

        public void ShowRoundComplete(bool success, Action completion)
        {
            if (success)
            {
                PlayCorrectAnimation(completion);
            }
            else
            {
                PlayWrongAnimation(completion);
            }
        }

        private void PlayCorrectAnimation(Action action)
        {
            correctCompletion = action;
            roundAnimator.SetTrigger(Constants.Animation.Round.CorrectRound);
        }

        public void CorrectAnimationDone()
        {
            correctCompletion?.Invoke();
            correctCompletion = null;
        }

        private void PlayWrongAnimation(Action action)
        {
            wrongCompletion = action;
            roundAnimator.SetTrigger(Constants.Animation.Round.WrongRound);
        }

        public void WrongAnimationDone()
        {
            wrongCompletion?.Invoke();
            wrongCompletion = null;
        }

        #endregion

        #region HUD lifecycle

        public void SetupRoundHUD(int rounds = 0, int time = 0, Action timerFinishedHandler = null)
        {
            hudController.SetupHud(rounds, time, timerFinishedHandler);
        }

        public void UpdateHUDColour(Color colour)
        {
            hudController.UpdateHudColour(colour);
        }

        public void IncrementRoundCount(int score)
        {
            hudController.IncrementRound(score);
        }

        public override void PauseGame()
        {
            base.PauseGame();
            hudController.PauseHUD();
        }

        public override void ResumeGame()
        {
            base.ResumeGame();
            hudController.ResumeHUD();
        }
        #endregion
    }
}
