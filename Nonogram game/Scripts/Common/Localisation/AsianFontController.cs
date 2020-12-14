using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Peak.QuixelLogic.Scripts.Common.Localisation
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class AsianFontController : MonoBehaviour
    {
        TextMeshProUGUI textField;

        private void Start()
        {
            textField = GetComponent<TextMeshProUGUI>();
            if (textField == null)
            {
                return;
            }
            SystemLanguage currentLanguage = LocalisationSystem.GetSystemLanguage();

            if (currentLanguage == SystemLanguage.Japanese)
            {
                textField.font = LocalisationSystem.GetJPFont();
                textField.fontMaterial = LocalisationSystem.GetJPFont().material;
                textField.lineSpacing = textField.wordSpacing = textField.characterSpacing = textField.paragraphSpacing = 0;
            }
            else if (currentLanguage == SystemLanguage.ChineseSimplified)
            {
                textField.font = LocalisationSystem.GetZHSFont();
                textField.fontMaterial = LocalisationSystem.GetZHSFont().material;
                textField.lineSpacing = textField.wordSpacing = textField.characterSpacing = textField.paragraphSpacing = 0;
            }
            else if (currentLanguage == SystemLanguage.ChineseTraditional)
            {
                textField.font = LocalisationSystem.GetZHTFont();
                textField.fontMaterial = LocalisationSystem.GetZHTFont().material;
                textField.lineSpacing = textField.wordSpacing = textField.characterSpacing = textField.paragraphSpacing = 0;
            }
            else if (currentLanguage == SystemLanguage.Russian)
            {
                for (int i = 0; i < transform.childCount; i++)
                {
                    Destroy(transform.GetChild(i).gameObject);
                }

                textField.font = LocalisationSystem.GetRUFont();
                textField.fontMaterial = LocalisationSystem.GetRUFont().material;
                textField.lineSpacing = textField.wordSpacing = textField.characterSpacing = textField.paragraphSpacing = 0;
                textField.fontStyle = FontStyles.Normal;
            }
            else if (currentLanguage == SystemLanguage.Korean)
            {
                textField.font = LocalisationSystem.GetKRFont();
                textField.fontMaterial = LocalisationSystem.GetKRFont().material;
                textField.lineSpacing = textField.wordSpacing = textField.characterSpacing = textField.paragraphSpacing = 0;
            }
            else return;
        }
    }
}