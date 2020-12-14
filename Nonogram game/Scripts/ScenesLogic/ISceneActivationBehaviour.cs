using UnityEngine;

namespace Peak.QuixelLogic.Scripts.ScenesLogic
{
    public interface ISceneActivationBehaviour
    {
        string name { get; }

        Camera RootCamera { get; }

        void Initialize();
        void Show();
        void Hide();

        void SetButtons(bool enabled);

        void DebugLog(string message);

        bool IsActive();
    }
}