using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Peak.Speedoku.Scripts.Common;
using Peak.Speedoku.Scripts.Common.Localisation;
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
        textField = GetComponent<TextMeshProUGUI>();
        if (textField == null)
        {
            return;
        }

        if (key != null) //Localise text if key
        {
            textField.text = FormatText(LocalisationSystem.GetLocalisedValue(key));
        }

        //Check language, switch font if necessary
        SystemLanguage currentLanguage = LocalisationSystem.GetSystemLanguage();
        switch (currentLanguage)
        {
            case SystemLanguage.Japanese:
                textField.font = LocalisationSystem.GetJPFont();
                textField.fontMaterial = LocalisationSystem.GetJPFont().material;
                textField.lineSpacing = textField.wordSpacing = textField.characterSpacing = textField.paragraphSpacing = 0;
                break;
            case SystemLanguage.ChineseSimplified:
                textField.font = LocalisationSystem.GetZHSFont();
                textField.fontMaterial = LocalisationSystem.GetZHSFont().material;
                textField.lineSpacing = textField.wordSpacing = textField.characterSpacing = textField.paragraphSpacing = 0;
                break;
            case SystemLanguage.ChineseTraditional:
                textField.font = LocalisationSystem.GetZHTFont();
                textField.fontMaterial = LocalisationSystem.GetZHTFont().material;
                textField.lineSpacing = textField.wordSpacing = textField.characterSpacing = textField.paragraphSpacing = 0;
                break;
            case SystemLanguage.Russian:
                for (int i = 0; i < transform.childCount; i++)
                {
                    Destroy(transform.GetChild(i).gameObject);
                }

                textField.font = LocalisationSystem.GetRUFont();
                textField.fontMaterial = LocalisationSystem.GetRUFont().material;
                textField.lineSpacing = textField.wordSpacing = textField.characterSpacing = textField.paragraphSpacing = 0;
                textField.fontStyle = FontStyles.Normal;
                break;
            case SystemLanguage.Korean:
                textField.font = LocalisationSystem.GetKRFont();
                textField.fontMaterial = LocalisationSystem.GetKRFont().material;
                textField.lineSpacing = textField.wordSpacing = textField.characterSpacing = textField.paragraphSpacing = 0;
                break;
            default:
                return;
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