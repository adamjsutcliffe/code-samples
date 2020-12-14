using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Peak.QuixelLogic.Scripts.Game;
using UnityEngine.UI;
using Peak.QuixelLogic.Scripts.Common.Extensions;
using Peak.QuixelLogic.Scripts.Common;
using UnityEngine.EventSystems;
using JetBrains.Annotations;
using TMPro;
using Peak.QuixelLogic.Scripts.Settings;
using Peak.QuixelLogic.Scripts.Common.AnalyticsScripts;

namespace Peak.QuixelLogic.Scripts.ScenesLogic
{
    public class ToolbarUIActivator : SceneActivationBehaviour<ToolbarUIActivator>
    {
        private bool ToolToggled = true;

        [SerializeField]
        private Animator toolToggleAnimator;

        [SerializeField]
        private Image toolToggleButton;

        [SerializeField]
        private Animator ftueHandAnimator;

        [SerializeField]
        private GlobalSettings globalSettings;

        [SerializeField]
        private ButtonController hintButton;

        public HintButtonScript HintButtonScript => hintButtonScript;

        [SerializeField]
        private HintButtonScript hintButtonScript;

        [SerializeField]
        private GameObject rewardedVideoButton;

        [SerializeField]
        private Animator toolbarHolderAnimator;

        public Button ToolToggle => toolToggle;

        [SerializeField]
        private Button toolToggle;

        private const string ControlLayoutPrefsKey = "ControlsLayout";

        [SerializeField]
        private HorizontalLayoutGroup toolbarControls;

        [SerializeField]
        private RectTransform buttonControls;

        [SerializeField]
        private RectTransform toggleControls;

        private BoardCreationScript currentBoardCreationScript;

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Show()
        {
            base.Show();

            if (PlayerPrefs.GetInt(ControlLayoutPrefsKey) == 1)
                SwitchToggleControlLeft();
        }

        public override void Hide()
        {
            base.Hide();
        }

        public override void SetButtons(bool enabled)
        {
            base.SetButtons(enabled);
        }

        public void ToggleControlsLayout()
        {
            if (PlayerPrefs.GetInt(ControlLayoutPrefsKey, 0) == 0)
            {
                SwitchToggleControlLeft();
            }
            else
            {
                SwitchToggleControlRight();
            }
        }

        public void SwitchToggleControlLeft()
        {
            buttonControls.transform.SetSiblingIndex(1);
            toggleControls.transform.SetSiblingIndex(0);
            PlayerPrefs.SetInt(ControlLayoutPrefsKey, 1);
            DebugLog($"[TEST] switching control left - pref: {PlayerPrefs.GetInt(ControlLayoutPrefsKey)}");
        }

        public void SwitchToggleControlRight() // default
        {
            buttonControls.transform.SetSiblingIndex(0);
            toggleControls.transform.SetSiblingIndex(1);
            PlayerPrefs.SetInt(ControlLayoutPrefsKey, 0);
            DebugLog($"[TEST] switching control right - pref: {PlayerPrefs.GetInt(ControlLayoutPrefsKey)}");

        }

        public void PassBoardCreationScript(BoardCreationScript board)
        {
            currentBoardCreationScript = board;
            ToolToggled = true;
        }

        public void SetPlayerHints()
        {
            rewardedVideoButton.SetActive(SceneActivationBehaviour<GameLogicActivator>.Instance.GameController.Player.FtuePassed);
            hintButton.gameObject.SetActive(SceneActivationBehaviour<GameLogicActivator>.Instance.GameController.Player.MainPuzzleIndex >= 5);

            hintButtonScript.state = SceneActivationBehaviour<GameLogicActivator>.Instance.GameController.Player.Hints > 0 ? HintButtonScript.HintState.freeHints : HintButtonScript.HintState.costHints;
        }

