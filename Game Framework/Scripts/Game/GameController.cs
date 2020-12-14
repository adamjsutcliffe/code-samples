using System.Collections;
using Peak.Speedoku.Scripts.Common;
using Peak.Speedoku.Scripts.Common.AnalyticsScripts;
using Peak.Speedoku.Scripts.Settings;
using Peak.Speedoku.Scripts.Game.Gameplay;
using UnityEngine;
using Peak.UnityGameFramework.Scripts.ScenesLogic;
using Peak.Speedoku.Scripts.ScenesLogic;
using Peak.UnityGameFramework.Scripts.Settings;

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
        private SessionScript sessionScript;

        //private List<Int> ruleIndexes;

        public void Initialise()
        {
            print("GAME CONTROLLER Init ");
            InitialiseSession();
        }

        private void InitialiseSession()
        {
            print("SESSION INIT");
            sessionScript.SessionStarted += GameStartedHandler;
            sessionScript.SessionPaused += GamePausedHandler;
            sessionScript.SessionResumed += GameResumedHandler;
            sessionScript.SessionFinished += GameFinishedHandler;
            sessionScript.SessionQuit += GameQuitHandler;

            SceneActivationBehaviour<SPEGameSceneActivator>.Instance.GamePaused += sessionScript.OnPauseClick;
            //SceneActivationBehaviour<PauseMenuActivator>.Instance.GameResumed += sessionScript.OnResumeClick;
            //SceneActivationBehaviour<PauseMenuActivator>.Instance.GameQuit += sessionScript.OnQuitClick;
        }

        #region 
        public RuleSettings GetRules()
        {
            return globalSettings.RulesList[RandomRuleSettingsIndex()];
        }

        private int RandomRuleSettingsIndex()
        {
            //TODO: update with random generator of rule set count
            return 0;
        }

        //public ProgressData GetProgressInfo()
        //{
        //    return GetLocationInfo(player.ProgressCounter);
        //}

        public void UpdatePlayerProgress()
        {
            //player.ProgressCounter += 1;
            //serverController.PersistPlayerProgress(player);
        }

        public int GetDeductionScore(int secondsUsed, int correct)
        {
            //bool allCorrect = correct == 3;
            //if (allCorrect)
            //{
            //    int score = Mathf.Max(levelMaxPoints - secondsUsed, 10);
            //    return score * correct;
            //}
            return correct;
        }
        #endregion

        public void SetupGameHandler(MainGameData gameData)
        {
            print("[GC] Setup game handler");
            sessionScript.SetupGame(gameData);
        }

        public bool ShouldStartWithFtuePopup()
        {
            return false;
        }

        public void FtuePopupShown()
        {
        }

        public void StartGameHandler()
        {
            print("[GC] Start game handler");
            
            sessionScript.StartGame();
        }

        #region Session handlers
        private void GameStartedHandler(MainGameData gameData)
        {
            print("[GC] Started");
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
        }

        private void GameFinishedHandler(MainGameData gameData)
        {
            print($"[GC] Finished Time: {gameData.SecondsUsed} ");
            StartCoroutine(FinishWithDelay(gameData));
        }

        private IEnumerator FinishWithDelay(MainGameData gameData)
        {
            yield return new WaitForSeconds(1f);
            SceneActivationBehaviour<SPEGameSceneActivator>.Instance.GameFinished(gameData);
        }

        #endregion

        //public int GetGameControllerInt()
        //{
        //    return 4;
        //}


        void OnApplicationFocus(bool hasFocus)
        {
            Debug.Log($"OnApplicationFocus -> hasFocus: {hasFocus}");
        }

        void OnApplicationPause(bool pauseStatus)
        {
            Debug.Log($"OnApplicationPause -> pauseStatus: {pauseStatus}");
        }
    }
}

