using System;
using UnityEngine;

namespace Peak.Speedoku.Scripts.Settings
{
    [Serializable]
    public sealed class CoinSettings
    {
        [Tooltip("Amount of coins a player starts with")]
        public int StartCoinCount;
    }
}
