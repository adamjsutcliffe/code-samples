using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Peak.QuixelLogic.Scripts.Settings
{
    public sealed class GlobalSettings : ScriptableObject
    {
        [Tooltip("Coin specific settings")]
        public CoinSettings Coins;

        [Tooltip("Hint specific settings")]
        public HintSettings Hints;

        [Tooltip("Film specific settings")]
        public FilmSettings Film;

        [Tooltip("The overall level order")]
        public LevelOrderSettings LevelOrderSettings;

        [Tooltip("Difficulty level setting groups")]
        public LevelGroupingSettings[] levelGroupingSettings;
    }
}