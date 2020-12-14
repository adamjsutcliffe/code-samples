using UnityEngine;

namespace Peak.Speedoku.Scripts.Common
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
            print($"AdjustForIphoneX Screen: {Screen.width}, {Screen.height} safe area: {Screen.safeArea}");
#if UNITY_IOS
            bool deviceIsIphoneX = UnityEngine.iOS.Device.generation.ToString().Contains("iPhoneX");

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
#elif SIMULATE_IPHONEX
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
