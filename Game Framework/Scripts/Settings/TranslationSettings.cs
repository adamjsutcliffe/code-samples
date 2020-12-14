using System;
using System.Collections.Generic;
using UnityEngine;

namespace Peak.UnityGameFramework.Scripts.Settings
{
    /// <summary>
    /// Settings for text localisations
    /// </summary>
    [Serializable]
    public sealed class TranslationSettings : ScriptableObject
    {
        [Tooltip("The name of the SystemLanguage. E.g. https://docs.unity3d.com/ScriptReference/SystemLanguage.html")]
        public SystemLanguage Language;

        [Tooltip("A list of records")]
        public TranslationRecordSettings[] TranslationRecords;

        public Dictionary<string, string> Texts { get; } =
            new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);

        public override string ToString()
        {
            return $"Localisation for {Language} " +
                   $"contains {TranslationRecords?.Length} {nameof(TranslationRecords)}.";
        }
    }
}