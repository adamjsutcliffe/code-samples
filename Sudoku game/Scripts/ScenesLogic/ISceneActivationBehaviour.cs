using UnityEngine;

namespace Peak.Speedoku.Scripts.ScenesLogic
{
    public interface ISceneActivationBehaviour
    {
        string name { get; }

        Camera RootCamera { get; }

        void Initialize();
        void Show(bool animated = false);
        void Hide();

        void SetButtonsEnabled(bool isEnabled);
    }
}