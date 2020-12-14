using System;
using System.Collections;
using System.Collections.Generic;
using Peak.Speedoku.Scripts.Settings;
using Peak.UnityGameFramework.Scripts.Settings;
using TMPro;
using UnityEngine;

namespace Peak.Speedoku.Scripts.Common.Localisation
{
    public class LocalisationSystem : MonoBehaviour
    {
        private static SystemLanguage currentLanguage;

        public static bool isInit;

        private static GlobalSettings globalSettings;

        [SerializeField]
        private TMP_FontAsset JPfont;

        [SerializeField]
        private TMP_FontAsset ZHSfont;

        [SerializeField]
        private TMP_FontAsset ZHTfont;

        [SerializeField]
        private TMP_FontAsset RUfont;

        [SerializeField]
        private TMP_FontAsset KRfont;

        public static TMP_FontAsset JPFont;

        public static TMP_FontAsset ZHSFont;

        public static TMP_FontAsset ZHTFont;

        public static TMP_FontAsset RUFont;

        public static TMP_FontAsset KRFont;

        public static Dictionary<string, SystemLanguage> supportedLanguages = new Dictionary<string, SystemLanguage>
        {
            {"en", SystemLanguage.English},
            {"fr", SystemLanguage.French},
            {"it", SystemLanguage.Italian},
            {"de", SystemLanguage.German},
            {"es", SystemLanguage.Spanish},
            {"pt-br", SystemLanguage.Portuguese},
            {"jp", SystemLanguage.Japanese},
            {"zh-s", SystemLanguage.ChineseSimplified},
            {"zh-t", SystemLanguage.ChineseTraditional },
            {"id", SystemLanguage.Indonesian},
            {"ru", SystemLanguage.Russian},
            {"kr", SystemLanguage.Korean}
        };

        private static Dictionary<string, string> localisedEN;
        private static Dictionary<string, string> localisedFR;
        private static Dictionary<string, string> localisedIT;
        private static Dictionary<string, string> localisedDE;
        private static Dictionary<string, string> localisedES;
        private static Dictionary<string, string> localisedPT;
        private static Dictionary<string, string> localisedJP;
        private static Dictionary<string, string> localisedZHS;
        private static Dictionary<string, string> localisedZHT;
        private static Dictionary<string, string> localisedIN;
        private static Dictionary<string, string> localisedRU;
        private static Dictionary<string, string> localisedKR;

        public void Awake()
        {
            JPFont = JPfont;
            ZHSFont = ZHSfont;
            ZHTFont = ZHTfont;
            RUFont = RUfont;
            KRFont = KRfont;

#if CHEATS
            string selectedLanguage = PlayerPrefs.GetString(Constants.Languages.CurrentLanguage, "");
            if (selectedLanguage.Equals("") || selectedLanguage.Equals("en"))
            {
                PlayerPrefs.SetString(Constants.Languages.CurrentLanguage, Constants.Languages.English);
                currentLanguage = SystemLanguage.English;
            }
            else
            {
                supportedLanguages.TryGetValue(selectedLanguage, out currentLanguage);
            }
#elif !CHEATS
            currentLanguage = Application.systemLanguage;

            if (!supportedLanguages.ContainsValue(currentLanguage) || currentLanguage == SystemLanguage.Unknown)
            {
                currentLanguage = SystemLanguage.English;
            }
#endif
            Init();

            return;
        }

        public static void Init()
        {
            LocalisationStringLoader localisationStringLoader = new LocalisationStringLoader();
            localisationStringLoader.LoadCSV();

            localisedEN = localisationStringLoader.GetDictionaryValues("en");
            localisedFR = localisationStringLoader.GetDictionaryValues("fr");
            localisedIT = localisationStringLoader.GetDictionaryValues("it");
            localisedDE = localisationStringLoader.GetDictionaryValues("de");
            localisedES = localisationStringLoader.GetDictionaryValues("es");
            localisedPT = localisationStringLoader.GetDictionaryValues("pt-br");
            localisedJP = localisationStringLoader.GetDictionaryValues("jp");
            localisedZHS = localisationStringLoader.GetDictionaryValues("zh-s");
            localisedZHT = localisationStringLoader.GetDictionaryValues("zh-t");
            localisedIN = localisationStringLoader.GetDictionaryValues("id");
            localisedRU = localisationStringLoader.GetDictionaryValues("ru");
            localisedKR = localisationStringLoader.GetDictionaryValues("kr");

            isInit = true;
        }

        private static String CheckForNonBreakingLine(string value)
        {
            if (value.Contains("u00A0")) // non breaking space
            {
                value = value.Replace("\\u00A0", "\u00A0");
            }
            if (value.Contains("u000a")) // line break
            {
                value = value.Replace("\\u000a", "\u000a");
            }
            return value;
        }

        private static string GetDefaultEnglishValue(string key)
        {
            string value = key;
            if (localisedEN.TryGetValue(key, out value))
            {
                if (!string.IsNullOrEmpty(value))
                {
                    value = CheckForNonBreakingLine(value);
                }
                else value = "{error-key not found}";
            }

            return value;
        }

