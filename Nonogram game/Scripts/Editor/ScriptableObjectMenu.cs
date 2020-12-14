using System;
using System.Collections.Generic;
using System.IO;
using Peak.QuixelLogic.Scripts.Common;
using Peak.QuixelLogic.Scripts.Settings;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;
using System.Linq;

namespace Peak.QuixelLogic.Scripts.Editor
{
    public static class ScriptableObjectMenu
    {
        [MenuItem("PEAK/Change Language/English", priority = 101)]
        public static void SetLanguageToEN() { PlayerPrefs.SetString(Constants.Languages.CurrentLanguage, "en"); Debug.Log($"[LANGUAGE] Menu change language to -> {PlayerPrefs.GetString(Constants.Languages.CurrentLanguage, "")}"); }
        [MenuItem("PEAK/Change Language/French", priority = 101)]
        public static void SetLanguageToFR() { PlayerPrefs.SetString(Constants.Languages.CurrentLanguage, "fr"); Debug.Log($"[LANGUAGE] Menu change language to -> {PlayerPrefs.GetString(Constants.Languages.CurrentLanguage, "")}"); }
        [MenuItem("PEAK/Change Language/Italian", priority = 101)]
        public static void SetLanguageToIT() { PlayerPrefs.SetString(Constants.Languages.CurrentLanguage, "it"); Debug.Log($"[LANGUAGE] Menu change language to -> {PlayerPrefs.GetString(Constants.Languages.CurrentLanguage, "")}"); }
        [MenuItem("PEAK/Change Language/German", priority = 101)]
        public static void SetLanguageToDE() { PlayerPrefs.SetString(Constants.Languages.CurrentLanguage, "de"); Debug.Log($"[LANGUAGE] Menu change language to -> {PlayerPrefs.GetString(Constants.Languages.CurrentLanguage, "")}"); }
        [MenuItem("PEAK/Change Language/Spanish", priority = 101)]
        public static void SetLanguageToES() { PlayerPrefs.SetString(Constants.Languages.CurrentLanguage, "es"); Debug.Log($"[LANGUAGE] Menu change language to -> {PlayerPrefs.GetString(Constants.Languages.CurrentLanguage, "")}"); }
        [MenuItem("PEAK/Change Language/Portuguese", priority = 101)]
        public static void SetLanguageToPT() { PlayerPrefs.SetString(Constants.Languages.CurrentLanguage, "pt-br"); Debug.Log($"[LANGUAGE] Menu change language to -> {PlayerPrefs.GetString(Constants.Languages.CurrentLanguage, "")}"); }
        [MenuItem("PEAK/Change Language/Japanese", priority = 101)]
        public static void SetLanguageToJP() { PlayerPrefs.SetString(Constants.Languages.CurrentLanguage, "jp"); Debug.Log($"[LANGUAGE] Menu change language to -> {PlayerPrefs.GetString(Constants.Languages.CurrentLanguage, "")}"); }
        [MenuItem("PEAK/Change Language/Chinese simplified", priority = 101)]
        public static void SetLanguageToZHS() { PlayerPrefs.SetString(Constants.Languages.CurrentLanguage, "zh-s"); Debug.Log($"[LANGUAGE] Menu change language to -> {PlayerPrefs.GetString(Constants.Languages.CurrentLanguage, "")}"); }
        [MenuItem("PEAK/Change Language/Chinese traditional", priority = 101)]
        public static void SetLanguageToZHT() { PlayerPrefs.SetString(Constants.Languages.CurrentLanguage, "zh-t"); Debug.Log($"[LANGUAGE] Menu change language to -> {PlayerPrefs.GetString(Constants.Languages.CurrentLanguage, "")}"); }
        [MenuItem("PEAK/Change Language/Indonesian", priority = 101)]
        public static void SetLanguageToIn() { PlayerPrefs.SetString(Constants.Languages.CurrentLanguage, "id"); Debug.Log($"[LANGUAGE] Menu change language to -> {PlayerPrefs.GetString(Constants.Languages.CurrentLanguage, "")}"); }
        [MenuItem("PEAK/Change Language/Russian", priority = 101)]
        public static void SetLanguageToRU() { PlayerPrefs.SetString(Constants.Languages.CurrentLanguage, "ru"); Debug.Log($"[LANGUAGE] Menu change language to -> {PlayerPrefs.GetString(Constants.Languages.CurrentLanguage, "")}"); }
        [MenuItem("PEAK/Change Language/Korean", priority = 101)]
        public static void SetLanguageToKR() { PlayerPrefs.SetString(Constants.Languages.CurrentLanguage, "kr"); Debug.Log($"[LANGUAGE] Menu change language to -> {PlayerPrefs.GetString(Constants.Languages.CurrentLanguage, "")}"); }

