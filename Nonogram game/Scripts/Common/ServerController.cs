using System;
using System.Linq;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Peak.QuixelLogic.Scripts.Common.Extensions;
using Peak.QuixelLogic.Scripts.Game;
using Peak.QuixelLogic.Scripts.Settings;
using UnityEngine;
using Fabric.Crashlytics;

namespace Peak.QuixelLogic.Scripts.Common
{
    public sealed class ServerController : MonoBehaviour
    {
        private GlobalSettings globalSettings;

        #region Pref names used as keys for playerprefs

        public const string PlayerDataPrefsName = "QuixelLogic-Player";
        public const string VersionPrefsName = "QuixelLogic-Version";

        //Examples
        public const string CoinPrefsName = "QuixelLogic-Coins";
        public const string MainLevelIndexPrefsName = "QuixelLogic-MainLevelIndex";
        public const string LevelGroupIndexPrefsName = "QuixelLogic-LevelGroupIndex";

        //FTUE
        public const string FtueDataPrefsName = "QuixelLogic-FTUE";

        // Level info
        public const string LevelProgressInformation = "QuixelLogic-LevelProgress";

        #endregion

        // Incremented for each update
        private const int ApplicationVersion = 3;  ////////////////////// ++

        #region Debug persistance logic

        private bool HasPlayerData => PlayerPrefs.HasKey(PlayerDataPrefsName);

        private string PlayerData
        {
            get { return PlayerPrefs.GetString(PlayerDataPrefsName); }
            set { PlayerPrefs.SetString(PlayerDataPrefsName, value); }
        }

        private bool HasFtueData => PlayerPrefs.HasKey(FtueDataPrefsName);

        private string FtueData
        {
            get { return PlayerPrefs.GetString(FtueDataPrefsName); }
            set { PlayerPrefs.SetString(FtueDataPrefsName, value); }
        }

        private bool HasDataVersion => PlayerPrefs.HasKey(VersionPrefsName);

        private int DataVersion
        {
            get { return PlayerPrefs.GetInt(VersionPrefsName); }
            set { PlayerPrefs.SetInt(VersionPrefsName, value); }
        }

        public enum PlayerDataType
        {
            Name = 0,
            Guid = 1,
            Coins = 2,
            Hints = 3,
            Film = 4,
            Stars = 5,
            IsNew = 6,
            GroupIndex = 7,
            CurrentLevelInGroupIndex = 8,
            MainPuzzleIndex = 9,
            NewLocation = 10,
            FtuePassed = 11,
            LevelProgress = 12,
            GameComplete = 13,
            ActiveGameSessions = 14
        }

        #endregion

        private bool debugMode;
        private void Awake()
        {
#if CHEATS
            debugMode = true;
#else
            debugMode = false;
#endif
        }

        #region migrations

        public void RunMigrations(GlobalSettings settings)
        {
            if (!HasDataVersion)
            {
                DataVersion = 0;
            }

            if (DataVersion != ApplicationVersion)
            {
                if (DataVersion < 2)
                {
                    //MigrateToVersion2(settings);
                }
                if (DataVersion < 3)
                {
                    MigrateToVersion3(settings);
                }

                DataVersion = ApplicationVersion;
            }
        }

        //private void MigrateToVersion1(GlobalSettings settings)
        //{
        //    print("Test migrate ");

        //    // TODO: If players have made it past the new level cap (405), scale back their progress?

        //    if (HasPlayerData)
        //    {
        //        print("Migrate - has player data");

        //        string[] data = PlayerData.Split(',');

        //        string[] playerName = data[(int)PlayerDataType.Name].Split(':');
        //        playerName[1] = playerName[1].Trim('"');

        //        string[] playerGuid = data[(int)PlayerDataType.Guid].Split(':');
        //        playerGuid[1] = playerGuid[1].Trim('"');

        //        string[] coins = data[(int)PlayerDataType.Coins].Split(':');
        //        string[] hints = data[(int)PlayerDataType.Hints].Split(':');
        //        string[] stars = data[(int)PlayerDataType.Stars].Split(':');

        //        // old version of player data had puzzle index at position 7
        //        string[] mainPuzzleIndex = data[7].Split(':');

