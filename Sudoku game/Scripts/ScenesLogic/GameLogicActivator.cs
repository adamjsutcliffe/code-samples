using Peak.Speedoku.Scripts.Common;
using Peak.Speedoku.Scripts.Common.AnalyticsScripts;
using Peak.Speedoku.Scripts.Game;
using Peak.Speedoku.Scripts.Game.Gameplay;
using UnityEngine;

namespace Peak.Speedoku.Scripts.ScenesLogic
{
    public sealed class GameLogicActivator : SceneActivationBehaviour<GameLogicActivator>
    {
        [SerializeField]
        private InputController inputController;
        [SerializeField]
        private GameController gameController;
        [SerializeField]
        private SessionScript sessionScript;
        [SerializeField]
        private ServerController serverController;
        [SerializeField]
        private AnalyticsController analyticsController;
        [SerializeField]
        private AdsController adsController;

        public InputController InputController => inputController;
        public GameController GameController => gameController;
        public SessionScript SessionScript => sessionScript;
        public ServerController ServerController => serverController;
        public AnalyticsController AnalyticsController => analyticsController;
        public AdsController AdsController => adsController;

        public override void Initialize()
        {
            base.Initialize();

            SceneActivationBehaviour<MainMenuActivator>.Instance.GamePaused += sessionScript.OnPauseClick;
            SceneActivationBehaviour<PauseMenuActivator>.Instance.GameResumed += sessionScript.OnResumeClick;
            SceneActivationBehaviour<PauseMenuActivator>.Instance.GameQuit += sessionScript.OnQuitClick;

            gameController.Initialise();
            inputController.Initialise();
#if PLATFORM_IOS
            iOSHapticFeedbackHelper.InitialiseiOSHaptics();
#endif
        }

        public override void Show(bool animated = false)
        {
            base.Show(animated);

            //ftueController.InitializeFtue();
        }

        public override void Hide()
        {
            base.Hide();
        }
    }
}