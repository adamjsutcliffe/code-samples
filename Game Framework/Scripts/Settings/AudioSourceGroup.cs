namespace Peak.UnityGameFramework.Scripts.Settings
{
    /// <summary>
    /// Describes the behaviour of the sound affected by its mixer
    /// </summary>
    public enum AudioSourceGroup
    {
        /// <summary>
        /// Single sound play for repetitive sounds (like clicks)
        /// </summary>
        Expendable = 0,
        /// <summary>
        /// Single sound play for special notifications
        /// </summary>
        Notification = 1,
        /// <summary>
        /// Single sound play for events that may occur randomly
        /// </summary>
        Event = 2,
        /// <summary>
        /// Continuously plays at the background (first channel)
        /// </summary>
        Background = 3,
        /// <summary>
        /// Continuously plays at the background (second channel)
        /// </summary>
        BackgroundAmbient = 4,
    }
}