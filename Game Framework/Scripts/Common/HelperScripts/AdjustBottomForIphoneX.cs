using UnityEngine;

namespace Peak.Speedoku.Scripts.Common
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
            bool deviceIsIphoneX = UnityEngine.iOS.Device.generation.ToString().Contains("iPhoneX");

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
#elif SIMULATE_IPHONEX
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
