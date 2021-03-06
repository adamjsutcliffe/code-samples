﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEditor;
using Peak.Speedoku.Scripts.Autogenerated;
using TMPro;
using Peak.Speedoku.Scripts.Game;
using Peak.Speedoku.Scripts.Common;
using Peak.Speedoku.Scripts.Game.Gameplay;
using System;
using System.Linq;
using UnityEngine.UI;

namespace Peak.Speedoku.Scripts.ScenesLogic
{
    public class MainMenuActivator : SceneActivationBehaviour<MainMenuActivator>
    {
        private GameController gameController;
        private ServerController serverController;

        [SerializeField] Animator animator;
        [SerializeField] GameplayScript gameplay;

        [SerializeField] Image background;

        [Header("Main menu")]
        [SerializeField] TextMeshProUGUI playButtonLabel;
        //[SerializeField] TextMeshProUGUI deductionScoreLabel;
        [SerializeField] DeductionController deductionController;

        [Header("Post game")]
        [SerializeField] TimeHolderController timerController;
        [SerializeField] CheckBoxHolderController checkBoxController;
        [SerializeField] ProgressBarController progressController;

        public GameplayScript Gameplay => gameplay;
        public event Action GamePaused;

        public override void Initialize()
        {
            base.Initialize();

            gameController = SceneActivationBehaviour<GameLogicActivator>.Instance.GameController;
            serverController = SceneActivationBehaviour<GameLogicActivator>.Instance.ServerController;
        }

        public override void Show(bool animated = false)
        {
            UpdatePlayButtonText();
            deductionController.UpdateDeductionText(gameController.Player.DeductionScore);
            ProgressData data = gameController.GetProgressInfo();
            background.sprite = gameController.BackgroundForCollection(data.collection).locationBackground;
            base.Show(animated);
        }

        public override void Hide()
        {
            base.Hide();
        }

        public void PlayButtonHandler()
        {
            string startAnimationTrigger = Constants.Animation.MainMenu.StartGamePlay;
            if (replayData != null && !gameController.IsInFtue())
            {
                gameController.Player.MainGameLevelIndex += 1;
                serverController.PersistPlayerProgress(gameController.Player);
                startAnimationTrigger = Constants.Animation.MainMenu.StartGameBoth;
                replayData = null;
            }
            animator.SetTrigger(startAnimationTrigger);
        }

        public void SettingsButtonHandler()
        {
            InterfaceController.Instance.Show(GameWindow.SettingsMenu, true);
        }

        public void SetupGame()
        {
            MainGameData mainGameData = new MainGameData
            {
                Ruleset = gameController.GetRules(),
                Level = gameController.Player.MainGameLevelIndex + 1
            };
            if (replayData != null)
            {
                mainGameData.answers = replayData.answers;
                replayData = null;
            }

            gameController.SetupGameHandler(mainGameData);
        }

        public void SetupProgressBar()
        {
            ProgressData data = gameController.GetProgressInfo();
            gameController.UpdatePlayerProgress();
            progressController.SetupProgressBar(data.progress, data.locationLimit, gameController.BackgroundForCollection(data.collection).locationName);
        }



        public void UpdateProgressBar()
        {
            progressController.IncrementProgress(() =>
            {
                SceneActivationBehaviour<OverlayUISceneActivator>.Instance.StartScreenTransition(ChangeBackground);
            });
        }

        private void ChangeBackground()
        {
            ProgressData data = gameController.GetProgressInfo();
            LocalisationController.Instance.PlayAudioClip(Settings.Autogenerated.SoundSettingsKey.LocationUnlocked);
            background.sprite = gameController.BackgroundForCollection(data.collection).locationBackground;
            progressController.SetupProgressBar(data.progress, data.locationLimit, gameController.BackgroundForCollection(data.collection).locationName);

            SceneActivationBehaviour<OverlayUISceneActivator>.Instance.EndScreenTransition(() =>
            {
                ShowFeedbackPopup(data.collection);
            });
        }

