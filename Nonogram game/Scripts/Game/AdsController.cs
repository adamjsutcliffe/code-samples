using System;
using System.Collections;
using Peak.QuixelLogic.Scripts.Common.AnalyticsScripts;
using Peak.QuixelLogic.Scripts.ScenesLogic;
using UnityEngine;
using GoogleMobileAds.Api;
using Peak.QuixelLogic.Scripts.Common;
using GoogleMobileAds.Api.Mediation.AppLovin;
using GoogleMobileAds.Api.Mediation.Tapjoy;
using GoogleMobileAds.Api.Mediation.Vungle;
using GoogleMobileAds.Api.Mediation.IronSource;
using Fabric.Crashlytics;

namespace Peak.QuixelLogic.Scripts.Game
{
    public sealed class AdsController : MonoBehaviour
    {
        private AnalyticsController analytics;

        
        private BannerView bannerView;
        private InterstitialAd interstitial;
        private RewardBasedVideoAd rewardBasedVideo;

        private Action interstitialStartedCallback;
        private Action interstitialFinishedCallback;
        private Action interstitialErrorCallback;

        private Action rewardBasedVideoStartedCallback;
        private Action rewardBasedVideoFinishedCallback;
        private Action rewardBasedVideoErrorCallback;
        private Action rewardBasedVideoCancelledCallback;

        private const string AdMobQuixelIDiOS = "ca-app-pub-1582776119070207~9838890482";
        private const string AdMobQuixelIDAndroid = "ca-app-pub-1582776119070207~5242745012";

        //private const string AppTestID = "ca-app-pub-3940256099942544~1458002511";
        private const string BannerTestID = "ca-app-pub-3940256099942544/2934735716";
        //private const string InterstitialTestID = "ca-app-pub-3940256099942544/4411468910";
        //private const string RewardedVideoTestID = "ca-app-pub-3940256099942544/1712485313";

        private const string QuixelInterstitialIDiOS = "ca-app-pub-1582776119070207/9761472976";
        private const string QuixelRewardedVideoIDiOS = "ca-app-pub-1582776119070207/4872232081";

        private const string QuixelInterstitialIDAndroid = "ca-app-pub-1582776119070207/3917699838";
        private const string QuixelRewardedVideoIDAndroid = "ca-app-pub-1582776119070207/1838331402";

        //private const string NathaniPhoneTestDeviceID = "382308e0a674708a0b74c6f8c8762fb5";
        [SerializeField] private float AdFailRetryTime = 30.0f;

        public DateTime lastInterstitialShownTime;

        private Coroutine refetchInterstitialCoroutine;
        private Coroutine refetchRewardedVideoCoroutine;

        [SerializeField] private ConnectionUtil interstitialUtilities;
        [SerializeField] private ConnectionUtil rewardedUtilities;

        public void Initialize()
        {
            lastInterstitialShownTime = DateTime.Now;
            Debug.Log($"[ADS] Last interstitial shown time -> {DateTime.Now.ToString()}");
            InitAdNetwork();
        }

        public void InitAdNetwork()
        {
            Debug.Log("[ADS] [ADMOB] INIT AD CONTROLLER");

#if UNITY_ANDROID
            string appId = AdMobQuixelIDAndroid;
#elif UNITY_IPHONE
            string appId = AdMobQuixelIDiOS;
#else
            string appId = "unexpected_platform";
#endif

            MobileAds.SetiOSAppPauseOnBackground(true);

            // Initialize the Google Mobile Ads SDK.
            MobileAds.Initialize(appId);

            // TODO: show banner on start up?
            //RequestBanner();

            // Get singleton reward based video ad reference.
            this.rewardBasedVideo = RewardBasedVideoAd.Instance;

            // RewardBasedVideoAd is a singleton, so handlers should only be registered once.
            this.rewardBasedVideo.OnAdLoaded += this.HandleRewardBasedVideoLoaded;
            this.rewardBasedVideo.OnAdFailedToLoad += this.HandleRewardBasedVideoFailedToLoad;
            this.rewardBasedVideo.OnAdOpening += this.HandleRewardBasedVideoOpened;
            this.rewardBasedVideo.OnAdStarted += this.HandleRewardBasedVideoStarted;
            this.rewardBasedVideo.OnAdRewarded += this.HandleRewardBasedVideoRewarded;
            this.rewardBasedVideo.OnAdClosed += this.HandleRewardBasedVideoClosed;
            this.rewardBasedVideo.OnAdLeavingApplication += this.HandleRewardBasedVideoLeftApplication;

            Fetch(AdType.RewardedVideo);
            Fetch(AdType.Interstitial);
        }

