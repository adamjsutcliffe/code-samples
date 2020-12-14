using UnityEngine;

namespace Peak.UnityGameFramework.Scripts.ScenesLogic
{
    public interface ISceneActivationBehaviour
    {
        string name { get; }

        Camera RootCamera { get; }

        void Initialize();
        void Show(bool animated = false);
        void Hide();

        void SetButtonsEnabled(bool isEnabled);

        void StartGame();
        void StartRound(int difficulty);
        void PauseGame();
        void ResumeGame();
        void EndGame();
        void QuitGame();
    }
}