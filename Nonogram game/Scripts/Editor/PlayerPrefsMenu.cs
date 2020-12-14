using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Peak.QuixelLogic.Scripts.Common;
using Peak.QuixelLogic.Scripts.ScenesLogic;
using Peak.QuixelLogic.Scripts.Settings;

namespace Peak.QuixelLogic.Scripts.Editor
{
    public sealed class PlayerPrefsMenu : MonoBehaviour
    {
        [MenuItem("PEAK/Player Prefs/Clear all prefs!")]
        public static void ClearAllPrefs()
        {
            PlayerPrefs.DeleteAll();

            GlobalSettings settings = (GlobalSettings)AssetDatabase.LoadAssetAtPath("Assets/" + GameConstants.GameName.NameOfGame + "/Settings/GlobalSettings.asset", typeof(GlobalSettings));

            for (int i = 0; i < settings.levelGroupingSettings.Length; i++)
            {
                settings.levelGroupingSettings[i].Locked = true;
            }

            for (int j = 0; j < 6; j++)
            {
                settings.LevelOrderSettings.RuleSettings[j].IsFtue = true;
            }

            Debug.Log("Unity Preferences are cleared!");
        }
    }
}