        [MenuItem("PEAK/Add Settings/Localisation settings (main)", priority = 102)]
        public static void CreateLocalisationSettings()
        {
            string soundName = "Assets/" + GameConstants.GameName.NameOfGame + "/Settings/Localisations/SoundSettings.asset";
            AssertExistingAsset(soundName);
            string localisationName = "Assets/" + GameConstants.GameName.NameOfGame + "/Settings/Localisations/LocalisationSettings.asset";
            AssertExistingAsset(localisationName);

            SoundSettings soundAsset = ScriptableObject.CreateInstance<SoundSettings>();
            AssetDatabase.CreateAsset(soundAsset, soundName);

            LocalisationSettings localisationAsset = ScriptableObject.CreateInstance<LocalisationSettings>();
            localisationAsset.Sound = new[] { soundAsset };
            AssetDatabase.CreateAsset(localisationAsset, localisationName);

            Selection.activeObject = localisationAsset;
            EditorGUIUtility.PingObject(localisationAsset);
        }

        [MenuItem("PEAK/Add Settings/Global settings (main)", priority = 101)]
        public static void CreateGlobalSettings()
        {
            string name = "Assets/" + GameConstants.GameName.NameOfGame + "/Settings/GlobalSettings.asset";

            AssertExistingAsset(name);

            ScriptableObject asset = ScriptableObject.CreateInstance<GlobalSettings>();
            AssetDatabase.CreateAsset(asset, name);

            Selection.activeObject = asset;
            EditorGUIUtility.PingObject(asset);
        }

