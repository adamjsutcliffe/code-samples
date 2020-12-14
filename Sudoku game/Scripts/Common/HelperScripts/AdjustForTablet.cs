using UnityEngine;
using UnityEngine.UI;

namespace Peak.Speedoku.Scripts.Common
{
    [RequireComponent(typeof(CanvasScaler))]
    public class AdjustForTablet : MonoBehaviour
    {
        private void Start()
        {
            CanvasScaler canvasScaler = GetComponent<CanvasScaler>();

            // iOS

#if SIMULATE_IPAD
            canvasScaler.matchWidthOrHeight = 1;
#elif UNITY_IOS
            bool deviceIsIpad = UnityEngine.iOS.Device.generation.ToString().Contains("iPad");

            if (deviceIsIpad)
            {
                canvasScaler.matchWidthOrHeight = 1;
            }
#endif
            // Android

#if SIMULATE_ANDROID_TABLET
                        canvasScaler.matchWidthOrHeight = 1;
#elif UNITY_ANDROID

            float ssw;
            if (Screen.width > Screen.height)
            {
                ssw = Screen.width;
            }
            else
            {
                ssw = Screen.height;
            }

            float screenWidth = Screen.width / Screen.dpi;
            float screenHeight = Screen.height / Screen.dpi;
            float size = Mathf.Sqrt(Mathf.Pow(screenWidth, 2) + Mathf.Pow(screenHeight, 2));
            if (size >= 6.5f)
            {
                canvasScaler.matchWidthOrHeight = 1;
            }
#endif
        }
    }
}