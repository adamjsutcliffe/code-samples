using System;
using UnityEngine;

namespace Peak.QuixelLogic.Scripts.Settings
{
    [Serializable]
    public sealed class FilmSettings
    {
        [Tooltip("Maximum film amount a player can carry")]
        public int MaxFilmCount;

        [Tooltip("Amount of film that a player recieves every increment")]
        public int FilmRewardCount;

        [Tooltip("Time between film awarding in minutes")]
        public int FilmAwardTimerInSeconds;

        [Tooltip("Starting film amount for a new player")]
        public int StartFilmCount;

        [Tooltip("Cost of more film when out of film")]
        public int MoreFilmPurchaseCost;
    }
}