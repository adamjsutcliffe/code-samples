using UnityEngine;

namespace Peak.QuixelLogic.Scripts.Common
{
    public class AdjustBottomForIphoneX : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("Use if the transform anchor is stretched vertically.")]
        private bool isStretched = true;

        [SerializeField]
        [Tooltip("The bottom offset, guide is 32 pixels.")]
        private float bottomOffset = 32.0f;

        private void Awake()
        {
#if UNITY_IOS
            bool deviceIsIphoneX = UnityEngine.iOS.Device.generation == UnityEngine.iOS.DeviceGeneration.iPhoneX
                || UnityEngine.iOS.Device.generation == UnityEngine.iOS.DeviceGeneration.iPhoneXR
                || UnityEngine.iOS.Device.generation == UnityEngine.iOS.DeviceGeneration.iPhoneXS
                || UnityEngine.iOS.Device.generation == UnityEngine.iOS.DeviceGeneration.iPhoneXSMax;

            if (deviceIsIphoneX)
            {
                if (isStretched)
                {
                    RectTransform r = (RectTransform)transform;
                    r.offsetMin = new Vector2(r.offsetMin.x, r.offsetMin.y + bottomOffset);
                }
                else
                {
                    Vector3 newPosition = transform.localPosition + new Vector3(0f, bottomOffset, 0f);
                    transform.localPosition = newPosition;
                }
            }
#endif

#if SIMULATE_IPHONEX
            if (isStretched)
            {
                RectTransform r = (RectTransform)transform;
                r.offsetMin = new Vector2(r.offsetMin.x, r.offsetMin.y + bottomOffset);
            }
            else
            {
                Vector3 newPosition = transform.localPosition + new Vector3(0f, bottomOffset, 0f);
                transform.localPosition = newPosition;
            }
#endif
        }
    }
}
