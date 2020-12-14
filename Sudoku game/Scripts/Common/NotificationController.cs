using System;
using System.Collections;
using JetBrains.Annotations;
using Peak.Speedoku.Scripts.Common.AnalyticsScripts;
using UnityEngine;

#if UNITY_ANDROID

using Assets.SimpleAndroidNotifications;

#elif UNITY_IOS

using UnityEngine.iOS;
using NotificationServices = UnityEngine.iOS.NotificationServices;
using LocalNotification = UnityEngine.iOS.LocalNotification;

#endif

namespace Peak.Speedoku.Scripts.Common
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
        private float postGameWindowClosedDelay;

        [SerializeField]
        private AnalyticsController analyticsController;

        [SerializeField, Tooltip("Delay before the permissions will be checked and the result goes to analytics")]
        private float waitForConfirmationDelay;

        [UsedImplicitly]
        public void Start()
        {
            // at app start clear all scheduled notification as far as we should not be notified while in game
            ClearAllNotifications();
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
            print("[NOTIFICATION] Ask permissions");

            yield return new WaitForSeconds(postGameWindowClosedDelay);

#if UNITY_ANDROID // 1. RegisterForNotifications via manifest
            // 2. Assuming it's allowed on install

            //analyticsController.SendNotificationPermissionsResult(true);

#elif UNITY_IOS

            NotificationServices.RegisterForNotifications(
                NotificationType.Alert | NotificationType.Badge | NotificationType.Sound);
#endif
        }

        public void SendPermissionsOutcomeForiOS()
        {
#if UNITY_IOS
            //analyticsController.SendNotificationPermissionsResult(
                //UnityEngine.iOS.NotificationServices.enabledNotificationTypes != NotificationType.None);
#endif
        }

        public void ScheduleFutureNotification(int seconds)
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
                SmallIcon = NotificationIcon.Coin,
                SmallIconColor = new Color(0, 0.5f, 0),
                CallbackData = "ScheduleFutureNotification", // name of method

                Delay = TimeSpan.FromSeconds(seconds),
                Title = androidAppTitle,
                Message = string.Format(scheduledEventNotificationMessage)
            };
            NotificationManager.SendCustom(notification);

#elif UNITY_IOS

            //LocalNotification notification = new LocalNotification
            //{
            //    applicationIconBadgeNumber = 1,
            //    alertBody = string.Format(scheduledEventNotificationMessage, giftName),
            //    fireDate = DateTime.Now.AddSeconds(seconds),
            //    soundName = LocalNotification.defaultSoundName
            //};
            //NotificationServices.ScheduleLocalNotification(notification);

#endif
        }

        // Call by main game OnApplicationPause
        public void ScheduleReturnBackToGameNotification()
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
                CallbackData = "ScheduleReturnBackToGameNotification", // name of method

                Delay = TimeSpan.FromSeconds(inactiveLatelyNotificationDelayInSeconds),
                Title = androidAppTitle,
                Message = inactiveLatelyMessage
            };
            NotificationManager.SendCustom(notification);

#elif UNITY_IOS

            LocalNotification notification = new LocalNotification
            {
                applicationIconBadgeNumber = 1,
            alertBody = inactiveLatelyMessage,
            fireDate = DateTime.Now.AddSeconds(inactiveLatelyNotificationDelayInSeconds),
                soundName = LocalNotification.defaultSoundName
            };
            NotificationServices.ScheduleLocalNotification(notification);

#endif
        }

    }
}


