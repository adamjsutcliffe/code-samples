using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Peak.Speedoku.Scripts.Common;

namespace Peak.Speedoku.Scripts.Editor
{
    public sealed class PlayerPrefsMenu : MonoBehaviour
    {
        [MenuItem("PEAK/Player Prefs/Clear all prefs")]
        public static void ClearAllPrefs()
        {
            PlayerPrefs.DeleteAll();
            Debug.Log("Unity Preferences are cleared!");
        }
    }
}