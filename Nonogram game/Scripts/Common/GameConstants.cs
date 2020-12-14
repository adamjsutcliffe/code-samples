using System.Collections;
using System.Collections.Generic;
using Peak.QuixelLogic.Scripts.Common.Localisation;
using UnityEngine;

namespace Peak.QuixelLogic.Scripts.Common
{
    public static class GameConstants
    {
        public const int DefaultPoolSize = 10;

        public static class GameName
        {
            public const string NameOfGame = "QuixelLogic";
        }

        public static class CellColours
        {
            public static readonly Color32 PaletteColour_x = new Color32(255, 255, 255, 0);
            public static readonly Color32 PaletteColour_0 = new Color32(0, 0, 0, 0);
            public static readonly Color32 PaletteColour_1 = new Color32(95, 87, 79, 0);
            public static readonly Color32 PaletteColour_2 = new Color32(148, 141, 146, 0);
            public static readonly Color32 PaletteColour_3 = new Color32(194, 195, 199, 0);
            public static readonly Color32 PaletteColour_4 = new Color32(126, 37, 83, 0);
            public static readonly Color32 PaletteColour_5 = new Color32(235, 0, 71, 0);
            public static readonly Color32 PaletteColour_6 = new Color32(255, 135, 141, 0);
            public static readonly Color32 PaletteColour_7 = new Color32(255, 204, 170, 0);
            public static readonly Color32 PaletteColour_8 = new Color32(37, 54, 105, 0);
            public static readonly Color32 PaletteColour_9 = new Color32(0, 93, 209, 0);
            public static readonly Color32 PaletteColour_a = new Color32(1, 185, 245, 0);
            public static readonly Color32 PaletteColour_b = new Color32(171, 82, 54, 0);
            public static readonly Color32 PaletteColour_c = new Color32(245, 150, 0, 0);
            public static readonly Color32 PaletteColour_d = new Color32(249, 219, 58, 0);
            public static readonly Color32 PaletteColour_e = new Color32(0, 135, 81, 0);
            public static readonly Color32 PaletteColour_f = new Color32(79, 215, 26, 0);

            public static readonly Color32 cellDeselected = new Color32(189, 189, 191, 255);
            public static readonly Color32 cellSelected = new Color32(69, 69, 70, 255);

            public static readonly Color32 hintColour = new Color32(133, 222, 92, 255);
        }

        public static class UIColours
        {
            public static readonly Color32 LabelColourDefault = new Color32(109, 109, 109, 130);
            public static readonly Color32 LabelColourHighlighted = new Color32(9, 109, 102, 255);

            public static readonly Color32 TextColourDefault = new Color32(255, 255, 255, 255);
            public static readonly Color32 TextColourHighlighted = new Color32(255, 255, 255, 255);

            public static readonly Color32 TextColourDimmed = new Color32(255, 255, 255, 255);
        }

        public static class ButtonColours
        {
            public static readonly Color32 ButtonTextInactiveColour = new Color32(255, 255, 255, 255);
            public static readonly Color32 ButtonTextDisabledColour = new Color32(54, 120, 104, 255);
        }

        public static class SettingKeys
        {
            public const string GlobalMusicEnabled = "Settings_Music_On";
            public const string GlobalSfxEnabled = "Settings_Sfx_On";
            public const string QualitySettingsEnabled = "Quality_On";
            public const string NotificationsOn = "NotificationsOn";
        }

        public static class ToolToggleAnimationTriggers
        {
            public const string ToggleToCross = "ToggleToCross";
            public const string ToggleToPaint = "ToggleToPaint";
        }

        public static class MainGame
        {
            public static string Level = LocalisationSystem.GetLocalisedValue("level");
            public static string ContactUsEmailTemplate = LocalisationSystem.GetLocalisedValue("contactEmailBody");

            public static class FeedbackMessages
            {
                public static string Incredible = LocalisationSystem.GetLocalisedValue("feedback_incred"); //"Incredible!";
                public static string Great = LocalisationSystem.GetLocalisedValue("feedback_great");  //"Great!";
                public static string Good = LocalisationSystem.GetLocalisedValue("feedback_good");  //"Good!";
            }

