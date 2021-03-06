﻿using System.Collections;
using System.Collections.Generic;
using Peak.Speedoku.Scripts.Common;
using Peak.Speedoku.Scripts.Common.AnalyticsScripts;
using Peak.Speedoku.Scripts.ScenesLogic;
using Peak.Speedoku.Scripts.Settings;
using Peak.Speedoku.Scripts.Game.Gameplay;
using UnityEngine;
using Peak.Speedoku.Scripts.Autogenerated;

namespace Peak.Speedoku.Scripts.Game
{
    public struct ProgressData
    {
        public int progress;
        public int locationLimit;
        public int collection;
    }

    public class GameController : MonoBehaviour
    {
        [SerializeField]
        private GlobalSettings globalSettings;

        [SerializeField]
        private InputController inputController;

        [SerializeField]
        private ServerController serverController;

        [SerializeField]
        private AdsController adsController;

        [SerializeField]
        private NotificationController notificationController;

        [SerializeField]
        private AnalyticsController analyticsController;

        [SerializeField]
        private SessionScript sessionScript;

        [SerializeField]
        private FtueController ftueController;

        public Player Player => player;
        private Player player;

        [Header("Game Level Data")]
        [SerializeField] private int loopStartIndex = 0;
        [SerializeField] private int levelMaxPoints = 300;
        public void Initialise()
        {
            // load and setup references
            serverController.RunMigrations();
            player = serverController.LoadPlayerData(globalSettings);
            //adsController.Initialize(player.Guid);
            ftueController.InitialiseFtue();
            // Register user - if new, send device data
            analyticsController.RegisterUser(player);
            if (player.IsNew)
            {
                //analyticsController.SendDeviceData();
                analyticsController.FirstInteraction();
                player.IsNew = false;
                serverController.PersistPlayerProgress(player);
            }
            print($"SESSION INIT - Player level: {player.MainGameLevelIndex}");
            sessionScript.SessionStarted += GameStartedHandler;
            sessionScript.SessionPaused += GamePausedHandler;
            sessionScript.SessionResumed += GameResumedHandler;
            sessionScript.SessionFinished += GameFinishedHandler;
            sessionScript.SessionQuit += GameQuitHandler;
        }

        #region 
        public RuleSettings GetRules()
        {
            if (ftueController.ShouldPlayFirstGame)
            {
                return ftueController.FtueGameOne;
            }
            if (ftueController.ShouldPlaySecondGame)
            {
                return ftueController.FtueGameTwo;
            }

            if (player.MainGameLevelIndex < loopStartIndex && player.MainGameLevelIndex < globalSettings.RulesList.Count)
            {
                return globalSettings.RulesList[player.MainGameLevelIndex];
            }
            else
            {
                int loopRuleCount = globalSettings.RulesList.Count - loopStartIndex;
                int remainder =(player.MainGameLevelIndex - loopStartIndex) % loopRuleCount;
                print($"Player index: {player.MainGameLevelIndex} Remainder {remainder}");
                return globalSettings.RulesList[remainder];
            }
        }

        public ProgressData GetProgressInfo()
        {
            return GetLocationInfo(player.ProgressCounter);
        }

        public void UpdatePlayerProgress()
        {
            player.ProgressCounter += 1;
            serverController.PersistPlayerProgress(player);
        }

        private ProgressData GetLocationInfo(int index)
        {
            ProgressData collectionData = new ProgressData();
            int[] locationBoundaries = globalSettings.locationBoundaries;
            int locationCounter = 0;
            int runningBoundary = 0;
            int currentProgress;
            for (int i = 0; i < locationBoundaries.Length; i++)
            {
                int currentBoundary = locationBoundaries[i];
                currentProgress = runningBoundary - index;
                runningBoundary += currentBoundary;
                
                if (index < runningBoundary)
                {
                    collectionData.progress = Mathf.Abs(currentProgress);
                    collectionData.locationLimit = currentBoundary;
                    collectionData.collection = locationCounter;
                    break;
                }
                locationCounter += 1;
            }
            while (index >= runningBoundary)
            {
                currentProgress = runningBoundary - index;
                runningBoundary += locationBoundaries[locationBoundaries.Length - 1];
                collectionData.progress = Mathf.Abs(currentProgress);
                collectionData.locationLimit = locationBoundaries[locationBoundaries.Length - 1];
                collectionData.collection = locationCounter;
                locationCounter += 1;
            }
            return collectionData;
        }

