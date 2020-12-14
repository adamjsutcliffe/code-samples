using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Peak.UnityGameFramework.Scripts.ScenesLogic
{
    public class RootGameSceneActivator : SceneActivationBehaviour<RootGameSceneActivator>
    {
        [SerializeField]
        public Light[] sceneLights;

        [SerializeField]
        public Cube cubeScript;

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Show(bool animated = false)
        {
            base.Show(animated);
        }

        public override void Hide()
        {
            base.Hide();
        }

        public void LoadGame(string gameID)
        {
            cubeScript.LoadGame(gameID);
        }

        public override void StartRound(int difficulty)
        {
        //    base.StartRound(difficulty);
        //}
        //public void StartGameRound(int difficulty)
        //{
            cubeScript.StartRoundWithDifficulty(difficulty);
        }

        public void FinishRound(bool success, int score, string analytics)
        {
            print($"Root - finish round -> analytics: {analytics}");
            cubeScript.FinishRound(success, score, analytics);
        }

        #region HUD handling

        public void PauseButtonPressed()
        {
            print("Pause pressed");
            cubeScript.PausePressed();
        }

        public void HelpButtonPressed()
        {
            print("help pressed");
            cubeScript.HelpPressed();
        }

        #endregion
    }
}