            public static class FeatureMessages
            {
                public static string NewLocationUnlocked = LocalisationSystem.GetLocalisedValue("newLocationUnlock");
                public static string CollectionComplete = LocalisationSystem.GetLocalisedValue("colComplete");
                public static string AllCollectionsComplete = LocalisationSystem.GetLocalisedValue("allColsComplete");
                public static string ClaimNewLocationHeader = LocalisationSystem.GetLocalisedValue("newLocation");
                public static string ClaimYourPrizeHeader = LocalisationSystem.GetLocalisedValue("yourPrize");
                public static string GoldPlayError = LocalisationSystem.GetLocalisedValue("goldPlayError");
                public static string ReplayTitle = LocalisationSystem.GetLocalisedValue("replayTitle");
                public static string ReplayError = LocalisationSystem.GetLocalisedValue("replayError");
                public static string FreeFilmIn = LocalisationSystem.GetLocalisedValue("filmCountdown1");
                public static string Film = LocalisationSystem.GetLocalisedValue("film");
            }
        }

        public static class CollectionView
        {
            public const float LevelRowSize = 235;
            public const float CollectionHeaderSize = 155f;

            public static string Collection = LocalisationSystem.GetLocalisedValue("collection");
            public static string Locked = LocalisationSystem.GetLocalisedValue("locked");
        }

        public static class FilmFeature
        {
            public const string DayLastOpened = "DayLastOpened";
            public const string DateTimeFilmLastSpent = "DateTimeFilmLastSpent";
            public static string Full = LocalisationSystem.GetLocalisedValue("full");
        }

        public static class HowToPlay
        {
            public static List<string> steps = new List<string>
            {
                LocalisationSystem.GetLocalisedValue("howToPlay1"),
                LocalisationSystem.GetLocalisedValue("howToPlay2"),
                LocalisationSystem.GetLocalisedValue("howToPlay3"),
                LocalisationSystem.GetLocalisedValue("howToPlay4"),
                LocalisationSystem.GetLocalisedValue("howToPlay5"),
                LocalisationSystem.GetLocalisedValue("howToPlay6"),
                LocalisationSystem.GetLocalisedValue("howToPlay7"),
                LocalisationSystem.GetLocalisedValue("howToPlay8"),
            };
        }

        public static class Ftue
        {
            public static class FirstGame
            {
                public static class FtueMessages
                {
                    public static string Instructions1_1 = LocalisationSystem.GetLocalisedValue("ftue1_1"); //"The goal is to reveal a hidden picture by using the numbers on the sides as clues.";
                    public static string Instructions1_2 = LocalisationSystem.GetLocalisedValue("ftue1_2"); //"This number 5 means there are FIVE adjacent painted tiles in this row.";
                    public static string Instructions1_3 = LocalisationSystem.GetLocalisedValue("ftue1_3"); //"This column only has FIVE tiles, so that means all FIVE tiles need to be painted!";
                    public static string Instructions1_4 = LocalisationSystem.GetLocalisedValue("ftue1_4"); //"Cool! By completing that column you also found the single correct tile for this row!";
                    public static string Instructions1_5 = LocalisationSystem.GetLocalisedValue("ftue1_5"); //"Remember to always start with the big numbers first!";
                    public static string Instructions1_6 = LocalisationSystem.GetLocalisedValue("ftue1_6"); //"Great! We know this row can only have ONE tile. So..";
                    public static string Instructions1_7 = LocalisationSystem.GetLocalisedValue("ftue1_7"); //"..this series of 4 adjacent tiles can only go here!";
                    public static string Instructions1_8 = LocalisationSystem.GetLocalisedValue("ftue1_8"); //"There’s only one missing tile in the Column of 4! Can you find it?";
                }

                public static class TargetCellVectors
                {
                    public static List<Vector2> TargetCells1_2 = new List<Vector2> { new Vector2(2, 1), new Vector2(2, 2), new Vector2(2, 3), new Vector2(2, 4), new Vector2(2, 5) };
                    public static List<Vector2> TargetCells1_3 = new List<Vector2> { new Vector2(1, 3), new Vector2(3, 3), new Vector2(4, 3), new Vector2(5, 3) };
                    public static List<Vector2> TargetCells1_5 = new List<Vector2> { new Vector2(3, 1), new Vector2(3, 2), new Vector2(3, 4), new Vector2(3, 5), new Vector2(4, 1), new Vector2(4, 2), new Vector2(4, 4), new Vector2(4, 5) };
                    public static List<Vector2> TargetCells1_7 = new List<Vector2> { new Vector2(1, 2) };
                    public static List<Vector2> TargetCells1_8 = new List<Vector2> { new Vector2(1, 4) };
                }

