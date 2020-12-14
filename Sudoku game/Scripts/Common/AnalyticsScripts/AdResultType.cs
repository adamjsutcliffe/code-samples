namespace Peak.Speedoku.Scripts.Common.AnalyticsScripts
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
        // Add more sources of videos if there are any
    }

    public enum AdType
    {
        NotSet = 0,
        RewardedVideo = 1,
        Interstitial = 2,
        Banner = 3
    }

    //Game data
    public enum GameSourceType
    {
        Start = 0,
        Quit = 1,
        Finish = 2,
        Pause = 3,
        Resume = 4
    }

    public enum GameResultType
    {
        NotSet = 0,
        Success = 1,
        Fail = 2
    }
}
