using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Peak.Speedoku.Scripts.Common
{
    /// <summary>
    /// Provides extended behaviour for button enable/disable/interactable graphics tint
    /// </summary>
    [RequireComponent(typeof(Button))]
    public sealed class ButtonTextColourTint : GraphicsColourTint
    {
        //private void Awake()
        //{
        //    if (selectables == null || selectables.Length == 0)
        //    {
        //        selectables = new Selectable[] { GetComponent<Button>() };
        //    }
        //
        //    if (texts == null || texts.Length == 0)
        //    {
        //        texts = GetComponentsInChildren<TextMeshProUGUI>();
        //    }
        //}
    }
}