        [MenuItem("PEAK/Add Settings/Create puzzle images", priority = 101)]
        public static void Bitmap()
        {
            Dictionary<string, Color32> CellColourDictionary = new Dictionary<string, Color32>();

            CellColourDictionary.Add("x".ToUpperInvariant(), GameConstants.CellColours.PaletteColour_x);
            CellColourDictionary.Add("0".ToUpperInvariant(), GameConstants.CellColours.PaletteColour_0);
            CellColourDictionary.Add("1".ToUpperInvariant(), GameConstants.CellColours.PaletteColour_1);
            CellColourDictionary.Add("2".ToUpperInvariant(), GameConstants.CellColours.PaletteColour_2);
            CellColourDictionary.Add("3".ToUpperInvariant(), GameConstants.CellColours.PaletteColour_3);
            CellColourDictionary.Add("4".ToUpperInvariant(), GameConstants.CellColours.PaletteColour_4);
            CellColourDictionary.Add("5".ToUpperInvariant(), GameConstants.CellColours.PaletteColour_5);
            CellColourDictionary.Add("6".ToUpperInvariant(), GameConstants.CellColours.PaletteColour_6);
            CellColourDictionary.Add("7".ToUpperInvariant(), GameConstants.CellColours.PaletteColour_7);
            CellColourDictionary.Add("8".ToUpperInvariant(), GameConstants.CellColours.PaletteColour_8);
            CellColourDictionary.Add("9".ToUpperInvariant(), GameConstants.CellColours.PaletteColour_9);
            CellColourDictionary.Add("a".ToUpperInvariant(), GameConstants.CellColours.PaletteColour_a);
            CellColourDictionary.Add("b".ToUpperInvariant(), GameConstants.CellColours.PaletteColour_b);
            CellColourDictionary.Add("c".ToUpperInvariant(), GameConstants.CellColours.PaletteColour_c);
            CellColourDictionary.Add("d".ToUpperInvariant(), GameConstants.CellColours.PaletteColour_d);
            CellColourDictionary.Add("e".ToUpperInvariant(), GameConstants.CellColours.PaletteColour_e);
            CellColourDictionary.Add("f".ToUpperInvariant(), GameConstants.CellColours.PaletteColour_f);

            List<string> levelStrings = new List<string>();
            List<string> levelNames = new List<string>();

            TextAsset stringSheet = (TextAsset)AssetDatabase.LoadAssetAtPath("Assets/Resources/QuixelStrings.txt", typeof(TextAsset));
            List<string> lines = stringSheet.text.Split('\n').ToList();
            lines.ForEach(line =>
            {
                List<string> parameters = line.Split(',').ToList();
                levelNames.Add(parameters[3]);
                levelStrings.Add(parameters[6]);
            });

            for (int i = 0; i < levelStrings.Count; i++)
            {
                string tempString = levelStrings[i];
                int gridSize = ((int)Mathf.Sqrt(tempString.Length));
                int x = -1;
                int y = gridSize - 1;

                var texture = new Texture2D(gridSize, gridSize, TextureFormat.RGB24, false);

                for (int j = 0; j < tempString.Length; j++)
                {
                    Color color = CellColourDictionary[tempString.Substring(j, 1)];
                    x += 1;
                    texture.SetPixel(x, y, color);

                    if (x == gridSize - 1)
                    {
                        y -= 1;
                        x = -1;
                    }
                }

                // Apply all SetPixel calls
                texture.Apply();

                byte[] bytes = texture.EncodeToPNG();

                int f = i + 1;

                File.WriteAllBytes("Assets/QuixelLogic/Textures/PuzzleImages/" + f + "_" + levelNames[i] + ".png", bytes);
            }
        }

        /// <summary>
        /// Function to make atlas' of pixel cells
        /// </summary>
        //public static void Bitmap()
        //{
        //    Dictionary<string, Color32> CellColourDictionary = new Dictionary<string, Color32>();

        //    CellColourDictionary.Add("x".ToUpperInvariant(), GameConstants.CellColours.PaletteColour_x);
        //    CellColourDictionary.Add("0".ToUpperInvariant(), GameConstants.CellColours.PaletteColour_0);
        //    CellColourDictionary.Add("1".ToUpperInvariant(), GameConstants.CellColours.PaletteColour_1);
        //    CellColourDictionary.Add("2".ToUpperInvariant(), GameConstants.CellColours.PaletteColour_2);
        //    CellColourDictionary.Add("3".ToUpperInvariant(), GameConstants.CellColours.PaletteColour_3);
        //    CellColourDictionary.Add("4".ToUpperInvariant(), GameConstants.CellColours.PaletteColour_4);
        //    CellColourDictionary.Add("5".ToUpperInvariant(), GameConstants.CellColours.PaletteColour_5);
        //    CellColourDictionary.Add("6".ToUpperInvariant(), GameConstants.CellColours.PaletteColour_6);
        //    CellColourDictionary.Add("7".ToUpperInvariant(), GameConstants.CellColours.PaletteColour_7);
        //    CellColourDictionary.Add("8".ToUpperInvariant(), GameConstants.CellColours.PaletteColour_8);
        //    CellColourDictionary.Add("9".ToUpperInvariant(), GameConstants.CellColours.PaletteColour_9);
        //    CellColourDictionary.Add("a".ToUpperInvariant(), GameConstants.CellColours.PaletteColour_a);
        //    CellColourDictionary.Add("b".ToUpperInvariant(), GameConstants.CellColours.PaletteColour_b);
        //    CellColourDictionary.Add("c".ToUpperInvariant(), GameConstants.CellColours.PaletteColour_c);
        //    CellColourDictionary.Add("d".ToUpperInvariant(), GameConstants.CellColours.PaletteColour_d);
        //    CellColourDictionary.Add("e".ToUpperInvariant(), GameConstants.CellColours.PaletteColour_e);
        //    CellColourDictionary.Add("f".ToUpperInvariant(), GameConstants.CellColours.PaletteColour_f);