        // Returns an ad request with custom ad targeting.
        private AdRequest CreateAdRequest()
        {
            return new AdRequest.Builder()
                //.AddTestDevice(NathaniPhoneTestDeviceID)
                .Build();
        }

        public void Fetch(AdType adType)
        {
            switch (adType)
            {
                case AdType.Interstitial:

                    if (!SceneActivationBehaviour<GameLogicActivator>.Instance.GameController.Player.HasRemovedAds)
                    {
                        Debug.Log("[ADS] [ADMOB] Fetch - ad type: " + adType.ToString());
                        Crashlytics.Log($"[ADMOB] Fetch interstitial");
                        interstitialUtilities.CheckInternet(AdFailRetryTime, RequestInterstitial, RefetchInterstitial);
                    }
                    break;

                case AdType.RewardedVideo:

                    Debug.Log("[ADS] [ADMOB] Fetch - ad type: " + adType.ToString());
                    Crashlytics.Log($"[ADMOB] Fetch rewarded video");
                    rewardedUtilities.CheckInternet(AdFailRetryTime, RequestRewardBasedVideo, RefetchRewardedVideo);
                    break;
            }
        }

        public void GDPRAcceptance()
        {
            //#if !UNITY_EDITOR
            AppLovin.SetHasUserConsent(true);
            Tapjoy.SetUserConsent("1");
            //UnityAds.SetGDPRConsentMetaData(true);
            Vungle.UpdateConsentStatus(VungleConsent.ACCEPTED);
            IronSource.SetConsent(true);
            //#endif
        }

        #region Interstitial


        public bool IsInterstitialTooSoon()
        {
            DateTime now = DateTime.Now;
            TimeSpan diff = now.Subtract(lastInterstitialShownTime);
            int seconds = diff.Seconds + (diff.Minutes * 60);
            return seconds < Constants.Ads.InterstitialTimeRule;
        }

        private void RequestInterstitial()
        {
            Debug.Log("[ADS] [ADMOB] request interstitial");
            Crashlytics.Log($"[ADMOB] Request interstitial");

            // These ad units are configured to always serve test ads.
#if UNITY_EDITOR
            string adUnitId = "unused";
#elif UNITY_ANDROID
        string adUnitId = QuixelInterstitialIDAndroid;
#elif UNITY_IPHONE
        string adUnitId = QuixelInterstitialIDiOS;
#else
            string adUnitId = "unexpected_platform";
#endif

            // Clean up interstitial ad before creating a new one.
            if (this.interstitial != null)
            {
                DestroyInterstitial();
            }

            // Create an interstitial.
            this.interstitial = new InterstitialAd(adUnitId);

            // Register for ad events.
            this.interstitial.OnAdLoaded += this.HandleInterstitialLoaded; // loaded - nothing
            this.interstitial.OnAdFailedToLoad += this.HandleInterstitialFailedToLoad; // failed to load - TODO: fetch again?
            this.interstitial.OnAdOpening += this.HandleInterstitialOpened; // opened
            this.interstitial.OnAdClosed += this.HandleInterstitialClosed; // closed
            this.interstitial.OnAdLeavingApplication += this.HandleInterstitialLeftApplication; // leaving app

            // Load an interstitial ad.
            this.interstitial.LoadAd(this.CreateAdRequest());
        }

        private void DestroyInterstitial()
        {
            Debug.Log("[ADS] [ADMOB] destroy current interstitial");
            Crashlytics.Log($"[ADMOB] Destroy current interstitial");

            this.interstitial.Destroy();
            interstitial = null;
        }

        public void TryShowInterstitial(Action started = null, Action finished = null, Action error = null)
        {
            Debug.Log("[ADS] [ADMOB] try show interstitial");
            Crashlytics.Log($"[ADMOB] Try show interstitial");

            interstitialStartedCallback = started;
            interstitialFinishedCallback = finished;
            interstitialErrorCallback = error;
#if !UNITY_EDITOR
            ShowInterstitial();
#elif UNITY_EDITOR
            InvokeAndLog(interstitialFinishedCallback, nameof(interstitialFinishedCallback));
            CleanUpInterstitialActions();
#endif
        }

