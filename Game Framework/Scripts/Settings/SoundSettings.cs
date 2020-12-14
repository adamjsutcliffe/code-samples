using System;
using System.Collections.Generic;
using UnityEngine;

namespace Peak.UnityGameFramework.Scripts.Settings
{
    /// <summary>
    /// Settings for audio localisations
    /// </summary>
    [Serializable]
    public sealed class SoundSettings : ScriptableObject
    {
        [Tooltip("The name of the SystemLanguage. E.g. https://docs.unity3d.com/ScriptReference/SystemLanguage.html")]
        public SystemLanguage Language;

        [Tooltip("A list of records")]
        public AudioRecordSettings[] AudioRecords;

        public Dictionary<string, AudioRecordSettings> CachedRecords { get; } =
            new Dictionary<string, AudioRecordSettings>(StringComparer.InvariantCultureIgnoreCase);

        public override string ToString()
        {
            return $"Localisation for {Language} " +
                   $"contains {AudioRecords?.Length} {nameof(AudioRecords)}.";
        }
    }
}