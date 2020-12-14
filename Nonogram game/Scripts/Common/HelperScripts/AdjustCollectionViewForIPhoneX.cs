using UnityEngine;

namespace Peak.QuixelLogic.Scripts.Common
{
    public class AdjustCollectionViewForIPhoneX : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("Use if the transform anchor is stretched vertically.")]
        private bool isStretched = true;

        [SerializeField]
        [Tooltip("The top offset, guide is 66 pixels.")]
        private float topOffset = 66.0f;

        [SerializeField]
        private RectTransform rectTransform;

        private void Awake()
        {
#if UNITY_IOS
            bool deviceIsIphoneX = UnityEngine.iOS.Device.generation == UnityEngine.iOS.DeviceGeneration.iPhoneX;

            if (deviceIsIphoneX)
            {
                if (isStretched)
                {
                    RectTransform r = (RectTransform)transform;
                    r.offsetMax = new Vector2(r.offsetMax.x, r.offsetMax.y - topOffset);
                }
                else
                {
                    if (gameObject.name.Equals("TopGreenBlock"))
                    {
                        Vector2 originalSize = rectTransform.sizeDelta;
                        rectTransform.sizeDelta = new Vector2(originalSize.x, originalSize.y + topOffset);
                    }
                    else if (gameObject.name.Equals("Scroll View"))
                    {
                        Vector2 offSetMax = rectTransform.offsetMax;
                        rectTransform.offsetMax = new Vector2(offSetMax.x, -topOffset);
                    }
                }
            }
#endif

#if SIMULATE_IPHONEX
            if (gameObject.name.Equals("TopGreenBlock"))
            {
                Vector2 originalSize = rectTransform.sizeDelta;
                rectTransform.sizeDelta = new Vector2(originalSize.x, originalSize.y + topOffset);
            }
            else if (gameObject.name.Equals("Scroll View"))
            {
                Vector2 offSetMax = rectTransform.offsetMax;
                rectTransform.offsetMax = new Vector2(offSetMax.x, -topOffset);
            }
#endif
        }
    }
}