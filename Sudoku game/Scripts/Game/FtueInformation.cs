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
        public bool IsGameOnePassed;
        public bool IsGameTwoPassed;
        //public bool IsPart3Passed;
        //public bool IsPart4Passed;

        public bool isGdprNotificationShown;
        public bool isNotificationPopupShown;

        public bool wasFeedbackGiven;

        public override string ToString()
        {
            return $"{nameof(IsPopupPassed)}: {IsPopupPassed}, " +
                $"\n{nameof(IsGameOnePassed)}: {IsGameOnePassed}," +
                $"\n{nameof(IsGameTwoPassed)}: {IsGameTwoPassed}, " +
                //$"\n{nameof(IsPart3Passed)}: {IsPart3Passed}, " +
                //$"\n{nameof(IsPart4Passed)}: {IsPart4Passed}, " +
                $"\n{nameof(isGdprNotificationShown)}: {isGdprNotificationShown}," +
                $"\n{nameof(isNotificationPopupShown)}: {isNotificationPopupShown}," +
                $"\n{nameof(wasFeedbackGiven)}: {wasFeedbackGiven},";
        }
    }
}
