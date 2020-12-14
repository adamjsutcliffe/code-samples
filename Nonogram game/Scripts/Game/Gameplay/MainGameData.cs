using Peak.QuixelLogic.Scripts.Common.AnalyticsScripts;
using Peak.QuixelLogic.Scripts.Settings;

namespace Peak.QuixelLogic.Scripts.Game.Gameplay
{
    /// <summary>
    /// Stores current main game progress (on board, session)
    /// </summary>
    public class MainGameData
    {
        public RuleSettings Ruleset { get; set; }

        public int TimeLimit => Ruleset.TimeLimit;
        public int SecondsLeft { get; set; }

        public int StarScore { get; set; }

        public bool Replay { get; set; }
        public GameType GameType { get; set; }

        public int PlayerFilmRemaining { get; set; }

        public override string ToString()
        {
            return $"Game data: " +
                $"{nameof(Ruleset)}: {Ruleset.name}" +

                $"{nameof(TimeLimit)}: {TimeLimit}, " +
                $"{nameof(SecondsLeft)}: {SecondsLeft}, " +
                $"{nameof(StarScore)}: {StarScore}, " +
                $"{nameof(Replay)}: {Replay}, ";
        }
    }
}