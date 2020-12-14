using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Peak.Speedoku.Scripts.Common
{
    public static class Constants
    {
        public const int DefaultPoolSize = 10;

        public static class GameName
        {
            // IMPORTANT - must be changed before scenes can be made correctly
            public const string NameOfGame = "Speedoku";
        }

        public static class MainGame
        {
            public const string ContactUsEmailTemplate = "Tell us what you think about the game! Thanks for your feedback!";
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

            public static class Keyboard
            {
                public const string ShowKeyboard = "ShowKeyboard";
                public const string HideKeyboard = "HideKeyboard";
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
        }

        public static class FtueStrings
        {
            public const string GameOneStringOne = "Welcome! There’s only one rule to Sudoku.";
            public const string GameOneStringTwo = "You must fill in the empty squares so that each ROW, COLUMN and SQUARE only have ONE of each number. Let’s try it!";
            public const string GameOneStringThree = "Which number from 1 to 9 is missing in this empty tile?";
            public const string GameOneStringFour = "Select the number from 1 to 9 missing in the highlighted row by using the buttons below.";
            public const string GameOneStringFive = "That’s it!";
            public const string GameOneStringSix = "Let’s try this column. What number from 1 to 9 is missing from it?";
            public const string GameOneStringSeven = "Correct!";
            public const string GameOneStringEight = "This rule applies to rows, columns and also, SQUARES!";
            public const string GameOneStringNine = "Which number is missing from this square?";
            public const string GameOneStringTen = "Great stuff! You completed your first Speedoku!";

            public const string GameStringWrongRow = "Not that one. Pick the only number from 1 to 9 that’s missing from the highlighted row.";
            public const string GameStringWrongCol = "Not that one. Pick the only number from 1 to 9 that’s missing from the highlighted column.";
            public const string GameStringWrongSqr = "Not that one. Pick the only number from 1 to 9 that’s missing from the highlighted square.";

            public const string GameTwoStringOne = "Let’s try a different technique.";
            public const string GameTwoStringTwo = "This square is missing many numbers. But which one goes in our highlighted tile?";
            public const string GameTwoStringThree = "The clues are in the nearby squares! Look at these FIVES.";
            public const string GameTwoStringFour = "We know we can only have ONE number 5 per column or row.";
            public const string GameTwoStringFive = "So, on these rows, there can be NO MORE Fives.";
            public const string GameTwoStringSix = "There’s only one place left in the square that can hold a 5. It’s our highlighted tile!";
            public const string GameTwoStringSeven = "Now that you know which number goes in that tile, select it from the keypad below!";
            public const string GameTwoStringEight = "Fantastic! You should always look for several of the same number around each square.";
            public const string GameTwoStringNine = "Look at these nines.";
            public const string GameTwoStringTen = "They provide helpful clues on where a nine CANNOT exist on the highlighted square.";
            public const string GameTwoStringEleven = "We know we can only have ONE OF EACH per column or row.";
            public const string GameTwoStringTwelve = "So in these rows and column, there CANNOT be any nines.";
            public const string GameTwoStringThirteen = "There’s only one place left in the highlighted square that can hold a 9!";
            public const string GameTwoStringFourteen = "So, which number has to go on our highlighted tile?";
            public const string GameTwoStringFifteen = "Great! Just one more to go!";
            public const string GameTwoStringSixteen = "In this Square, we are missing the 2, 7 and 8. Which one can only go on the selected tile?";
            public const string GameTwoStringSeventeen = "Try again... This square is missing the 2, 7 and 8. Which one goes it that tile?";
            public const string GameTwoStringEighteen = "Fantastic! You learned fast!";
            public const string GameTwoStringNineteen = "You can always find these tips in the pause menu. Good luck!";
        }

        public static class FeedbackInfo
        {
            public const int FirstCheck = 1;
            public const int SecondCheck = 3;
        }
    }
}