        //        Player player = new Player();

        //        string levelProg = BlankLevelInfo(settings);

        //        player.Name = playerName[1];
        //        player.Guid = playerGuid[1];
        //        player.Coins = coins[1].ParseToInt();

        //        player.Hints = hints[1].ParseToInt();
        //        player.Stars = stars[1].ParseToInt();
        //        player.IsNew = false;
        //        player.GroupIndex = 0;
        //        player.CurrentLevelInGroupIndex = 0;

        //        // assign new puzzle index

        //        int puzzleIndex = mainPuzzleIndex[1].ParseToInt();
        //        //player.MainPuzzleIndex = mainPuzzleIndex[1].ParseToInt();

        //        if (puzzleIndex >= 200)
        //        {
        //            puzzleIndex = 200;
        //        }
        //        else if (puzzleIndex < 200 && puzzleIndex > 6)
        //        {
        //            Mathf.RoundToInt(puzzleIndex / 2);
        //        }

        //        player.MainPuzzleIndex = puzzleIndex;

        //        player.NewLocation = false;
        //        player.FtuePassed = player.MainPuzzleIndex > 6;     // only set ftue as passed if player is past level 6
        //        player.LevelProgress = levelProg;

        //        if (player.FtuePassed)
        //        {
        //            for (int i = 0; i < settings.LevelOrderSettings.RuleSettings.Length; i++)
        //            {
        //                settings.LevelOrderSettings.RuleSettings[i].IsFtue = false;
        //            }
        //        }
        //        else // cancel ftue flags on games that players have completed
        //        {
        //            for (int i = 0; i < player.MainPuzzleIndex; i++)
        //            {
        //                settings.LevelOrderSettings.RuleSettings[i].IsFtue = false;
        //            }
        //        }

        //        PersistPlayerProgress(player);
        //    }
        //}

        private void MigrateToVersion2(GlobalSettings settings)
        {
            print("[TEST] migrating to version 2");

            PlayerPrefs.DeleteAll();

            FtueInformation ftue = new FtueInformation();

            Player player = new Player();
            player = CreateDefaultPlayer(settings);

            PersistPlayerProgress(player);
            PersistFtue(ftue);

            return;
        }

        private void MigrateToVersion3(GlobalSettings settings)
        {
            Player player = LoadPlayerData(settings);
            if (player.bonusSkus == null)
            {
                Debug.Log("MIGRATE V3");
                player.bonusSkus = new List<string>();
            }
        }


        #endregion

        #region Load data functions

        private string UniqueIDOf(string levelString)
        {
            return levelString.Substring(0, levelString.Length - 1);
        }

        private string ProgressIdentifierOf(string levelString)
        {
            return levelString.Substring(levelString.Length - 1, 1);
        }

        private string RefactorString(string ID, string i)
        {
            string refactoredString = ID.Substring(0, ID.Length - 1);
            refactoredString += i;
            return refactoredString;
        }

