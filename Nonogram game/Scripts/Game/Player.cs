using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Peak.QuixelLogic.Scripts.Game
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
        /// Available hints.
        /// </summary>
        public int Hints;

        /// <summary>
        /// Count of how much film the player currently has.
        /// </summary>
        public int Film;

        /// <summary>
        /// Total stars that player has earned.
        /// </summary>
        public int Stars;

        /// <summary>
        /// Shows if the user has just created (not serializable)
        /// </summary>
        public bool IsNew;

        /// <summary>
        /// Index of the level group the player is up to.
        /// </summary>
        public int GroupIndex;

        public int CurrentLevelInGroupIndex;

        /// <summary>
        /// Index of a next game ruleset
        /// </summary>
        public int MainPuzzleIndex;

        public bool NewLocation;

        public bool FtuePassed;

        public string LevelProgress;

        public bool GameComplete;

        public int ActiveGameSessions;

        public bool HasRemovedAds;

        public List<string> bonusSkus;

        public List<string> ABTestKeys;

        public bool ShouldShowAcceptNotification()
        {
            //Debug.Log($"[TEST] Should show notification: main puzzle index: {MainPuzzleIndex} game sessions: {ActiveGameSessions}");

#if UNITY_IOS
            if (MainPuzzleIndex.Equals(11) || ActiveGameSessions.Equals(2))
            {
                return true;
            }
#endif
            return false;
        }

        public int CompletedLevels { get; set; }

        public bool ShouldShowReviewPopup { get; set; }

        public bool ShouldShowPostGameRewardedVideo { get; set; }

        public bool ShouldShowIntroVideo()
        {
            return ActiveGameSessions.Equals(1);
        }

        public bool IsBonusPurchased(string purchaseId)
        {
            for (int i = 0; i < bonusSkus.Count; i++)
            {
                string bonus = bonusSkus[i];
                if (bonus.Equals(purchaseId))
                {
                    return true;
                }
            }
            return false;
        }

        public override string ToString()
        {
            return $"{nameof(Name)}: {Name}, " +
                $"\n{nameof(Guid)}: {Guid}, " +
                $"\n{nameof(Coins)}: {Coins}, " +
                $"\n{nameof(Hints)}: {Hints}, " +
                $"\n{nameof(Film)}: {Film}, " +
                $"\n{nameof(Stars)}: {Stars}, " +
                $"\n{nameof(IsNew)}: {IsNew}, " +
                $"\n{nameof(GroupIndex)}: {GroupIndex}, " +
                $"\n{nameof(CurrentLevelInGroupIndex)}: {CurrentLevelInGroupIndex}, " +
                $"\n{nameof(MainPuzzleIndex)}: {MainPuzzleIndex}" +
                $"\n{nameof(NewLocation)}: {NewLocation}" +
                $"\n{nameof(LevelProgress)}: {LevelProgress}" +
                $"\n{nameof(GameComplete)}: {GameComplete}" +
                $"\n{nameof(ActiveGameSessions)}: {ActiveGameSessions}" +
                $"\n{nameof(bonusSkus)}: {bonusSkus.ToArray()}" +
                $"\n{nameof(ABTestKeys)}: {ABTestKeys.ToArray()}";
        }
    }
}