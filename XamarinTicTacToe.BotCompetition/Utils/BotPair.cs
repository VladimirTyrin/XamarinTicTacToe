using XamarinTicTacToe.BotCompetition.Enums;
using XamarinTicTacToe.Engine.Enums;

namespace XamarinTicTacToe.BotCompetition.Utils
{
    internal class BotPair
    {
        public BotKind FirstPlayer { get; set; }
        public BotKind SecondPlayer { get; set; }
        public MatchResult MatchResult { get; set; }
    }
}