        private string BuildLevelProgress(Player player, GlobalSettings settings)
        {
            string s = player.LevelProgress;
            string[] values = s.Split(',');
            List<string> savedProgressStringList = values.ToList(); // list of all saved progress strings from playerprefs

            string newProgress = BlankLevelInfo(settings);
            string[] values2 = newProgress.Split(',');
            List<string> newProgressStringList = values2.ToList(); // list of all active levels

            StringBuilder builder = new StringBuilder();

            string[] validGoldProgressIdentifiers = { "P", "1", "2", "3" };

            var savedStringList = savedProgressStringList.Select((value, index) => new { value, index }).Where(oldData => oldData.index < player.MainPuzzleIndex).ToList();
            var newStringList = newProgressStringList.Select((value, index) => new { value, index }).Where(newData => newData.index < player.MainPuzzleIndex).ToList();

            newStringList.ForEach(newData =>
            {
                if (settings.LevelOrderSettings.RuleSettings[newData.index].IsGold)
                {
                    string oldGoldString = savedStringList[newData.index].value;
                    newProgressStringList.RemoveAt(newData.index);
                    newProgressStringList.Insert(newData.index, RefactorString(newData.value, validGoldProgressIdentifiers.Any(validGoldProgressID =>
                    validGoldProgressID.Contains(ProgressIdentifierOf(oldGoldString)))
                    ? ProgressIdentifierOf(oldGoldString) : "U"));
                }
                else
                {
                    if (savedStringList.Any(x => x.value.Contains(UniqueIDOf(newData.value))))
                    {
                        var regularString = savedStringList.FirstOrDefault(x => x.value.Contains(UniqueIDOf(newData.value)));

                        int n;
                        newProgressStringList.RemoveAt(newData.index);

                        newProgressStringList.Insert(newData.index, !int.TryParse(ProgressIdentifierOf(regularString.value), out n)
                        ? RefactorString(newData.value, "3") : n > 0
                        ? RefactorString(newData.value, ProgressIdentifierOf(regularString.value)) : RefactorString(newData.value, "3"));
                    }
                    else
                    {
                        newProgressStringList.RemoveAt(newData.index);
                        newProgressStringList.Insert(newData.index, RefactorString(newData.value, "3"));
                    }
                }
            });

            builder.Clear();

            newProgressStringList.Select((value, index) => new { value, index }).ToList().ForEach(x =>
            {
                builder.Append(x.index.Equals(newProgressStringList.Count - 1) ? x.value : string.Concat(x.value, ","));
                if (x.value.EndsWith("1", StringComparison.InvariantCultureIgnoreCase) || x.value.EndsWith("2", StringComparison.InvariantCultureIgnoreCase) || x.value.EndsWith("3", StringComparison.InvariantCultureIgnoreCase))
                {
                    player.CompletedLevels += 1;
                }
            });

            return builder.ToString();
        }

        public Player LoadLevelProgress(Player player, GlobalSettings settings)
        {
            Crashlytics.Log($"Loading level progress - {JsonUtility.ToJson(player)}");

            int mainIndex = player.MainPuzzleIndex;
            int groupTally = 0;
            int group = 0;
            int level = 0;

            if (!player.GameComplete || player.MainPuzzleIndex < settings.LevelOrderSettings.RuleSettings.Length)
            {
                for (int k = 0; k < settings.levelGroupingSettings.Length; k++)
                {
                    settings.levelGroupingSettings[k].Locked = true;
                }

                for (int l = 0; l < settings.levelGroupingSettings.Length; l++)
                {
                    for (int m = 0; m < settings.levelGroupingSettings[l].Levels.Count; m++)
                    {
                        if (level >= mainIndex)
                        {
                            player.GroupIndex = l;
                            player.CurrentLevelInGroupIndex = groupTally;
                            settings.levelGroupingSettings[group].Locked = false;

                            player.GameComplete = false;
                            player.MainPuzzleIndex = level;
                            player.LevelProgress = BuildLevelProgress(player, settings);

                            print("BUILDER  " + player.LevelProgress);

                            PersistPlayerProgress(player);

                            print($"[PREFS] [PLAYER] LOADED player: {PlayerData}");
                            return player;
                        }

                        groupTally += 1;
                        level += 1;
                    }

                    settings.levelGroupingSettings[group].Locked = false;
                    group += 1;
                    groupTally = 0;
                    level += settings.levelGroupingSettings[l].GoldLevels.Count;
                }

                return player;
            }
            else
            {
                player.LevelProgress = BuildLevelProgress(player, settings);
                PersistPlayerProgress(player);
                print($"[PREFS] [PLAYER] LOADED player (game complete): {PlayerData}");
                return player;
            }
        }

        public Player LoadPlayerData(GlobalSettings settings)
        {
            Player player = new Player();

            if (HasPlayerData)
            {
                try
                {
                    JsonUtility.FromJsonOverwrite(PlayerData, player);
                    player = LoadLevelProgress(player, settings);
                    Crashlytics.SetUserIdentifier(player.Guid);
                }
                catch (Exception e)
                {
                    Debug.LogException(e, this);

                    print($"[PREFS] [PLAYER] Exception caught, creating default player: {PlayerData}");
                    player = CreateDefaultPlayer(settings);
                }
            }
            else
            {
                print($"[PREFS] [PLAYER] No player data found, creating default player: {PlayerData}");
                player = CreateDefaultPlayer(settings);
                PersistPlayerProgress(player);
            }

            //print($"[PREFS] [PLAYER] Setting server game level: {PlayerData}");
            //player.MainPuzzleIndex = HasMainLevelIndexData ? MainGameLevelIndex : 0;
            return player;
        }

