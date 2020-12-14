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
            float cameraAspect = Camera.main.aspect;
            canvasScaler.matchWidthOrHeight = cameraAspect > 0.6 ? 1 : 0;
        }
    }
}