        private void ShowFeedbackPopup(int location)
        {
            print($"Change background to collection: {location} checks: {Constants.FeedbackInfo.FirstCheck} && {Constants.FeedbackInfo.SecondCheck}");
            if ( (location == Constants.FeedbackInfo.FirstCheck) ||
                (location == Constants.FeedbackInfo.SecondCheck && gameController.ShouldShowFeedbackFormAgain()) )
            {
                InterfaceController.Instance.Show(GameWindow.FeedbackPopup, true);
            }
        }

        private void UpdatePlayButtonText()
        {
            if (gameController.IsInFtue())
            {
                playButtonLabel.text = $"PLAY";
            }
            else
            {
                playButtonLabel.text = $"LEVEL {gameController.Player.MainGameLevelIndex + 1}";
            }
        }

        #region Game scene handlers
        public void StartGame()
        {
            gameController.StartGameHandler();
        }

        public void KeyboardButtonHandler(KeyButtonScript sender)
        {
            print($"[GSA] Keyboard clicked - sender: {sender.KeyNumber}");
            gameplay.OnKeyboardPressed(sender);
        }

        public void PauseButtonHandler()
        {
            InterfaceController.Instance.Show(GameWindow.PauseMenu, true);
            LocalisationController.Instance.PlayAudioClip(Settings.Autogenerated.SoundSettingsKey.PopupSound);
            GamePaused?.Invoke();
        }

        public void QuitButtonHandler()
        {
            gameplay.EndGameSession();
            animator.SetTrigger(Constants.Animation.MainMenu.ExitGameQuit);
            UpdatePlayButtonText();
        }

        #endregion

        #region Post game handlers

        private MainGameData replayData;

        public void GameFinished(MainGameData gameData)
        {
            InterfaceController.Instance.SetButtonsEnabled(false);
            replayData = gameData;
            checkBoxController.SetupWithAnswers(gameData.answers);
            timerController.SetupTimerText(gameData.SecondsUsed);
            // Need all correct -> just next level button
            // 1/2 correct -> retry and next level buttons
            // no correct -> retry button
            bool complete = gameData.answers.All(x => x.correct);
            string animationTrigger;

            int correctCount;
            if (complete)
            {
                if (!gameData.Ruleset.IsFtueGame)
                {
                    gameController.Player.MainGameLevelIndex += 1;
                }

                correctCount = 3;
                replayData = null;
                animationTrigger = Constants.Animation.MainMenu.ExitGameComplete;
            }
            else
            {
                correctCount = gameData.answers.Where(x => x.correct == true).Count();

                animationTrigger = correctCount > 0 ? Constants.Animation.MainMenu.ExitGameIncomplete : Constants.Animation.MainMenu.ExitGameFail;
            }
            gameController.Player.DeductionScore += gameController.GetDeductionScore(gameData.SecondsUsed, correctCount);
            serverController.PersistPlayerProgress(gameController.Player);
            //TODO: check for FTUE level
            //check which Level it is too 
            //playButtonLabel.text = $"LEVEL {gameData.Level + 1}";
            UpdatePlayButtonText();

            animator.SetTrigger(animationTrigger);
        }

        public void GameFinishedAnimationOver()
        {
            if (checkBoxController.isActiveAndEnabled)
            {
                if (replayData == null)
                {
                    SetupProgressBar();
                }
                checkBoxController.ShowAnswers(() =>
                {
                    deductionController.UpdateDeductionText(gameController.Player.DeductionScore, true);
                    if (replayData == null)
                    {
                        animator.SetTrigger(Constants.Animation.MainMenu.ShowLocationBar);
                    }
                });
            }
            InterfaceController.Instance.SetButtonsEnabled(true);
        }

        public void RetryLevelHandler()
        {
            print("Retry level clicked");
            if (replayData == null)
            {
                PlayButtonHandler();
                return;
            }
            animator.SetTrigger(Constants.Animation.MainMenu.StartGameRetry);
        }

        #endregion
    }
}