        public FtueInformation LoadFtueInformation()
        {
            FtueInformation ftue = new FtueInformation();

            if (HasFtueData)
            {
                JsonUtility.FromJsonOverwrite(FtueData, ftue);

                print($"[PREFS] [FTUE] LOADED ftue data: {FtueData}");
                Crashlytics.Log($"Loading FTUE information - {FtueData}");

                return ftue;
            }

            return ftue;
        }

        #endregion


        #region Save data functions

        public void PersistPlayerProgress(Player player)
        {
            PlayerData = JsonUtility.ToJson(player);
            print($"[PREFS] [PLAYER] SAVED player: {PlayerData}");
            Crashlytics.Log($"Persist player progress - {PlayerData}");
        }

        public void PersistFtue(FtueInformation ftue)
        {
            FtueData = JsonUtility.ToJson(ftue);
            print($"[PREFS] [FTUE] SAVED ftue: {FtueData}");
            Crashlytics.Log($"Persist FTUE progress - {FtueData}");
        }

        #endregion


        private Player CreateDefaultPlayer(GlobalSettings settings)
        {
            print($"[PREFS] [PLAYER] CREATE NEW player");

            string levelProg = BlankLevelInfo(settings);

            settings.levelGroupingSettings[0].Locked = false;
            for (int i = 1; i < settings.levelGroupingSettings.Length; i++)
            {
                settings.levelGroupingSettings[i].Locked = true;
            }

            Player player = new Player
            {
                Name = "New User",
                Guid = Guid.NewGuid().ToString(),
                Coins = settings.Coins.StartCoinCount,
                Hints = settings.Hints.StartHintCount,
                Film = settings.Film.StartFilmCount,
                IsNew = true,
                GroupIndex = 0,
                CurrentLevelInGroupIndex = 0,
                MainPuzzleIndex = 0,
                NewLocation = false,
                FtuePassed = false,
                LevelProgress = levelProg,
                GameComplete = false,
                ActiveGameSessions = 0,
                bonusSkus = new List<string>()
            };

            Crashlytics.SetUserIdentifier(player.Guid);

            return player;
        }

        private string BlankLevelInfo(GlobalSettings settings)
        {
            StringBuilder builder = new StringBuilder();

            for (int i = 0; i < settings.LevelOrderSettings.RuleSettings.Length; i++)
            {
                builder.Append(string.Concat(settings.LevelOrderSettings.RuleSettings[i].UniqueID, "-"));

                if (!settings.LevelOrderSettings.RuleSettings[i].IsGold)
                {
                    builder.Append(string.Concat(0.ToString(), ","));
                }
                else
                {
                    if (i.Equals(settings.LevelOrderSettings.RuleSettings.Length - 1))
                    {
                        builder.Append("L");
                    }
                    else builder.Append(string.Concat("L", ","));
                }
            }

            return builder.ToString();
        }

        //    private String AppendLevelInfo(GlobalSettings settings, Player player)
        //    {
        //        string newLevelString = player.LevelProgress;
        //        string appendLevelString = ",";

        //        // check what strings from the new level order are present in existing saved string
        //        for (int i = 0; i < settings.LevelOrderSettings.RuleSettings.Length; i++)
        //        {
        //            if (!newLevelString.Contains(settings.LevelOrderSettings.RuleSettings[i].UniqueID))
        //            {
        //                appendLevelString += settings.LevelOrderSettings.RuleSettings[i].UniqueID + "-" + 0.ToString() + ",";
        //            }
        //        }

        //        newLevelString += appendLevelString;
        //        return newLevelString;
        //    }

        private void DebugLog(string message)
        {
            if (!debugMode)
            {
                return;
            }

            Debug.Log(message, gameObject);
        }
    }
}