using System.Linq;
using XamarinTicTacToe.Engine.Utils;

namespace XamarinTicTacToe.Engine.Bots.BrainTvs
{
    internal class CellGroup
    {
        public int QuantityType { get; set; }
        public Cell[] Cells { get; set; }
        public Cell[] FreeCells { get; set; }
        public bool Open { get; set; }
        public FlowDirection FlowDirection { get; set; }

        public Cell GetPossibleMove() => FreeCells.First();
        public string CellsDisplay() => string.Join(" ", Cells.Select(c => $"({c.X}, {c.Y})"));
    }
}
