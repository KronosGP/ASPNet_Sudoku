using System;
using System.Collections.Generic;
using System.Linq;

namespace ASPNet_Sudoku.Controllers
{
    internal class SudokuGenerator
    {
        private int[,] board;

        public int[,] GenerateSudoku(int difficulty)
        {
            board = new int[9, 9];
            SolveSudoku();
            RemoveCells(difficulty);
            return board;
        }

        private bool SolveSudoku()
        {
            for (int row = 0; row < 9; row++)
            {
                for (int col = 0; col < 9; col++)
                {
                    if (board[row, col] == 0)
                    {
                        for (int num = 1; num <= 9; num++)
                        {
                            if (IsSafe(row, col, num))
                            {
                                board[row, col] = num;

                                if (SolveSudoku())
                                {
                                    return true;
                                }
                                else
                                {
                                    board[row, col] = 0;
                                }
                            }
                        }
                        return false;
                    }
                }
            }
            return true;
        }

        private bool IsSafe(int row, int col, int num)
        {
            return !UsedInRow(row, num) &&
                   !UsedInCol(col, num) &&
                   !UsedInBox(row - row % 3, col - col % 3, num);
        }

        private bool UsedInRow(int row, int num)
        {
            for (int col = 0; col < 9; col++)
            {
                if (board[row, col] == num)
                {
                    return true;
                }
            }
            return false;
        }

        private bool UsedInCol(int col, int num)
        {
            for (int row = 0; row < 9; row++)
            {
                if (board[row, col] == num)
                {
                    return true;
                }
            }
            return false;
        }

        private bool UsedInBox(int boxStartRow, int boxStartCol, int num)
        {
            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    if (board[row + boxStartRow, col + boxStartCol] == num)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private void RemoveCells(int difficulty)
        {
            Random rand = new Random();
            List<int> cells = Enumerable.Range(0, 81).ToList();

            while (cells.Count > difficulty)
            {
                int randomIndex = rand.Next(cells.Count);
                int cellIndex = cells[randomIndex];
                int row = cellIndex / 9;
                int col = cellIndex % 9;
                board[row, col] = 0;
                cells.RemoveAt(randomIndex);
            }
        }
    }
}