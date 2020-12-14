﻿using System.Collections;
using System.Collections.Generic;
using Peak.Speedoku.Scripts.Common;
using Peak.Speedoku.Scripts.Common.AnalyticsScripts;
using Peak.Speedoku.Scripts.Game.Gameplay;
using Peak.Speedoku.Scripts.ScenesLogic;
using Peak.Speedoku.Scripts.Settings;
using UnityEngine;

namespace Peak.Speedoku.Scripts.Game
{
    public partial class FtueController : MonoBehaviour //INFO: FTUE GAME TWO
    {
        public void StartGameTwo(GridController gridController, KeyboardController keyboardController)
        {
            print("Start FTUE game TWO");
            overlay = SceneActivationBehaviour<OverlayUISceneActivator>.Instance;
            grid = gridController;
            keyboard = keyboardController;
            keyboard.SetKeyboardClickHandler(KeyboardFtueInput);
            grid.ResetFtueGrid();
            currentTarget = grid.GetFtueTargetAtIndex(18);

            overlay.ShowFtueSkipButton(true);
            overlay.ShowFtueGameplayMessage(Constants.FtueStrings.GameTwoStringOne);
            overlay.SubscribeForFullScreenTap(GameTwoStepOneTap);
            grid.HighLightBigSquare(currentTarget.Index);
            analyticsController.TutorialStepComplete(11, "NewTechnique");
        }

        private void GameTwoStepOneTap()
        {
            print("Tapped game two step one");
            
            overlay.HideFtueMessages();
            overlay.ShowFtueGameplayMessage(Constants.FtueStrings.GameTwoStringTwo);
            overlay.SubscribeForFullScreenTap(GameTwoStepTwoTap);
            analyticsController.TutorialStepComplete(12, "MissingMany");
        }

        private void GameTwoStepTwoTap()
        {
            print("Tapped game two step two");
            overlay.HideFtueMessages();
            
            overlay.ShowFtueGameplayMessage(Constants.FtueStrings.GameTwoStringThree, showTapHand: false);
            StartCoroutine(ShowStepTwoHighlightAfterDelay());
        }

        private IEnumerator ShowStepTwoHighlightAfterDelay()
        {
            yield return new WaitForSeconds(2.0f);
            grid.ClearFtueHighlights();
            grid.ShowCircleHighlight(12);
            grid.ShowCircleHighlight(7);
            LocalisationController.Instance.PlayAudioClip(Settings.Autogenerated.SoundSettingsKey.FtueHint01);
            yield return new WaitForSeconds(0.5f);
            overlay.AddTapHandToMessage();
            overlay.SubscribeForFullScreenTap(GameTwoStepThreeTap);
            analyticsController.TutorialStepComplete(13, "ClueFives");
        }

        private void GameTwoStepThreeTap()
        {
            print("Tapped game two step three");
            overlay.HideFtueMessages();
            
            overlay.ShowFtueGameplayMessage(Constants.FtueStrings.GameTwoStringFour, showTapHand: false);
            StartCoroutine(ShowStepThreeHighlightAfterDelay());
        }

        private IEnumerator ShowStepThreeHighlightAfterDelay()
        {
            yield return new WaitForSeconds(2.0f);
            //grid.ClearFtueHighlights();
            grid.ShowCircleArrowHighlight(12, 3, FtueArrowDirection.Left);
            grid.ShowCircleArrowHighlight(7, 7, FtueArrowDirection.Left);
            LocalisationController.Instance.PlayAudioClip(Settings.Autogenerated.SoundSettingsKey.FtueHint01);
            overlay.AddTapHandToMessage();
            overlay.SubscribeForFullScreenTap(GameTwoStepFourTap);
            analyticsController.TutorialStepComplete(14, "OnePerColumn");
        }

        private void GameTwoStepFourTap()
        {
            print("Tapped game two step four");
            overlay.HideFtueMessages();
            
            overlay.ShowFtueGameplayMessage(Constants.FtueStrings.GameTwoStringFive, showTapHand: false);
            
            StartCoroutine(ShowStepFourHighlightAfterDelay());
        }

        private IEnumerator ShowStepFourHighlightAfterDelay()
        {
            yield return new WaitForSeconds(2.0f);
            grid.ClearFtueHighlights();
            grid.ShowIgnoreHighlight(2, 5);
            grid.ShowIgnoreHighlight(9, 5);
            grid.ShowIgnoreHighlight(10, 5);
            LocalisationController.Instance.PlayAudioClip(Settings.Autogenerated.SoundSettingsKey.FtueHint01);
            yield return new WaitForSeconds(0.5f);
            overlay.AddTapHandToMessage();
            overlay.SubscribeForFullScreenTap(GameTwoStepFiveTap);
            analyticsController.TutorialStepComplete(15, "RowFives");
        }

