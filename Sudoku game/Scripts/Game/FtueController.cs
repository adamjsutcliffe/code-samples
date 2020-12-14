using System.Collections;
using System.Collections.Generic;
using Peak.Speedoku.Scripts.Common;
using Peak.Speedoku.Scripts.Common.AnalyticsScripts;
using Peak.Speedoku.Scripts.Game.Gameplay;
using Peak.Speedoku.Scripts.Settings;
using UnityEngine;

namespace Peak.Speedoku.Scripts.Game
{
    public partial class FtueController : MonoBehaviour
    {
        [SerializeField] private ServerController serverController;
        [SerializeField] private AnalyticsController analyticsController;

        [Header("FTUE Levels")]
        [SerializeField] private RuleSettings ftueGameOne;
        [SerializeField] private RuleSettings ftueGameTwo;

        // Some FTUE information
        private FtueInformation ftueInformation;

        public bool ShouldShowFtuePopup => ftueInformation != null && !ftueInformation.IsPopupPassed;
        public bool ShouldPlayFirstGame => ftueInformation != null && !ftueInformation.IsGameOnePassed;
        public bool ShouldPlaySecondGame => ftueInformation != null && !ftueInformation.IsGameTwoPassed;
        public bool ShouldShowFeedbackAgain => ftueInformation != null && !ftueInformation.wasFeedbackGiven;

        public RuleSettings FtueGameOne => ftueGameOne;
        public RuleSettings FtueGameTwo => ftueGameTwo;

        public void InitialiseFtue()
        {
            ftueInformation = serverController.LoadFtueInformation();
            serverController.PersistFtue(ftueInformation);
        }

        public void FtuePopupClosed()
        {
            ftueInformation.IsPopupPassed = true;
            serverController.PersistFtue(ftueInformation);
            //analyticsController.TutorialFinished();
        }

        public void GdprAccepted()
        {
            ftueInformation.isGdprNotificationShown = true;
            serverController.PersistFtue(ftueInformation);
        }

        public void NotificationsAccepted()
        {
            ftueInformation.isNotificationPopupShown = true;
            serverController.PersistFtue(ftueInformation);
        }

        public void ResetFtueHandler()
        {
            ftueInformation.IsGameOnePassed = false;
            ftueInformation.IsGameTwoPassed = false;
            serverController.PersistFtue(ftueInformation);
        }

        public void SkipFtueHandler()
        {
            grid?.ClearFtueHighlights();
            ftueInformation.IsGameOnePassed = true;
            ftueInformation.IsGameTwoPassed = true;
            serverController.PersistFtue(ftueInformation);
        }

        public void StartFtueGame(MainGameData gameData, GridController gridController, KeyboardController keyboardController)
        {
            if (gameData.Ruleset.levelData.Equals(ftueGameOne.levelData))
            {
                analyticsController.TutorialStarted();
                print("START FTUE LEVEL ONE");
                StartGameOne(gridController, keyboardController);
            }
            else if (gameData.Ruleset.levelData.Equals(ftueGameTwo.levelData))
            {
                print("START FTUE LEVEL TWO");
                StartGameTwo(gridController, keyboardController);
            }
            else
            {
                print("UNKNOWN FTUE LEVEL!!!");
            }
        }

        public void FeedbackWasGiven()
        {
            ftueInformation.wasFeedbackGiven = true;
            serverController.PersistFtue(ftueInformation);
        }
    }
}
