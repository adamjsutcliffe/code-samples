﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Peak.QuixelLogic.Scripts.Common;
using Peak.QuixelLogic.Scripts.Autogenerated;
using Peak.QuixelLogic.Scripts.Game.Gameplay;
using TMPro;
using Peak.QuixelLogic.Scripts.Game;
using JetBrains.Annotations;
using UnityEngine.UI;
using Peak.QuixelLogic.Scripts.Common.AnalyticsScripts;
using Peak.QuixelLogic.Scripts.Common.Localisation;
using System.Text;

namespace Peak.QuixelLogic.Scripts.ScenesLogic
{
    public class PostGameSceneActivator : SceneActivationBehaviour<PostGameSceneActivator>
    {
        public GameObject boardHolder;

        [SerializeField]
        private TextMeshProUGUI coinReward;

        [SerializeField]
        private ParticleSystem coinFX;

        private int coinRewardAmount;

        private int starRewardAmount;

        [SerializeField]
        private GameObject feedbackMessageRoot;

        [SerializeField]
        private TextMeshProUGUI feedbackMessageText;

        #region Score banner

        [SerializeField]
        private GameObject scoreBanner;

        [SerializeField]
        private Animator scoreBannerAnimator;

        [SerializeField]
        private TopBannerScript bannerScript;

        #endregion

        [SerializeField]
        private Animator puzzleImageAnimator;

        [SerializeField]
        private Animator puzzleHeaderAnimator;

        [SerializeField]
        private Image starburstImage;

        [SerializeField]
        private Animator endGameAlien;

        [SerializeField]
        private GameObject rewardedVideoButton;

        [SerializeField]
        private TextMeshProUGUI rewardedVideoButtonText;

        public int temporaryPlayerCoins { get; set; }

        private int postGameCoinMultiplier;

        public override void Initialize()
        {
            base.Initialize();
            postGameCoinMultiplier = SceneActivationBehaviour<GameLogicActivator>.Instance.GameController.GlobalSettings.Coins.PostGameCoinMultiplier;
        }

        public override void Show()
        {
            base.Show();
        }

        public override void Hide()
        {
            base.Hide();
        }

        public void SetBoardHolderScale(Vector3 scale)
        {
            boardHolder.transform.localScale = scale;
        }

        public void ShowBoardPostGame(MainGameData gameData, bool nextCollectionUnlocked = false) // if location has been unlocked
        {
            GameController gameController = SceneActivationBehaviour<GameLogicActivator>.Instance.GameController;

            SceneActivationBehaviour<UICoinCounterActivator>.Instance.SetButtons(false);

            starRewardAmount = gameData.StarScore;

            if (gameData.Replay)
            {
                coinRewardAmount = 10;
            }
            else
            {
                coinRewardAmount = GetFeedbackCoins(gameData.StarScore);
                SceneActivationBehaviour<CollectionScreenActivator>.Instance.ShowHidePlayButton(false);
            }

            temporaryPlayerCoins = gameController.Player.Coins + coinRewardAmount;

            //ParticleSystem.EmissionModule coinFxEmission = coinFX.emission;
            //coinFxEmission.rateOverTime = GetCoinEmissionRate(coinRewardAmount);

            rewardedVideoButton.SetActive(false);

            bool shouldGrantRewardedVideoOption = gameController.GameSessionCounter % 3 == 0 || gameController.Player.ShouldShowPostGameRewardedVideo;
            if (gameController.Player.FtuePassed && shouldGrantRewardedVideoOption)
            {
                if (gameData.StarScore.Equals(3))
                {
                    rewardedVideoButtonText.text = string.Concat(LocalisationSystem.GetLocalisedValue("coinsx"), postGameCoinMultiplier.ToString());
                    rewardedVideoButton.SetActive(true);
                    gameController.Player.ShouldShowPostGameRewardedVideo = false;
                }
                else
                {
                    gameController.Player.ShouldShowPostGameRewardedVideo = true;
                }
            }

            feedbackMessageText.text = GetFeedbackMessage(gameData.StarScore);
            coinReward.text = "+" + coinRewardAmount.ToString();

            puzzleImageAnimator.SetTrigger("PostGameAnimation");
            puzzleHeaderAnimator.SetTrigger("PostGameAnimation");

            PersistCoins(coinRewardAmount, CoinSourceType.PuzzleSolved);

            if (nextCollectionUnlocked)
            {
                PersistCoins(50, CoinSourceType.NewLocationUnlocked);
                SceneActivationBehaviour<GameLogicActivator>.Instance.GameController.NewLocationRewardClaimed();
            }

            return;
        }

        private int GetFeedbackCoins(int starsAchieved)
        {
            return starsAchieved.Equals(3)
                ? SceneActivationBehaviour<GameLogicActivator>.Instance.GameController.GlobalSettings.Coins.CoinRewardThreeStarCount
                : starsAchieved.Equals(2) ? SceneActivationBehaviour<GameLogicActivator>.Instance.GameController.GlobalSettings.Coins.CoinRewardTwoStarCount
                : SceneActivationBehaviour<GameLogicActivator>.Instance.GameController.GlobalSettings.Coins.CoinRewardOneStarCount;
        }

