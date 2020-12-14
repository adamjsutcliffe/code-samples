using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Peak.UnityGameFramework.Scripts.Common
{
    public static class Constants
    {
        public const int DefaultPoolSize = 10;

        public static class GameName
        {
            // IMPORTANT - must be changed before scenes can be made correctly
            public const string NameOfGame = "Speedoku";
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

        public static class Scene
        {
            public const string ShowScene = "Show";
            public const string PopScene = "Pop";
        }

        public static class SettingKeys
        {
            public const string GlobalMusicEnabled = "Settings_Music_On";
            public const string GlobalSfxEnabled = "Settings_Sfx_On";
            public const string QualitySettingsEnabled = "Quality_On";
        }

        public static class Analytics
        {
            public static class Events
            {
                // More analytic events to be added here

                public const string FtueProgress = "FtueProgress";

                public const string DeviceInfo = "DeviceInfo";

                public const string AdWatched = "AdWatched";

                public const string GameEvent = "GameEvent";

                public const string ScreenChange = "ScreenChange";


                //Unity Analytic Events

                public const string GameStart = "GameStart";
                public const string GameQuit = "GameQuit";
                public const string GameFinish = "GameFinish";
            }

            public static class Parameters
            {
                // More analytic parameters to be added here

                //common
                public const string User = "User";
                public const string AppVersion = "AppVersion";
                public const string UnityAnalyticsUserId = "uaid";

                //1 FtueProgress
                //public const string FtueProgress = "Step";

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
                public const string GameId = "GameId";
                public const string GameResult = "GameResult";
                public const string GameLevel = "GameLevel";
                public const string GameTime = "GameTime";
                public const string GameData = "GameData";

                //ScreenChange
                public const string ScreenFrom = "From";
                public const string ScreenTo = "To";
            }

            public static class AdjustEvents
            {
                public const string TutorialComplete = "1jr3tq";
                public const string GamePlayed = "67xwgm";
                public const string CoinSpent = "d0g93c";
                public const string AdImpression = "hezxfz";
                public const string AdClick = "o1jjzy";
                public const string ShopView = "oodfxc";
                public const string PackageClick = "n94xlp";
                public const string Purchase = "d52ebr";
            }
        }

        public static class Animation
        {
            public static class MainMenu
            {
                public const string StartGamePlay = "StartGamePlay";
                public const string StartGameBoth = "StartGameBoth";
                public const string StartGameRetry = "StartGameRetry";
                public const string CompleteGame = "CompleteGame";
                public const string ExitGameComplete = "ExitGameComplete";
                public const string ExitGameIncomplete = "ExitGameIncomplete";
                public const string ExitGameFail = "ExitGameFail";
                public const string ExitGameQuit = "ExitGameQuit";
                public const string ShowLocationBar = "ShowLocationBar";
            }

            public static class CheckBox
            {
                public const string ShowAnswer = "ShowAnswer";
                public const string ShowText = "ShowText";
            }

            public static class GridSquare
            {
                public const string PopTarget = "PopTarget";
            }

            public static class QuestionMark
            {
                public const string ShowQuestionMark = "ShowQuestionMark";
            }

            public static class Overlay
            {
                public const string StartTransition = "StartTransition";
                public const string EndTransition = "ExitTransition";
            }

            public static class Round
            {
                public const string CorrectRound = "CorrectRound";
                public const string WrongRound = "WrongRound";
            }
        }
    }
}