        private void GameTwoStepFiveTap()
        {
            print("Tapped game two step five");
            overlay.HideFtueMessages();
            overlay.ShowFtueGameplayMessage(Constants.FtueStrings.GameTwoStringSix);
            overlay.SubscribeForFullScreenTap(GameTwoStepSixTap);
            analyticsController.TutorialStepComplete(16, "OneFive");
        }

        private void GameTwoStepSixTap()
        {
            print("Tapped game two step six");
            overlay.HideFtueMessages();
            grid.ClearFtueHighlights();
            overlay.ShowFtueGameplayMessage(Constants.FtueStrings.GameTwoStringSeven, showTapHand: false);
            currentQuestion = ResponseQuestion.questionFour;
            keyboard.ShowKeyboard();
            keyboard.HighlightKeyboard();
            analyticsController.TutorialStepComplete(17, "FivesQuestion");
        }

        private void GameTwoStepSixWrong()
        {
            print("Tapped game two step six wrong");
            overlay.HideFtueMessages();
            grid.ClearFtueHighlights();
            overlay.ShowFtueGameplayMessage(Constants.FtueStrings.GameStringWrongSqr, showTapHand: false);
        }

        private void GameTwoStepSixCorrect()
        {
            print("Tapped game two step six correct");
            overlay.HideFtueMessages();
            grid.ClearFtueHighlights();
            overlay.ShowFtueGameplayMessage(Constants.FtueStrings.GameTwoStringEight);
            keyboard.HideKeyboard();
            currentQuestion = ResponseQuestion.questionNone;
            grid.ResetFtueGrid();
            currentTarget = grid.GetFtueTargetAtIndex(67);
            grid.HighLightBigSquare(currentTarget.Index);
            overlay.SubscribeForFullScreenTap(GameTwoStepSevenTap);
            analyticsController.TutorialStepComplete(18, "FourthCorrect");
        }

        private void GameTwoStepSevenTap()
        {
            print("Tapped game one step seven");
            overlay.HideFtueMessages();
            grid.ClearFtueHighlights();
            overlay.ShowFtueGameplayMessage(Constants.FtueStrings.GameTwoStringNine, showTapHand: false);
            StartCoroutine(ShowStepSevenHighlightAfterDelay());
        }

        private IEnumerator ShowStepSevenHighlightAfterDelay()
        {
            yield return new WaitForSeconds(2.0f);

            grid.ShowCircleHighlight(30);
            grid.ShowCircleHighlight(54);
            grid.ShowCircleHighlight(79);
            LocalisationController.Instance.PlayAudioClip(Settings.Autogenerated.SoundSettingsKey.FtueHint01);
            yield return new WaitForSeconds(0.5f);
            overlay.AddTapHandToMessage();
            overlay.SubscribeForFullScreenTap(GameTwoStepEightTap);
            analyticsController.TutorialStepComplete(19, "LookNines");
        }

        private void GameTwoStepEightTap()
        {
            print("Tapped game one step eight");
            overlay.HideFtueMessages();
            overlay.ShowFtueGameplayMessage(Constants.FtueStrings.GameTwoStringTen);
            overlay.SubscribeForFullScreenTap(GameTwoStepNineTap);
            analyticsController.TutorialStepComplete(20, "CluesNines");
        }

        private void GameTwoStepNineTap()
        {
            print("Tapped game one step nine");
            overlay.HideFtueMessages();
            overlay.ShowFtueGameplayMessage(Constants.FtueStrings.GameTwoStringEleven, showTapHand: false);
            //overlay.SubscribeForFullScreenTap(GameTwoStepTenTap);
            StartCoroutine(ShowStepNineHighlightAfterDelay());
        }

        private IEnumerator ShowStepNineHighlightAfterDelay()
        {
            yield return new WaitForSeconds(1.0f);

            grid.ShowCircleArrowHighlight(30, 5, FtueArrowDirection.Down);
            LocalisationController.Instance.PlayAudioClip(Settings.Autogenerated.SoundSettingsKey.FtueHint01);
            yield return new WaitForSeconds(0.5f);
            grid.ShowCircleArrowHighlight(54, 8, FtueArrowDirection.Right);
            LocalisationController.Instance.PlayAudioClip(Settings.Autogenerated.SoundSettingsKey.FtueHint02);
            yield return new WaitForSeconds(0.5f);
            grid.ShowCircleArrowHighlight(79, 7, FtueArrowDirection.Left);
            LocalisationController.Instance.PlayAudioClip(Settings.Autogenerated.SoundSettingsKey.FtueHint03);
            yield return new WaitForSeconds(0.5f);
            overlay.AddTapHandToMessage();
            overlay.SubscribeForFullScreenTap(GameTwoStepTenTap);
            analyticsController.TutorialStepComplete(21, "ExcludeNines");
        }

        private void GameTwoStepTenTap()
        {
            print("Tapped game one step ten");
            overlay.HideFtueMessages();
            overlay.ShowFtueGameplayMessage(Constants.FtueStrings.GameTwoStringTwelve, showTapHand: false);
            StartCoroutine(ShowStepTenHighlightAfterDelay());
        }

