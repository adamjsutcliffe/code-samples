using System;
using UnityEngine;

namespace Peak.QuixelLogic.Scripts.Settings
{
    [Serializable]
    public sealed class CoinSettings
    {
        [Tooltip("Amount of coins a player starts with")]
        public int StartCoinCount;

        [Tooltip("Amount of coins a player earns for a 3 star game")]
        public int CoinRewardThreeStarCount;

        [Tooltip("Amount of coins a player earns for a 2 star game")]
        public int CoinRewardTwoStarCount;

        [Tooltip("Amount of coins a player earns for a 1 star game")]
        public int CoinRewardOneStarCount;

        [Tooltip("Post game rewarded video multiplier")]
        public int PostGameCoinMultiplier;

        [Tooltip("Amount of coins a player earns for watching a rewarded video")]
        public int RewardedVideoCoinRewardCount;

        [Header("Costs")]
        [Tooltip("Cost of a level replay")]
        public int LevelReplayCost;

        [Tooltip("Cost of a gold level unlock")]
        public int GoldLevelUnlockCost;
    }
}
