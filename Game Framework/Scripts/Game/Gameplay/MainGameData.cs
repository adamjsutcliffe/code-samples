using Peak.Speedoku.Scripts.Settings;

namespace Peak.Speedoku.Scripts.Game.Gameplay
{
    /// <summary>
    /// Stores current main game progress (on board, session)
    /// </summary>
    public class MainGameData
    {
        public RuleSettings Ruleset { get; set; }
        public int Level { get; set; }
        public int Score { get; set; }
        public int SecondsUsed { get; set; }
        public GridAnswer[] answers { get; set; }

        public override string ToString()
        {
            return $"Game data: " +
                $"{nameof(Ruleset)}: {Ruleset.Id}" +
                $"{nameof(Level)}: {Level}, " +
                $"{nameof(Score)}: {Score}, " +
                $"{nameof(SecondsUsed)}: {SecondsUsed}, " +
                $"{nameof(answers)}: {answers}";
        }
    }
}