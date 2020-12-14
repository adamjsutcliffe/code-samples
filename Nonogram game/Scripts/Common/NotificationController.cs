using System;
using System.Collections;
using JetBrains.Annotations;
using Peak.QuixelLogic.Scripts.Common.AnalyticsScripts;
using UnityEngine;
using System.Globalization;

#if UNITY_ANDROID

using Assets.SimpleAndroidNotifications;

#elif UNITY_IOS

using UnityEngine.iOS;
using NotificationServices = UnityEngine.iOS.NotificationServices;
using LocalNotification = UnityEngine.iOS.LocalNotification;

#endif

namespace Peak.QuixelLogic.Scripts.Common
{
    public sealed class NotificationController : MonoBehaviour
    {
        // Notification message example
        [SerializeField]
        private string scheduledEventNotificationMessage;

        [SerializeField]
        private string inactiveLatelyMessage;

        [SerializeField]
        private int inactiveLatelyNotificationDelayInSeconds;

        [SerializeField, Tooltip("Android allows to set the title separately")]
        private string androidAppTitle;

        [Header("Other")]
        [SerializeField, Tooltip("Delay before the permissions are asked after the post game window is closed")]
        private float popupCloseDelay;

        [SerializeField]
        private AnalyticsController analyticsController;

        [SerializeField, Tooltip("Delay before the permissions will be checked and the result goes to analytics")]
        private float waitForConfirmationDelay;

        private TimeSpan followingMidnight;
        private TimeSpan followingEightAM;

        public static NotificationController Instance { get; private set; }

        public bool notificationsOn { get; set; }

        private void Awake()
        {
            Instance = this;
        }

        public void Start()
        {
            // at app start clear all scheduled notification as far as we should not be notified while in game
            ClearAllNotifications();
#if UNITY_ANDROID
            if (!PlayerPrefs.HasKey(GameConstants.SettingKeys.NotificationsOn))
            {
                ToggleNotifications();
            }
            else
            {
                if (PlayerPrefs.GetInt(GameConstants.SettingKeys.NotificationsOn) == 1)
                {
                    ToggleNotifications();
                }
            }
#elif UNITY_IOS
            bool shouldShowNotifications = PlayerPrefs.HasKey(GameConstants.SettingKeys.NotificationsOn) && PlayerPrefs.GetInt(GameConstants.SettingKeys.NotificationsOn) == 1;
            if (shouldShowNotifications)
            {
                ToggleNotifications();
            }
#endif

        }

        // Call by main game OnApplicationPause
        public void ClearAllNotifications()
        {
#if UNITY_ANDROID

            NotificationManager.CancelAll();

#elif UNITY_IOS

            if (NotificationServices.localNotificationCount > 0)
            {
                //foreach (LocalNotification notification in NotificationServices.localNotifications)
                //{
                //    Debug.Log("INCOME ALERTS: " + notification.alertBody);
                //}
            }

            // cancel all notifications first.
            NotificationServices.CancelAllLocalNotifications();
            NotificationServices.ClearLocalNotifications();

#endif
        }

        public IEnumerator AskForPermissionsCoroutine()
        {
            yield return new WaitForSeconds(popupCloseDelay);

#if UNITY_ANDROID // 1. RegisterForNotifications via manifest
            // 2. Assuming it's allowed on install

            //analyticsController.SendNotificationPermissionsResult(true);

#elif UNITY_IOS

            NotificationServices.RegisterForNotifications(
                NotificationType.Alert | NotificationType.Badge | NotificationType.Sound);
            StartCoroutine(CheckForiOSPermissionOutcome());
#endif
        }

        private IEnumerator CheckForiOSPermissionOutcome()
        {
            yield return new WaitForSeconds(1.0f);
            SendPermissionsOutcomeForiOS();
        }

        public void SendPermissionsOutcomeForiOS()
        {
#if UNITY_IOS
            bool permissionAllowed = UnityEngine.iOS.NotificationServices.enabledNotificationTypes != NotificationType.None;
            //print($"Check for iOS permission - {permissionAllowed}");
            analyticsController.SendNotificationAcceptedEvent(permissionAllowed);
#endif
        }