        public LocationSetting BackgroundForCollection(int collection)
        {
            int index = collection >= globalSettings.LocationSettings.Length ? globalSettings.LocationSettings.Length % collection : collection;
            //print($"Background for collection: {index} collection: {collection} ({globalSettings.LocationSettings.Length} %  = {globalSettings.LocationSettings.Length % collection})");
            return globalSettings.LocationSettings[index];
        }

        public int GetDeductionScore(int secondsUsed, int correct)
        {
            bool allCorrect = correct == 3;
            if (allCorrect)
            {
                int score = Mathf.Max(levelMaxPoints - secondsUsed, 10);
                return score * correct;
            }
            return correct;
        }

        public bool IsInFtue()
        {
            return ftueController.ShouldPlayFirstGame || ftueController.ShouldPlaySecondGame;
        }

        public void ResetFtueHandler()
        {
            ftueController.ResetFtueHandler();
        }

        public void SkipFtueHandler()
        {
            ftueController.SkipFtueHandler();
            analyticsController.TutorialStepSkipped();
        }

        public bool ShouldShowFeedbackFormAgain()
        {
            return ftueController.ShouldShowFeedbackAgain;
        }

        public void FeedbackGivenAcccepted()
        {
            ftueController.FeedbackWasGiven();
        }

        #endregion

        public void SetupGameHandler(MainGameData gameData)
        {
            print("[GC] Setup game handler");
            if (ftueController.ShouldPlayFirstGame)
            {
                print("Setup FTUE GAME ONE");
                sessionScript.SetupFtueGame(gameData);
            }
            else if (ftueController.ShouldPlaySecondGame)
            {
                print("Setup FTUE GAME TWO");
                sessionScript.SetupFtueGame(gameData);
            }
            else
            {
                print("Setup NORMAL GAME");
                sessionScript.SetupGame(gameData);
            }
        }

        //public void SetupFtueGameHandler(MainGameData gameData)
        //{
        //    print("[GC] Setup FTUE game handler");
        //    sessionScript.SetupGame(gameData);
        //}

        public bool ShouldStartWithFtuePopup()
        {
            return ftueController.ShouldShowFtuePopup;
        }

        public void FtuePopupShown()
        {
            ftueController.FtuePopupClosed();
        }

        public void StartFtueGame(MainGameData gameData, GridController gridController, KeyboardController keyboardController)
        {
            ftueController.StartFtueGame(gameData, gridController, keyboardController);
        }

        public void StartGameHandler()
        {
            print("[GC] Start game handler");
            //sessionScript.SetupGame(gameData);
            
            sessionScript.StartGame();
        }

        #region Session handlers
        private void GameStartedHandler(MainGameData gameData)
        {
            print("[GC] Started");
            analyticsController.SendGameStart(gameData);
        }

        private void GamePausedHandler(MainGameData gameData)
        {
            print("[GC] Paused");
        }

        private void GameResumedHandler(MainGameData gameData)
        {
            print("[GC] Resumed");
        }

        private void GameQuitHandler(MainGameData gameData)
        {
            print("[GC] Quit");
            analyticsController.SendGameQuit(gameData);
        }

        private void GameFinishedHandler(MainGameData gameData)
        {
            print($"[GC] Finished Time: {gameData.SecondsUsed} ");
            analyticsController.SendGameFinished(gameData);
            StartCoroutine(FinishWithDelay(gameData));
        }

        private IEnumerator FinishWithDelay(MainGameData gameData)
        {
            //todo: add question mark animation
            //todo: add target flash
            yield return new WaitForSeconds(1f);
            SceneActivationBehaviour<MainMenuActivator>.Instance.GameFinished(gameData);
        }

        //private void GameExitHandler(MainGameData gameData)
        //{
        //    print($"[GC] Exited Time: {gameData.SecondsUsed}");
            
        //    if (player.MainGameLevelIndex < globalSettings.RulesList.Count - 1)
        //    {
        //        player.MainGameLevelIndex += 1;
        //    }
        //    else
        //    {
        //        player.MainGameLevelIndex = 0;
        //    }
        //    serverController.PersistPlayerProgress(player);
        //}

        #endregion
    }
}

