using System.Collections;
using System.Collections.Generic;
using Peak.Speedoku.Scripts.Common;
using Peak.Speedoku.Scripts.Common.AnalyticsScripts;
using Peak.Speedoku.Scripts.Game.Gameplay;
using Peak.Speedoku.Scripts.ScenesLogic;
using Peak.Speedoku.Scripts.Settings;
using UnityEngine;

namespace Peak.Speedoku.Scripts.Game
{
    public partial class FtueController : MonoBehaviour //INFO: FTUE GAME ONE
    {
        private OverlayUISceneActivator overlay;
        private GridController grid;
        private KeyboardController keyboard;
        private GridSquareScript currentTarget;
        private ResponseQuestion currentQuestion;

        private enum ResponseQuestion
        {
            questionNone = 0,
            questionOne = 1,
            questionTwo = 2,
            questionThree = 3,
            questionFour = 4,
            questionFive = 5,
            questionSix = 6
        }

        public void StartGameOne(GridController gridController, KeyboardController keyboardController)
        {
            print("Start FTUE game One");
            overlay = SceneActivationBehaviour<OverlayUISceneActivator>.Instance;

            grid = gridController;
            keyboard = keyboardController;
            keyboard.SetKeyboardClickHandler(KeyboardFtueInput);
            grid.ResetFtueGrid();
            currentTarget = grid.GetFtueTargetAtIndex(34);

            overlay.ShowFtueSkipButton(true);
            overlay.ShowFtueGameplayMessage(Constants.FtueStrings.GameOneStringOne);
            overlay.SubscribeForFullScreenTap(GameOneStepOneTap);

            grid.HighlightRow(currentTarget.Index);
            grid.ShowArrowHighlight(27, FtueArrowDirection.Left);
            analyticsController.TutorialStepComplete(1, "OneRule");
        }

        private void GameOneStepOneTap()
        {
            print("Tapped game one step one");
            overlay.HideFtueMessages();
            overlay.ShowFtueGameplayMessage(Constants.FtueStrings.GameOneStringTwo);
            overlay.SubscribeForFullScreenTap(GameOneStepTwoTap);
            analyticsController.TutorialStepComplete(2, "OneOfEach");
        }

        private void GameOneStepTwoTap()
        {
            print("Tapped game one step two");
            overlay.HideFtueMessages();
            grid.ClearFtueHighlights();
            overlay.ShowFtueGameplayMessage(Constants.FtueStrings.GameOneStringThree);
            grid.ShowArrowHighlight(34, FtueArrowDirection.Down);
            overlay.SubscribeForFullScreenTap(GameOneStepThreeTap);
            analyticsController.TutorialStepComplete(3, "RowQuestion");
        }

        private void GameOneStepThreeTap()
        {
            print("Tapped game one step three");
            overlay.HideFtueMessages();
            grid.ClearFtueHighlights();
            overlay.ShowFtueGameplayMessage(Constants.FtueStrings.GameOneStringFour, showTapHand: false);
            currentQuestion = ResponseQuestion.questionOne;
            //keyboard.SetKeyboardEnabled(true);
            keyboard.ShowKeyboard();
            keyboard.HighlightKeyboard();
            analyticsController.TutorialStepComplete(4, "SelectNumber");
        }

        private void GameOneStepFourWrong()
        {
            print("Tapped game one step four wrong");
            overlay.HideFtueMessages();
            overlay.ShowFtueGameplayMessage(Constants.FtueStrings.GameStringWrongRow, showTapHand: false);
        }

        private void GameOneStepFourCorrect()
        {
            print("Tapped game one step four correct");
            overlay.HideFtueMessages();
            grid.ClearFtueHighlights();
            overlay.ShowFtueGameplayMessage(Constants.FtueStrings.GameOneStringFive);
            //keyboard.SetKeyboardEnabled(false);
            keyboard.HideKeyboard();
            currentQuestion = ResponseQuestion.questionNone;
            overlay.SubscribeForFullScreenTap(GameOneStepFiveTap);
            analyticsController.TutorialStepComplete(5, "FirstCorrect");
        }

        private void GameOneStepFiveTap()
        {
            print("Tapped game one step five");
            overlay.HideFtueMessages();
            grid.ClearFtueHighlights();
            overlay.ShowFtueGameplayMessage(Constants.FtueStrings.GameOneStringSix, showTapHand: false);
            keyboard.ShowKeyboard();//SetKeyboardEnabled(true);
            keyboard.HighlightKeyboard();
            currentQuestion = ResponseQuestion.questionTwo;
            grid.ResetFtueGrid();
            currentTarget = grid.GetFtueTargetAtIndex(63);
            grid.HighlightColumn(currentTarget.Index);
            grid.ShowArrowHighlight(0, FtueArrowDirection.Up);
            analyticsController.TutorialStepComplete(6, "ColQuestion");
        }

        private void GameOneStepFiveWrong()
        {
            print("Tapped game one step five wrong");
            overlay.HideFtueMessages();
            overlay.ShowFtueGameplayMessage(Constants.FtueStrings.GameStringWrongCol, showTapHand: false);
        }

