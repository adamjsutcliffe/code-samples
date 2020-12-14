using System;
using UnityEngine;

namespace Peak.QuixelLogic.Scripts.Settings
{
    [Serializable]
    public sealed class HintSettings
    {
        [Tooltip("Amount of hints a player starts with")]
        public int StartHintCount;

        [Tooltip("Cost of additional hints")]
        public int HintCost;
    }
}