        //    int gridSizesToAtlas = 5;   // ***
        //    int atlasSize = 64;         // ***

        //    List<string> levelStrings = new List<string>();

        //    using (StreamReader sr = new StreamReader("/Users/nathan/Desktop/QuixelLevelStrings.txt"))
        //    {
        //        string line;
        //        while ((line = sr.ReadLine()) != null)
        //        {
        //            if (line.Length.Equals(gridSizesToAtlas * gridSizesToAtlas))
        //            {
        //                levelStrings.Add(line);
        //            }
        //        }
        //    }

        //    var texture = new Texture2D(atlasSize, atlasSize, TextureFormat.RGB24, true);

        //    int atlasRowNumber = 0;
        //    int atlasColumnNumber = 0;
        //    int possibleHorizontal = (Mathf.FloorToInt(atlasSize / gridSizesToAtlas));

        //    for (int i = 0; i < levelStrings.Count; i++) // 38
        //    {
        //        string tempString = levelStrings[i];
        //        int gridSize = ((int)Mathf.Sqrt(tempString.Length));
        //        int x = -1;
        //        int y = gridSize - 1;

        //        for (int j = 0; j < tempString.Length; j++)
        //        {
        //            Color color = CellColourDictionary[tempString.Substring(j, 1)];
        //            x += 1;

        //            texture.SetPixel((x + atlasRowNumber * gridSizesToAtlas), (y + atlasColumnNumber * gridSizesToAtlas), color);

        //            if (x == gridSize - 1)
        //            {
        //                y -= 1;
        //                x = -1;
        //            }
        //        }

        //        if (atlasRowNumber == possibleHorizontal - 1)
        //        {
        //            atlasRowNumber = -1;
        //            atlasColumnNumber += 1;
        //        }

        //        atlasRowNumber += 1;

        //        // Apply all SetPixel calls
        //        texture.Apply();

        //        byte[] bytes = texture.EncodeToPNG();

        //        string filename = (gridSizesToAtlas + "x" + gridSizesToAtlas + "-Atlas");

        //        File.WriteAllBytes("/Users/nathan/Desktop/QuixelImages/" + filename + ".png", bytes);
        //    }
        //}

        // ****************************************



        /// <summary>
        /// Use this function to create all level settings from string txt file.
        /// </summary>

        [MenuItem("PEAK/Add Settings/Read In Rule Settings", priority = 1)]
        public static void ReadInRuleSettings()
        {
            List<string> levelKeys = new List<string>();
            List<bool> golds = new List<bool>();
            List<string> levelNames = new List<string>();
            List<string> levelStrings = new List<string>();
            List<int> levelTimeLimits = new List<int>();
            List<int> levelTwoStarLimits = new List<int>();
            List<int> levelOneStarLimits = new List<int>();
            List<string> uniqueIDs = new List<string>();

            TextAsset stringSheet = (TextAsset)AssetDatabase.LoadAssetAtPath("Assets/Resources/QuixelStrings.txt", typeof(TextAsset));
            List<string> lines = stringSheet.text.Split('\n').ToList();
            lines.ForEach(line =>
            {
                List<string> parameters = line.Split(',').ToList();
                levelKeys.Add(parameters[0]);
                golds.Add(parameters[1].Contains('y'));
                levelNames.Add(parameters[3]);
                levelStrings.Add(parameters[6]);
                levelTimeLimits.Add(int.Parse(parameters[7]));
                levelTwoStarLimits.Add(int.Parse(parameters[8]));
                levelOneStarLimits.Add(int.Parse(parameters[9]));
                uniqueIDs.Add(parameters[13]);
            });

            int idNumber = 0;
            for (int i = 0; i < levelNames.Count; i++)
            {
                idNumber += 1;

                string name = "Assets/" + GameConstants.GameName.NameOfGame + "/Settings/BoardRules/" + idNumber + "_" + levelNames[i] + ".asset";

                Sprite item = (Sprite)AssetDatabase.LoadAssetAtPath("Assets/" + GameConstants.GameName.NameOfGame + "/Textures/PuzzleImages/" + idNumber + "_" + levelNames[i] + ".png", typeof(Sprite));

                AssertExistingAsset(name);

                RuleSettings newRuleSettings = ScriptableObject.CreateInstance("RuleSettings") as RuleSettings;
                newRuleSettings.Init(idNumber.ToString(), levelKeys[i], levelStrings[i], levelTimeLimits[i], levelTwoStarLimits[i], levelOneStarLimits[i], uniqueIDs[i], item, golds[i]);
                newRuleSettings.IsFtue = i < 6 ? true : false;
                AssetDatabase.CreateAsset(newRuleSettings, name);
            }
        }

