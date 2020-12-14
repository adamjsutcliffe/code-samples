using System;
using System.Collections.Generic;
//using com.adjust.sdk;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Analytics;
//using UnityEngine.Purchasing;
//using SnowplowTracker;
//using SnowplowTracker.Storage;
//using SnowplowTracker.Events;
//using SnowplowTracker.Payloads;
//using SnowplowTracker.Payloads.Contexts;
//using SnowplowTracker.Enums;
//using SnowplowTracker.Emitters;
//using Fabric.Crashlytics;
using Peak.Speedoku.Scripts.Game;
using Peak.Speedoku.Scripts.Game.Gameplay;
#if UNITY_IOS
using UnityEngine.iOS;
#endif
namespace Peak.UnityGameFramework.Scripts.Common.AnalyticsScripts
{
    /// <summary>
    /// Sends max 10 parameters into Unity Analytics
    /// </summary>
    public sealed class AnalyticsController : MonoBehaviour
    {
        [Tooltip("Allows Analytics Unity console log")]
        [SerializeField] private bool debugLog;

        [Tooltip("Allows to send event from the Unity Editor")]
        [SerializeField] private bool shouldSendEventInEditor;

        [Header("Snowplow")]
        [SerializeField] private string endPointURI = "collector.peakcloud.org";
        // Tracker
        //public static Tracker tracker;

        public string PlayerGuid { get; set; }
        private string appVersion;
        private string language;
        private string installMode;
        private string GameId;
        private string LevelID;
        private string deviceID;
        private string deviceModel;
        private string deviceOS;
        //private AdjustEnvironment environment;

        private bool ShouldSkipSendEvent;
        private bool debugMode;

        #region Unity Analytics

        private void Awake()
        {
            //basic setup
            appVersion = $"{Application.version}";
        }

        public void RegisterUser(Player player)
        {
            PlayerGuid = player.Guid;
            DebugLog($"[ANALYTICS] Register: {PlayerGuid}");
        }

        public void SendGameStart(MainGameData gameData)
        {
            GameId = Guid.NewGuid().ToString();
            AnalyticsResult analyticsResult = Analytics.CustomEvent(Constants.Analytics.Events.GameStart, new Dictionary<string, object>
            {
                {Constants.Analytics.Parameters.User, PlayerGuid},
                {Constants.Analytics.Parameters.AppVersion, appVersion},
                {Constants.Analytics.Parameters.GameId, GameId },
                {Constants.Analytics.Parameters.GameLevel, gameData.Ruleset.Id }
            });

            DebugLog($"[ANALYTICS] [{analyticsResult}] {Constants.Analytics.Events.GameStart}: {gameData.Ruleset.Id}");
            
        }

        public void SendGameQuit(MainGameData gameData)
        {
            GameId = Guid.NewGuid().ToString();
            AnalyticsResult analyticsResult = Analytics.CustomEvent(Constants.Analytics.Events.GameQuit, new Dictionary<string, object>
            {
                {Constants.Analytics.Parameters.User, PlayerGuid},
                {Constants.Analytics.Parameters.AppVersion, appVersion},
                {Constants.Analytics.Parameters.GameId, GameId },
                {Constants.Analytics.Parameters.GameLevel, gameData.Ruleset.Id },
                {Constants.Analytics.Parameters.GameTime, gameData.SecondsUsed}
            });

            DebugLog($"[ANALYTICS] [{analyticsResult}] {Constants.Analytics.Events.GameQuit}: {gameData.Ruleset.Id}");
        }

