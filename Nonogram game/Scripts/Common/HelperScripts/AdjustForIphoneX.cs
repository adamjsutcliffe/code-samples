using UnityEngine;

namespace Peak.QuixelLogic.Scripts.Common
{
    public class AdjustForIphoneX : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("Use if the transform anchor is stretched vertically.")]
        private bool isStretched;

        [SerializeField]
        [Tooltip("The top offset, guide is 66 pixels.")]
        private float topOffset = 66.0f;

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
                    r.offsetMax = new Vector2(r.offsetMax.x, r.offsetMax.y - topOffset);
                }
                else
                {
                    Vector3 newPosition = transform.localPosition + new Vector3(0f, -topOffset, 0f);
                    transform.localPosition = newPosition;
                }
            }
#endif

#if SIMULATE_IPHONEX
            if (isStretched)
            {
                RectTransform r = (RectTransform)transform;
                r.offsetMax = new Vector2(r.offsetMax.x, r.offsetMax.y - topOffset);
            }
            else
            {
                Vector3 newPosition = transform.localPosition + new Vector3(0f, -topOffset, 0f);
                transform.localPosition = newPosition;
            }
#endif
        }
    }
}