                public static class HandAnimations
                {
                    public const string Hand1_2 = "Hand1_2";
                    public const string Hand1_3 = "Hand1_3";
                    public const string Hand1_5 = "Hand1_5";
                    public const string Hand1_7 = "Hand1_7";
                }

                public static class DimAnimations
                {
                    public const string Dim1_2 = "Dim1_2";
                    public const string Dim1_3 = "Dim1_3";
                    public const string Dim1_4 = "Dim1_4";
                    public const string Dim1_5 = "Dim1_5";
                    public const string Dim1_6 = "Dim1_6";
                    public const string Dim1_7 = "Dim1_7";
                    public const string Dim1_8 = "Dim1_8";
                }
            }

            public static class SecondGame
            {
                public static class FtueMessages
                {
                    public static string Instructions2_0 = LocalisationSystem.GetLocalisedValue("ftue2_1"); //"Let’s find the hidden picture!";
                    public static string Instructions2_1 = LocalisationSystem.GetLocalisedValue("ftue2_2"); //"Remember to start with the big numbers first!";
                    public static string Instructions2_2 = LocalisationSystem.GetLocalisedValue("ftue2_3"); //"Great! Keep going!";
                    public static string Instructions2_3 = LocalisationSystem.GetLocalisedValue("ftue2_4"); //"A 2 and a 2? That means a pair of SEPARATE groups, each with 2 tiles!";
                    public static string Instructions2_4 = LocalisationSystem.GetLocalisedValue("ftue2_5"); //"There’s only one tile you can paint to make a GROUP of 3 tiles in this row!";
                    public static string Instructions2_5 = LocalisationSystem.GetLocalisedValue("ftue2_6"); //"Now finish these rows with THREE adjacent tiles to reveal the hidden picture!";
                }

                public static class TargetCellVectors
                {
                    public static List<Vector2> TargetCells2_1 = new List<Vector2> { new Vector2(2, 1), new Vector2(2, 2), new Vector2(2, 3), new Vector2(2, 4), new Vector2(2, 5) };
                    public static List<Vector2> TargetCells2_2 = new List<Vector2> { new Vector2(1, 2), new Vector2(3, 2), new Vector2(4, 2), new Vector2(5, 2), new Vector2(1, 4), new Vector2(3, 4), new Vector2(4, 4), new Vector2(5, 4) };
                    public static List<Vector2> TargetCells2_3 = new List<Vector2> { new Vector2(1, 1), new Vector2(1, 5) };
                    public static List<Vector2> TargetCells2_4 = new List<Vector2> { new Vector2(3, 3) };
                    public static List<Vector2> TargetCells2_5 = new List<Vector2> { new Vector2(4, 3), new Vector2(5, 3) };
                }

                public static class DimAnimations
                {
                    public const string Dim2_1 = "Dim2_1";
                    public const string Dim2_2 = "Dim2_2";
                    public const string Dim2_3 = "Dim2_3";
                    public const string Dim2_4 = "Dim2_4";
                    public const string Dim2_5 = "Dim2_5";
                }
            }

            public static class ThirdGame
            {
                public static class FtueMessages
                {
                    public static string Instructions3_0 = LocalisationSystem.GetLocalisedValue("ftue3_1"); // "Let’s learn something new!";
                    public static string Instructions3_1 = LocalisationSystem.GetLocalisedValue("ftue3_2"); //"First, let’s get these big numbers out of the way!";
                    public static string Instructions3_2 = LocalisationSystem.GetLocalisedValue("ftue3_3"); //"This row also needs 5 connected tiles to be complete.";
                    public static string Instructions3_3 = LocalisationSystem.GetLocalisedValue("ftue3_4"); //"Here you need a group of 4 connected black tiles. Paint the ones missing!";
                    public static string Instructions3_4 = LocalisationSystem.GetLocalisedValue("ftue3_5"); //"What tiles will you paint to get the 4 connected tiles needed here?";
                    public static string Instructions3_5 = LocalisationSystem.GetLocalisedValue("ftue3_6"); //"Several digits? This means you have TWO adjacent tiles and ONE single tile in this column!";
                    public static string Instructions3_6 = LocalisationSystem.GetLocalisedValue("ftue3_7"); //"This column also has 2 adjacent tiles, and one single tile. Paint the correct missing tile!";
                    public static string Instructions3_7 = LocalisationSystem.GetLocalisedValue("ftue3_8"); //"In this row, there are 3 separate painted tiles! Paint the missing one!";
                    public static string Instructions3_8 = LocalisationSystem.GetLocalisedValue("ftue3_9"); //"Complete this column so you have 3 adjacent tiles, an empty tile and a single painted tile.";
                }

