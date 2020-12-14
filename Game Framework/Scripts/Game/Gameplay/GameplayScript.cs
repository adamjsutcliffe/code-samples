using System;
using System.Collections;
using System.Collections.Generic;
using Peak.Speedoku.Scripts.Settings;
using UnityEngine;

namespace Peak.Speedoku.Scripts.Game.Gameplay
{
    /// <summary>
    /// To be attached to game play object i.e. a board or grid
    /// </summary>

    public sealed class GameplayScript : MonoBehaviour
    {
        //[SerializeField]
        //private SoundSettingsKey timerCountdownSound;

        #region Internal state

        /// <summary>
        /// The paused.
        /// </summary>
        private bool isPaused;

        /// <summary>
        /// Timer coroutine
        /// </summary>
        private Coroutine timerCoroutine;

        private MainGameData gameData;
        [SerializeField] private GridController gameBoard;

        public RuleSettings CurrentGameSettings => gameData.Ruleset;
        public MainGameData GameData => gameData;
        public GridController GameBoard => gameBoard;

        public bool IsPaused => isPaused;

        #endregion

        #region Events

        /// <summary>
        /// (1) Occurs when on started.
        /// </summary>
        public event GameStartedHandler OnGameStarted;

        /// <summary>
        /// (2) Occurs when on paused.
        /// </summary>
        public event GamePausedHandler OnPaused;

        /// <summary>
        /// (3) Occurs when on resumed.
        /// </summary>
        public event GameResumedHandler OnResumed;

        /// <summary>
        /// (4) Occurs when on game over when the goal is finished
        /// </summary>
        public event GameOverHandler OnGameOver;

        /// <summary>
        /// (5) Occurs when user manually quits the game
        /// </summary>
        public event GameQuitHandler OnGameQuit;

        /// <summary>
        /// (6) Occurs when exits the game after game over
        /// </summary>
        public event GameExitHandler OnGameExit;

        //public event KeyboardPressedHandler OnKeyboardPressed;

        public event TimeChangedHandler OnGameTimeChanged;

        #endregion

        /// <summary>
        /// Triggers when the game is started.
        /// </summary>
        private void TriggerGameStarted()
        {
            OnGameStarted?.Invoke(gameData);
            SetTimer(true);
        }

        /// <summary>
        /// Triggers when the game is paused.
        /// </summary>
        private void TriggerGamePaused()
        {
            OnPaused?.Invoke(gameData);
        }

        /// <summary>
        /// Triggers when the game is resumed.
        /// </summary>
        private void TriggerGameResumed()
        {
            OnResumed?.Invoke(gameData);
        }

        /// <summary>
        /// Triggers the game over.
		/// </summary>
        private void TriggerGameOver(bool isSucceed)
        {
            // disables input
            isPaused = true;

            // then fires
            OnGameOver?.Invoke(gameData);
            // stops timer
            SetTimer(false);
        }

        /// <summary>
        /// Triggers when user leaves the game.
        /// </summary>
        private void TriggerGameQuit()
        {
            OnGameQuit?.Invoke(gameData);
        }

        private void TriggerGameExit()
        {
            OnGameExit?.Invoke(gameData);
        }

        /// <summary>
        /// Triggers when the game time is changed.
        /// </summary>
        private void TriggerGameTimeChanged()
        {
            OnGameTimeChanged?.Invoke(gameData.SecondsUsed);
        }

        #region Unity methods

        /// <summary>
        /// Ticks the tock
        /// </summary>
        private IEnumerator TickTock()
        {
            while (true)
            {
                yield return new WaitForSeconds(1);

                gameData.SecondsUsed += 1;
                TriggerGameTimeChanged();
            }
        }

        #endregion

        /// <summary>
        /// Initialize the gameplay
        /// </summary>
        public void Configure(MainGameData mainGameData)
        {
            gameData = mainGameData;

            gameBoard.InitGridWithRules(mainGameData, EndSession);
        }

        /// <summary>
        /// Sets timer on or off
        /// </summary>
        public void SetTimer(bool isOn)
        {
            if (isOn)
            {
                if (timerCoroutine == null)
                {
                    timerCoroutine = StartCoroutine(TickTock());
                }
                else
                {
                    Debug.LogWarning("Gameplay clock is already ticking. Resetting time.");
                }
            }
            else
            {
                if (timerCoroutine != null)
                {
                    StopCoroutine(timerCoroutine);
                    timerCoroutine = null;
                }
            }
        }

        public void OnKeyboardPressed(KeyButtonScript sender)
        {
            gameBoard.KeyboardInput(sender.KeyNumber);
        }

        /// <summary>
        /// Starts the session.
        /// </summary>
        public void StartSession()
        {
            gameData.SecondsUsed = 0;
            gameData.Score = 0;

            // triggers default values
            TriggerGameTimeChanged();

            // triggers the game
            TriggerGameStarted();
        }

        /// <summary>
        /// Pauses the session.
        /// </summary>
        public void PauseSession()
        {
            //if (timerCoroutine != null)
            //{
            //    StopCoroutine(timerCoroutine);
            //    timerCoroutine = null;
            //}
            SetTimer(false);
            isPaused = true;
            TriggerGamePaused();
        }

        /// <summary>
        /// Resumes the session.
        /// </summary>
        public void ResumeSession()
        {
            isPaused = false;
            TriggerGameResumed();

            //timerCoroutine = StartCoroutine(TickTock());
            SetTimer(true);
        }

        /// <summary>
        /// Finishes the game.
        /// </summary>
        public void EndSession(GridAnswer[] answers)
        {
            //gameData.TargetsCorrect = GameConstants.TargetCount - answers.Length;
            gameData.answers = answers;
            TriggerGameOver(true);
        }


        public void EndGameSession()
        {
            TriggerGameExit();
        }
        /// <summary>
        /// Forces to finish the game. No awards window will be shown.
        /// </summary>
        public void QuitSession()
        {
            TriggerGameQuit();
        }

        #region Creator

        public void ConfigureCreator(MainGameData mainGameData)
        {
            gameData = mainGameData;

            gameBoard.InitCreatorGridWithRules(mainGameData.Ruleset, EndSession);
        }



        #endregion
    }
}