        private IEnumerator ShowStepTenHighlightAfterDelay()
        {
            yield return new WaitForSeconds(1.0f);
            grid.ClearFtueHighlights();
            grid.ShowIgnoreHighlight(57, 9);
            grid.ShowIgnoreHighlight(59, 9);
            grid.ShowIgnoreHighlight(66, 9);
            grid.ShowIgnoreHighlight(76, 9);
            grid.ShowIgnoreHighlight(77, 9);
            LocalisationController.Instance.PlayAudioClip(Settings.Autogenerated.SoundSettingsKey.FtueHint01);
            yield return new WaitForSeconds(0.5f);
            overlay.AddTapHandToMessage();
            overlay.SubscribeForFullScreenTap(GameTwoStepElevenTap);
            analyticsController.TutorialStepComplete(22, "NotNines");
        }

        private void GameTwoStepElevenTap()
        {
            print("Tapped game one step eleven");
            overlay.HideFtueMessages();
            overlay.ShowFtueGameplayMessage(Constants.FtueStrings.GameTwoStringThirteen);
            overlay.SubscribeForFullScreenTap(GameTwoStepTwelveTap);
            analyticsController.TutorialStepComplete(23, "OnlyOneNine");
        }

        private void GameTwoStepTwelveTap()
        {
            print("Tapped game one step twelve");
            overlay.HideFtueMessages();
            overlay.ShowFtueGameplayMessage(Constants.FtueStrings.GameTwoStringFourteen, showTapHand: false);
            grid.ClearFtueHighlights();
            currentQuestion = ResponseQuestion.questionFive;
            keyboard.ShowKeyboard();
            keyboard.HighlightKeyboard();
            analyticsController.TutorialStepComplete(24, "WhichNine");
        }

        private void GameTwoStepTwelveWrong()
        {
            print("Tapped game two step twelve wrong");
            overlay.HideFtueMessages();
            grid.ClearFtueHighlights();
            overlay.ShowFtueGameplayMessage(Constants.FtueStrings.GameStringWrongSqr, showTapHand: false);
        }

        private void GameTwoStepTwelveCorrect()
        {
            print("Tapped game two step twelve correct");
            overlay.HideFtueMessages();
            grid.ClearFtueHighlights();
            overlay.ShowFtueGameplayMessage(Constants.FtueStrings.GameTwoStringFifteen);
            keyboard.HideKeyboard();
            currentQuestion = ResponseQuestion.questionNone;
            grid.ResetFtueGrid();
            currentTarget = grid.GetFtueTargetAtIndex(16);
            grid.HighLightBigSquare(currentTarget.Index);
            overlay.SubscribeForFullScreenTap(GameTwoStepThirteenTap);
            analyticsController.TutorialStepComplete(25, "FifthCorrect");
        }

        private void GameTwoStepThirteenTap()
        {
            print("Tapped game one step thirteen");
            overlay.HideFtueMessages();
            overlay.ShowFtueGameplayMessage(Constants.FtueStrings.GameTwoStringSixteen, showTapHand: false);
            currentQuestion = ResponseQuestion.questionSix;
            keyboard.ShowKeyboard();
            keyboard.HighlightKeyboard();
            analyticsController.TutorialStepComplete(26, "WhichMissing");
        }

        private void GameTwoStepThirteenWrong()
        {
            print("Tapped game two step six wrong");
            overlay.HideFtueMessages();
            grid.ClearFtueHighlights();
            overlay.ShowFtueGameplayMessage(Constants.FtueStrings.GameTwoStringSeventeen, showTapHand: false);
        }

        private void GameTwoStepThirteenCorrect()
        {
            print("Tapped game two step six correct");
            overlay.HideFtueMessages();
            grid.ClearFtueHighlights();
            overlay.ShowFtueGameplayMessage(Constants.FtueStrings.GameTwoStringEighteen);
            keyboard.HideKeyboard();
            currentQuestion = ResponseQuestion.questionNone;
            overlay.SubscribeForFullScreenTap(GameTwoStepFourteenTap);
            analyticsController.TutorialStepComplete(27, "SixthCorrect");
        }

        private void GameTwoStepFourteenTap()
        {
            print("Tapped game one step eleven");
            overlay.HideFtueMessages();
            overlay.ShowFtueGameplayMessage(Constants.FtueStrings.GameTwoStringNineteen);
            overlay.SubscribeForFullScreenTap(GameTwoStepFifteenTap);
            analyticsController.TutorialStepComplete(28, "GameTwoComplete");
        }

        private void GameTwoStepFifteenTap()
        {
            print("Tapped game one step eleven");
            overlay.HideFtueMessages();
            grid.ClearFtueHighlights();
            grid.CompleteFtueGame();
            overlay.ShowFtueSkipButton(false);
            ftueInformation.IsGameTwoPassed = true;
            analyticsController.TutorialFinished();
            serverController.PersistFtue(ftueInformation);
        }
    }
}