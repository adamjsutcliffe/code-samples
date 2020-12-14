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
            float cameraAspect = Camera.main.aspect;
            canvasScaler.matchWidthOrHeight = cameraAspect > 0.6 ? 1 : 0;
        }
    }
}
