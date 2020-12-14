﻿//using Peak.WordFresh.Scripts.Game;
//using Peak.WordFresh.Scripts.Game.Gameplay;
//using Peak.WordFresh.Scripts.Settings;
using Peak.Speedoku.Scripts.Common;
using Peak.Speedoku.Scripts.Game;
using Peak.Speedoku.Scripts.Game.Gameplay;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.EventSystems;
using Peak.Speedoku.Scripts.Common.AnalyticsScripts;
using Peak.Speedoku.Scripts.ScenesLogic;
using Peak.Speedoku.Scripts.Autogenerated;

#if CHEATS
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
//using Peak.WordFresh.Scripts.Autogenerated;
//using Peak.WordFresh.Scripts.Map.UnityModel;
//using Peak.WordFresh.Scripts.ScenesLogic;
//using FyberPlugin;
#endif
namespace Peak.Speedoku.Scripts.Common
{
    public sealed class CheatController : MonoBehaviour
    {
        //[Header("Settings")]
        //[SerializeField]
        //private MapSettings settings;

        [Header("Main controllers")]
        [SerializeField]
        private GameController gameController;

        //[SerializeField]
        //private FtueController ftueController;

        [SerializeField]
        private SessionScript sessionScript;

        [SerializeField]
        private InputController inputController;

        //[Header("Other controllers")]
        //[SerializeField]
        //private StoreController storeController;

        [SerializeField]
        private ServerController serverController;

        [SerializeField]
        private AnalyticsController analyticsController;

        [SerializeField]
        private AdsController adController;

        [SerializeField]
        private EventSystem input;

#if CHEATS

        private enum CheatTab
        {
            Closed = 0,
            Other,
            Ftue,
            Levels,
            Framerate,
            Analytics,
            Events,
            Ads,
            Haptics
        }

        #region UI

        private bool shouldShowLevels;
        private bool shouldShowCoins;
        private bool shouldShowLetters;
        private bool shouldShowBooks;

        private GUIStyle whiteStyle;
        private GUIStyle blackStyle;

        private Vector2 scrollPosition;

        private CheatTab tab = CheatTab.Closed;

        private void Awake()
        {
            whiteStyle = whiteStyle ?? new GUIStyle
            {
                active = new GUIStyleState { background = Texture2D.whiteTexture },
                normal = new GUIStyleState { background = Texture2D.whiteTexture }
            };

            blackStyle = blackStyle ?? new GUIStyle
            {
                active = new GUIStyleState { background = Texture2D.blackTexture },
                normal = new GUIStyleState { background = Texture2D.blackTexture }
            };
        }

        private void OnGUI()
        {
            GUI.color = Color.white;
            GUI.skin.label.fontSize = (int)(0.02f * Screen.height);
            GUI.skin.button.fontSize = (int)(0.02f * Screen.height);
            GUI.skin.textField.fontSize = (int)(0.02f * Screen.height);
            GUI.skin.verticalScrollbar.fixedWidth = 0.04f * Screen.width;
            GUI.skin.horizontalScrollbar.fixedHeight = 0.04f * Screen.height;

            if (tab != CheatTab.Closed)
            {
                GUILayout.BeginArea(new Rect(0, 0, Screen.width, Screen.height), whiteStyle);

                DrawCheatMenuActive();

                switch (tab)
                {

                    //case CheatTab.Ftue:
                    //DrawFtue();
                    //break;

                    //case CheatTab.Levels:
                    //DrawLevels();
                    //break;
                    case CheatTab.Events:
                        DrawEvents();
                        break;

                    case CheatTab.Other:
                        DrawOther();
                        break;

                    case CheatTab.Framerate:
                        DrawFramerate();
                        break;

                    case CheatTab.Analytics:
                        DrawAnalytics();
                        break;
                    case CheatTab.Ads:
                        DrawAds();
                        break;

                    case CheatTab.Haptics:
                        DrawHaptics();
                        break;
                }

                GUILayout.EndArea();
            }
            else
            {
                DrawCheatMenuDisabled();
            }
        }

        private void DrawCheatMenuDisabled()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(30);
            if (GUILayout.Button(" [ CHEATS ] "))
            {
                ShowCheatMenu();
            }

            GUILayout.EndHorizontal();
        }

