using Peak.QuixelLogic.Scripts.Common;
using Peak.QuixelLogic.Scripts.Common.AnalyticsScripts;
using Peak.QuixelLogic.Scripts.Game;
using Peak.QuixelLogic.Scripts.Game.Gameplay;
using UnityEngine;
#if UNITY_IOS
using UnityEngine.iOS;
#endif
namespace Peak.QuixelLogic.Scripts.ScenesLogic
{
    public sealed class GameLogicActivator : SceneActivationBehaviour<GameLogicActivator>
    {
        [SerializeField]
        private GameController gameController;
        [SerializeField]
        private SessionScript sessionScript;
        [SerializeField]
        private FtueController ftueController;
        [SerializeField]
        private ServerController serverController;
        [SerializeField]
        private AnalyticsController analyticsController;
        [SerializeField]
        private AdsController adsController;

        public GameController GameController => gameController;
        public SessionScript SessionScript => sessionScript;
        public FtueController FtueController => ftueController;
        public ServerController ServerController => serverController;
        public AnalyticsController AnalyticsController => analyticsController;
        public AdsController AdsController => adsController;

        public override void Initialize()
        {
            base.Initialize();

            SceneActivationBehaviour<TopBarUIActivator>.Instance.GamePaused += sessionScript.OnPauseClick;
            SceneActivationBehaviour<BoardMenuActivator>.Instance.GameResumed += sessionScript.OnResumeClick;
            SceneActivationBehaviour<BoardMenuActivator>.Instance.GameRestarted += sessionScript.OnRestartClick;
            SceneActivationBehaviour<BoardMenuActivator>.Instance.GameQuit += sessionScript.OnQuitClick;

            gameController.Initialise();
        }

        public override void Show()
        {
            base.Show();

            ftueController.InitialiseFtue();
        }
    }
}