using System;
using XamarinTicTacToe.Engine.Enums;
using XamarinTicTacToe.Engine.Utils;

namespace XamarinTicTacToe.Engine.Bots.BrainTvsV2
{
    public class BrainTvsBotV2 : BotPlayer
    {
        public override string Name => "bra1n_tvs bot";
        public override Cell GetNextMove(FieldState fieldState)
        {
            if (_justStarted)
            {
                _justStarted = false;
                Initialize(fieldState);
            }

            if (fieldState.Step == 1)
            {
                return GetInitialMove(fieldState);
            }

            _winningMoveManager.Fill(fieldState.Field, fieldState.LastMove);
            var winningMove = _winningMoveManager.GetNextMove();
            if (winningMove != null)
            {
                return winningMove;
            }

            _cellGroupManager.Fill(fieldState.Field, fieldState.LastMove);
            
            var move = _cellGroupManager.GetNextMove();
            _winningMoveManager.AddMove(move);
            return move;
        }

        public override string Author => "Vladimir Tyrin";

        private void Initialize(FieldState initialState)
        {
            _width = initialState.Width;
            _height = initialState.Height;
            _cellGroupManager = new CellGroupManager(initialState);
            _winningMoveManager = new WinningMoveManager(initialState);
        }

        private Cell GetInitialMove(FieldState fieldState)
        {
            if (fieldState.PlayerSign == CellSign.X)
                return GetInitialMoveForX(fieldState);

            return GetInitialMoveForO(fieldState);
        }

        private Cell GetInitialMoveForX(FieldState fieldState)
        {
            var (centerX, centerY) = (_height / 2, _width / 2);

            for (var i = -1; i <= 1; ++i)
            {
                for (var j = -1; j <= 1; ++j)
                {
                    if (fieldState.Field[centerX + i, centerY + j] == CellSign.Empty)
                        return new Cell(centerX + i, centerY + j);
                }
            }

            return new Cell(_random.Next(_height), _random.Next(_width));
        }

        private Cell GetInitialMoveForO(FieldState fieldState)
        {
            var (lastX, lastY) = (fieldState.LastMove.X / 2, fieldState.LastMove.Y / 2);

            for (var i = -1; i <= 1; ++i)
            {
                for (var j = -1; j <= 1; ++j)
                {
                    if (fieldState.Field[lastX + i, lastY + j] == CellSign.Empty)
                        return new Cell(lastX + i, lastY + j);
                }
            }

            return new Cell(_random.Next(_height), _random.Next(_width));
        }

        private readonly Random _random = new Random();

        private bool _justStarted = true;

        private CellGroupManager _cellGroupManager;
        private WinningMoveManager _winningMoveManager;
        private int _width;
        private int _height;
    }
}