        private void DrawCheatMenuActive()
        {
            GUILayout.BeginHorizontal();

            if (GUILayout.Button(" [ HIDE CONSOLE ] "))
            {
                HideCheatMenu();
            }

            if (GUILayout.Button(" = Levels  = "))
            {
                tab = CheatTab.Levels;
            }

            //if (GUILayout.Button(" = Islands  = "))
            //{
            //    tab = CheatTab.Islands;
            //}

            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();

            if (GUILayout.Button(" = PlayerPrefs = "))
            {
                tab = CheatTab.Other;
            }
            if (GUILayout.Button(" = FTUE = "))
            {
                tab = CheatTab.Ftue;
            }

            GUILayout.EndHorizontal();

            //GUILayout.BeginHorizontal();

            //if (GUILayout.Button(" =  Analytics Info  = "))
            //{
            //    tab = CheatTab.Analytics;
            //}

            //if (GUILayout.Button(" =  Analytics Events  = "))
            //{
            //    tab = CheatTab.Events;
            //}

            //GUILayout.EndHorizontal();

            //GUILayout.BeginHorizontal();

            //if (GUILayout.Button(" = vSync and Framerate settings  = "))
            //{
            //    tab = CheatTab.Framerate;
            //}

            //GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            if (GUILayout.Button(" =  Ads Console  = "))
            {
                tab = CheatTab.Ads;
            }

            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button(" =  HAPTICS  = "))
            {
                tab = CheatTab.Haptics;
            }

