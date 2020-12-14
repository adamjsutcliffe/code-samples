using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Peak.Speedoku.Scripts.Common.Extensions;
using Peak.Speedoku.Scripts.Game;
using Peak.Speedoku.Scripts.Settings;
using UnityEngine;

namespace Peak.Speedoku.Scripts.Common
{
    public sealed class ServerController : MonoBehaviour
    {
        private GlobalSettings globalSettings;

        #region Pref names used as keys for playerprefs

        public const string PlayerDataPrefsName = Constants.GameName.NameOfGame + "-Player";
        public const string VersionPrefsName = Constants.GameName.NameOfGame + "-Version";

        //Examples
        public const string CoinPrefsName = Constants.GameName.NameOfGame + "-Coins";
        public const string LevelIndexPrefsName = Constants.GameName.NameOfGame + "-LevelIndex";

        //FTUE
        public const string FtueDataPrefsName = Constants.GameName.NameOfGame + "-FTUE";

        #endregion

        // Incremented for each update
        private const int ApplicationVersion = 1;

        private enum PlayerDataType
        {
            Name = 0,
            Guid = 1,
            Coins = 2,
            Level = 3,
        }

        #region Debug persistance logic

        private bool HasPlayerData => PlayerPrefs.HasKey(PlayerDataPrefsName);

        private string PlayerData
        {
            get { return PlayerPrefs.GetString(PlayerDataPrefsName); }
            set { PlayerPrefs.SetString(PlayerDataPrefsName, value); }
        }

        private bool HasDataVersion => PlayerPrefs.HasKey(VersionPrefsName);

        private int DataVersion
        {
            get { return PlayerPrefs.GetInt(VersionPrefsName); }
            set { PlayerPrefs.SetInt(VersionPrefsName, value); }
        }

        private bool HasFtueData => PlayerPrefs.HasKey(FtueDataPrefsName);

        private string FtueData
        {
            get { return PlayerPrefs.GetString(FtueDataPrefsName); }
            set { PlayerPrefs.SetString(FtueDataPrefsName, value); }
        }

        private bool HasLevelIndexData => PlayerPrefs.HasKey(LevelIndexPrefsName);

        private int MainGameLevelIndex
        {
            get { return PlayerPrefs.GetInt(LevelIndexPrefsName); }
            set { PlayerPrefs.SetInt(LevelIndexPrefsName, value); }
        }

        #endregion

        #region migrations

        public void RunMigrations(GlobalSettings customSettings = null)
        {
            globalSettings = globalSettings ?? customSettings;

            Debug.Log("[PREFS] Run migrations - data version: " + DataVersion);

            if (!HasDataVersion)
            {
                DataVersion = 0;
            }
        }

        #endregion

        #region Load data functions

        public Player LoadPlayerData(GlobalSettings settings)
        {
            Player player = new Player();

            if (HasPlayerData)
            {
                try
                {
                    JsonUtility.FromJsonOverwrite(PlayerData, player);
                    player.IsNew = false;
                    print($"[PREFS] [PLAYER] LOADED player: {PlayerData}");

                    return player;
                }
                catch
                {
                    print($"[PREFS] [PLAYER] Exception caught, creating default player: {PlayerData}");
                    //TODO should new player be created???
                    player = CreateDefaultPlayer(settings);
                    //TODO add crashlytics log
                }
            }
            else
            {
                print($"[PREFS] [PLAYER] No player data found, creating default player: {PlayerData}");
                player = CreateDefaultPlayer(settings);
                PersistPlayerProgress(player);
            }

            print($"[PREFS] [PLAYER] Setting server game level: {PlayerData}");
            player.MainGameLevelIndex = HasLevelIndexData ? MainGameLevelIndex : 0;
            return player;
        }

        public FtueInformation LoadFtueInformation()
        {
            FtueInformation ftue = new FtueInformation();

            if (HasFtueData)
            {
                JsonUtility.FromJsonOverwrite(FtueData, ftue);
                return ftue;
            }

            return ftue;
        }

        #endregion


        #region Save data functions

        public void PersistPlayerProgress(Player player)
        {
            //TODO: Add player validation step
            if (player.ValidationCheck())
            {
                PlayerData = JsonUtility.ToJson(player);
                MainGameLevelIndex = player.MainGameLevelIndex;

                print($"[PREFS] [PLAYER] SAVED player: {PlayerData}");
            }
        }

        public void PersistFtue(FtueInformation ftue)
        {
            //TODO: Add ftue information validation step
            FtueData = JsonUtility.ToJson(ftue);
            print($"[PREFS] [FTUE] SAVED ftue: {FtueData}");
        }

        #endregion

        private static Player CreateDefaultPlayer(GlobalSettings settings)
        {
            Player player = new Player
            {
                Name = "New User",
                Guid = Guid.NewGuid().ToString(),
                Coins = settings.Coins.StartCoinCount,
                IsNew = true,
                MainGameLevelIndex = 0,
                DeductionScore = 0
            };

            return player;
        }

    }
}


