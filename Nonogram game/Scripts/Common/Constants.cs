using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Peak.QuixelLogic.Scripts.Common
{
    public static class Constants
    {
        public static class Notifications
        {
            public const int OneDayInSeconds = 86400; //24 hours
            public const string DayNotificationScheduleTime = "DayNotificationScheduleTime";
        }

        public static class ReviewPopupLevelMilestones
        {
            public const int levelMilestoneOne = 21;
            public const int levelMilestoneTwo = 31;
            public const int levelMilestoneThree = 237;
        }

        public static class Languages
        {
            public const string CurrentLanguage = "CurrentLanguage";

            public const string English = "en";
            public const string French = "fr";
            public const string Italian = "it";
            public const string German = "de";
            public const string Spanish = "es";
            public const string Portuguese = "pt";
            public const string Japanese = "jp";
            public const string ChineseSimplified = "cs";
            public const string ChineseTraditional = "ct";
        }

        public static class ABTesting
        {
            public const string DefaultAB_IDstring = "unassignedABtest";
        }

        public static class Ads
        {
#if UNITY_IOS
            public const int InterstitialTimeRule = 45;
#elif UNITY_ANDROID
            public const int InterstitialTimeRule = 90;
#endif
        }

        public static class Analytics
        {
            public static class Events
            {
                // More analytic events to be added here

                public const string FtueProgress = "Ftue";
                public const string FtueComplete = "FtueComplete";

                public const string DeviceInfo = "DeviceInfo";

                public const string AdWatched = "AdWatched";
                public const string AdError = "AdError";
                public const string AdCancelled = "AdCancelled";
                public const string AdStarted = "AdStarted";

                public const string GameEvent = "GameEvent";
                public const string ScreenChange = "ScreenChange";
                public const string CoinsEarn = "CoinsEarn";
                public const string FilmEvent = "FilmEvent";
                public const string HintUse = "HintUse";
                public const string NewLocationUnlocked = "NewLocationUnlocked";
                public const string StoreOpen = "StoreOpen";
            }

            public static class Parameters
            {
                // More analytic parameters to be added here

                //common
                public const string User = "User";
                public const string AppVersion = "AppVersion";
                public const string UnityAnalyticsUserId = "uaid";

                //1 FtueProgress
                public const string FtueStep = "Step";

                //2 DeviceInfo
                public const string DeviceModel = "Model";
                public const string DeviceOs = "Os";
                public const string DeviceFps = "Fps";

                // Ads
                public const string AdResult = "AdResult";
                public const string AdSource = "AdSource";
                public const string AdType = "AdType";

                //Game
                public const string GameSource = "GameSource";
                public const string GameType = "GameType";
                public const string GameId = "GameId";
                public const string GameLevel = "GameLevel"; // puzzle ID & name
                public const string TimeTakenToSolve = "TimeTakenToSolve";
                public const string StarsEarned = "StarsEarned";
                public const string RemainingFilm = "RemainingFilm";

                public const string HintType = "HintType";

                public const string CoinsAwarded = "CoinsAwarded";
                public const string PlayerCoins = "PlayerCoins";
                public const string PlayerFilm = "PlayerFilm";
                public const string CoinSource = "CoinSource";

                public const string NewLocationIndex = "NewLocationIndex";

                //ScreenChange
                public const string ScreenFrom = "From";
                public const string ScreenTo = "To";
            }

            public static class AdjustEvents
            {
#if UNITY_IOS
                public const string AdClick = "hq23z6";
                public const string AdImpression = "raqk6f";
                public const string GamePlayed = "6ojsk6";
                public const string Purchase = "bh4jqk";
                public const string TutorialComplete = "3317ym";
                public const string CoinSpent = "wwjpof";
#elif UNITY_ANDROID
                public const string AdClick = "x0h20m";
                public const string AdImpression = "gcxdki";
                public const string GamePlayed = "6knunz";
                public const string Purchase = "yj2h56";
                public const string TutorialComplete = "cvtowz";
                public const string CoinSpent = "ko7cxm";
#endif
            }
        }
    }
}