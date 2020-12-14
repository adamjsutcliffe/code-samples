using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Peak.QuixelLogic.Scripts.Common
{
    public static class iOSHapticFeedback
    {
        public static HapticFeedback hapticFeedback;

        public static void InitialiseiOSHaptics()
        {
#if PLATFORM_IOS
            hapticFeedback = new HapticFeedback();
            hapticFeedback.LightStyle();
#endif
        }

        public static void OnSelection()
        {
#if PLATFORM_IOS
            hapticFeedback.SelectionOccurred();
#endif
        }
    }
}