                public static class TargetCellVectors
                {
                    public static List<Vector2> TargetCells3_1 = new List<Vector2> { new Vector2(2, 1), new Vector2(2, 2), new Vector2(2, 3), new Vector2(2, 4), new Vector2(2, 5) };
                    public static List<Vector2> TargetCells3_2 = new List<Vector2> { new Vector2(5, 1), new Vector2(5, 2), new Vector2(5, 3), new Vector2(5, 4), new Vector2(5, 5) };
                    public static List<Vector2> TargetCells3_3 = new List<Vector2> { new Vector2(3, 2), new Vector2(4, 2) };
                    public static List<Vector2> TargetCells3_4 = new List<Vector2> { new Vector2(3, 4), new Vector2(4, 4) };
                    public static List<Vector2> TargetCells3_5 = new List<Vector2> { new Vector2(1, 1) };
                    public static List<Vector2> TargetCells3_6 = new List<Vector2> { new Vector2(1, 5) };
                    public static List<Vector2> TargetCells3_7 = new List<Vector2> { new Vector2(1, 3) };
                    public static List<Vector2> TargetCells3_8 = new List<Vector2> { new Vector2(3, 3) };
                }

                public static class DimAnimations
                {
                    public const string Dim3_1 = "Dim3_1";
                    public const string Dim3_2 = "Dim3_2";
                    public const string Dim3_3 = "Dim3_3";
                    public const string Dim3_4 = "Dim3_4";
                    public const string Dim3_5 = "Dim3_5";
                    public const string Dim3_6 = "Dim3_6";
                    public const string Dim3_7 = "Dim3_7";
                    public const string Dim3_8 = "Dim3_8";
                }
            }

            public static class FourthGame
            {
                public static class FtueMessages
                {
                    public static string Instructions4_1 = LocalisationSystem.GetLocalisedValue("ftue4_1"); //"This is the star bar! The quicker you solve the puzzle, the more stars you earn!";
                    public static string Instructions4_2 = LocalisationSystem.GetLocalisedValue("ftue4_2"); //"Ready? Start with those big numbers first!";
                }
            }

            public static class FifthGame
            {
                public static class FtueMessages
                {
                    public static string Instructions5_1 = LocalisationSystem.GetLocalisedValue("ftue5_1"); //"Sometimes you can't tell where all the painted tiles go.";
                    public static string Instructions5_2 = LocalisationSystem.GetLocalisedValue("ftue5_2"); //"But one thing is for sure, these 3 tiles are always going to be correct!";
                    public static string Instructions5_3 = LocalisationSystem.GetLocalisedValue("ftue5_3"); //"Now to teach you about your new tool! But first, let’s fill these!";

                    public static string Instructions5_5 = LocalisationSystem.GetLocalisedValue("ftue5_4"); //"Now lets try the X tool!";
                    public static string Instructions5_6 = LocalisationSystem.GetLocalisedValue("ftue5_5"); // "You already found the correct tile in this row! Let’s cross the others out.";
                    public static string Instructions5_7 = LocalisationSystem.GetLocalisedValue("ftue5_6"); //"Cool! Just a few more to go!";
                    public static string Instructions5_8 = LocalisationSystem.GetLocalisedValue("ftue5_7"); //"Now it’s easy to remember which tiles NOT to paint!";
                    public static string Instructions5_9 = LocalisationSystem.GetLocalisedValue("ftue5_8"); //"Now toggle back to the MARKER and finish the puzzle. You can do it!";
                }

