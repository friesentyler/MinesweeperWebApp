using Microsoft.CodeAnalysis.CSharp.Syntax;
using MinesweeperWebApp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace MinesweeperWebApp.Models
{
    public class Board
    {
        public int Size { get; set; }
        public float PercentBombs { get; set; }
        public Cell[,] Cells { get; set; }
        public int RewardsRemaining { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public enum GameStatus { InProgress, Won, Lost }
        public int score { get; set; }
        public int collectedRewards { get; set; }

        [NonSerialized]
        Random random = new Random();

        /// <summary>
        /// constructor for board
        /// </summary>
        /// <param name="difficulty"></param>
        public Board(int difficulty)
        {
            if (difficulty == 1)
            {
                Size = 10;
                RewardsRemaining = 1;
                PercentBombs = 0.08f;
            }
            else if (difficulty == 2)
            {
                Size = 16;
                RewardsRemaining = 2;
                PercentBombs = 0.15f;
            }
            else if (difficulty == 3)
            {
                Size = 25;
                RewardsRemaining = 4;
                PercentBombs = 0.20f;
            }
            Cells = new Cell[Size, Size];
            score = 0;
            InitializeBoard();
        }

        /// <summary>
        /// Initialize the board with components
        /// </summary>
        private void InitializeBoard()
        {
            for (int col = 0; col < Size; col++)
            {
                for (int row = 0; row < Size; row++)
                {
                    Cells[col, row] = new Cell(col, row);
                }
            }
            SetupBombs();
            SetupRewards();
            CalculateNumberOfBombNeighbors();
            StartTime = DateTime.Now;
        }

        /// <summary>
        /// when the user implements this reward, all adjacent cells
        /// are visited automatically
        /// if one of the adjacent cells is a bomb, it is flagged
        /// </summary>
        /// <param name="Xcoord"></param>
        /// <param name="Ycoord"></param>
        public void UseSpecialBonus(int Xcoord, int Ycoord)
        {
            for (int col = -1; col < 2; col++)
            {
                for (int row = -1; row < 2; row++)
                {
                    if (IsCellOnBoard(Xcoord + col, Ycoord + row))
                    {
                        if (!Cells[Xcoord + col, Ycoord + row].IsBomb)
                        {
                            Cells[Xcoord + col, Ycoord + row].IsVisited = true;
                            score = score + 100;
                        }
                        else
                        {
                            Cells[Xcoord + col, Ycoord + row].IsFlagged = true;
                            score = score + 2;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// use after game is over to calculate final score 
        /// </summary>
        /// <param name="gameState"></param>
        /// <returns></returns>
        public int DetermineFinalScore()
        {
            int finalScore = score;
            int status = DetermineGameState();
            TimeSpan gameTime = EndTime - StartTime;
            // SUBTRACT 20 POINTS FOR EACH SECOND
            finalScore = finalScore - (Convert.ToInt32(gameTime.TotalSeconds) * 20);

            // if the user takes exceedingly long, we just give them
            // a score of zero
            if (finalScore < 0)
            {
                finalScore = 0;
            }
            if (status == -1)
            {
                finalScore = 0;
            }

            return finalScore;
        }

        /// <summary>
        /// update the score based on how many bomb neighbors there are
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void UpdateScore(int x, int y)
        {
            score = score + 100 + Cells[x, y].NumberOfBombNeighbors * 50;
        }

        /// <summary>
        /// helper function to determine if a cell is out of bounds
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        public bool IsCellOnBoard(int row, int col)
        {
            if (row < 0 || row >= Size || col < 0 || col >= Size)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// use during setup to calculate the number of bomb neighbors for each cell
        /// </summary>
        private void CalculateNumberOfBombNeighbors()
        {
            for (int col = 0; col < Size; col++)
            {
                for (int row = 0; row < Size; row++)
                {
                    Cells[col, row].NumberOfBombNeighbors = GetNumberOfBombNeighbors(col, row);
                }
            }

        }

        /// <summary>
        /// gets the number of bomb neighbors for a given coordinate
        /// </summary>
        /// <param name="xCoord"></param>
        /// <param name="yCoord"></param>
        /// <returns>int</returns>
        private int GetNumberOfBombNeighbors(int xCoord, int yCoord)
        {
            int numberOfBombs = 0;

            if (IsCellOnBoard(xCoord - 1, yCoord - 1) && Cells[xCoord - 1, yCoord - 1].IsBomb)
            {
                numberOfBombs++;
            }
            if (IsCellOnBoard(xCoord + 1, yCoord - 1) && Cells[xCoord + 1, yCoord - 1].IsBomb)
            {
                numberOfBombs++;
            }
            if (IsCellOnBoard(xCoord - 1, yCoord + 1) && Cells[xCoord - 1, yCoord + 1].IsBomb)
            {
                numberOfBombs++;
            }

            if (IsCellOnBoard(xCoord + 1, yCoord + 1) && Cells[xCoord + 1, yCoord + 1].IsBomb)
            {
                numberOfBombs++;
            }
            if (IsCellOnBoard(xCoord, yCoord - 1) && Cells[xCoord, yCoord - 1].IsBomb)
            {
                numberOfBombs++;
            }
            if (IsCellOnBoard(xCoord, yCoord + 1) && Cells[xCoord, yCoord + 1].IsBomb)
            {
                numberOfBombs++;
            }
            if (IsCellOnBoard(xCoord + 1, yCoord) && Cells[xCoord + 1, yCoord].IsBomb)
            {
                numberOfBombs++;
            }
            if (IsCellOnBoard(xCoord - 1, yCoord) && Cells[xCoord - 1, yCoord].IsBomb)
            {
                numberOfBombs++;
            }

            return numberOfBombs;
        }

        /// <summary>
        /// use during setup to place bombs on the board
        /// </summary>
        private void SetupBombs()
        {
            int numberOfBombs = (int)(Size * (float)Size * PercentBombs);

            while (numberOfBombs > 0)
            {
                // get a random coordinate on the board
                int xCoord = random.Next(0, Size - 1);
                int yCoord = random.Next(0, Size - 1);

                if (!Cells[xCoord, yCoord].IsBomb)
                {
                    Cells[xCoord, yCoord].IsBomb = true;
                    numberOfBombs--;
                }
                else
                {
                    continue;
                }
            }
        }

        // use during setup to place rewards on the board
        private void SetupRewards()
        {
            while (RewardsRemaining > 0)
            {
                // Get random coord on board
                int xCoord = random.Next(0, Size - 1);
                int yCoord = random.Next(0, Size - 1);

                if (!(Cells[xCoord, yCoord].HasSpecialReward && !Cells[xCoord, yCoord].IsBomb))
                {
                    Cells[xCoord, yCoord].HasSpecialReward = true;
                    RewardsRemaining--;
                }
                else
                {
                    continue;
                }
            }
        }

        /// <summary>
        /// loops through the entirety of the board and determines the current game state
        /// </summary>
        /// <returns>GameStatus</returns>
        public int DetermineGameState()
        {
            // loop through game board
            bool isWon = true;
            for (int row = 0; row < Size; row++)
            {
                for (int col = 0; col < Size; col++)
                {
                    // check if there are any non bomb cells that haven't been visited left
                    if (!Cells[col, row].IsBomb && !Cells[col, row].IsVisited)
                    {
                        isWon = false;
                    }
                    // check if there are any bomb cells that have been visited
                    if (Cells[col, row].IsBomb && Cells[col, row].IsVisited)
                    {
                        return -1;
                    }
                }
            }
            // return the proper game state based on booleans
            if (isWon)
            {
                EndTime = DateTime.Now;
                return 1;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// flood fill that recursively uncovers cells with
        /// no bomb neighbors
        /// it does uncovers the first layer of cell
        /// with bomb neighbors though
        /// </summary>
        /// <param name="col"></param>
        /// <param name="row"></param>
        public void FloodFill(int col, int row)
        {
            Console.ForegroundColor = ConsoleColor.White;

            // check if ur cell is on the board
            if (!IsCellOnBoard(col, row))
            {
                return;
            }

            if (Cells[col, row].HasSpecialReward && !Cells[col, row].IsVisited)
            {
                collectedRewards++;
            }

            // check if le cell is alredy filled
            if (Cells[col, row].IsVisited || Cells[col, row].NumberOfBombNeighbors > 0)
            {
                Cells[col, row].IsVisited = true;
                return;
            }

            Cells[col, row].IsVisited = true;

            // iterate through each adjacent cell
            for (int xCoord = -1; xCoord < 2; xCoord++)
            {
                for (int yCoord = -1; yCoord < 2; yCoord++)
                {
                    FloodFill(col + xCoord, row + yCoord);
                }
            }
        }
    }
}