        private void ShowInterstitial()
        {
            if (this.interstitial != null && this.interstitial.IsLoaded())
            {
                this.interstitial.Show();
            }
            else
            {
                Debug.Log("[ADS] [ADMOB] Interstitial is not ready yet");
                Crashlytics.Log($"[ADMOB] Interstitial is not ready yet");

                InvokeAndLog(interstitialErrorCallback, nameof(interstitialErrorCallback));
                CleanUpInterstitialActions(); // ?
            }
        }

        public void HandleInterstitialLoaded(object sender, EventArgs args)
        {
            Debug.Log("[ADS] [ADMOB] HandleInterstitialLoaded event received");
            Crashlytics.Log($"[ADMOB] HandleInterstitialLoaded event received, sender: {sender.ToString()}, message: {args.ToString()}");

            refetchInterstitialCoroutine = null;
        }

        public void HandleInterstitialFailedToLoad(object sender, AdFailedToLoadEventArgs args)
        {
            Debug.Log("[ADS] [ADMOB] HandleInterstitialFailedToLoad event received with message: " + args.Message);
            Crashlytics.Log($"[ADMOB] HandleInterstitialFailedToLoad event received, sender: {sender.ToString()}, message: {args.Message}");

            RefetchInterstitial();
        }

        private void RefetchInterstitial()
        {
            if (refetchInterstitialCoroutine != null) StopCoroutine(refetchInterstitialCoroutine);
            refetchInterstitialCoroutine = StartCoroutine(RefetchInterstitialHandler());
        }

        public void HandleInterstitialOpened(object sender, EventArgs args)
        {
            Debug.Log("[ADS] [ADMOB] HandleInterstitialOpened event received");
            Crashlytics.Log($"[ADMOB] HandleInterstitialOpened event received, sender: {sender.ToString()}, message: {args.ToString()}");

            InvokeAndLog(interstitialStartedCallback, nameof(interstitialStartedCallback));
        }

        public void HandleInterstitialClosed(object sender, EventArgs args)
        {
            Debug.Log($"[ADS] [ADMOB] HandleInterstitialClosed event received {args.ToString()}");
            Crashlytics.Log($"[ADMOB] HandleInterstitialClosed event received, sender: {sender.ToString()}, message: {args.ToString()}");

            InvokeAndLog(interstitialFinishedCallback, nameof(interstitialFinishedCallback));
            CleanUpInterstitialActions();

            Fetch(AdType.Interstitial);
        }

        public void HandleInterstitialLeftApplication(object sender, EventArgs args)
        {
            Debug.Log("[ADS] [ADMOB] HandleInterstitialLeftApplication event received");
            Crashlytics.Log($"[ADMOB] HandleInterstitialLeftApplication event received, sender: {sender.ToString()}, message: {args.ToString()}");
            SceneActivationBehaviour<GameLogicActivator>.Instance.AnalyticsController.SendAdClicked(AdType.Interstitial);
        }

        #endregion

        #region Reward based video

        private void RequestRewardBasedVideo()
        {
            Debug.Log("[ADS] [ADMOB] request rewarded video");
            Crashlytics.Log($"[ADMOB] Request rewarded video");

#if UNITY_EDITOR
            string adUnitId = "unused";
#elif UNITY_ANDROID
        string adUnitId = QuixelRewardedVideoIDAndroid;
#elif UNITY_IPHONE
        string adUnitId = QuixelRewardedVideoIDiOS;
#else
            string adUnitId = "unexpected_platform";
#endif

            this.rewardBasedVideo.LoadAd(this.CreateAdRequest(), adUnitId);
        }

        public void TryShowIncentivised(Action started = null, Action finished = null, Action error = null, Action cancelled = null)
        {
            Debug.Log("[ADS] [ADMOB] try show rewarded video");
            Crashlytics.Log($"[ADMOB] Try show rewarded video");

            rewardBasedVideoStartedCallback = started;
            rewardBasedVideoFinishedCallback = finished;
            rewardBasedVideoErrorCallback = error;
            rewardBasedVideoCancelledCallback = cancelled;
#if !UNITY_EDITOR
            ShowRewardBasedVideo();
#elif UNITY_EDITOR
            InvokeAndLog(rewardBasedVideoFinishedCallback, nameof(rewardBasedVideoFinishedCallback));
            CleanUpRewardedActions();
#endif
        }

