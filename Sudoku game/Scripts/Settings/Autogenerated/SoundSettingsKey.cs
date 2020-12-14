﻿/////////////////////////////////////////////////////////
/////////DO NOT MODIFY THIS AUTOGENERATED FILE///////////
///////////////////////////////////////////////////////// 
using System;
using System.Collections.Generic;

namespace Peak.Speedoku.Scripts.Settings.Autogenerated
{
    [Serializable]
    public enum SoundSettingsKey
    {
        NotSet = 0,
        ScoreAdded = 1,
        PopupSound = 2,
        BackgroundMusic = 3,
        LocationUnlocked = 4,
        LevelNumberUpdate = 5,
        NumberAdded = 6,
        CrossAppears = 7,
        ButtonTap = 8,
        BoardAppear = 9,
        CorrectAnswer = 10,
        FtueHint01 = 11,
        FtueHint02 = 12,
        FtueHint03 = 13,
    }

    public static class SoundSettingsValue
    {
        public static readonly Dictionary<SoundSettingsKey, string> Mapping = new Dictionary<SoundSettingsKey, string>
        {
             { SoundSettingsKey.NotSet, string.Empty },
             { SoundSettingsKey.ScoreAdded, "ScoreAdded" },
             { SoundSettingsKey.PopupSound, "PopupSound" },
             { SoundSettingsKey.BackgroundMusic, "BackgroundMusic" },
             { SoundSettingsKey.LocationUnlocked, "LocationUnlocked" },
             { SoundSettingsKey.LevelNumberUpdate, "LevelNumberUpdate" },
             { SoundSettingsKey.NumberAdded, "NumberAdded" },
             { SoundSettingsKey.CrossAppears, "CrossAppears" },
             { SoundSettingsKey.ButtonTap, "ButtonTap" },
             { SoundSettingsKey.BoardAppear, "BoardAppear" },
             { SoundSettingsKey.CorrectAnswer, "CorrectAnswer" },
             { SoundSettingsKey.FtueHint01, "FtueHint01" },
             { SoundSettingsKey.FtueHint02, "FtueHint02" },
             { SoundSettingsKey.FtueHint03, "FtueHint03" }
        };
    }
}