        public static string GetLocalisedValue(string key)
        {
            if (!isInit) { Init(); }
            string value = key;
            try
            {
                switch (currentLanguage)
                {
                    case SystemLanguage.English:
                        if (localisedEN.TryGetValue(key, out value))
                        {
                            if (!string.IsNullOrEmpty(value)) { value = CheckForNonBreakingLine(value); }
                            else value = "{error-key not found}";
                        }
                        else value = "{error-key not found}";
                        break;

                    case SystemLanguage.French:
                        if (localisedFR.TryGetValue(key, out value))
                        {
                            if (!string.IsNullOrEmpty(value)) { value = CheckForNonBreakingLine(value); }
                            else value = GetDefaultEnglishValue(key);
                        }
                        else value = GetDefaultEnglishValue(key);
                        break;

                    case SystemLanguage.Italian:
                        if (localisedIT.TryGetValue(key, out value))
                        {
                            if (!string.IsNullOrEmpty(value)) { value = CheckForNonBreakingLine(value); }
                            else value = GetDefaultEnglishValue(key);
                        }
                        else value = GetDefaultEnglishValue(key);
                        break;

                    case SystemLanguage.German:
                        if (localisedDE.TryGetValue(key, out value))
                        {
                            if (!string.IsNullOrEmpty(value)) { value = CheckForNonBreakingLine(value); }
                            else value = GetDefaultEnglishValue(key);
                        }
                        else value = GetDefaultEnglishValue(key);
                        break;

                    case SystemLanguage.Spanish:
                        if (localisedES.TryGetValue(key, out value))
                        {
                            if (!string.IsNullOrEmpty(value)) { value = CheckForNonBreakingLine(value); }
                            else value = GetDefaultEnglishValue(key);
                        }
                        else value = GetDefaultEnglishValue(key);
                        break;

                    case SystemLanguage.Portuguese:
                        if (localisedPT.TryGetValue(key, out value))
                        {
                            if (!string.IsNullOrEmpty(value)) { value = CheckForNonBreakingLine(value); }
                            else value = GetDefaultEnglishValue(key);
                        }
                        else value = GetDefaultEnglishValue(key);
                        break;

                    case SystemLanguage.Japanese:
                        if (localisedJP.TryGetValue(key, out value))
                        {
                            if (!string.IsNullOrEmpty(value)) { value = CheckForNonBreakingLine(value); }
                            else value = GetDefaultEnglishValue(key);
                        }
                        else value = GetDefaultEnglishValue(key);
                        break;

                    case SystemLanguage.ChineseSimplified:
                        if (localisedZHS.TryGetValue(key, out value))
                        {
                            if (!string.IsNullOrEmpty(value)) { value = CheckForNonBreakingLine(value); }
                            else value = GetDefaultEnglishValue(key);
                        }
                        else value = GetDefaultEnglishValue(key);
                        break;

                    case SystemLanguage.ChineseTraditional:
                        if (localisedZHT.TryGetValue(key, out value))
                        {
                            if (!string.IsNullOrEmpty(value)) { value = CheckForNonBreakingLine(value); }
                            else value = GetDefaultEnglishValue(key);
                        }
                        else value = GetDefaultEnglishValue(key);
                        break;

                    case SystemLanguage.Indonesian:
                        if (localisedIN.TryGetValue(key, out value))
                        {
                            if (!string.IsNullOrEmpty(value)) { value = CheckForNonBreakingLine(value); }
                            else value = GetDefaultEnglishValue(key);
                        }
                        else value = GetDefaultEnglishValue(key);
                        break;

                    case SystemLanguage.Russian:
                        if (localisedRU.TryGetValue(key, out value))
                        {
                            if (!string.IsNullOrEmpty(value)) { value = CheckForNonBreakingLine(value); }
                            else value = GetDefaultEnglishValue(key);
                        }
                        else value = GetDefaultEnglishValue(key);
                        break;

                    case SystemLanguage.Korean:
                        if (localisedKR.TryGetValue(key, out value))
                        {
                            if (!string.IsNullOrEmpty(value)) { value = CheckForNonBreakingLine(value); }
                            else value = GetDefaultEnglishValue(key);
                        }
                        else value = GetDefaultEnglishValue(key);
                        break;
                    default:
                        if (localisedEN.TryGetValue(key, out value))
                        {
                            if (!string.IsNullOrEmpty(value)) { value = CheckForNonBreakingLine(value); }
                            else value = "{error-key not found}";
                        }
                        else value = "{error-key not found}";
                        break;

                }
            }
            catch (Exception e)
            {
                value = "error";
                throw e;
            }

            return value;
        }

        public static SystemLanguage GetSystemLanguage()
        {
            return currentLanguage;
        }

        public static TMP_FontAsset GetJPFont()
        {
            return JPFont;
        }

        public static TMP_FontAsset GetZHSFont()
        {
            return ZHSFont;
        }

        public static TMP_FontAsset GetZHTFont()
        {
            return ZHTFont;
        }

        public static TMP_FontAsset GetRUFont()
        {
            return RUFont;
        }

        public static TMP_FontAsset GetKRFont()
        {
            return KRFont;
        }
    }
}