        private void GameOneStepFiveCorrect()
        {
            print("Tapped game one step five correct");
            overlay.HideFtueMessages();
            grid.ClearFtueHighlights();
            overlay.ShowFtueGameplayMessage(Constants.FtueStrings.GameOneStringSeven);
            keyboard.HideKeyboard();//.SetKeyboardEnabled(false);
            currentQuestion = ResponseQuestion.questionNone;
            overlay.SubscribeForFullScreenTap(GameOneStepSixTap);
            analyticsController.TutorialStepComplete(7, "SecondCorrect");
        }

        private void GameOneStepSixTap()
        {
            print("Tapped game one step six");
            overlay.HideFtueMessages();
            grid.ClearFtueHighlights();
            overlay.ShowFtueGameplayMessage(Constants.FtueStrings.GameOneStringEight);

            grid.ResetFtueGrid();
            currentTarget = grid.GetFtueTargetAtIndex(39);
            grid.HighlightSquare(currentTarget.Index);

            overlay.SubscribeForFullScreenTap(GameOneStepSevenTap);
            analyticsController.TutorialStepComplete(8, "SquareRule");
        }

        private void GameOneStepSevenTap()
        {
            print("Tapped game one step seven");
            overlay.HideFtueMessages();
            grid.ClearFtueHighlights();
            overlay.ShowFtueGameplayMessage(Constants.FtueStrings.GameOneStringNine, showTapHand: false);
            keyboard.ShowKeyboard();//.SetKeyboardEnabled(true);
            keyboard.HighlightKeyboard();
            currentQuestion = ResponseQuestion.questionThree;
            grid.HighlightSquare(currentTarget.Index);
            grid.ShowArrowHighlight(39, FtueArrowDirection.Left);
            analyticsController.TutorialStepComplete(9, "SquareQuestion");
        }

        private void GameOneStepSevenWrong()
        {
            print("Tapped game one step seven wrong");
            overlay.HideFtueMessages();
            overlay.ShowFtueGameplayMessage(Constants.FtueStrings.GameStringWrongSqr, showTapHand: false);
        }

        private void GameOneStepSevenCorrect()
        {
            print("Tapped game one step seven correct");
            overlay.HideFtueMessages();
            grid.ClearFtueHighlights();
            overlay.ShowFtueGameplayMessage(Constants.FtueStrings.GameOneStringTen);
            keyboard.HideKeyboard(); //.SetKeyboardEnabled(false);
            currentQuestion = ResponseQuestion.questionNone;
            overlay.SubscribeForFullScreenTap(GameOneStepEightTap);
            analyticsController.TutorialStepComplete(10, "GameOneComplete");
        }

        private void GameOneStepEightTap()
        {
            //End Game HERE
            overlay.HideFtueMessages();
            grid.ClearFtueHighlights();
            grid.CompleteFtueGame();
            overlay.ShowFtueSkipButton(false);
            ftueInformation.IsGameOnePassed = true;
            serverController.PersistFtue(ftueInformation);
        }

        #region - Helpers

        public void KeyboardFtueInput(int number)
        {
            currentTarget.UpdateGridNumber(number);
#if PLATFORM_IOS
            iOSHapticFeedbackHelper.OnSelection();
#endif
            if (grid.ValidateFtueTarget(currentTarget))
            {
                HandleCorrectAnswer();
            }
            else
            {
                HandleIncorrectAnswer();
            }
        }

        private void HandleCorrectAnswer()
        {
            //TODO add correct sound
            LocalisationController.Instance.PlayAudioClip(Settings.Autogenerated.SoundSettingsKey.CorrectAnswer);
            switch (currentQuestion)
            {
                case ResponseQuestion.questionOne:
                    GameOneStepFourCorrect();
                    break;
                case ResponseQuestion.questionTwo:
                    GameOneStepFiveCorrect();
                    break;
                case ResponseQuestion.questionThree:
                    GameOneStepSevenCorrect();
                    break;
                case ResponseQuestion.questionFour:
                    GameTwoStepSixCorrect();
                    break;
                case ResponseQuestion.questionFive:
                    GameTwoStepTwelveCorrect();
                    break;
                case ResponseQuestion.questionSix:
                    GameTwoStepThirteenCorrect();
                    break;
            }
        }

        private void HandleIncorrectAnswer()
        {
            switch (currentQuestion)
            {
                case ResponseQuestion.questionOne:
                    GameOneStepFourWrong();
                    break;
                case ResponseQuestion.questionTwo:
                    GameOneStepFiveWrong();
                    break;
                case ResponseQuestion.questionThree:
                    GameOneStepSevenWrong();
                    break;
                case ResponseQuestion.questionFour:
                    GameTwoStepSixWrong();
                    break;
                case ResponseQuestion.questionFive:
                    GameTwoStepTwelveWrong();
                    break;
                case ResponseQuestion.questionSix:
                    GameTwoStepThirteenWrong();
                    break;
            }
        }

        #endregion
    }
}