        [UsedImplicitly]
        public void ToggleTool()
        {
            iOSHapticFeedback.OnSelection();

            if (ToolToggled) // if paint
            {
                ToggleToCross();
            }
            else // if cross
            {
                ToggleToPaint();
            }
        }

        [UsedImplicitly]
        public void ToggleToCross()
        {
            ToolToggled = false;

            currentBoardCreationScript.SetModeToMarker();

            toolToggleAnimator.SetTrigger(GameConstants.ToolToggleAnimationTriggers.ToggleToCross);
        }

        [UsedImplicitly]
        public void ToggleToPaint()
        {
            ToolToggled = true;

            currentBoardCreationScript.SetModeToPencil();

            if (toolToggleAnimator != null && !toolToggleAnimator.GetCurrentAnimatorStateInfo(0).IsName(GameConstants.ToolToggleAnimationTriggers.ToggleToPaint))
            {
                toolToggleAnimator.SetTrigger(GameConstants.ToolToggleAnimationTriggers.ToggleToPaint);
            }
        }

        public void GameFiveToolToggleTap()
        {
            ShowHandAnimation(false);
            ToggleToPaint();
            ToolToggle.interactable = true;
        }

        [UsedImplicitly]
        public void GetHint()
        {
            SceneActivationBehaviour<GameLogicActivator>.Instance.GameController.HintUseHandler((bool enoughHints, int hintsLeft, bool enoughCoins, int playerCoins) =>
            {
                DebugLog($"[HINT] enough hints? {enoughHints}, hints left: {hintsLeft}, enough coins: {enoughHints}, player coins: {playerCoins}");

                if (enoughHints)
                {
                    SceneActivationBehaviour<BoardUIActivator>.Instance.TryGetHint();

                    hintButtonScript.state = !hintsLeft.Equals(0) ? HintButtonScript.HintState.freeHints : HintButtonScript.HintState.costHints;
                }
                else
                {
                    if (enoughCoins)
                    {
                        // issue hint
                        SceneActivationBehaviour<BoardUIActivator>.Instance.TryGetHint();
                        SceneActivationBehaviour<UICoinCounterActivator>.Instance.SetPlayerCoins(playerCoins);
                    }
                    else
                    {
                        // pause game
                        SceneActivationBehaviour<UICoinCounterActivator>.Instance.SetButtons(false);
                        SceneActivationBehaviour<BoardUIActivator>.Instance.PauseGameHandler();
                        SceneActivationBehaviour<StoreUIActivator>.Instance.OpenStore(() =>
                        {

                            SceneActivationBehaviour<UICoinCounterActivator>.Instance.SetButtons(true);
                        }, QLPurchaseSource.QLPurchaseSourceHint);
                    }
                }
            });
        }

        public void ForceHintFtue(Vector2 forcedCell)
        {
            SceneActivationBehaviour<BoardUIActivator>.Instance.TryGetHint(forcedCell);
        }

        public void HintShakeEffect(bool shake)
        {
            if (hintButtonScript.isActiveAndEnabled)
            {
                hintButtonScript.ShakeHint(shake);
            }
            else return;
        }

        public void ShowHandAnimation(bool show, string animation = null)
        {
            if (show)
            {
                ftueHandAnimator.gameObject.SetActive(true);
                ftueHandAnimator.SetTrigger(animation);
            }
            else
            {
                if (ftueHandAnimator.gameObject.activeSelf)
                {
                    ftueHandAnimator.SetTrigger("Stop");
                    ftueHandAnimator.gameObject.SetActive(false);
                }
            }
        }

        public void ExitToolbar(bool exit = true)
        {
            if (exit)
            {
                toolbarHolderAnimator?.SetTrigger("Exit");
            }
            else
            {
                toolbarHolderAnimator?.SetTrigger("Idle");
            }
        }

        [UsedImplicitly]
        public void ClaimRewardedVideo()
        {
            SceneActivationBehaviour<GameLogicActivator>.Instance.GameController.CallRewardedVideo(AdSourceType.InGame, true);
        }
    }
}