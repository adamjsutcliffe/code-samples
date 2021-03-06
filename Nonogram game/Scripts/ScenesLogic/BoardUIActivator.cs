﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Peak.QuixelLogic.Scripts.Game;
using Peak.QuixelLogic.Scripts.Game.Gameplay;
using System;
using Peak.QuixelLogic.Scripts.Settings;
using JetBrains.Annotations;
using Peak.QuixelLogic.Scripts.Autogenerated;
using UnityEngine.UI;
using Peak.QuixelLogic.Scripts.Common;

namespace Peak.QuixelLogic.Scripts.ScenesLogic
{
    public class BoardUIActivator : SceneActivationBehaviour<BoardUIActivator>
    {
        [SerializeField] private Canvas[] backgroundCanvases;

        [SerializeField]
        private Image backgroundImage;

        [SerializeField]
        private GameObject boardPrefab;

        [SerializeField]
        private GameObject boardPlaceHolder;

        private GameObject FreshBoard;

        private BoardCreationScript boardCreationScript;

        private BonusBarUIActivator bonusBarUIActivator;

        private GameplayScript boardGameplayScript;

        public event Action<Vector2?> HintRequested;

        public GameAlienController QuixelController => quixelController;

        [SerializeField]
        private GameAlienController quixelController;

        [SerializeField]
        private GameObject quixel;

        [SerializeField]
        private Animator boardUIAnimator;

        [SerializeField]
        private Animator alienHolderAnimator;

        private Player player;

        public override void Initialize()
        {
            base.Initialize();
            bonusBarUIActivator = SceneActivationBehaviour<BonusBarUIActivator>.Instance;
        }

        public int GetGridSize()
        {
            return boardCreationScript.GridSize;
        }

        public override void Show()
        {
            base.Show();
            foreach (Canvas bgCanvas in backgroundCanvases)
            {
                bgCanvas.gameObject.SetActive(true);
            }
        }

        public override void Hide()
        {
            base.Hide();
            foreach (Canvas bgCanvas in backgroundCanvases)
            {
                bgCanvas.gameObject.SetActive(false);
            }
        }

        public void DestroyBoard()
        {
            HideImage();

            if (FreshBoard != null)
            {
                Destroy(FreshBoard);
            }
        }

        public void TrySpawnQuixel()
        {
            if (player.FtuePassed || player.MainPuzzleIndex == 5)
            {
                GameObject boardQuixel = Instantiate(quixel, canvasRoot.transform, false);
                alienHolderAnimator = boardQuixel.GetComponent<Animator>();
                quixelController = boardQuixel.GetComponentInChildren<GameAlienController>();

                quixelController.shouldShowCharacter = true;
                quixelController.ShowHideCharacter(true);
            }
        }

        public void DestroyQuixel()
        {
            if (quixelController != null)
            {
                Destroy(alienHolderAnimator.gameObject, 1);
                quixelController = null; alienHolderAnimator = null;
            }
        }

        public void AlienCelebrateTrigger(bool postGame = false)
        {
            if (quixelController != null && boardGameplayScript.GameData.SecondsLeft < boardGameplayScript.GameData.TimeLimit)
            {
                quixelController.TryToCelebrate(postGame);
            }
        }

        public GameplayScript LoadBoard(RuleSettings ruleSettings)
        {
            //print($"[GAME] Loading board - ruleset name: {ruleSettings.PuzzleNameKey}");

            player = SceneActivationBehaviour<GameLogicActivator>.Instance.GameController.Player;

            for (int i = 0; i < boardPlaceHolder.transform.childCount; i++)
            {
                Destroy(boardPlaceHolder.transform.GetChild(i));
            }

            backgroundImage.sprite = ruleSettings.BoardBackgroundImage;
            FreshBoard = Instantiate(boardPrefab, boardPlaceHolder.transform);

            boardCreationScript = FreshBoard.GetComponent<BoardCreationScript>();
            boardGameplayScript = FreshBoard.GetComponent<GameplayScript>();

            boardCreationScript.InitiateBoard(ruleSettings);
            bonusBarUIActivator.InitiateBonusBar(ruleSettings, boardGameplayScript);

            Vector3 boardScale = boardPlaceHolder.transform.localScale;
            SceneActivationBehaviour<PostGameSceneActivator>.Instance.SetBoardHolderScale(boardScale);

            TrySpawnQuixel();

            return boardGameplayScript;
        }

        public void DisableTouchGrid()
        {
            boardCreationScript.DisableTouchGrid();
        }

        public void ExitAlien(bool exit = true)
        {
            if (exit)
            {
                if (alienHolderAnimator != null && alienHolderAnimator.gameObject.activeSelf)
                {
                    alienHolderAnimator.SetTrigger("Exit");
                    DestroyQuixel();
                }
            }
            else
            {
                if (alienHolderAnimator != null && alienHolderAnimator.gameObject.activeSelf)
                    alienHolderAnimator.SetTrigger("Idle");
            }
        }

        public void ExitBoard(bool exit = true)
        {
            if (exit)
            {
                if (boardUIAnimator != null && boardUIAnimator.gameObject.activeSelf)
                    boardUIAnimator.SetTrigger("Exit");
            }
            else
            {
                if (boardUIAnimator != null && boardUIAnimator.gameObject.activeSelf)
                    boardUIAnimator.SetTrigger("Idle");
            }
        }

        [UsedImplicitly]
        private void OnApplicationPause(bool isPaused)
        {
            GameLogicActivator gameLogicActivator = SceneActivationBehaviour<GameLogicActivator>.Instance;

            if (gameLogicActivator == null)
            {
                Debug.LogWarning("WARNING! GameLogicActivator is not initialized. It may be caused by app switching on load.");
                return;
            }

            bool isNotInGame = !gameLogicActivator.SessionScript.IsInGame;
            bool isGamePaused = gameLogicActivator.SessionScript.IsGamePaused;
            bool isFtueGame = gameLogicActivator.SessionScript.IsFtueGame;
            //print($"[PAUSE] Background pause: {isPaused} -> Not in game: {isNotInGame} Paused: {isGamePaused} Time: {Time.time}");
            if (isNotInGame || isGamePaused || isFtueGame)
            {
                // if not in a game, ignore
                // if is true, ignore
                return;
            }

            if (isPaused)
            {
                // App going to background
                SceneActivationBehaviour<TopBarUIActivator>.Instance.PauseGameButtonHandler();
                quixelController.quixelState = GameAlienController.AnimationStates.idle;
            }
            else
            {
                // App is restoring
            }
        }

        public void SendBoardToPostGame()
        {
            // Move puzzle image to post game screen
            GameObject thisBoard = boardCreationScript.gameObject;
            GameObject targetBoard = SceneActivationBehaviour<PostGameSceneActivator>.Instance.boardHolder;
            thisBoard.transform.SetParent(targetBoard.transform, false);
            boardCreationScript.gameObject.transform.localScale = new Vector3(1, 1, 1);

            boardCreationScript.HideLabels();
        }

        public void HideImage()
        {
            boardCreationScript.HideImage();
        }

        public void TryGetHint(Vector2? ForcedCellCoordinate = null)
        {
            HintRequested?.Invoke(ForcedCellCoordinate);
        }

        public void PauseGameHandler()
        {
            if (boardGameplayScript != null)
            {
                boardGameplayScript.PauseSession();
            }
        }

        public void ResumeGameHandler()
        {
            if (boardGameplayScript != null && !SceneActivationBehaviour<BoardMenuActivator>.Instance.IsActive())
            {
                boardGameplayScript.ResumeSession();
            }
        }
    }
}