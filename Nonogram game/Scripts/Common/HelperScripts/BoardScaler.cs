using UnityEngine;

namespace Peak.QuixelLogic.Scripts.Common
{
    /// <summary>
    /// Allows to scale the board so it keeps the real size the same for any resolution (in pixels).
    /// Works in both runtime and editor.
    /// </summary>
    [ExecuteInEditMode]
    public sealed class BoardScaler : MonoBehaviour
    {
        private float screenWidth;
        private float screenHeight;
        private float screenAspect;

        private float previousAspect;

        private void LateUpdate()
        {
            ScaleScreen();
        }

        private void Awake()
        {
            ScaleScreen();
        }

        private void ScaleScreen()
        {
            if (transform.childCount == 0)
            {
                return;
            }
            else
            {
                screenWidth = Screen.width;
                screenHeight = Screen.height;
                screenAspect = screenWidth / screenHeight;

                if (Mathf.Approximately(previousAspect, screenAspect))
                {
                    return;
                }

                float x = previousAspect = screenAspect;
                float xx = x * x;
                float xxx = x * xx;
                float xxxx = x * xxx;

                // Quartic approximation
                float y = 1.30585f * xxxx - 5.96384f * xxx + 10.3036f * xx - 8.37306f * x + 3.19815f;

                transform.localScale = y * Vector3.one;

                Debug.Log($"[BoardScaler] {screenWidth}x{screenHeight}={screenAspect} ~> scale: {y}");
            }


        }
    }
}