        private void ShowRewardBasedVideo()
        {
            if (this.rewardBasedVideo != null && this.rewardBasedVideo.IsLoaded())
            {
                this.rewardBasedVideo.Show();
            }
            else
            {
                Debug.Log("[ADS] [ADMOB] Reward based video ad is not ready yet");
                Crashlytics.Log($"[ADMOB] Rewarded video is not ready yet");

                InvokeAndLog(rewardBasedVideoErrorCallback, nameof(rewardBasedVideoErrorCallback));
                CleanUpRewardedActions(); // ?
            }
        }

        public void HandleRewardBasedVideoLoaded(object sender, EventArgs args)
        {
            Debug.Log("[ADS] [ADMOB] HandleRewardBasedVideo Loaded");
            Crashlytics.Log($"[ADMOB] HandleRewardBasedVideo Loaded event received, sender: {sender.ToString()}, message: {args.ToString()}");

            refetchRewardedVideoCoroutine = null;
        }

        public void HandleRewardBasedVideoFailedToLoad(object sender, AdFailedToLoadEventArgs args)
        {
            Debug.Log("[ADS] [ADMOB] HandleRewardBasedVideo FailedToLoad with message: " + args.Message);
            Crashlytics.Log($"[ADMOB] HandleRewardBasedVideo FailedToLoad event received, sender: {sender.ToString()}, message: {args.ToString()}");

            RefetchRewardedVideo();
        }

        private void RefetchRewardedVideo()
        {
            if (refetchRewardedVideoCoroutine != null) StopCoroutine(refetchRewardedVideoCoroutine);
            refetchRewardedVideoCoroutine = StartCoroutine(RefetchRewardedVideoHandler());
        }

        public void HandleRewardBasedVideoOpened(object sender, EventArgs args)
        {
            Debug.Log("[ADS] [ADMOB] HandleRewardBasedVideo Opened");
            Crashlytics.Log($"[ADMOB] HandleRewardBasedVideo Opened event received, sender: {sender.ToString()}, message: {args.ToString()}");

            InvokeAndLog(rewardBasedVideoStartedCallback, nameof(rewardBasedVideoStartedCallback));
        }

        public void HandleRewardBasedVideoStarted(object sender, EventArgs args)
        {
            Debug.Log("[ADS] [ADMOB] HandleRewardBasedVideo Started");
            Crashlytics.Log($"[ADMOB] HandleRewardBasedVideo Started event received, sender: {sender.ToString()}, message: {args.ToString()}");
        }

        public void HandleRewardBasedVideoClosed(object sender, EventArgs args)
        {
            Debug.Log("[ADS] [ADMOB] HandleRewardBasedVideo Closed");
            Crashlytics.Log($"[ADMOB] HandleRewardBasedVideo Closed event received, sender: {sender.ToString()}, message: {args.ToString()}");

            InvokeAndLog(rewardBasedVideoCancelledCallback, nameof(rewardBasedVideoCancelledCallback));
            CleanUpRewardedActions();

            Fetch(AdType.RewardedVideo);
        }

        public void HandleRewardBasedVideoRewarded(object sender, Reward args)
        {
            string type = args.Type;
            double amount = args.Amount;
            Debug.Log("[ADS] [ADMOB] HandleRewardBasedVideo Rewarded for " + amount.ToString() + " " + type);
            Crashlytics.Log($"[ADMOB] HandleRewardBasedVideo Rewarded event received, sender: {sender.ToString()}, message: {args.ToString()}");

            InvokeAndLog(rewardBasedVideoFinishedCallback, nameof(rewardBasedVideoFinishedCallback));
            CleanUpRewardedActions();
        }

        public void HandleRewardBasedVideoLeftApplication(object sender, EventArgs args)
        {
            Debug.Log("[ADS] [ADMOB] HandleRewardBasedVideo LeftApplication");
            Crashlytics.Log($"[ADMOB] HandleRewardBasedVideo LeftApplication event received, sender: {sender.ToString()}, message: {args.ToString()}");
            SceneActivationBehaviour<GameLogicActivator>.Instance.AnalyticsController.SendAdClicked(AdType.RewardedVideo);
        }

        #endregion

        #region Banner ad

