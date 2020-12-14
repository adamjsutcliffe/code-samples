using System.Collections;
using System.Collections.Generic;
using Peak.Speedoku.Scripts.Settings;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace Peak.Speedoku.Scripts.Editor
{
    public class PeakPreProcessBuild : IPreprocessBuildWithReport
    {
        public int callbackOrder { get { return 0; } }

        public void OnPreprocessBuild(BuildReport report)
        {
            //throw new System.NotImplementedException();
            Debug.Log($"[PreBP] Pre build process -> {report}");

            GlobalSettings settings = (GlobalSettings)AssetDatabase.LoadAssetAtPath("Assets/Speedoku/Settings/GlobalSettings.asset", typeof(GlobalSettings));

            if (settings.Coins.StartCoinCount <= 0)
            {
                Debug.LogError($"[ERROR][SETTINGS] Coin start count: {settings.Coins.StartCoinCount}");
            }

            for (int i = 0; i < settings.RulesList.Count; i++)
            {
                RuleSettings rule = settings.RulesList[i];
                if (!rule.IsRuleValid())
                {
                    Debug.LogError($"[ERROR][SETTINGS] Rule {i} is INVALID");
                }
            }
        }
    }
}
