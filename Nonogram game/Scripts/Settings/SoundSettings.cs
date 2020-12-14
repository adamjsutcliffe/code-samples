using System;
using System.Collections.Generic;
using UnityEngine;

namespace Peak.QuixelLogic.Scripts.Settings
{
    /// <summary>
    /// Settings for audio localisations
    /// </summary>
    [Serializable]
    public sealed class SoundSettings : ScriptableObject
    {
        [Tooltip("A list of records")]
        public AudioRecordSettings[] AudioRecords;

        public Dictionary<string, AudioRecordSettings> CachedRecords { get; } =
            new Dictionary<string, AudioRecordSettings>(StringComparer.InvariantCultureIgnoreCase);

        public override string ToString()
        {
            return $"Sounds contain {AudioRecords?.Length} {nameof(AudioRecords)}.";
        }
    }
}