                public static class TargetCellVectors
                {
                    public static List<Vector2> TargetCells5_2 = new List<Vector2> { new Vector2(2, 2), new Vector2(3, 2), new Vector2(4, 2) };
                    public static List<Vector2> TargetCells5_3 = new List<Vector2> { new Vector2(1, 3), new Vector2(2, 3), new Vector2(3, 3), new Vector2(4, 3), new Vector2(5, 3) };
                    public static List<Vector2> TargetCells5_4 = new List<Vector2> { new Vector2(3, 1), new Vector2(3, 4), new Vector2(3, 5), new Vector2(4, 1), new Vector2(4, 4), new Vector2(4, 5) };
                    public static List<Vector2> TargetCells5_6 = new List<Vector2> { new Vector2(1, 1), new Vector2(1, 2), new Vector2(1, 4), new Vector2(1, 5) };
                    public static List<Vector2> TargetCells5_7 = new List<Vector2> { new Vector2(2, 1), new Vector2(5, 1), new Vector2(2, 5), new Vector2(5, 5) };
                }

                public static class HighlightCellAnimations
                {
                    public const string HighlightCells5_1 = "Highlight2_1";
                }

                public static class HandAnimations
                {
                    public const string Hand5_5 = "Hand2_5";
                    public const string Hand5_9 = "Hand2_9";
                }
            }

            public static class SixthGame
            {
                public static class FtueMessages
                {
                    public static string Instructions6_1 = LocalisationSystem.GetLocalisedValue("ftue6_1"); // "If you’re ever stuck you can use a hint. Try it!";
                    public static string Instructions6_2 = LocalisationSystem.GetLocalisedValue("ftue6_2"); //"Tiles revealed by using the hint can’t be undone!";
                }

                public static class HandAnimations
                {
                    public const string Hand6_1 = "Hand3_1";
                }
            }

            public static class CollectionView
            {
                public static class FtueMessages
                {
                    public static string InstructionsCollection_1 = LocalisationSystem.GetLocalisedValue("ftueColl1"); //"This is your collection of pixel puzzles!";
                    public static string InstructionsCollection_2 = LocalisationSystem.GetLocalisedValue("ftueColl2"); //"Replay previous levels by tapping them or play a new puzzle by pressing play!";

                    public static string InstructionsCollection_3 = LocalisationSystem.GetLocalisedValue("ftueColl3");
                    public static string InstructionsCollection_4 = LocalisationSystem.GetLocalisedValue("ftueColl4");
                }
            }
        }

        public static class NotificationPopups
        {
            public static string NotificationPopupTitle = LocalisationSystem.GetLocalisedValue("notifPopupTitle"); //"smart human!";
            public static string NotificationPopupBody = LocalisationSystem.GetLocalisedValue("notifPopupBody"); //"You're getting smarter with every picture! Let's do this every day!";
            public static string OutOfFilmNotificationPopupTitle = LocalisationSystem.GetLocalisedValue("notifFilmPopupTitle"); //"don't miss out!";
            public static string OutOfFilmNotificationPopupBody = LocalisationSystem.GetLocalisedValue("notifFilmPopupBody"); //"Turn notifications on! I'll tell you when there's more free film!";
        }

        public static class NotificationMessages
        {
            public static string DayNotif_v1 = LocalisationSystem.GetLocalisedValue("24hrNotif_v1");
            public static string DayNotif_v2 = LocalisationSystem.GetLocalisedValue("24hrNotif_v2");
            public static string DayNotif_v3 = LocalisationSystem.GetLocalisedValue("24hrNotif_v3");
            public static string FilmNotif_v1 = LocalisationSystem.GetLocalisedValue("filmNotif_v1");
            public static string FilmNotif_v2 = LocalisationSystem.GetLocalisedValue("filmNotif_v2");
        }

        public static class StoreMessages
        {
            public static string TitleSuccess = LocalisationSystem.GetLocalisedValue("success");
            public static string TitleCancelled = LocalisationSystem.GetLocalisedValue("cancelled");
            public static string TitleConnectivity = LocalisationSystem.GetLocalisedValue("noConnection");
            public static string TitleFailed = LocalisationSystem.GetLocalisedValue("failed");
            public static string MessageSuccess = LocalisationSystem.GetLocalisedValue("purchaseSuccess");
            public static string MessageCancelled = LocalisationSystem.GetLocalisedValue("purchaseCancelled");
            public static string MessageConnectivity = LocalisationSystem.GetLocalisedValue("noInternetAccess");
            public static string MessageFailed = LocalisationSystem.GetLocalisedValue("failed_message");
            public static string ButtonClaim = LocalisationSystem.GetLocalisedValue("claim");
            public static string ButtonContinue = LocalisationSystem.GetLocalisedValue("continue");
        }
    }
}