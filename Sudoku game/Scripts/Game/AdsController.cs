using System;
using System.Collections;
using Peak.Speedoku.Scripts.Common.AnalyticsScripts;
using Peak.Speedoku.Scripts.ScenesLogic;
using UnityEngine;

namespace Peak.Speedoku.Scripts.Game
{
    public sealed class AdsController : MonoBehaviour
    {
        /// <summary>
        /// This Application ID at Fyber
        /// </summary>
        [Header("Fyber/HeyZap app id")]
        [SerializeField]
        private string appId;

        /// <summary>
        /// Verbous Fyber logging
        /// </summary>
        [SerializeField]
        private bool enabledLogging;

        [SerializeField]
        private int retriesCount;

        [SerializeField]
        private float timeOutTime = 5.0f;

        private AnalyticsController analytics;

        /// <summary>
        /// The banner refresh rate time in seconds.
        /// </summary>
        [SerializeField]
        private int bannerRefreshRate = 30;

        //public event Action BannerShown;
        //public event Action BannerHidden;


        ///// <summary>
        ///// Shows weather a banner is hidden by any cause
        ///// </summary>
        //public bool ShouldHideBanner()
        //{
        //    return isBannerHidden || IsBannerHiddenByPlayer();
        //}

        ///// <summary>
        ///// Shows weather a banner is hidden by a purchase or FTUE
        ///// </summary>
        //private bool IsBannerHiddenByPlayer()
        //{
        //    bool areAdsDisabled = SceneActivationBehaviour<NoAdsActivator>.Instance.AreAdsDisabled();
        //    bool isInFtue = SceneActivationBehaviour<GameLogicActivator>.Instance.FtueController.IsMainFtueInProgress;
        //    return areAdsDisabled || isInFtue;
        //}

        //private bool isBannerHidden;

        private Action adErrorCallback;
        private Action rewardConfirmationCallback;
        private Action adCanceledCallback;
        private Action adFinishedCallback;
        private Action adStartedCallback;

        #region API


        public void Awake()
        {
            print("INIT AD CONTROLLER");



            //TODO: show banner on start??
            //StartBanner();
        }

        // **** INTERSTITIAL ADS ****

        public void TryShowInterstitial(Action adStartedCallback, Action finished = null, Action error = null)
        {

#if UNITY_EDITOR
            InvokeAndLog(adStartedCallback, nameof(adStartedCallback));
            InvokeAndLog(finished, nameof(finished));
            return;
#endif

           
        }

       

        public void RemoveAdsPurchased()
        {
            //TODO: implement remove ad purchase handler if necessary
            return;
        }

        // **** REWARDED ADS ****

        public void ShowAdForReward(AdSourceType source,
            Action error = null, Action success = null, Action cancel = null, Action finished = null)
        {

#if UNITY_EDITOR
            Debug.Log("[FYBER] ShowAdForReward, EDITOR -> RewardedVideo");
            InvokeAndLog(success, nameof(success));
            return;
#endif
            adErrorCallback = error;
            rewardConfirmationCallback = success;
            adCanceledCallback = cancel;
            adFinishedCallback = finished;

           
        }


        #endregion

        private void OnNativeExceptionReceivedFromSdk(string message)
        {
            //handle exception
            Debug.LogError("[FYBER] SDK Exception: " + message);
        }


        private void CleanUpActions()
        {
            Debug.Log("[FYBER] Clean up");

            ////Remove all callbacks
            adErrorCallback = null;
            rewardConfirmationCallback = null;
            adCanceledCallback = null;
            adFinishedCallback = null;
            adStartedCallback = null;
        }

        private void InvokeAndLog(Action action, string actionName)
        {
            Debug.Log($"[FYBER] Invoke {actionName}, isNull? {action == null}");
            action?.Invoke();
        }
    }
}


