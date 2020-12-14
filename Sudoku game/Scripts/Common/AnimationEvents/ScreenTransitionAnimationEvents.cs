using System.Collections;
using System.Collections.Generic;
using Peak.Speedoku.Scripts.ScenesLogic;
using UnityEngine;

namespace Peak.Speedoku.Scripts.Common.AnimationEvents
{
    public class ScreenTransitionAnimationEvents : MonoBehaviour
    {
        public void ScreenTransitionStartDone()
        {
            SceneActivationBehaviour<OverlayUISceneActivator>.Instance.StartTransitionAnimationDone();
        }

        public void ScreenTransitionExitDone()
        {
            SceneActivationBehaviour<OverlayUISceneActivator>.Instance.EndTransitionAnimationDone();
        }
    }
}
