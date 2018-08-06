using XamarinTicTacToe.Engine.Enums;
using XamarinTicTacToe.Engine.Utils;

namespace XamarinTicTacToe.Engine.Interfaces
{
    public interface IPlayer
    {
        string Name { get; }
        PlayerType Type { get; }
        /// <summary>
        ///     Get next move
        /// </summary>
        /// <param name="fieldState">Current game state.</param>
        /// <returns></returns>
        Cell GetNextMove(FieldState fieldState);
    }
}
