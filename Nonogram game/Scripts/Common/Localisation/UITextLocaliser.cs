using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Peak.QuixelLogic.Scripts.Common;
using Peak.QuixelLogic.Scripts.Common.Localisation;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class UITextLocaliser : MonoBehaviour
{

    public enum TextAppearance
    {
        NoChange = 0,
        CamalCaps = 1,
        AllCaps = 2
    }

    TextMeshProUGUI textField;

    public string key;
    public TextAppearance textFormat = TextAppearance.NoChange;

    public void Awake()
    {
        if (key != null)
        {
            textField = GetComponent<TextMeshProUGUI>();
            if (textField != null)
            {
                textField.text = FormatText(LocalisationSystem.GetLocalisedValue(key));
            }
        }
    }

    private string FormatText(string value)
    {
        switch (textFormat)
        {
            case TextAppearance.AllCaps:
                return CultureInfo.CurrentCulture.TextInfo.ToUpper(value);
            case TextAppearance.CamalCaps:
                return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(value);
            case TextAppearance.NoChange:
            default:
                return value;
        }
    }
}