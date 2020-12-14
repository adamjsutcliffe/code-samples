using System;
using UnityEngine;

namespace Peak.Speedoku.Scripts.Settings
{
    /// <summary>
    /// Translation key and its values
    /// </summary>
    [Serializable]
    public sealed class TranslationRecordSettings
    {
        [Tooltip("")]
        public string key;

        [Tooltip("")]
        public string text;
    }
}