﻿using System.Collections.Generic;
using XamarinTicTacToe.Engine.Enums;
using XamarinTicTacToe.Engine.Interfaces;

namespace XamarinTicTacToe.Engine.Utils
{
    public class GameEndedEventArgs
    {
        public GameState State { get; set; }
        /// <summary>
        ///     null if state is draw
        /// </summary>
        public IPlayer Winner { get; set; }
        /// <summary>
        ///     null if state is draw
        /// </summary>
        public IPlayer Loser { get; set; }
        public CellSign[,] Field { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public List<Cell> History { get; set; }
        public List<Cell> WinningSet { get; set; }
    }
}