        public void SendGameFinished(MainGameData gameData)
        {
            GameId = Guid.NewGuid().ToString();
            List<string> answerData = new List<string>();
            for (int i = 0; i < gameData.answers.Length; i++)
            {
                GridAnswer answer = gameData.answers[i];
                answerData.Add($"{answer.index}:{answer.number}={answer.correct}");
            }
            AnalyticsResult analyticsResult = Analytics.CustomEvent(Constants.Analytics.Events.GameFinish, new Dictionary<string, object>
            {
                {Constants.Analytics.Parameters.User, PlayerGuid},
                {Constants.Analytics.Parameters.AppVersion, appVersion},
                {Constants.Analytics.Parameters.GameId, GameId },
                {Constants.Analytics.Parameters.GameLevel, gameData.Ruleset.Id },
                {Constants.Analytics.Parameters.GameTime, gameData.SecondsUsed},
                {Constants.Analytics.Parameters.GameData, string.Join(",",answerData)}
            });

            DebugLog($"[ANALYTICS] [{analyticsResult}] {Constants.Analytics.Events.GameFinish}: {gameData.Ruleset.Id}");
            //AdjustEvent adjustEvent = new AdjustEvent(Constants.Analytics.AdjustEvents.GamePlayed);
            //Adjust.trackEvent(adjustEvent);
        }

        public void TutorialFinished() //SendFtueProgress(string ftueStep)
        {
            //AdjustEvent adjustEvent = new AdjustEvent(Constants.Analytics.AdjustEvents.TutorialComplete);
            //Adjust.trackEvent(adjustEvent);
        }

        #endregion

        #region Snowplow Analytics init
        /*
        private void Awake()
        {

#if CHEATS
            debugMode = true;
#else
            debugMode = false;
#endif
            ShouldSkipSendEvent = Application.isEditor && !shouldSendEventInEditor;
            appVersion = $"{Application.version}";
            language = Application.systemLanguage.ToString();
            installMode = Application.installMode.ToString();
#if UNITY_IOS
            deviceID = UnityEngine.iOS.Device.generation.ToString();
#else
            deviceID = SystemInfo.deviceModel;
#endif
            deviceModel = SystemInfo.deviceModel;
            deviceOS = SystemInfo.operatingSystem;
        }

        // User Info
        public void SetAdjustEnviroment(AdjustEnvironment environment)
        {
            this.environment = environment;
        }

        public void RegisterUser(Player player)
        {
            PlayerGuid = ""; // player.Guid;

            HttpProtocol httpProtocol = HttpProtocol.HTTPS;
            if (debugMode)
            {
                httpProtocol = HttpProtocol.HTTP;
                endPointURI = "54.172.83.105";
            }

            IEmitter emitter = new AsyncEmitter(endPointURI, httpProtocol, HttpMethod.POST, 500, 52000, 52000);
#if UNITY_IOS
            string appID = "speedoku_ios";
#else
            string appID = "speedoku_android";
#endif

            Debug.Log($"[ANALYTICS][SETUP] appID: '{appID}' URI: '{endPointURI}' protocol: '{httpProtocol}'");
            tracker = new Tracker(emitter, "peak", appID, GetSubject(), GetSession(), DevicePlatforms.Mobile);
            tracker.StartEventTracking();
            DebugLog($"[ANALYTICS] SetUserId={PlayerGuid} should skip send events: {ShouldSkipSendEvent}");
        }

        private Session GetSession()
        {
            return new Session("snowplow_session.dict", 1800, 1800, 30);
        }

        private Subject GetSubject()
        {
            Subject subject = new Subject();
            subject.SetUserId(PlayerGuid);
            subject.SetLanguage(language);
            subject.SetTimezone(TimeZoneInfo.Local.DisplayName);
            subject.SetScreenResolution(Screen.width, Screen.height);
            DebugLog($"[SP] SUBJECT: {subject.GetPayload()}");
            return subject;
        }

        private IContext UserContext()
        {
            string deploymentType = environment == AdjustEnvironment.Production ? "release" : "dev";
            string version = $"{deploymentType}_{appVersion}";

            //QLContextUser userContext = new QLContextUser(PlayerGuid, SHRDeviceAudioPortType.SHRDeviceAudioPortTypeOther, SystemInfo.batteryLevel, version, deviceID, deviceOS, installMode);
            IContext context = new GenericContext()
                //.SetSchema(GetSchemaForEvent(userContext))
                //.AddDict(userContext.snowplowProperties())
                .Build();
            return context;
        }

        private IContext MobileContext()
        {
#if UNITY_IOS
            string idfa = Adjust.getIdfa() ?? "unknown_idfa";
#else
            string idfa = "unknown_idfa";
            Adjust.getGoogleAdId((string googleAdId) =>
            {
                idfa = googleAdId;
            });
#endif
            string[] versionInfo = deviceOS.Split(' ');
            string versionNumber = versionInfo.Last();
            DebugLog($"Mobile context idfa: {idfa} -> version: {versionNumber}");
            MobileContext context = new MobileContext()
                .SetOsType(deviceOS)                    //REQUIRED!
                .SetOsVersion(versionNumber)            //REQUIRED!
                .SetDeviceManufacturer(deviceModel)     //REQUIRED!
                .SetDeviceModel(deviceModel)            //REQUIRED!
                .SetNetworkType(NetworkType.Mobile)
#if UNITY_IOS
            .SetAppleIdfa(idfa)
            .SetAppleIdfv(Device.vendorIdentifier ?? "Editor")
#else
            .SetAndroidIdfa(idfa)
#endif
                .Build();
            DebugLog($"MOBILE CONTEXT ({deviceOS}) : {context.GetJson()}");
            return context;
        }

        private void TrackEvent(iEventInterface snowPlowEvent)
        {
            SelfDescribingJson eventData = new SelfDescribingJson(GetSchemaForEvent(snowPlowEvent), snowPlowEvent.snowplowProperties());
            Unstructured unstructured = new Unstructured();
            unstructured.SetCustomContext(new List<IContext> { UserContext(),
                                                               MobileContext() });
            unstructured.SetEventData(eventData);
            unstructured.Build();
            tracker.Track(unstructured);
            DebugLog($"[ANALYTICS] Track event: {snowPlowEvent.name()} -> {snowPlowEvent.debugDescription()}");
            DebugLog($"[ANALYTICS] Payload: {unstructured.GetPayload().ToString()}");

            Crashlytics.Log($"Snowplow event: {snowPlowEvent.debugDescription()}");
        }

        private string GetSchemaForEvent(iEventInterface snowPlowEvent)
        {
            return string.Format("iglu:net.peak/{0}/jsonschema/{1}", snowPlowEvent.snowplowName(), snowPlowEvent.version());
        }
        */
        #endregion

