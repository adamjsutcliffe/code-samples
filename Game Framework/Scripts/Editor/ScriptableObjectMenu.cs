using System;
using System.IO;
using Peak.UnityGameFramework.Scripts.Common;
using Peak.Speedoku.Scripts.Settings;
using Peak.UnityGameFramework.Scripts.Settings;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Peak.UnityGameFramework.Scripts.Editor
{
    public static class ScriptableObjectMenu
    {
        [MenuItem("PEAK/Add Settings/Localisation settings (main)", priority = 102)]
        public static void CreateLocalisationSettings()
        {
            string soundName = "Assets/" + Constants.GameName.NameOfGame + "/Settings/Localisations/SoundSettings.asset";
            AssertExistingAsset(soundName);
            string textName = "Assets/" + Constants.GameName.NameOfGame + "/Settings/Localisations/TranslationSettings.asset";
            AssertExistingAsset(textName);
            string localisationName = "Assets/" + Constants.GameName.NameOfGame + "/Settings/Localisations/LocalisationSettings.asset";
            AssertExistingAsset(localisationName);

            SoundSettings soundAsset = ScriptableObject.CreateInstance<SoundSettings>();
            AssetDatabase.CreateAsset(soundAsset, soundName);

            TranslationSettings translationAsset = ScriptableObject.CreateInstance<TranslationSettings>();
            AssetDatabase.CreateAsset(translationAsset, textName);

            LocalisationSettings localisationAsset = ScriptableObject.CreateInstance<LocalisationSettings>();
            localisationAsset.Sound = new[] { soundAsset };
            localisationAsset.Text = new[] { translationAsset };
            AssetDatabase.CreateAsset(localisationAsset, localisationName);

            Selection.activeObject = localisationAsset;
            EditorGUIUtility.PingObject(localisationAsset);
        }

        [MenuItem("PEAK/Add Settings/Global settings (main)", priority = 101)]
        public static void CreateGlobalSettings()
        {
            string name = "Assets/" + Constants.GameName.NameOfGame + "/Settings/GlobalSettings.asset";

            AssertExistingAsset(name);

            ScriptableObject asset = ScriptableObject.CreateInstance<GlobalSettings>();
            AssetDatabase.CreateAsset(asset, name);

            Selection.activeObject = asset;
            EditorGUIUtility.PingObject(asset);
        }

        [MenuItem("PEAK/Add Settings/New Rule settings", priority = 1)]
        public static void CreateRuleSettings()
        {
            string name = "Assets/" + Constants.GameName.NameOfGame + "/Settings/NewRuleSettings"
                          + Random.Range(int.MinValue, 0) + ".asset";

            AssertExistingAsset(name);

            ScriptableObject asset = ScriptableObject.CreateInstance<RuleSettings>();
            AssetDatabase.CreateAsset(asset, name);

            Selection.activeObject = asset;
            EditorGUIUtility.PingObject(asset);
        }

        private static void AssertExistingAsset(string assetPath)
        {
            string assetName = Path.GetFileNameWithoutExtension(assetPath);
            string assetFolder = Path.GetDirectoryName(assetPath);

            if (AssetDatabase.FindAssets(assetName, new[] { assetFolder }).Length > 0)
            {
                string message = "The asset with the same name already exists! Please rename the existing one. Asset name: " + assetName;
                throw new Exception(message);
            }
        }
    }
}