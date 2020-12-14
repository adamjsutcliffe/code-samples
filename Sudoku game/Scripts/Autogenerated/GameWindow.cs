﻿/////////////////////////////////////////////////////////
/////////DO NOT MODIFY THIS AUTOGENERATED FILE///////////
///////////////////////////////////////////////////////// 
using System;
using System.Collections.Generic;

// ReSharper disable InconsistentNaming
namespace Peak.Speedoku.Scripts.Autogenerated
{
    [Serializable]
    public enum GameWindow
    {
        NoWindow = 0,
        GameScene = 1,
        GlobalLogic = 2,
        LoadingMenu = 3,
        StartGame = 4,
        PauseMenu = 5,
        MainMenu = 6,
        OverlayUIScene = 7,
        TutorialPopup = 8,
        SkipFtuePopup = 9,
        SettingsMenu = 10,
        FeedbackPopup = 11
    }

    public static class GameWindowNames
    {
        public static readonly Dictionary<GameWindow, string> Mapping = new Dictionary<GameWindow, string>
        {
             { GameWindow.NoWindow, string.Empty },
             { GameWindow.GameScene , "GameScene" },
             { GameWindow.GlobalLogic , "GlobalLogic" },
             { GameWindow.LoadingMenu , "LoadingMenu" },
             { GameWindow.StartGame , "StartGame" },
             { GameWindow.PauseMenu , "PauseMenu" },
             { GameWindow.MainMenu , "MainMenu" },
             { GameWindow.OverlayUIScene , "OverlayUIScene" },
             { GameWindow.TutorialPopup, "TutorialPopup" } ,
             { GameWindow.SkipFtuePopup, "SkipFtuePopup" },
             { GameWindow.SettingsMenu, "SettingsMenu" },
             { GameWindow.FeedbackPopup, "FeedbackPopup" }
        };
    }
}