        private string GetFeedbackMessage(int starsAchieved)
        {
            return starsAchieved.Equals(3)
                ? GameConstants.MainGame.FeedbackMessages.Incredible
                : starsAchieved.Equals(2) ? GameConstants.MainGame.FeedbackMessages.Great : GameConstants.MainGame.FeedbackMessages.Good;
        }

        //private int GetCoinEmissionRate(int rewardAmount)
        //{
        //    switch (rewardAmount)
        //    {
        //        case 25:
        //            return 16;
        //        case 10:
        //            return 10;
        //        case 5:
        //            return 4;
        //        case 1:
        //            return 1;
        //        default:
        //            return 1;
        //    }
        //}

        private void PersistCoins(int coinAmount, CoinSourceType coinSource)
        {
            SceneActivationBehaviour<GameLogicActivator>.Instance.GameController.CoinsGrantedHandler(coinAmount, coinSource);
        }

        public void PostGameRewardedVideo()
        {
            SceneActivationBehaviour<GameLogicActivator>.Instance.GameController.CallPostGameRewardedVideo(() =>
            {

            }, () =>
            {
                SceneActivationBehaviour<PopupRewardedVideoActivator>.Instance.ShowVideoErrorPopup(() =>
                {
                    return;
                });
            }, () =>
            {
                return;
            }, () =>
            {
                rewardedVideoButton.SetActive(false);

                StartCoroutine(ChangeCoinText());

                int extraCoinsToAward = (coinRewardAmount * postGameCoinMultiplier) - coinRewardAmount;

                PersistCoins(extraCoinsToAward, CoinSourceType.PostGameVideo);
                SceneActivationBehaviour<UICoinCounterActivator>.Instance.CoinCounterClaim(temporaryPlayerCoins + extraCoinsToAward, extraCoinsToAward, 0.25f);
            });
        }

        private IEnumerator ChangeCoinText()
        {
            puzzleImageAnimator.SetTrigger("CoinChange");
            yield return new WaitForSeconds(0.15f);
            coinReward.text = "+" + (coinRewardAmount * postGameCoinMultiplier).ToString();
        }

        [UsedImplicitly] // by animation
        public void ShowCoinGrantAnimation()
        {
            SceneActivationBehaviour<UICoinCounterActivator>.Instance.CoinCounterClaim(temporaryPlayerCoins, coinRewardAmount, 0.25f);
        }

        [UsedImplicitly] // by animation
        public void ShowScoreBanner()
        {
            scoreBannerAnimator.SetTrigger(starRewardAmount.ToString());
        }

        private void GameFinishedCleanUp()
        {
            SceneActivationBehaviour<BoardUIActivator>.Instance.DestroyBoard();

            // TO DO: Set reverse animation before screen changes (nice transition)
            feedbackMessageRoot.SetActive(false);
            //ShowNextButton(false);

            boardHolder.transform.localPosition = new Vector3(70, -30, 0);
            puzzleImageAnimator.Rebind();
            puzzleHeaderAnimator.Rebind();
            starburstImage.color = new Color(starburstImage.color.r, starburstImage.color.g, starburstImage.color.b, 0);

            Hide();
            ResetUIAnimations();
        }

        public void NextButtonHandler()
        {
            GameFinishedCleanUp();

            if (SceneActivationBehaviour<GameLogicActivator>.Instance.GameController.Player.FtuePassed)
            {
                InterfaceController.Instance.Show(GameWindow.CollectionScreen);

                InterfaceController.Instance.Hide(GameWindow.BoardUI);
                InterfaceController.Instance.Hide(GameWindow.BoardBlur);
                InterfaceController.Instance.Hide(GameWindow.BoardMenu);
                InterfaceController.Instance.Hide(GameWindow.TopBarUI);
                InterfaceController.Instance.Hide(GameWindow.BonusBarUI);
                InterfaceController.Instance.Hide(GameWindow.ToolbarUI);
            }
            else
            {
                InterfaceController.Instance.Hide(GameWindow.BonusBarUI);
                InterfaceController.Instance.Hide(GameWindow.ToolbarUI);
                InterfaceController.Instance.Hide(GameWindow.TopBarUI);
                SceneActivationBehaviour<BoardUIActivator>.Instance.QuixelController?.ShowHideCharacter(false);

                SceneActivationBehaviour<CollectionScreenActivator>.Instance.PlayNextLevel();
            }
        }

        private void ResetUIAnimations()
        {
            if (scoreBannerAnimator.gameObject.activeSelf)
                scoreBannerAnimator.SetTrigger("Idle");

            SceneActivationBehaviour<TopBarUIActivator>.Instance.ExitTopbar(false);
            SceneActivationBehaviour<BonusBarUIActivator>.Instance.ExitBonusBar(false);
            SceneActivationBehaviour<BoardUIActivator>.Instance.ExitAlien(false);
            SceneActivationBehaviour<BoardUIActivator>.Instance.ExitBoard(false);
            SceneActivationBehaviour<ToolbarUIActivator>.Instance.ExitToolbar(false);
        }

        public void CelebrateAlien()
        {
            if (endGameAlien.gameObject.activeSelf)
                endGameAlien.SetTrigger("Celebrate");
        }
    }
}