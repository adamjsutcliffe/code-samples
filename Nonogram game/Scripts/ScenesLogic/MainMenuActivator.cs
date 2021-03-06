using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Peak.QuixelLogic.Scripts.Autogenerated;
using Peak.QuixelLogic.Scripts.Common;
using Peak.QuixelLogic.Scripts.Common.AnalyticsScripts;
using Peak.QuixelLogic.Scripts.ScenesLogic;
using Peak.QuixelLogic.Scripts.Settings;
using TMPro;
using Peak.QuixelLogic.Scripts.Game;
using UnityEngine.UI;
using JetBrains.Annotations;
using Peak.QuixelLogic.Scripts.Game.Gameplay;
using System.IO;
//using UnityEditor;

namespace Peak.QuixelLogic.Scripts.ScenesLogic
{
    public class MainMenuActivator : SceneActivationBehaviour<MainMenuActivator>
    {
        [SerializeField] private Canvas backgroundCanvas;

        [SerializeField]
        private Image backgroundImage;

        [SerializeField]
        private ButtonController settingsButton;

        [SerializeField]
        private ButtonController playButton;

        [SerializeField]
        private GameObject QLLogo;

        [SerializeField]
        private Animator mainMenuCanvasAnimator;

        [SerializeField]
        private Animator mainMenuBackgroundAnimator;

        private Player currentPlayer;

        private GlobalSettings settings;

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Show()
        {
            base.Show();
            backgroundCanvas.gameObject.SetActive(true);

            SetBackground();

            // check whether intro movie should be shown
            if (currentPlayer.ShouldShowIntroVideo() && currentPlayer.MainPuzzleIndex == 0 && SceneActivationBehaviour<GameLogicActivator>.Instance.FtueController.ShouldShowGdprPopup)
            {
                LoadIntro();
            }
            else
            {
                //TODO: GDPR check on load 
                if (SceneActivationBehaviour<GameLogicActivator>.Instance.FtueController.ShouldShowGdprPopup)
                {
                    GameWindow[] introScene = { GameWindow.PopUpGdpr };
                    InterfaceController.Instance.Load(introScene, () => { SceneActivationBehaviour<PopUpGdprActivator>.Instance.ShowGDPRPopup(MainMenuLoad); });
                }
                else
                {
                    MainMenuLoad();
                }
            }
        }

        private void LoadIntro()
        {
#if UNITY_ANDROID
            GameWindow[] introScene = { GameWindow.PopUpGdpr };
            InterfaceController.Instance.Load(introScene, () => { SceneActivationBehaviour<PopUpGdprActivator>.Instance.ShowGDPRPopup(MainMenuLoad); });
#elif UNITY_IOS
            GameWindow[] introScenes = { GameWindow.IntroVideo, GameWindow.PopUpGdpr };
            InterfaceController.Instance.Load(introScenes, () =>
            {
                SceneActivationBehaviour<IntroVideoActivator>.Instance.ShowGDPRAndIntro();
            });
#endif
        }

        public void TransitionAnimation()
        {
            Show();
            SetPreviousBackground();

            settingsButton.gameObject.SetActive(false);
            playButton.gameObject.SetActive(false);
            QLLogo.SetActive(false);

            SoundController.Instance.PlayAudioClip(Settings.Autogenerated.SoundSettingsKey.Shimmeringtransition);

            mainMenuBackgroundAnimator.SetTrigger("BGChange");

            Invoke(nameof(EnterMainMenuElements), 2.5f);
        }

        public void EnterMainMenuElements()
        {
            mainMenuCanvasAnimator.enabled = true;
            SetBackground();
            SceneActivationBehaviour<OverlayUISceneActivator>.Instance.ToggleBlocker(false);
        }

        public void SetPreviousBackground()
        {
            backgroundImage.sprite = settings.levelGroupingSettings[currentPlayer.GroupIndex - 1].MainMenuBackgroundImage;
        }

        public void SetBackground()
        {
            backgroundImage.sprite = settings.levelGroupingSettings[currentPlayer.GroupIndex].MainMenuBackgroundImage;
        }

        public void MainMenuLoad()
        {
            mainMenuCanvasAnimator?.SetTrigger("Enter");

            SceneActivationBehaviour<UICoinCounterActivator>.Instance.SetPlayerCoins(currentPlayer.Coins); // coins
            SceneActivationBehaviour<UIFilmCounterActivator>.Instance.TryToGiveDailyReward(currentPlayer); // attempt to award player with daily film
#if UNITY_ANDROID
            if (SceneActivationBehaviour<GameLogicActivator>.Instance.FtueController.ShouldShowNotificationPopup)
            {
                SceneActivationBehaviour<GameLogicActivator>.Instance.GameController.ScheduleOneDayNotification();
                SceneActivationBehaviour<GameLogicActivator>.Instance.FtueController.NotificationsAccepted();
            }
#endif
            // try to show notifications popup if this is second time app is open and if player hasn't already accepted
            if (currentPlayer.ShouldShowAcceptNotification())
            {
                SceneActivationBehaviour<GameLogicActivator>.Instance.GameController.TryShowNotificationPopup(GameConstants.NotificationPopups.NotificationPopupTitle, GameConstants.NotificationPopups.NotificationPopupBody);
            }
        }

        public override void Hide()
        {
            base.Hide();
            backgroundCanvas.gameObject.SetActive(false);
        }

        public void SetCurrentProgressOnMainMenu(Player player, GlobalSettings globalSettings)
        {
            currentPlayer = player;
            settings = globalSettings;

            //playButton.gameObject.SetActive(!currentPlayer.NewLocation);
            SceneActivationBehaviour<CollectionScreenActivator>.Instance.PopulateCollectionScreen(currentPlayer);
            SceneActivationBehaviour<UIFilmCounterActivator>.Instance.SetFilmCounterText(player.Film);

            SceneActivationBehaviour<UILevelProgressCounterActivator>.Instance.SetLevelProgress(currentPlayer);
        }

        [UsedImplicitly]
        public void PlayClickedHandler()
        {
            mainMenuCanvasAnimator.enabled = false;

            if (currentPlayer.FtuePassed)
            {
                SceneActivationBehaviour<OverlayUISceneActivator>.Instance.ScreenWipeTransition(new List<GameWindow> { GameWindow.MainMenu }, new List<GameWindow> { GameWindow.CollectionScreen });
                SceneActivationBehaviour<GameLogicActivator>.Instance.FtueController.CollectionViewFtueStep();

                if (!SceneActivationBehaviour<CollectionScreenActivator>.Instance.NewLocationUnlocked)
                {
                    SceneActivationBehaviour<CollectionScreenActivator>.Instance.ShowCollectionEvent();
                }
            }
            else
            {
                SceneActivationBehaviour<CollectionScreenActivator>.Instance.PlayNextLevel();
            }
        }

        public void ShowSettingsWindow()
        {
            InterfaceController.Instance.Show(GameWindow.SettingsMenuUI);
        }

        //WaitForSeconds delay = new WaitForSeconds(2f);

        //private IEnumerator ClaimingRewardAnimation()
        //{
        //    yield return delay;

        //    SoundController.Instance.PlayAudioClip(Settings.Autogenerated.SoundSettingsKey.Locationunlocked);
        //    InterfaceController.Instance.Hide(GameWindow.PopupClaimCoins);

        //    mainMenuCanvasAnimator.enabled = true;
        //}
    }
}