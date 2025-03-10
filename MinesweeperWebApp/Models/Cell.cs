﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinesweeperWebApp.Models
{
    [Serializable]
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

        public Cell Clone()
        {
            return new Cell(this.Row, this.Column)
            {
                Row = this.Row,
                Column = this.Column,
                IsVisited = this.IsVisited,
                IsBomb = this.IsBomb,
                IsFlagged = this.IsFlagged,
                NumberOfBombNeighbors = this.NumberOfBombNeighbors,
                HasSpecialReward = this.HasSpecialReward,
            };
        }

        // **Equality Check** - Compare two cells
        public override bool Equals(object obj)
        {
            if (obj is not Cell other) return false;
            return Column == other.Column &&
                   Row == other.Row &&
                   IsVisited == other.IsVisited &&
                   IsBomb == other.IsBomb &&
                   IsFlagged == other.IsFlagged &&
                   NumberOfBombNeighbors == other.NumberOfBombNeighbors;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Column, Row, IsVisited, IsBomb, IsFlagged, NumberOfBombNeighbors);
        }
    }
}