        [MenuItem("PEAK/Create localisation txt file", priority = 1)]
        public static void CreateLocalisationTextFile()
        {
            List<string> LocalisationStringLines = new List<string>();
            string qMark = "\"";

            using (StreamReader sr = new StreamReader("Assets/Resources/strings.txt"))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    LocalisationStringLines.Add(line);
                }
            }

            for (int line = 0; line < LocalisationStringLines.Count; line++)
            {
                string tempS = LocalisationStringLines[line];
                tempS = tempS.Insert(0, qMark);

                for (int character = 0; character < tempS.Length; character++)
                {
                    if (tempS[character].Equals(';'))
                    {
                        tempS = tempS.Insert(character, qMark);
                        tempS = tempS.Insert(character + 2, qMark);

                        if (tempS[character + 1].Equals(';'))
                        {
                            character += 2;
                        }
                        else character += 3;
                    }
                }

                tempS += qMark;

                using (StreamWriter sw = new StreamWriter("Assets/Resources/localisation.txt", true))
                {
                    sw.WriteLine(tempS);
                }
            }
        }

        private static string GetPrefabSuffix(int regularLevelCount, int goldLevelCount)
        {
            return goldLevelCount == 0 ? "6" : string.Concat(regularLevelCount.ToString(), "-", goldLevelCount.ToString());
        }

        [MenuItem("PEAK/Add Settings/Create Level Groups", priority = 1)]
        public static void CreateLevelGroups()
        {
            List<int> collectionSizes = new List<int> { 6, 9, 12, 15, 18, 18, 21, 18, 21, 21, 21, 21, 21, 24, 27, 27, 27, 27, 24, 27, 27, 27, 30, 30, 30, 30, 39, 39, 39, 39, 39, 39 };

            LevelOrderSettings levelOrder = (LevelOrderSettings)AssetDatabase.LoadAssetAtPath("Assets/" + GameConstants.GameName.NameOfGame + "/Settings/LevelOrder/LevelOrder.asset", typeof(LevelOrderSettings));
            int levelNumber = 0;

            // make level group assets
            for (int i = 1; i < (collectionSizes.Count + 1); i++) // needs to be one more than number of collections
            {
                string name = "Assets/" + GameConstants.GameName.NameOfGame + "/Settings/LevelGroupings/LevelGroup" + i + ".asset";

                Sprite levelCard = (Sprite)AssetDatabase.LoadAssetAtPath("Assets/" + GameConstants.GameName.NameOfGame + "/Textures/Icons/Level card icons/QXALUIBlurredPhoto" + i + ".png", typeof(Sprite));
                Sprite background = (Sprite)AssetDatabase.LoadAssetAtPath("Assets/" + GameConstants.GameName.NameOfGame + "/Textures/Backgrounds/QXALUIBackground" + i + ".png", typeof(Sprite));
                Sprite backgroundMain = (Sprite)AssetDatabase.LoadAssetAtPath("Assets/" + GameConstants.GameName.NameOfGame + "/Textures/Backgrounds/QXALUIBackgroundMain" + i + ".png", typeof(Sprite));

                AssertExistingAsset(name);

                LevelGroupingSettings newLevelGroupSettings = ScriptableObject.CreateInstance("LevelGroupingSettings") as LevelGroupingSettings;
                newLevelGroupSettings.Init(i.ToString(), false, levelCard, background, backgroundMain);

                for (int j = 0; j < collectionSizes[i - 1]; j++)
                {
                    if (!levelOrder.RuleSettings[levelNumber + j].IsGold)
                    {
                        newLevelGroupSettings.Levels.Add(levelOrder.RuleSettings[levelNumber + j]);
                    }
                    else
                    {
                        newLevelGroupSettings.GoldLevels.Add(levelOrder.RuleSettings[levelNumber + j]);
                    }
                }

                string suffix = GetPrefabSuffix(newLevelGroupSettings.Levels.Count, newLevelGroupSettings.GoldLevels.Count);
                GameObject prefab = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/" + GameConstants.GameName.NameOfGame + "/Prefabs/CollectionWindows/Collection-" + suffix + ".prefab", typeof(GameObject));
                newLevelGroupSettings.CollectionPrefab = prefab;

                levelNumber += collectionSizes[i - 1];

                AssetDatabase.CreateAsset(newLevelGroupSettings, name);
                AssetDatabase.SaveAssets();
            }
        }

        [MenuItem("PEAK/Add Settings/New Rule settings", priority = 1)]
        public static void CreateRuleSettings()
        {
            string name = "Assets/" + GameConstants.GameName.NameOfGame + "/Settings/BoardRules/RuleSettings"
                          + Random.Range(int.MinValue, 0) + ".asset";

            AssertExistingAsset(name);

            ScriptableObject asset = ScriptableObject.CreateInstance<RuleSettings>();
            AssetDatabase.CreateAsset(asset, name);

            Selection.activeObject = asset;
            EditorGUIUtility.PingObject(asset);
        }

        [MenuItem("PEAK/Add Settings/New Level Grouping settings", priority = 1)]
        public static void CreateDifficultyLevelSettings()
        {
            string name = "Assets/" + GameConstants.GameName.NameOfGame + "/Settings/LevelGroupings/NewLevelGroupSettings"
                          + Random.Range(int.MinValue, 0) + ".asset";

            AssertExistingAsset(name);

            ScriptableObject asset = ScriptableObject.CreateInstance<LevelGroupingSettings>();
            AssetDatabase.CreateAsset(asset, name);

            Selection.activeObject = asset;
            EditorGUIUtility.PingObject(asset);
        }

        [MenuItem("PEAK/Add Settings/Level order settings", priority = 1)]
        public static void CreateLevelOrderSettings()
        {
            string name = "Assets/" + GameConstants.GameName.NameOfGame + "/Settings/LevelGroupings/LevelOrder"
                          + Random.Range(int.MinValue, 0) + ".asset";

            AssertExistingAsset(name);

            ScriptableObject asset = ScriptableObject.CreateInstance<LevelOrderSettings>();
            AssetDatabase.CreateAsset(asset, name);

            Selection.activeObject = asset;
            EditorGUIUtility.PingObject(asset);
        }

        [MenuItem("PEAK/Add Settings/Create store settings", priority = 1)]
        public static void CreateStoreSettings()
        {
            string name = "Assets/" + GameConstants.GameName.NameOfGame + "/Settings/StoreSettings/StoreSettings.asset";

            AssertExistingAsset(name);

            ScriptableObject asset = ScriptableObject.CreateInstance<StoreSettings>();
            AssetDatabase.CreateAsset(asset, name);

            Selection.activeObject = asset;
            EditorGUIUtility.PingObject(asset);
        }

        [MenuItem("PEAK/Add Settings/Create store product settings", priority = 1)]
        public static void CreateStoreProductSettings()
        {
            string name = "Assets/" + GameConstants.GameName.NameOfGame + "/Settings/StoreSettings/StoreProductSettings"
                          + Random.Range(int.MinValue, 0) + ".asset";

            AssertExistingAsset(name);

            ScriptableObject asset = ScriptableObject.CreateInstance<StoreProductSettings>();
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