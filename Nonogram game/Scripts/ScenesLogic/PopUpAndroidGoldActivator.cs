using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Peak.QuixelLogic.Scripts.Common;
using Peak.QuixelLogic.Scripts.Autogenerated;
using System;
using JetBrains.Annotations;
using Peak.QuixelLogic.Scripts.Common.AnalyticsScripts;
using Peak.QuixelLogic.Scripts.Game;
using Peak.QuixelLogic.Scripts.Settings;
using TMPro;
using UnityEngine.UI;
using Object = UnityEngine.Object;
using Peak.QuixelLogic.Scripts.Game.CollectionScripts;

namespace Peak.QuixelLogic.Scripts.ScenesLogic
{
    public sealed class PopUpAndroidGoldActivator : SceneActivationBehaviour<PopUpAndroidGoldActivator>
    {
        [SerializeField]
        private TextMeshProUGUI coinTextField;

        [SerializeField]
        private TextMeshProUGUI levelTextField;

        [SerializeField]
        private Image levelBackgroundImage;

        private Action<bool> coinPurchaseClicked;

        private int goldLevelUnlockCost = 200;

        private RuleSettings thisLevelRules;
        private Object levelBeingPlayed;

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Show()
        {
            base.Show();
            SceneActivationBehaviour<UICoinCounterActivator>.Instance.SetButtons(false);
        }

        public override void Hide()
        {
            base.Hide();
            SceneActivationBehaviour<UICoinCounterActivator>.Instance.SetButtons(!InterfaceController.Instance.IsAnyPopupSceneActive());
        }

        #region out of film panel

        private RuleSettings ruleSettings;

        [UsedImplicitly]
        public void ShowGoldPopup(RuleSettings levelRules, Object goldCard, Sprite cardBackground, Action<bool> coinButtonClicked) //RuleSettings selectedRuleset, Action CoinPurchaseClicked, Action WatchVideoClicked, Action ExitClicked)
        {
            thisLevelRules = levelRules;
            levelBeingPlayed = goldCard;
            goldLevelUnlockCost = SceneActivationBehaviour<GameLogicActivator>.Instance.GameController.GlobalSettings.Coins.GoldLevelUnlockCost;
            coinTextField.text = $"{goldLevelUnlockCost}";
            coinPurchaseClicked = coinButtonClicked;
            levelTextField.text = thisLevelRules.Id;
            levelBackgroundImage.sprite = cardBackground;
            print($"Gold popup for {thisLevelRules.Id}");

            Show();
        }

        [UsedImplicitly]
        public void WatchVideoButton()
        {
            print("Watch video for gold");
            SceneActivationBehaviour<GameLogicActivator>.Instance.AdsController.TryShowIncentivised(() =>
            {
                //started
                DebugLog("[ADS] rewarded video started");
                SceneActivationBehaviour<GameLogicActivator>.Instance.AnalyticsController.SendAdResult(AdResultType.Started, AdType.RewardedVideo, AdSourceType.AndroidGold);
                SoundController.Instance.MuteMusic(true);
            }, () =>
            {
                //finished
                DebugLog("[ADS] rewarded video finished");
                SceneActivationBehaviour<GameLogicActivator>.Instance.AnalyticsController.SendAdResult(AdResultType.Watched, AdType.RewardedVideo, AdSourceType.AndroidGold);
                SoundController.Instance.MuteMusic(false);
                ((GoldLevelCardScript)levelBeingPlayed).thisGoldCardState = GoldLevelCardScript.GoldCardState.Purchased;
                SceneActivationBehaviour<CollectionScreenActivator>.Instance.PopulateCollectionScreen(SceneActivationBehaviour<GameLogicActivator>.Instance.GameController.Player);
                SceneActivationBehaviour<CollectionScreenActivator>.Instance.PlayChosenLevel(thisLevelRules, levelBeingPlayed);
                Hide();
            }, () =>
            {
                //error
                DebugLog("[ADS] rewarded video error");
                SceneActivationBehaviour<GameLogicActivator>.Instance.AnalyticsController.SendAdResult(AdResultType.Error, AdType.RewardedVideo, AdSourceType.AndroidGold);
                SoundController.Instance.MuteMusic(false);
                SceneActivationBehaviour<PopupRewardedVideoActivator>.Instance.ShowVideoErrorPopup(() =>
                {
                   //do i need a close handler??
                });
            }, () =>
            {
                //cancelled
                DebugLog("[ADS] rewarded video cancelled");
                SceneActivationBehaviour<GameLogicActivator>.Instance.AnalyticsController.SendAdResult(AdResultType.Aborted, AdType.RewardedVideo, AdSourceType.AndroidGold);
                SoundController.Instance.MuteMusic(false);
            });
        }

        [UsedImplicitly]
        public void CoinPurchaseButton()
        {
            print("Coin purchase for gold");
            if (SceneActivationBehaviour<GameLogicActivator>.Instance.GameController.Player.Coins >= SceneActivationBehaviour<GameLogicActivator>.Instance.GameController.GlobalSettings.Coins.GoldLevelUnlockCost)
            {
                //purchase level
                int playerCoins = SceneActivationBehaviour<GameLogicActivator>.Instance.GameController.Player.Coins - goldLevelUnlockCost;
                SceneActivationBehaviour<UICoinCounterActivator>.Instance.CoinCounterSpend(playerCoins, (goldLevelUnlockCost), 0f);
                SceneActivationBehaviour<GameLogicActivator>.Instance.GameController.CoinsSpentHandler(goldLevelUnlockCost, CoinSourceType.GoldPurchase);

                coinPurchaseClicked?.Invoke(true);
                Hide();
            }
            else
            {
                //open store
                SceneActivationBehaviour<UICoinCounterActivator>.Instance.ShowStore();
                Hide();
            }
        }
        #endregion
    }
}