            GUILayout.EndHorizontal();
        }

        private void HideCheatMenu()
        {
            tab = CheatTab.Closed;

            // unblock UI
            input.enabled = true;
        }

        private void ShowCheatMenu()
        {
            tab = CheatTab.Other;

            // block UI
            input.enabled = false;
        }



        private void DrawOther()
        {
            GUI.color = Color.black;

            GUI.color = Color.red;

            GUILayout.Label("NOTE! You must restart the app in order to apply these changes!");

            GUI.color = Color.white;

            //AdsController adsController = SceneActivationBehaviour<GameLogicActivator>.Instance.AdsController;
            //if (GUILayout.Button("SET ADS AdMob"))
            //{
            //    adsController.CreateRequesterAdMob();
            //}
            //if (GUILayout.Button("SET ADS Facebook"))
            //{
            //    adsController.CreateRequesterFacebook();
            //}
            //if (GUILayout.Button("SET ADS Inneractive"))
            //{
            //    adsController.CreateRequesterInneractive();
            //}
            //if (GUILayout.Button("SET ADS ALL"))
            //{
            //    adsController.CreateRequesterAll();
            //}

            if (GUILayout.Button("CLEAR ALL PREFS!"))
            {
                PlayerPrefs.DeleteAll();
                Debug.Log("Unity Preferences are cleared!");
            }

            //if (GUILayout.Button("Clear progress for ALL ISLANDS!"))
            //{
            //    for (int index = 0; index < settings.Islands.Length; index++)
            //    {
            //        PlayerPrefs.DeleteKey(ServerController.IslandProgressPrefsName + settings.Islands[index].IslandId);
            //    }

            //    Debug.Log("Island progress are cleared!");
            //}

            //if (GUILayout.Button("Reset FTUE"))
            //{
            //    PlayerPrefs.DeleteKey(ServerController.FtueDataPrefsName);
            //    PlayerPrefs.DeleteKey(ServerController.FtueGiftIndexPrefsName);
            //}

            //if (GUILayout.Button("Complete all FTUE"))
            //{
            //    PlayerPrefs.SetString(ServerController.FtueDataPrefsName,
            //        "True,True,True,True,True,True,True,True,True,True,True,True,True,True,True,True,True,True,True,True,True,True,True,True,True,True,True,True,True,True,True,True,True,True,True");
            //    PlayerPrefs.SetInt(ServerController.FtueGiftIndexPrefsName, 999);
            //}

            //if (GUILayout.Button("Set FTUE before game 2"))
            //{
            //    PlayerPrefs.SetString(ServerController.FtueDataPrefsName,
            //        "True,True,True,True,False,False,False,False,False,False,False,False,False,False,False,False,False,False,False,False,False,False,False,False,False,False,False,False");
            //    PlayerPrefs.SetInt(ServerController.FtueGiftIndexPrefsName, 1);
            //}

            if (GUILayout.Button("Reset Saved data Version"))
            {
                PlayerPrefs.DeleteKey(ServerController.VersionPrefsName);
            }

            if (GUILayout.Button("Reset Player"))
            {
                PlayerPrefs.DeleteKey(ServerController.PlayerDataPrefsName);
            }

            if (gameController.Player == null)
            {
                GUILayout.Label("Player data is not available now.");
            }
            else
            {
                GUI.color = Color.red;
                GUILayout.Label("------------");
                GUILayout.Label("NOTE! UI elements (text, buttons, numbers) are NOT updated!.\n" +
                                "If you need them, please restart the app!");

                GUI.color = Color.white;

                if (GUILayout.Button($"Add 1000 coins! (coins: {gameController.Player.Coins})"))
                {
                    gameController.Player.Coins += 1000;
                    serverController.PersistPlayerProgress(gameController.Player);
                }

                if (GUILayout.Button($"Reset coins to 0 (coins: {gameController.Player.Coins})"))
                {
                    gameController.Player.Coins = 0;
                    serverController.PersistPlayerProgress(gameController.Player);
                }

                //if (GUILayout.Button($"Next game number (games count: {gameController.Player.GamesPlayed})"))
                //{
                //    gameController.Player.GamesPlayed++;
                //    serverController.PersistPlayerProgress(gameController.Player);
                //}

                //if (GUILayout.Button($"Reset games played to 0 (games count: {gameController.Player.GamesPlayed})"))
                //{
                //    gameController.Player.GamesPlayed = 0;
                //    serverController.PersistPlayerProgress(gameController.Player);
                //}

                //if (GUILayout.Button(
                //    $"Toggle Hammer tool (Tool is {(gameController.Player.HasHammer ? "active" : "disabled")})"))
                //{
                //    gameController.Player.HasHammer ^= true;
                //    serverController.PersistPlayerProgress(gameController.Player);
                //}

                //if (GUILayout.Button($"Reset ads-disable time (time: {serverController.LoadAdsTemporaryDisable()})"))
                //{
                //    serverController.PersistAdsTemporaryDisable(DateTime.UtcNow - TimeSpan.FromHours(24));
                //}

                //if (GUILayout.Button(
                //    $"Toggle Ads flag (Ads are {(gameController.Player.HasRemovedAds ? "removed" : "shown")})"))
                //{
                //    gameController.Player.HasRemovedAds ^= true;
                //    serverController.PersistPlayerProgress(gameController.Player);
                //}

                //if (GUILayout.Button($"Reset stars to 0 (stars: {gameController.Player.Stars})"))
                //{
                //    gameController.Player.Stars = 0;
                //    serverController.PersistPlayerProgress(gameController.Player);
                //}

                //if (GUILayout.Button($"Add a star (stars: {gameController.Player.Stars})"))
                //{
                //    gameController.Player.Stars += 1;
                //    serverController.PersistPlayerProgress(gameController.Player);
                //}

                if (GUILayout.Button($"Reset game level to 0 (level: {gameController.Player.MainGameLevelIndex})"))
                {
                    gameController.Player.MainGameLevelIndex = 0;
                    serverController.PersistPlayerProgress(gameController.Player);
                }

                //if (GUILayout.Button($"Next game level (level: {gameController.Player.MainGameLevelIndex})"))
                //{
                //    gameController.MoveToNextLevel();
                //    serverController.PersistPlayerProgress(gameController.Player);
                //}
            }
        }

        //private void DrawFtue()
        //{
        //    GUI.color = Color.black;

        //    GUILayout.Label("FTUE:");

        //    FtueInformation data = ftueController.FtueData;
        //    if (data != null)
        //    {
        //        GUILayout.Label($"{nameof(data.NextGiftBoxIndex)} = {data.NextGiftBoxIndex}");

        //        FieldInfo[] fields = typeof(FtueInformation)
        //            .GetFields(BindingFlags.Instance | BindingFlags.Public)
        //            .Where(x => x.Name != nameof(data.NextGiftBoxIndex))
        //            .ToArray();

        //        PropertyInfo[] props = typeof(FtueInformation)
        //            .GetProperties(BindingFlags.Instance | BindingFlags.Public)
        //            .Where(x => x.Name != nameof(data.NextGiftBoxIndex))
        //            .ToArray();

        //        scrollPosition = GUILayout.BeginScrollView(scrollPosition, blackStyle);

        //        foreach (PropertyInfo prop in props)
        //        {
        //            GUILayout.BeginHorizontal();
        //            bool value = (bool)prop.GetValue(data, null);
        //            if (GUILayout.Button("Toggle", GUILayout.Width(70)))
        //            {
        //                prop.SetValue(data, value ^ true, null);
        //                serverController.PersistFtue(ftueController.FtueData);
        //            }
        //            GUILayout.Label($"[{value}] {prop.Name}");
        //            GUILayout.EndHorizontal();
        //        }

        //        foreach (FieldInfo field in fields)
        //        {
        //            GUILayout.BeginHorizontal();
        //            bool value = (bool)field.GetValue(data);
        //            if (GUILayout.Button("Toggle", GUILayout.Width(70)))
        //            {
        //                field.SetValue(data, value ^ true);
        //                serverController.PersistFtue(ftueController.FtueData);
        //            }
        //            GUILayout.Label($"[{value}] {field.Name}");
        //            GUILayout.EndHorizontal();
        //        }

        //        GUILayout.EndScrollView();
        //    }

        //}

        private void DrawFramerate()
        {
            GUI.color = Color.black;

            GUILayout.Label("Target framerate: " + Application.targetFrameRate);
            foreach (int tfps in new[] { -1, 30, 60, 120 })
            {
                if (GUILayout.Button($"Set {tfps}", GUILayout.Width(70)))
                {
                    Application.targetFrameRate = tfps;
                }
            }

            GUILayout.Label("Vertical sync.: " + QualitySettings.vSyncCount);
            foreach (int vsync in new[] { 0, 1, 2, 4 })
            {
                if (GUILayout.Button($"Set {vsync}", GUILayout.Width(70)))
                {
                    QualitySettings.vSyncCount = vsync;
                }
            }

            GUILayout.TextArea("The number of VSyncs that should pass between each frame. Use 'Don't Sync' (0) to not wait for VSync");

            GUILayout.TextArea("Additionally if the QualitySettings.vSyncCount property is set, the targetFrameRate will be ignored " +
                               "and instead the game will use the vSyncCount and the platform's default render rate to determine the " +
                               "target frame rate. For example, if the platform's default render rate is 60 frames per second and " +
                               "vSyncCount is set to 2, the game will target 30 frames per second.");
        }

        private void DrawAnalytics()
        {
            GUI.color = Color.black;

            GUILayout.Label("Application version: " + Application.version);

            GUI.color = Color.white;
            GUILayout.TextArea("Applicaition version should be manually increased BEFORE publishing. " +
                               "Otherwise, it will send incorrect events!");
            GUI.color = Color.black;

            GUILayout.Label("USER    : " + SceneActivationBehaviour<GameLogicActivator>.Instance.AnalyticsController.PlayerGuid);

            GUI.color = Color.white;
            GUILayout.TextArea("USER is a debug-only parameter used to filter out test events!");
            GUI.color = Color.black;

            GUILayout.Label("UAID    : " + Guid.Parse(AnalyticsSessionInfo.userId));
            GUILayout.Label("SESSION : " + AnalyticsSessionInfo.sessionId);

            GUI.color = Color.white;
            GUILayout.TextArea("UAID is used by Unity Analytics.");
            GUI.color = Color.black;

            GUILayout.Label("Application ID: " + Application.identifier);
            GUILayout.Label("Build with Unity version: " + Application.unityVersion);
            GUILayout.Label("Cloud ID: " + Application.cloudProjectId);
            GUILayout.Label("Build ID: " + Application.buildGUID);

            GUILayout.Space(10);
            GUILayout.Label("There're some last analytic events sent to Unity:");

            //foreach (string log in SceneActivationBehaviour<GameLogicActivator>.Instance.AnalyticsController.Logs)
            //{
            //    GUILayout.TextArea(log, blackStyle);
            //}
        }

        private void DrawEvents()
        {
            GUI.color = Color.black;

            GUILayout.Label("Analytics Events test");

            GUI.color = Color.white;
            GUILayout.TextArea("Select an event to fire it then check the info tab to see fire logs. ");

            GUI.color = Color.black;

            GUILayout.Space(10);

            GUILayout.Label("Registration Event");

            GUI.color = Color.white;

            GUILayout.Space(10);

            //if (GUILayout.Button($"Test device data"))
            //{
            //    analyticsController.SendDeviceData();
            //}

            GUI.color = Color.black;

            GUILayout.Space(10);

            GUILayout.Label("Screen Event");

            GUI.color = Color.white;

            GUILayout.Space(10);

            //if (GUILayout.Button($"Test screen change"))
            //{
            //    analyticsController.SendScreenChangeEvent(GameWindow.LoadingMenu, GameWindow.ExampleUIScene); //SendDeviceData();
            //}

            GUI.color = Color.black;

            GUILayout.Space(20);

            GUILayout.Label("Game Events");

            GUI.color = Color.white;
            GUILayout.TextArea("Game event ID is ONLY set on start event.");

            GUILayout.Space(10);

            GUI.color = Color.white;

            //if (GUILayout.Button($"Game Start"))
            //{
            //    analyticsController.SendGameData(GameSourceType.Start, GameResultType.NotSet, "1");
            //}

            //GUILayout.Space(10);

            //if (GUILayout.Button($"Game Pause"))
            //{
            //    analyticsController.SendGameData(GameSourceType.Pause, GameResultType.NotSet, "1");
            //}

            //GUILayout.Space(10);

            //if (GUILayout.Button($"Game Resume"))
            //{
            //    analyticsController.SendGameData(GameSourceType.Resume, GameResultType.NotSet, "1");
            //}

            //GUILayout.Space(10);

            //if (GUILayout.Button($"Game Quit"))
            //{
            //    analyticsController.SendGameData(GameSourceType.Quit, GameResultType.Fail, "1");
            //}

            //GUILayout.Space(10);

            //if (GUILayout.Button($"Game Finish - Fail"))
            //{
            //    analyticsController.SendGameData(GameSourceType.Finish, GameResultType.Fail, "1");
            //}

            //GUILayout.Space(10);

            //if (GUILayout.Button($"Game Finish - Success"))
            //{
            //    analyticsController.SendGameData(GameSourceType.Finish, GameResultType.Success, "1");
            //}

            //GUI.color = Color.black;

            //GUILayout.Label("Ad Events");

            //GUI.color = Color.white;


            //GUILayout.Space(20);

            //if (GUILayout.Button("Request - Banner"))
            //{
            //    analyticsController.SendAdResult(AdResultType.Requested, AdSourceType.NotSet, AdType.Banner);
            //}

            //GUILayout.Space(10);

            //if (GUILayout.Button("Started - Interstitial"))
            //{
            //    analyticsController.SendAdResult(AdResultType.Started, AdSourceType.NotSet, AdType.Interstitial);
            //}

            //GUILayout.Space(10);

            //if (GUILayout.Button("Watched - RewardedVideo"))
            //{
            //    analyticsController.SendAdResult(AdResultType.Watched, AdSourceType.NotSet, AdType.RewardedVideo);
            //}

            //GUILayout.Space(10);

            //if (GUILayout.Button("Error - Banner"))
            //{
            //    analyticsController.SendAdResult(AdResultType.Error, AdSourceType.NotSet, AdType.Banner);
            //}

            //GUILayout.Space(10);

            //if (GUILayout.Button("Aborted - Interstitial"))
            //{
            //    analyticsController.SendAdResult(AdResultType.Aborted, AdSourceType.NotSet, AdType.Interstitial);
            //}
        }

        private void DrawAds()
        {
            GUI.color = Color.black;
            GUILayout.Label("HEYZAP Ad Console");
            if (GUILayout.Button("Show Console"))
            {
                Debug.Log("Show HEYZAP Console!");
                //IntegrationAnalyzer.ShowTestSuite();
            }
        }

        private void DrawHaptics()
        {
            HapticFeedback hf = new HapticFeedback();

            GUI.color = Color.black;
            GUILayout.Label("HAPTICS");

            GUI.color = Color.white;

            GUILayout.Space(20);

            GUILayout.BeginHorizontal();

            if (GUILayout.Button("=  LIGHT  ="))
            {
                hf.LightStyle();
            }

            if (GUILayout.Button("=  MEDIUM  ="))
            {
                hf.MediumStyle();
            }

            if (GUILayout.Button("=  HEAVY  ="))
            {
                hf.HeavyStyle();
            }

            GUILayout.EndHorizontal();

            GUILayout.Space(20);

            if (GUILayout.Button("IMPACT"))
            {
                hf.ImpactOccurred();
            }

            GUILayout.Space(10);

            if (GUILayout.Button("SELECTION"))
            {
                hf.SelectionOccurred();
            }

            GUILayout.Space(10);

            if (GUILayout.Button("SUCCESS"))
            {
                hf.SelectionOccurred();
            }

            GUILayout.Space(10);

            if (GUILayout.Button("WARNING"))
            {
                hf.SelectionOccurred();
            }

            GUILayout.Space(10);

            if (GUILayout.Button("ERROR"))
            {
                hf.SelectionOccurred();
            }
        }

        #endregion

#endif
    }
}