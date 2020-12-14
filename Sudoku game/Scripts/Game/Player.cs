using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Peak.Speedoku.Scripts.Game
{
    /// <summary>
    /// Holds all player information
    /// </summary>

    [Serializable]
    public sealed class Player
    {
        /// <summary>
        /// Player's name
        /// </summary>
        public string Name;

        /// <summary>
        /// Player's unique ID for Ads, Ranking, Saves, etc.
        /// </summary>
        public string Guid;

        /// <summary>
        /// Available coins
        /// </summary>
        public int Coins;

        /// <summary>
        /// Shows if the user has just created (not serializable)
        /// </summary>
        public bool IsNew;

        /// <summary>
        /// Index of a next game ruleset (both basic and cycled)
        /// </summary>
        public int MainGameLevelIndex;

        /// <summary>
        /// Deduction score, incremented with each successful level passed
        /// </summary>
        public int DeductionScore;

        /// <summary>
        /// Progress counter, dictates to background number, use remainder once past current background limit to loop them
        /// </summary>
        public int ProgressCounter;

        public override string ToString()
        {
            return $"{nameof(Name)}: {Name}, " +
                $"\n{nameof(Guid)}: {Guid}, " +
                $"\n{nameof(Coins)}: {Coins}, " +
                $"\n{nameof(IsNew)}: {IsNew}, " +
                $"\n{nameof(MainGameLevelIndex)}: {MainGameLevelIndex}, " +
                $"\n{nameof(DeductionScore)}: {DeductionScore}," +
                $"\n{nameof(ProgressCounter)}: {ProgressCounter},";
        }

        public bool ValidationCheck()
        {
            //TODO: check types and complex data for correctness
            /*if (Name is null)
            {
                Debug.LogError("Player Name cannot be null");
            }
            if (Guid is null)
            {
                Debug.LogError("Player GUID cannot be null");
            }
            if (MainGameLevelIndex < 0)
            {
                Debug.LogError("Player MainGameLevelIndex cannot be less than zero");
            }
            if (DeductionScore < 0)
            {
                Debug.LogError("Player DeductionScore cannot be less than zero");
            }
            if (ProgressCounter < 0)
            {
                Debug.LogError("Player LocationNumber cannot be less than zero");
            }*/
            return true;
        }
    }
}

