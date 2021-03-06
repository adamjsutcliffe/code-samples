using System;
using UnityEngine;

namespace Peak.Speedoku.Scripts.Settings
{
    /// <summary>
    /// Settings for audio and text localisations
    /// </summary>
    [Serializable]
    public sealed class LocalisationSettings : ScriptableObject
    {
        [Tooltip("Localisable Sound settings")]
        public SoundSettings[] Sound;

        [Tooltip("Localisable Text settings")]
        public TranslationSettings[] Text;
    }
}