namespace Peak.QuixelLogic.Scripts.Common.AnalyticsScripts
{
    //Ad data
    public enum AdResultType
    {
        Watched = 0,
        Aborted = 1,
        Error = 2,
        Requested = 3,
        Started = 4,
    }

    public enum AdSourceType
    {
        NotSet = 0,
        InGame = 1,
        ReplayPopUp = 2,
        GoldCardPopUp = 3,
        FilmPopUp = 4,
        PostGame = 5,
        AndroidGold = 6
        // Add more sources of videos if there are any
    }

    public enum AdType
    {
        NotSet = 0,
        RewardedVideo = 1,
        Interstitial = 2,
        Banner = 3
    }
}