﻿/////////////////////////////////////////////////////////
/////////DO NOT MODIFY THIS AUTOGENERATED FILE///////////
///////////////////////////////////////////////////////// 
using System;
using System.Collections.Generic;

namespace Peak.UnityGameFramework.Scripts.Settings.Autogenerated
{
    [Serializable]
    public enum SoundSettingsKey
    {
        NotSet = 0,

        //SPE Sounds

        //ScoreAdded = 1,
        //PopupSound = 2,
        //BackgroundMusic = 3,
        //LocationUnlocked = 4,
        //LevelNumberUpdate = 5,
        //NumberAdded = 6,
        //CrossAppears = 7,
        //ButtonTap = 8,
        //BoardAppear = 9

        //WPA Sounds
        WpaCountDown = 10,
        WpaPathComplete = 11,
        WpaWordCreated = 12,
        WpaPickUpTile = 13,
        WpaPlaceTile = 14,
        WpaReturnTile = 15,
        WpaGridMove = 16,
        WpaShuffle = 17,
        UIButtonDown = 18,
        WpaSlideTouch = 19,
        GUICorrect = 20,
        GUIWrong = 21,
    }

    public static class SoundSettingsValue
    {
        public static readonly Dictionary<SoundSettingsKey, string> Mapping = new Dictionary<SoundSettingsKey, string>
        {
             { SoundSettingsKey.NotSet, string.Empty },

            //SPE Sounds
            //{ SoundSettingsKey.ScoreAdded, "ScoreAdded" },
            //{ SoundSettingsKey.PopupSound, "PopupSound" },
            //{ SoundSettingsKey.BackgroundMusic, "BackgroundMusic" },
            //{ SoundSettingsKey.LocationUnlocked, "LocationUnlocked" },
            //{ SoundSettingsKey.LevelNumberUpdate, "LevelNumberUpdate" },
            //{ SoundSettingsKey.NumberAdded, "NumberAdded" },
            //{ SoundSettingsKey.CrossAppears, "CrossAppears" },
            //{ SoundSettingsKey.ButtonTap, "ButtonTap" },
            //{ SoundSettingsKey.BoardAppear, "BoardAppear" }

            //WPA Sounds
            { SoundSettingsKey.WpaCountDown, "GUIOutroCountDown" },
            { SoundSettingsKey.WpaPathComplete, "sfx_pathComplete" },
            { SoundSettingsKey.WpaWordCreated, "sfx_wordCreated" },
            { SoundSettingsKey.WpaPickUpTile, "sfx_wordPath_pickUp" },
            { SoundSettingsKey.WpaPlaceTile, "sfx_wordPath_placeTile" },
            { SoundSettingsKey.WpaReturnTile, "sfx_wordPath_returnTile" },
            { SoundSettingsKey.WpaGridMove, "WPAGridMove" },
            { SoundSettingsKey.WpaShuffle, "WPAShuffe" },
            { SoundSettingsKey.UIButtonDown, "UIButtonDown" },
            { SoundSettingsKey.WpaSlideTouch, "sfx_slider_touch" },
            { SoundSettingsKey.GUICorrect, "GUICorrect" },
            { SoundSettingsKey.GUIWrong, "GUIWrong" },
        };
    }
}