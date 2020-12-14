using System;

namespace Peak.QuixelLogic.Scripts.Game
{
    /// <summary>
    /// Stores information about the FTUE
    /// </summary>

    [Serializable]
    public sealed class FtueInformation
    {
        public bool IsPart1Passed;
        public bool IsPart2Passed;
        public bool IsPart3Passed;
        public bool IsPart4Passed;
        public bool IsPart5Passed;
        public bool IsPart6Passed;
        public bool IsHintTipPassed;
        public bool IsCollectionViewPassed;
        public bool IsFilmIntroPassed;
        public bool NotificationPopupShown;
        public bool OneDayNotificationScheduled;
        public bool isGdprNotificationShown;

        public override string ToString()
        {
            return $"{nameof(IsPart1Passed)}: {IsPart1Passed}, " +
                $"\n{nameof(IsPart2Passed)}: {IsPart2Passed}, " +
                $"\n{nameof(IsPart3Passed)}: {IsPart3Passed}, " +
                $"\n{nameof(IsPart4Passed)}: {IsPart4Passed}, " +
                $"\n{nameof(IsPart5Passed)}: {IsPart5Passed}, " +
                $"\n{nameof(IsPart6Passed)}: {IsPart6Passed}, " +
                $"\n{nameof(IsHintTipPassed)}: {IsHintTipPassed}, " +
                $"\n{nameof(IsCollectionViewPassed)}: {IsCollectionViewPassed}, " +
                $"\n{nameof(IsFilmIntroPassed)}: {IsFilmIntroPassed}, " +
                $"\n{nameof(NotificationPopupShown)}: {NotificationPopupShown}, " +
                $"\n{nameof(OneDayNotificationScheduled)}: {OneDayNotificationScheduled}, ";
        }
    }
}