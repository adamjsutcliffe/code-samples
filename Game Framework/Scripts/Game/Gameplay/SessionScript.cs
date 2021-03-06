﻿using System;
using System.Collections;
using JetBrains.Annotations;
using Peak.Speedoku.Scripts.ScenesLogic;
using Peak.Speedoku.Scripts.Settings;
using Peak.UnityGameFramework.Scripts.ScenesLogic;
using UnityEngine;

namespace Peak.Speedoku.Scripts.Game.Gameplay
{
    /// <summary>
    /// Holds and manipulates UI in a session of the game
    /// </summary>
    public sealed class SessionScript : MonoBehaviour
    {
        #region Internal state

        /// <summary>
        /// Settings object
        /// </summary>
        private RuleSettings currentSessionRuleset;

        /// <summary>
        /// The game play.
        /// </summary>
        private GameplayScript gameplay;

        #endregion

        /// <summary>
        /// Shows if the game is in progress (timer is ticking, no goal is reached)
        /// </summary>
        public bool IsInGame { get; private set; }

        /// <summary>
        /// Check for if game is already paused
        /// </summary>
        public bool IsGamePaused => gameplay != null && gameplay.IsPaused;

        #region Events

        /// <summary>
        /// Fires when a new game has started
        /// </summary>
        public event GameStartedHandler SessionStarted;

        /// <summary>
        /// Fires when the game is paused
        /// </summary>
        public event GamePausedHandler SessionPaused;

        /// <summary>
        /// Fires when the game is resumed
        /// </summary>
        public event GameResumedHandler SessionResumed;

        /// <summary>
        /// Fires when the game is quit early
        /// </summary>
        public event GameQuitHandler SessionQuit;

        /// <summary>
        /// Fires when one game core cycle is finished
        /// </summary>
        public event GameOverHandler SessionFinished;

        /// <summary>
        /// Fires when one game exits
        /// </summary>
        //public event GameExitHandler SessionExited;

        #endregion

        #region Unity callbacks

        public void SetupGame(MainGameData gameData)
        {
            currentSessionRuleset = gameData.Ruleset;

            // create & initialize
            gameplay = SceneActivationBehaviour<SPEGameSceneActivator>.Instance.Gameplay; //Instantiate(currentSessionRuleset.GameBoardPrefab);

            gameplay.Configure(gameData);

            // handlers
            gameplay.OnGameTimeChanged += GameTimeChangedHandler;

            gameplay.OnGameStarted += GameStartedHandler;
            gameplay.OnPaused += PausedHandler;
            gameplay.OnResumed += ResumedHandler;
            gameplay.OnGameOver += GameOverHandler;
            gameplay.OnGameQuit += GameQuitHandler;
            //gameplay.OnGameExit += GameExitHandler;

            //// go!
            //gameplay.StartSession();

            //return gameplay;
        }

        public void StartGame()
        {
            // go!
            gameplay.StartSession();
        }

        private void GameStartedHandler(MainGameData gameData)
        {
            print($"[SESSION] Started");
            SessionStarted?.Invoke(gameData);
        }

        private void PausedHandler(MainGameData gameData)
        {
            print($"[SESSION] Paused");
            SessionPaused?.Invoke(gameData);
        }

        private void ResumedHandler(MainGameData gameData)
        {
            print($"[SESSION] Resumed");
            SessionResumed?.Invoke(gameData);
        }

        private void GameQuitHandler(MainGameData gameData)
        {
            print($"[SESSION] GameQuit");
            IsInGame = false;

            //Update gameplay count even though quit for ad purposes
            GameEndedHandler();
            SessionQuit?.Invoke(gameData);
            currentSessionRuleset = null;
        }
        private void GameOverHandler(MainGameData gameData)
        {
            print($"[SESSION] GameOver");
            IsInGame = false;
            GameEndedHandler();
            SessionFinished?.Invoke(gameData);
            
        }

        //private void GameExitHandler(MainGameData gameData)
        //{
        //    print("[SESSION] GameExit");
        //    GameEndedHandler();
        //    IsInGame = false;
        //    SessionExited?.Invoke(gameData);
        //}

        private void GameTimeChangedHandler(int time)
        {
            //SceneActivationBehaviour<BoardUiActivator>.Instance.GameBoardGuiScript.SetGameTime(time);
            //print($"[SESSION] Game time changed: {time}");
        }

        #endregion

        #region UI callbacks

        public void GameEndedHandler()
        {
            print("[SESSION] Game ended -> Completion or failed AND quit");
            // remove handlers
            gameplay.OnGameTimeChanged -= GameTimeChangedHandler;

            gameplay.OnGameStarted -= GameStartedHandler;
            gameplay.OnPaused -= PausedHandler;
            gameplay.OnResumed -= ResumedHandler;
            gameplay.OnGameOver -= GameOverHandler;
            gameplay.OnGameQuit -= GameQuitHandler;
            //gameplay.OnGameExit -= GameExitHandler;
        }

        /// <summary>
        /// Raises the pause click event.
        /// </summary>
        [UsedImplicitly] // by Game UI
        public void OnPauseClick()
        {
            gameplay.PauseSession();
        }

        /// <summary>
        /// Raises the resume click event.
        /// </summary>
        [UsedImplicitly] // by Game UI
        public void OnResumeClick()
        {
            gameplay.ResumeSession();
        }

        /// <summary>
        /// Raises the quit the game event.
        /// </summary>
        [UsedImplicitly] // by Game UI
        public void OnQuitClick()
        {
            gameplay.QuitSession();
        }

        #endregion
    }
}