        private void RequestBanner()
        {
            // These ad units are configured to always serve test ads.
#if UNITY_EDITOR
            string adUnitId = "unused";
#elif UNITY_ANDROID
        string adUnitId = "ca-app-pub-3940256099942544/6300978111";
#elif UNITY_IPHONE
        string adUnitId = BannerTestID;     // TODO: THIS IS TEST
#else
            string adUnitId = "unexpected_platform";
#endif

            // Clean up banner ad before creating a new one.
            if (this.bannerView != null)
            {
                this.bannerView.Destroy();
            }

            // Create a 320x50 banner at the top of the screen.
            this.bannerView = new BannerView(adUnitId, AdSize.SmartBanner, AdPosition.Bottom);

            // Register for ad events.
            this.bannerView.OnAdLoaded += this.HandleAdLoaded;
            this.bannerView.OnAdFailedToLoad += this.HandleAdFailedToLoad;
            this.bannerView.OnAdOpening += this.HandleAdOpened;
            this.bannerView.OnAdClosed += this.HandleAdClosed;
            this.bannerView.OnAdLeavingApplication += this.HandleAdLeftApplication;

            // Load a banner ad.
            this.bannerView.LoadAd(this.CreateAdRequest());
        }

        public void HandleAdLoaded(object sender, EventArgs args)
        {
            Debug.Log("[ADS] [ADMOB] HandleAdLoaded event received");
        }

        public void HandleAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
        {
            Debug.Log("[ADS] [ADMOB] HandleFailedToReceiveAd event received with message: " + args.Message);
        }

        public void HandleAdOpened(object sender, EventArgs args)
        {
            Debug.Log("[ADS] [ADMOB] HandleAdOpened event received");
        }

        public void HandleAdClosed(object sender, EventArgs args)
        {
            Debug.Log("[ADS] [ADMOB] HandleAdClosed event received");
        }

        public void HandleAdLeftApplication(object sender, EventArgs args)
        {
            Debug.Log("[ADS] [ADMOB] HandleAdLeftApplication event received");
            SceneActivationBehaviour<GameLogicActivator>.Instance.AnalyticsController.SendAdClicked(AdType.Banner);
        }

        #endregion

        private IEnumerator RefetchInterstitialHandler()
        {
            Debug.Log("[ADS] [ADMOB] Refetch interstitial handler");
            yield return new WaitForSeconds(AdFailRetryTime);

            Fetch(AdType.Interstitial);
            refetchInterstitialCoroutine = null;
            yield break;
        }

        private IEnumerator RefetchRewardedVideoHandler()
        {
            Debug.Log("[ADS] [ADMOB] Refetch rewarded video handler");
            yield return new WaitForSeconds(AdFailRetryTime);

            Fetch(AdType.RewardedVideo);
            refetchRewardedVideoCoroutine = null;
            yield break;
        }

        private void OnNativeExceptionReceivedFromSdk(string message)
        {
            //handle exception
            Debug.LogError("[ADMOB] SDK Exception: " + message);
            Crashlytics.Log($"[ADMOB] OnNativeExceptionReceivedFromSdk : {message}");
        }

        private void CleanUpInterstitialActions()
        {
            Debug.Log("[ADMOB] Clean up interstitial actions");
            Crashlytics.Log($"[ADMOB] Clean up interstitial actions");

            interstitialErrorCallback = null;
            interstitialStartedCallback = null;
            interstitialFinishedCallback = null;

            // remove registers for ad events.
            if (this.interstitial != null)
            {
                this.interstitial.OnAdLoaded -= this.HandleInterstitialLoaded;
                this.interstitial.OnAdFailedToLoad -= this.HandleInterstitialFailedToLoad;
                this.interstitial.OnAdOpening -= this.HandleInterstitialOpened;
                this.interstitial.OnAdClosed -= this.HandleInterstitialClosed;
                this.interstitial.OnAdLeavingApplication -= this.HandleInterstitialLeftApplication;
            }
        }

        private void CleanUpRewardedActions()
        {
            Debug.Log("[ADMOB] Clean up rewarded actions");
            Crashlytics.Log($"[ADMOB] Clean up rewarded actions");

            rewardBasedVideoErrorCallback = null;
            rewardBasedVideoStartedCallback = null;
            rewardBasedVideoFinishedCallback = null;
            rewardBasedVideoCancelledCallback = null;
        }

        private void InvokeAndLog(Action action, string actionName)
        {
            Debug.Log($"[ADMOB] Invoke {actionName}, isNull? {action == null}");
            Crashlytics.Log($"[ADMOB] Invoke {actionName}, isNull? {action == null}");
            action?.Invoke();
        }
    }
}
