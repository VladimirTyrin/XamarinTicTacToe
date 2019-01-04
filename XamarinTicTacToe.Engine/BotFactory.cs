using XamarinTicTacToe.Engine.Bots.BrainTvs;
using XamarinTicTacToe.Engine.Bots.BrainTvsV2;
using XamarinTicTacToe.Engine.Bots.Trivial;
using XamarinTicTacToe.Engine.Enums;

namespace XamarinTicTacToe.Engine
{
    public static class BotFactory
    {
        public static BotPlayer BuildBot(BotKind kind)
        {
            switch (kind)
            {
                case BotKind.Trivial:
                    return new TrivialBot();
                case BotKind.BrainTvs:
                    return new BrainTvsBot();
                case BotKind.BrainTvsV2:
                    return new BrainTvsBotV2();
                default:
                    return null;
            }
        }
    }
}
