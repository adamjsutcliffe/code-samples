using System;
using UnityEngine;

namespace Peak.UnityGameFramework.Scripts.Settings
{
    /// <summary>
    /// Sound key and its values
    /// </summary>
    [Serializable]
    public sealed class AudioRecordSettings
    {
        [Tooltip("The name of a clip")]
        public string Key;

        [Tooltip("The source of a clip")]
        public AudioClip AudioClip;

        [Tooltip("The type of the clip")]
        public AudioSourceGroup SourceGroup;

        [Tooltip("If checked, the sound can be interrupted by a sound of the same group")]
        public bool CanBeInterrupted;

        [Tooltip("If checked, the sound will not be played if the same sound is already being played")]
        public bool ShouldNotPlayInARow;
    }
}