using XamarinTicTacToe.Engine.Interfaces;

namespace XamarinTicTacToe.Engine.Utils
{
    public class GameStateChangedEventArgs
    {
        public FieldState CurrentState { get; set; }
        public IPlayer CurrentPlayer { get; set; }
    }
}
