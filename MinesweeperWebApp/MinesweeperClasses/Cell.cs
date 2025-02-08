using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinesweeperWebApp.MineSweeperClasses
{
    public class Cell
    {
        public int Row { get; set; }
        public int Column { get; set; }
        public bool IsVisited { get; set; }
        public bool IsBomb { get; set; }
        public bool IsFlagged { get; set; }
        public int NumberOfBombNeighbors { get; set; }
        public bool HasSpecialReward { get; set; }

        /// <summary>
        /// Cell constructor
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        public Cell(int row, int column)
        {
            Row = row;
            Column = column;
        }
    }
}
