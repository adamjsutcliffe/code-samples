using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Peak.QuixelLogic.Scripts.Game;
using Peak.QuixelLogic.Scripts.Settings;

namespace Peak.QuixelLogic.Scripts.Common
{
    public class LevelIDAssignerScript : MonoBehaviour
    {
        [Header("Check or uncheck to set level IDs according to settings order")]
        public bool SetIDs;

        [SerializeField]
        private GlobalSettings globalSettings;

        private void OnValidate()
        {
            LevelOrderSettings levelOrderSettings = globalSettings.LevelOrderSettings;

            for (int i = 0; i < levelOrderSettings.RuleSettings.Length; i++)
            {
                int levelIdentifier = i + 1;
                string result = "" + levelIdentifier;

                levelOrderSettings.RuleSettings[i].Id = result;
            }
        }
    }
}