        public void ScheduleFilmReplenishmentNotification(double secondsUntilFull)
        {
            if ((int)secondsUntilFull > 0)
            {
                if (notificationsOn)
                {
                    secondsUntilFull = Mathf.Round((float)BedtimeCheck(secondsUntilFull));
#if UNITY_ANDROID
                    NotificationParams notification = new NotificationParams
                    {
                        Id = NotificationIdHandler.GetNotificationId(),
                        Ticker = "Ticker",
                        Sound = true,
                        Vibrate = true,
                        Light = true,
                        LargeIcon = "app_icon",
                        SmallIcon = NotificationIcon.Event,
                        SmallIconColor = new Color(0, 0.5f, 0),
                        Delay = TimeSpan.FromSeconds(secondsUntilFull),
                        Title = androidAppTitle,
                        Message = RandomFilmReplenishmentMessage()
                    };
                    NotificationManager.SendCustom(notification);
#elif UNITY_IOS

                    LocalNotification notification = new LocalNotification
                    {
                        applicationIconBadgeNumber = 1,
                        alertBody = RandomFilmReplenishmentMessage(),
                        fireDate = DateTime.Now.AddSeconds((int)secondsUntilFull),
                        soundName = LocalNotification.defaultSoundName
                    };
                    NotificationServices.ScheduleLocalNotification(notification);
#endif
                }
            }
        }

        // Call by main game OnApplicationPause
        public void ScheduleOneDayNotification()
        {
            string dayNotificationScheduleTime = PlayerPrefs.GetString(Constants.Notifications.DayNotificationScheduleTime, "");

            if (dayNotificationScheduleTime != "")
            {
                DateTime scheduledOneDayNotificationTime = DateTime.ParseExact(dayNotificationScheduleTime, "yyyyMMddHHmmss", CultureInfo.CurrentCulture);
                TimeSpan timeSpan = scheduledOneDayNotificationTime - DateTime.Now;

                if (timeSpan.TotalSeconds > 0)
                {
#if UNITY_ANDROID
                    NotificationParams notification = new NotificationParams
                    {
                        Id = NotificationIdHandler.GetNotificationId(),
                        Ticker = "Ticker",
                        Sound = true,
                        Vibrate = true,
                        Light = true,
                        LargeIcon = "app_icon",
                        SmallIcon = NotificationIcon.Heart,
                        SmallIconColor = new Color(0, 0.5f, 0),
                        Delay = TimeSpan.FromSeconds(timeSpan.TotalSeconds),
                        Title = androidAppTitle,
                        Message = Random24HourMessage()
                    };
                    NotificationManager.SendCustom(notification);
#elif UNITY_IOS
                    if (notificationsOn)
                    {
                        LocalNotification notification = new LocalNotification
                        {
                            applicationIconBadgeNumber = 1,
                            alertBody = Random24HourMessage(),
                            fireDate = DateTime.Now.AddSeconds(timeSpan.TotalSeconds),
                            soundName = LocalNotification.defaultSoundName
                        };
                        NotificationServices.ScheduleLocalNotification(notification);
                    }
#endif
                }
            }
        }

        private double BedtimeCheck(double secondsUntilFull)
        {
            TimeSpan targetTime = DateTime.Now.AddSeconds((int)secondsUntilFull).TimeOfDay;

            followingMidnight = new TimeSpan(DateTime.Today.AddHours(24).Hour, DateTime.Today.AddHours(24).Minute, 0);
            followingEightAM = new TimeSpan(DateTime.Today.AddHours(32).Hour, DateTime.Today.AddHours(32).Minute, 0);

            if (targetTime > followingMidnight && targetTime <= followingEightAM)
            {
                secondsUntilFull += (followingEightAM - targetTime).TotalSeconds;
            }

            return secondsUntilFull;
        }

        private string Random24HourMessage()
        {
            int random = UnityEngine.Random.Range(0, 3);
            if (random.Equals(0)) { return $"\ud83d\udcf7 {GameConstants.NotificationMessages.DayNotif_v1}"; }
            if (random.Equals(1)) { return $"\ud83d\udcf7 {GameConstants.NotificationMessages.DayNotif_v2}"; }
            if (random.Equals(2)) { return $"{GameConstants.NotificationMessages.DayNotif_v3} \ud83d\udcf7 \ud83d\udc7e"; }
            return null;
        }

        private string RandomFilmReplenishmentMessage()
        {
            int random = UnityEngine.Random.Range(0, 2);
            if (random.Equals(0)) { return $"\ud83d\udcf7 {GameConstants.NotificationMessages.FilmNotif_v1}"; }
            if (random.Equals(1)) { return $"\ud83d\udcf7 {GameConstants.NotificationMessages.FilmNotif_v2}"; }
            return null;
        }

        public bool ToggleNotifications()
        {
            bool isEnabled = notificationsOn ^= true;
            PlayerPrefs.SetInt(GameConstants.SettingKeys.NotificationsOn, isEnabled ? 1 : 0);

            if (!isEnabled)
            {
                ClearAllNotifications();
            }
            //else
            //{
            //    ScheduleOneDayNotification();
            //}

            return notificationsOn;
        }
    }
}