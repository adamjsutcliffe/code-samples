using System;

namespace Peak.Speedoku.Scripts.Game
{
    /// <summary>
    /// Stores information about the FTUE
    /// </summary>

    [Serializable]
    public sealed class FtueInformation
    {
        public bool IsPopupPassed;
        public bool IsPart1Passed;
        public bool IsPart2Passed;
        public bool IsPart3Passed;
        public bool IsPart4Passed;

        public bool isGdprNotificationShown;
        public bool isNotificationPopupShown;

        public override string ToString()
        {
            return $"{nameof(IsPopupPassed)}: {IsPopupPassed}, " +
                $"\n{nameof(IsPart1Passed)}: {IsPart1Passed}," +
                $"\n{nameof(IsPart2Passed)}: {IsPart2Passed}, " +
                $"\n{nameof(IsPart3Passed)}: {IsPart3Passed}, " +
                $"\n{nameof(IsPart4Passed)}: {IsPart4Passed}, " +
                $"\n{nameof(isGdprNotificationShown)}: {isGdprNotificationShown}," +
                $"\n{nameof(isNotificationPopupShown)}: {isNotificationPopupShown},";
        }
    }
}
