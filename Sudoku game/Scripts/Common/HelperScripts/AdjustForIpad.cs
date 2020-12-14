using UnityEngine;
using UnityEngine.UI;

namespace Peak.Speedoku.Scripts.Common
{
    [RequireComponent(typeof(CanvasScaler))]
    public class AdjustForIpad : MonoBehaviour
    {
        private void Start()
        {
            CanvasScaler canvasScaler = GetComponent<CanvasScaler>();

#if UNITY_IOS
            bool deviceIsIpad = UnityEngine.iOS.Device.generation.ToString().Contains("iPad");

            if (deviceIsIpad)
            {
                canvasScaler.matchWidthOrHeight = 1;
            }

#elif SIMULATE_IPAD
            canvasScaler.matchWidthOrHeight = 1;
#endif
        }
    }
}
