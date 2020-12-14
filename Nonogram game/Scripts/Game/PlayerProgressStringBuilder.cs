using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Peak.QuixelLogic.Scripts.Common;
using Peak.QuixelLogic.Scripts.Game;
using Peak.QuixelLogic.Scripts.Game.CollectionScripts;
using Peak.QuixelLogic.Scripts.Game.Gameplay;
using Peak.QuixelLogic.Scripts.ScenesLogic;
using Peak.QuixelLogic.Scripts.Settings;
using UnityEngine;

namespace Peak.QuixelLogic.Scripts.Game
{
    public sealed class PlayerProgressStringBuilder : MonoBehaviour
    {
        public static PlayerProgressStringBuilder Instance { get; private set; }

        [SerializeField]
        private GlobalSettings globalSettings;

        [SerializeField]
        private ServerController serverController;

        private void Awake()
        {
            if (Instance)
            {
                Debug.LogWarning("[Player Progress String Builder] string builder duplicate instantiation.");
            }
            else
            {
                Instance = this;
            }
        }

        public void RemakePlayerProgressString(MainGameData gameData)
        {
            Player player = SceneActivationBehaviour<GameLogicActivator>.Instance.GameController.Player;

            List<string> progressStrings = new List<string>(1000);
            string s = player.LevelProgress;
            string[] values = s.Split(',');
            int currentStarRecord = 1;

            for (int i = 0; i < values.Length; i++)
            {
                progressStrings.Add(values[i]);

                if (values[i].Contains(gameData.Ruleset.UniqueID))
                {
                    currentStarRecord = int.Parse(values[i].Substring(values[i].Length - 1));
                }
            }

            StringBuilder builder = new StringBuilder();

            for (int j = 0; j < progressStrings.Count; j++)
            {
                if (progressStrings[j].Contains(gameData.Ruleset.UniqueID))
                {
                    if (gameData.StarScore >= currentStarRecord) // if player has exceeded their record for score on this level
                    {
                        builder.Append(string.Concat(gameData.Ruleset.UniqueID, "-", gameData.StarScore));

                        if (!j.Equals(progressStrings.Count - 1))
                        {
                            builder.Append(",");
                        }

                        if (!gameData.Replay)
                        {
                            player.CompletedLevels += 1;
                        }
                    }
                    else
                    {
                        builder.Append(string.Concat(gameData.Ruleset.UniqueID, "-", currentStarRecord));

                        if (!j.Equals(progressStrings.Count - 1))
                        {
                            builder.Append(",");
                        }
                    }
                }
                else
                {
                    builder.Append(progressStrings[j]);

                    if (!j.Equals(progressStrings.Count - 1))
                    {
                        builder.Append(",");
                    }
                }
            }

            player.LevelProgress = builder.ToString();
            serverController.PersistPlayerProgress(player);

            SceneActivationBehaviour<MainMenuActivator>.Instance.SetCurrentProgressOnMainMenu(player, globalSettings);

            return;
        }

        public void RemakePlayerGoldString(string uniqueID, GoldLevelCardScript.GoldCardState cardState, MainGameData gameData = null)
        {
            Player player = SceneActivationBehaviour<GameLogicActivator>.Instance.GameController.Player;

            List<string> progressStrings = new List<string>(1000);
            string s = player.LevelProgress;
            string[] values = s.Split(',');

            for (int i = 0; i < values.Length; i++)
            {
                progressStrings.Add(values[i]);
            }

            StringBuilder builder = new StringBuilder();

            for (int j = 0; j < progressStrings.Count; j++)
            {
                if (progressStrings[j].Contains(uniqueID))
                {
                    if (cardState.Equals(GoldLevelCardScript.GoldCardState.Unlocked))
                    {
                        builder.Append(string.Concat(uniqueID, "-U"));
                        if (!j.Equals(progressStrings.Count - 1))
                        {
                            builder.Append(",");
                        }
                    }
                    else if (cardState.Equals(GoldLevelCardScript.GoldCardState.Purchased))
                    {
                        builder.Append(string.Concat(uniqueID, "-P"));
                        if (!j.Equals(progressStrings.Count - 1))
                        {
                            builder.Append(",");
                        }
                    }
                    else if (cardState.Equals(GoldLevelCardScript.GoldCardState.Complete))
                    {
                        builder.Append(string.Concat(uniqueID, "-", gameData.StarScore));
                        if (!j.Equals(progressStrings.Count - 1))
                        {
                            builder.Append(",");
                        }

                        if (!gameData.Replay)
                        {
                            player.CompletedLevels += 1;
                        }
                    }
                }
                else
                {
                    builder.Append(progressStrings[j]);

                    if (!j.Equals(progressStrings.Count - 1))
                    {
                        builder.Append(",");
                    }
                }
            }

            player.LevelProgress = builder.ToString();
            serverController.PersistPlayerProgress(player);

            if (gameData != null)
            {
                SceneActivationBehaviour<MainMenuActivator>.Instance.SetCurrentProgressOnMainMenu(player, globalSettings);
            }

            return;
        }
    }
}