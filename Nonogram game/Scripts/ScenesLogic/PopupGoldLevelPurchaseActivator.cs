using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Peak.QuixelLogic.Scripts.Common;
using Peak.QuixelLogic.Scripts.Autogenerated;
using JetBrains.Annotations;
using System;
using Peak.QuixelLogic.Scripts.Common.AnalyticsScripts;
using TMPro;

namespace Peak.QuixelLogic.Scripts.ScenesLogic
{
    public class PopupGoldLevelPurchaseActivator : SceneActivationBehaviour<PopupGoldLevelPurchaseActivator>
    {
        public enum ButtonSelected
        {
            Purchase,
            WatchVideo,
            Exit
        }

        [SerializeField]
        private GameObject PurchaseGoldLevelPanel;

        [SerializeField]
        private GameObject NoCoinsPanel;

        [SerializeField]
        private TextMeshProUGUI buttonText;

        [SerializeField]
        private TextMeshProUGUI noCoinsPopupText;

        private Action<bool> buttonClicked;

        private int goldLevelUnlockCost;

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

        private void OnEnable()
        {
            noCoinsPopupText.text = GameConstants.MainGame.FeatureMessages.GoldPlayError;
            goldLevelUnlockCost = SceneActivationBehaviour<GameLogicActivator>.Instance.GameController.GlobalSettings.Coins.GoldLevelUnlockCost;
            buttonText.text = goldLevelUnlockCost.ToString();
        }

        public void ShowPopupGoldPurchasePanel(Action<bool> ButtonClicked)
        {
            buttonClicked = ButtonClicked;
            SoundController.Instance.PlayAudioClip(Settings.Autogenerated.SoundSettingsKey.Popupappear);

            if (SceneActivationBehaviour<GameLogicActivator>.Instance.GameController.Player.Coins >= goldLevelUnlockCost)
            {
                PurchaseGoldLevelPanel.SetActive(true);
            }
            else
            {
                NoCoinsPanel.SetActive(true);
            }

            Show();
        }

        [UsedImplicitly]
        public void PurchaseLevelButton()
        {
            int playerCoins = SceneActivationBehaviour<GameLogicActivator>.Instance.GameController.Player.Coins - goldLevelUnlockCost;
            SceneActivationBehaviour<UICoinCounterActivator>.Instance.CoinCounterSpend(playerCoins, (goldLevelUnlockCost), 0f);
            SceneActivationBehaviour<GameLogicActivator>.Instance.GameController.CoinsSpentHandler(goldLevelUnlockCost, CoinSourceType.GoldPurchase);

            PurchaseGoldLevelPanel.SetActive(false);
            buttonClicked?.Invoke(true);

            Hide();
        }

        [UsedImplicitly]
        public void WatchVideoButton()
        {
            ClaimRewardedVideo();
            ExitPanel();
        }

        [UsedImplicitly]
        public void ClaimRewardedVideo()
        {
            SceneActivationBehaviour<GameLogicActivator>.Instance.GameController.CallRewardedVideo(AdSourceType.GoldCardPopUp);
        }

        [UsedImplicitly]
        public void PurchaseCoinsHandler()
        {
            SceneActivationBehaviour<UICoinCounterActivator>.Instance.SetButtons(false);
            SceneActivationBehaviour<StoreUIActivator>.Instance.OpenStore(() =>
            {

                SceneActivationBehaviour<UICoinCounterActivator>.Instance.SetButtons(true);
            }, QLPurchaseSource.QLPurchaseSourceGold);
            ExitPanel();
        }

        [UsedImplicitly]
        public void ExitPanel()
        {
            PurchaseGoldLevelPanel.SetActive(false); // change for animation to shrink? then hide?
            NoCoinsPanel.SetActive(false);
            buttonClicked?.Invoke(false);
            Hide();
        }
    }
}