        #region Snowplow API
        /*
        // ftue progress

        public void SendFtueProgress(string ftueLevelName)
        {
            if (ShouldSkipSendEvent)
            {
                DebugLog($"[ANALYTICS] [FTUE] [Skip] {Constants.Analytics.Events.FtueProgress}: {ftueLevelName}");
                return;
            }

            //QLFtueProgress ftueProgress = new QLFtueProgress(ftueLevelName);
            //TrackEvent(ftueProgress);

            DebugLog($"[ANALYTICS] [FTUE] {Constants.Analytics.Events.FtueProgress}: {ftueLevelName}");

            return;
        }

        // ftue complete

        public void SendFtueComplete()
        {
            if (ShouldSkipSendEvent)
            {
                //DebugLog($"[ANALYTICS] [ADJUST] {Constants.Analytics.AdjustEvents.TutorialComplete}");
                return;
            }

            DebugLog("[ADJUST] New install ");
            //AdjustEvent adjustEvent = new AdjustEvent(Constants.Analytics.AdjustEvents.TutorialComplete);
            //Adjust.trackEvent(adjustEvent);
        }

        //Game Analytics
        // Start = 0, Quit = 1, Finish = 2, Pause = 3, Resume = 4, Restart = 5

        //private QLGameSourceType MapGameSource(GameSourceType source)
        //{
        //    switch (source)
        //    {
        //        case GameSourceType.Start:
        //            return QLGameSourceType.QLGameSourceTypeStart;
        //        case GameSourceType.Finish:
        //            return QLGameSourceType.QLGameSourceTypeFinish;
        //        case GameSourceType.Pause:
        //            return QLGameSourceType.QLGameSourceTypePause;
        //        case GameSourceType.Quit:
        //            return QLGameSourceType.QLGameSourceTypeQuit;
        //        case GameSourceType.Restart:
        //            return QLGameSourceType.QLGameSourceTypeRestart;
        //        case GameSourceType.Resume:
        //            return QLGameSourceType.QLGameSourceTypeResume;
        //        default:
        //            //TODO add asset here? -> SHouldn't reach here
        //            return QLGameSourceType.QLGameSourceTypeQuit;
        //    }
        //}

        //private QLGameType MapGameType(GameType type)
        //{
        //    switch (type)
        //    {
        //        case GameType.Normal:
        //            return QLGameType.QLGameTypeNormal;
        //        case GameType.Gold:
        //            return QLGameType.QLGameTypeGold;
        //        case GameType.ReplayNormal:
        //            return QLGameType.QLGameTypeReplayNormal;
        //        case GameType.ReplayGold:
        //            return QLGameType.QLGameTypeReplayGold;
        //        default:
        //            //TODO add asset here? -> SHouldn't reach here
        //            return QLGameType.QLGameTypeNormal;
        //    }
        //}

        //public void SendGameData(GameSourceType gameSource, MainGameData gameData)
        //{
        //    LevelID = gameData.Ruleset.Id + "_" + gameData.Ruleset.PuzzleNameKey; //.PuzzleName;

        //    if (gameSource == GameSourceType.Start)
        //    {
        //        GameId = Guid.NewGuid().ToString();

        //        AdjustEvent adjustEvent = new AdjustEvent(Constants.Analytics.AdjustEvents.GamePlayed);
        //        Adjust.trackEvent(adjustEvent);
        //    }

        //    int timeTakenToSolve = 0;
        //    int starsEarned = 0;
        //    if (gameSource == GameSourceType.Finish)
        //    {
        //        timeTakenToSolve = gameData.TimeLimit - gameData.SecondsLeft;
        //        starsEarned = gameData.StarScore;
        //    }

        //    if (ShouldSkipSendEvent)
        //    {
        //        DebugLog($"[ANALYTICS] [Skip] {Constants.Analytics.Events.GameEvent}: {MapGameSource(gameSource).ToString()} " +
        //            $"{Constants.Analytics.Parameters.GameType}: {MapGameType(gameData.GameType).ToString()} " +
        //            $"{Constants.Analytics.Parameters.GameId}: {GameId} " +
        //                 $"{Constants.Analytics.Parameters.GameLevel}: {LevelID}" +
        //                 $"{Constants.Analytics.Parameters.TimeTakenToSolve}: {timeTakenToSolve}" +
        //                 $"{Constants.Analytics.Parameters.StarsEarned}: {starsEarned}" +
        //                 $"{Constants.Analytics.Parameters.RemainingFilm}: {gameData.PlayerFilmRemaining}");
        //        return;
        //    }

        //    QLGameEvent gameEvent = new QLGameEvent(MapGameSource(gameSource), MapGameType(gameData.GameType), GameId, LevelID, timeTakenToSolve, starsEarned, gameData.PlayerFilmRemaining);
        //    TrackEvent(gameEvent);
        //}

        // Coins earned

        //private QLCoinSourceType MapCoinSource(CoinSourceType source)
        //{
        //    switch (source)
        //    {
        //        case CoinSourceType.NewLocationUnlocked:
        //            return QLCoinSourceType.QLCoinSourceTypeNewLocationUnlocked;
        //        case CoinSourceType.PuzzleSolved:
        //            return QLCoinSourceType.QLCoinSourceTypePuzzleSolved;
        //        case CoinSourceType.VideoWatched:
        //            return QLCoinSourceType.QLCoinSourceTypeVideoWatched;
        //        case CoinSourceType.GoldCollectionComplete:
        //            return QLCoinSourceType.QLCoinSourceTypeGoldCollectionComplete;
        //        case CoinSourceType.Store:
        //            return QLCoinSourceType.QLCoinSourceTypeStore;
        //        case CoinSourceType.PostGameVideo:
        //            return QLCoinSourceType.QLCoinSourceTypePostGameVideo;
        //        default:
        //            //TODO add asset here? -> SHouldn't reach here
        //            return QLCoinSourceType.QLCoinSourceTypePuzzleSolved;
        //    }
        //}
        //public void CoinsEarned(int coins, int delta, CoinSourceType sourceType)
        //{
        //    if (ShouldSkipSendEvent)
        //    {
        //        DebugLog($"[ANALYTICS] [Skip] {Constants.Analytics.Events.CoinsEarn}: {MapCoinSource(sourceType).ToString()} " +
        //                             $"{Constants.Analytics.Parameters.CoinsAwarded}: {delta} " +
        //                             $"{Constants.Analytics.Parameters.PlayerCoins}: {coins} " +
        //                             $"{Constants.Analytics.Parameters.GameId}: {GameId} " +
        //                             $"{Constants.Analytics.Parameters.GameLevel}: {LevelID}");
        //        return;
        //    }

        //    QLCoinsEarned coinsEarned = new QLCoinsEarned(MapCoinSource(sourceType), delta, coins, GameId, LevelID);
        //    TrackEvent(coinsEarned);

        //    if (delta < 0) //coins spent
        //    {
        //        AdjustEvent adjustEvent = new AdjustEvent(Constants.Analytics.AdjustEvents.CoinSpent);
        //        adjustEvent.addPartnerParameter("quantity", $"{delta * -1}");
        //        Adjust.trackEvent(adjustEvent);
        //        DebugLog($"[ADJUST] coins spent : {delta * -1}");
        //    }
        //    DebugLog($"[ANALYTICS] coins spent : {delta}");
        //    DebugLog($"[ANALYTICS]][COINS] {Constants.Analytics.Events.CoinsEarn}: {MapCoinSource(sourceType).ToString()} " +
        //                             $"{Constants.Analytics.Parameters.CoinsAwarded}: {delta} " +
        //                             $"{Constants.Analytics.Parameters.PlayerCoins}: {coins} " +
        //                             $"{Constants.Analytics.Parameters.GameId}: {GameId} " +
        //                             $"{Constants.Analytics.Parameters.GameLevel}: {LevelID}");
        //}

        // film event analytics

        //private QLFilmSourceType MapFilmEvent(FilmSourceType source)
        //{
        //    switch (source)
        //    {
        //        case FilmSourceType.RewardedVideo:
        //            return QLFilmSourceType.QLFilmSourceTypeRewardedVideo;
        //        case FilmSourceType.CoinPurchase:
        //            return QLFilmSourceType.QLFilmSourceTypeCoinPurchase;
        //        case FilmSourceType.DailyReward:
        //            return QLFilmSourceType.QLFilmSourceTypeDailyReward;
        //        default:
        //            //TODO add asset here? -> SHouldn't reach here
        //            return QLFilmSourceType.QLFilmSourceTypeRewardedVideo;
        //    }
        //}

        //public void FilmEarned(int playerFilm, FilmSourceType source)
        //{
        //    if (ShouldSkipSendEvent)
        //    {
        //        DebugLog($"[ANALYTICS] [Skip] {Constants.Analytics.Events.FilmEvent}: {MapFilmEvent(source).ToString()} " +
        //                             $"{Constants.Analytics.Parameters.PlayerFilm}: {playerFilm} ");
        //        return;
        //    }

        //    QLFilmEvent filmEvent = new QLFilmEvent(MapFilmEvent(source), playerFilm);
        //    TrackEvent(filmEvent);

        //    DebugLog($"[ANALYTICS]][FILM] {Constants.Analytics.Events.FilmEvent}: {MapFilmEvent(source).ToString()} " +
        //                             $"{Constants.Analytics.Parameters.PlayerFilm}: {playerFilm} ");
        //}

        // Hint used

        //private QLHintType MapHintType(HintType hintType)
        //{
        //    switch (hintType)
        //    {
        //        case HintType.FreeHint:
        //            return QLHintType.QLHintTypeFreeHint;
        //        case HintType.PaidHint:
        //            return QLHintType.QLHintTypePaidHint;
        //        default:
        //            //TODO add asset here? -> SHouldn't reach here
        //            return QLHintType.QLHintTypeFreeHint;
        //    }
        //}

        //public void HintUsed(HintType hintType)
        //{
        //    if (ShouldSkipSendEvent)
        //    {
        //        DebugLog($"[ANALYTICS] [Skip] {Constants.Analytics.Events.HintUse}: {MapHintType(hintType).ToString()} " +
        //                             $"{Constants.Analytics.Parameters.GameId}: {GameId} " +
        //                             $"{Constants.Analytics.Parameters.GameLevel}: {LevelID}");
        //        return;
        //    }

        //    QLHintUsed hintUsed = new QLHintUsed(MapHintType(hintType), GameId, LevelID);
        //    TrackEvent(hintUsed);

        //    DebugLog($"[ANALYTICS]][HINT] {Constants.Analytics.Events.HintUse}: {MapHintType(hintType).ToString()} " +
        //                         $"{Constants.Analytics.Parameters.GameId}: {GameId} " +
        //                         $"{Constants.Analytics.Parameters.GameLevel}: {LevelID}");
        //}


        // New location unlocked

        //public void NewLocationUnlocked(Player player)
        //{
        //    if (ShouldSkipSendEvent)
        //    {
        //        DebugLog($"[ANALYTICS] [Skip] {Constants.Analytics.Events.NewLocationUnlocked}" +
        //                                    $"{Constants.Analytics.Parameters.NewLocationIndex}: {player.GroupIndex} " +
        //                                    $"{Constants.Analytics.Parameters.GameId}: {GameId} " +
        //                                    $"{Constants.Analytics.Parameters.GameLevel}: {LevelID}");
        //        return;
        //    }

        //    QLNewLocationUnlocked newLocationUnlocked = new QLNewLocationUnlocked(player.GroupIndex, GameId, LevelID);
        //    TrackEvent(newLocationUnlocked);

        //    DebugLog($"[ANALYTICS]][NEW LOCATION] {Constants.Analytics.Events.NewLocationUnlocked}" +
        //                                    $"{Constants.Analytics.Parameters.NewLocationIndex}: {player.GroupIndex} " +
        //                                    $"{Constants.Analytics.Parameters.GameId}: {GameId} " +
        //                                    $"{Constants.Analytics.Parameters.GameLevel}: {LevelID}");
        //}

        //public void SendDeviceData()
        //{
        //    if (ShouldSkipSendEvent)
        //    {
        //        DebugLog($"[ANALYTICS] [Skip] {Constants.Analytics.Events.DeviceInfo}: " +
        //              $"{SystemInfo.deviceModel}, {SystemInfo.operatingSystem}");
        //        //DebugLog($"[ANALYTICS] [ADJUST] {Constants.Analytics.AdjustEvents.NewInstall}");
        //        return;
        //    }

        //    //DebugLog("[ADJUST] New install ");
        //    //AdjustEvent adjustEvent = new AdjustEvent(Constants.Analytics.AdjustEvents.NewInstall);
        //    //Adjust.trackEvent(adjustEvent);

        //    QLNewUser newUserEvent = new QLNewUser();
        //    TrackEvent(newUserEvent);
        //}

        //public void SendABTestUserID(string GUID, string ABtest_ID)
        //{
        //    // TODO: send snowplow analytic



        //    DebugLog($"[TEST] Send AB test key ({ABtest_ID}) event with user ID ({GUID})");
        //    return;
        //}

        // Store open

        //public void StoreOpenEvent()
        //{
        //    QLStoreOpenSource storeOpenSource = MapStoreOpenSource(SceneActivationBehaviour<StoreUIActivator>.Instance.StoreOpenSource);

        //    if (ShouldSkipSendEvent)
        //    {
        //        DebugLog($"[ANALYTICS] [Skip] {Constants.Analytics.Events.StoreOpen}: {storeOpenSource.ToString()}");
        //        return;
        //    }

        //    QLStoreOpen storeOpen = new QLStoreOpen(storeOpenSource);
        //    TrackEvent(storeOpen);

        //    DebugLog($"[ANALYTICS] [STORE OPEN] {Constants.Analytics.Events.StoreOpen}: {storeOpenSource.ToString()}");
        //}

        //private QLStoreOpenSource MapStoreOpenSource(QLPurchaseSource purchaseSource)
        //{
        //    switch (purchaseSource)
        //    {
        //        case QLPurchaseSource.QLPurchaseSourceMenu:
        //            return QLStoreOpenSource.QLStoreOpenSourceMenu;
        //        case QLPurchaseSource.QLPurchaseSourceHint:
        //            return QLStoreOpenSource.QLStoreOpenSourceHint;
        //        case QLPurchaseSource.QLPurchaseSourceGold:
        //            return QLStoreOpenSource.QLStoreOpenSourceGold;
        //        case QLPurchaseSource.QLPurchaseSourceReplay:
        //            return QLStoreOpenSource.QLStoreOpenSourceReplay;
        //        case QLPurchaseSource.QLPurchaseSourceCollection:
        //            return QLStoreOpenSource.QLStoreOpenSourceCollection;
        //        case QLPurchaseSource.QLPurchaseSourceFilm:
        //            return QLStoreOpenSource.QLStoreOpenSourceFilm;
        //        case QLPurchaseSource.QLPurchaseSourceInGame:
        //            return QLStoreOpenSource.QLStoreOpenSourceInGame;
        //        default:
        //            //TODO add asset here? -> SHouldn't reach here
        //            return QLStoreOpenSource.QLStoreOpenSourceMenu;
        //    }
        //}

        // Store PURCHASE

        //public void SendPurchaseEvent(QLPurchaseState state, string productId, float price, string currency, int reward, QLPurchaseSource source, string failReason = null)
        //{
        //    if (state == QLPurchaseState.QLPurchaseStateSucceded)
        //    {
        //        AdjustEvent adjustEvent = new AdjustEvent(Constants.Analytics.AdjustEvents.Purchase);
        //        adjustEvent.setRevenue(Convert.ToDouble(price), currency);
        //        Adjust.trackEvent(adjustEvent);
        //    }

        //    QLPurchase purchaseEvent = new QLPurchase(state, productId, price, currency, reward, source, failReason);
        //    TrackEvent(purchaseEvent);
        //}

        //public void SendNotificationAcceptedEvent(bool accepted)
        //{
        //    QLNotification notificationEvent = new QLNotification(accepted ? "YES" : "NO");
        //    TrackEvent(notificationEvent);
        //}

        //public void SendScreenChangeEvent(GameWindow from, GameWindow to)
        //{
        //    //AnalyticsEvent
        //    if (ShouldSkipSendEvent)
        //    {
        //        DebugLog($"[ANALYTICS] [Skip] {Constants.Analytics.Events.ScreenChange}: " +
        //                 $"From: {from.ToString()} To: {to.ToString()}");
        //        return;
        //    }

        //    AnalyticsResult analyticsResult = AnalyticsEvent.ScreenVisit(to.ToString(), new Dictionary<string, object>
        //    {
        //        {Constants.Analytics.Parameters.ScreenFrom, from},
        //        {Constants.Analytics.Parameters.ScreenTo, to}
        //    });
        //    DebugLog($"[ANALYTICS] [{analyticsResult}] {Constants.Analytics.Events.ScreenChange}: " +
        //                 $"From: {from.ToString()} To: {to.ToString()}");

        //    return;
        //}

        // ad analytics

        //private QLAdSourceType MapAdSource(AdSourceType source)
        //{
        //    switch (source)
        //    {
        //        case AdSourceType.NotSet:
        //            return QLAdSourceType.QLAdSourceTypeNotSet;
        //        case AdSourceType.FilmPopUp:
        //            return QLAdSourceType.QLAdSourceTypeFilmPopUp;
        //        case AdSourceType.GoldCardPopUp:
        //            return QLAdSourceType.QLAdSourceTypeGoldCardPopUp;
        //        case AdSourceType.InGame:
        //            return QLAdSourceType.QLAdSourceTypeInGame;
        //        case AdSourceType.ReplayPopUp:
        //            return QLAdSourceType.QLAdSourceTypeReplayPopUp;
        //        case AdSourceType.PostGame:
        //            return QLAdSourceType.QLAdSourceTypePostGame;
        //        case AdSourceType.AndroidGold:
        //            return QLAdSourceType.QLAdSourceTypeAndroidGold;
        //        default:
        //            //TODO add asset here? -> SHouldn't reach here
        //            return QLAdSourceType.QLAdSourceTypeNotSet;
        //    }
        //}

        //private QLAdType MapAdType(AdType type)
        //{
        //    switch (type)
        //    {
        //        case AdType.Banner:
        //            return QLAdType.QLAdTypeBanner;
        //        case AdType.Interstitial:
        //            return QLAdType.QLAdTypeInterstitial;
        //        case AdType.RewardedVideo:
        //            return QLAdType.QLAdTypeRewarded;
        //        default:
        //            //TODO add asset here? -> SHouldn't reach here
        //            return QLAdType.QLAdTypeNotSet;
        //    }
        //}

        //private QLAdResultType MapAdResult(AdResultType result)
        //{
        //    switch (result)
        //    {
        //        case AdResultType.Started:
        //            return QLAdResultType.QLAdResultTypeStarted;
        //        case AdResultType.Requested:
        //            return QLAdResultType.QLAdResultTypeRequested;
        //        case AdResultType.Error:
        //            return QLAdResultType.QLAdResultTypeError;
        //        case AdResultType.Watched:
        //            return QLAdResultType.QLAdResultTypeWatched;
        //        case AdResultType.Aborted:
        //            return QLAdResultType.QLAdResultTypeAborted;
        //        default:
        //            //TODO add asset here? -> SHouldn't reach here
        //            return QLAdResultType.QLAdResultTypeStarted;
        //    }
        //}

        //public void SendAdResult(AdResultType result, AdType type, AdSourceType adSource)
        //{
        //    if (ShouldSkipSendEvent)
        //    {
        //        DebugLog($"[ANALYTICS] [Skip] AD EVENT: {MapAdResult(result).ToString()} {MapAdSource(adSource).ToString()}, {MapAdType(type).ToString()}");
        //        return;
        //    }

        //    QLAdEvent adEvent = new QLAdEvent(MapAdResult(result), MapAdType(type), MapAdSource(adSource));
        //    TrackEvent(adEvent);

        //    if (result == AdResultType.Watched)
        //    {
        //        AdjustEvent adjustEvent = new AdjustEvent(Constants.Analytics.AdjustEvents.AdImpression);
        //        adjustEvent.addPartnerParameter("type", $"{type.ToString()}");
        //        adjustEvent.addPartnerParameter("eCPM", "0.0");
        //        Adjust.trackEvent(adjustEvent);
        //        DebugLog($"[ADJUST] AD WATCHED - {adjustEvent.ToString()} - {type.ToString()}");
        //    }
        //}

        //public void SendAdClicked(AdType type)
        //{
        //    AdjustEvent adjustEvent = new AdjustEvent(Constants.Analytics.AdjustEvents.AdClick);
        //    adjustEvent.addPartnerParameter("type", $"{type.ToString()}");
        //    Adjust.trackEvent(adjustEvent);
        //}
        */
        #endregion

        private void DebugLog(string message)
        {
            if (!debugLog)
            {
                return;
            }

            Debug.Log(message);
        }
    }
}
