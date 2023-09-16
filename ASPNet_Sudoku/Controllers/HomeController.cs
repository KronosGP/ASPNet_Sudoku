using System;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace ASPNet_Sudoku.Controllers
{
    public class HomeController : Controller
    {
        private int [,] solvedSudokuBoard;
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult GenerateSudoku()
        {
            if (Request.Form.TryGetValue("difficulty", out StringValues difficultyValue) &&
                int.TryParse(difficultyValue, out int difficulty))
            {
                int[,] sudokuBoard = new int[9, 9];
                if (FillSudoku(sudokuBoard) && RemoveNumbers(sudokuBoard, difficulty))
                {
                    solvedSudokuBoard = sudokuBoard;
                    if (RemoveNumbers(sudokuBoard, difficulty))
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.Append("<table border='1'>");

                        for (int row = 0; row < 9; row++)
                        {
                            sb.Append("<tr>");
                            for (int col = 0; col < 9; col++)
                            {
                                if (sudokuBoard[row, col] == 0)
                                {
                                    sb.AppendFormat("<td><input type='text' name='cell_{0}_{1}' maxlength='1' size=1 oninput=\"this.value = this.value.replace(/[^0-9]/g, '');\"/></td>", row, col);
                                }
                                else
                                {
                                    sb.AppendFormat("<td>{0}</td>", sudokuBoard[row, col]);
                                }
                            }
                            sb.Append("</tr>");
                        }

                        sb.Append("</table>");

                        ViewBag.SudokuTable = sb.ToString();
                    }
                }
            }

            return View("Index");
        }

        private bool FillSudoku(int[,] board)
        {
            return FillSudokuHelper(board, 0, 0);
        }

        private bool FillSudokuHelper(int[,] board, int row, int col)
        {
            if (row == 9)
                return true;

            if (col == 9)
                return FillSudokuHelper(board, row + 1, 0);

            for (int num = 1; num <= 9; num++)
            {
                if (IsSafe(board, row, col, num))
                {
                    board[row, col] = num;

                    if (FillSudokuHelper(board, row, col + 1))
                        return true;

                    board[row, col] = 0;
                }
            }

            return false;
        }

        private bool IsSafe(int[,] board, int row, int col, int num)
        {
            for (int x = 0; x < 9; x++)
            {
                if (board[row, x] == num || board[x, col] == num)
                    return false;
            }

            int startRow = row - row % 3;
            int startCol = col - col % 3;

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (board[i + startRow, j + startCol] == num)
                        return false;
                }
            }

            return true;
        }

        private bool RemoveNumbers(int[,] board, int count)
        {
            Random rand = new Random();
            int removedCount = 0;

            while (removedCount < count)
            {
                int row = rand.Next(9);
                int col = rand.Next(9);

                if (board[row, col] != 0)
                {
                    int temp = board[row, col];
                    board[row, col] = 0;

                    if (!IsUniqueSolution(board))
                    {
                        board[row, col] = temp;
                    }
                    else
                    {
                        removedCount++;
                    }
                }
            }

            return true;
        }

        private bool IsUniqueSolution(int[,] board)
        {
            // TODO: Zaimplementuj sprawdzanie unikalności rozwiązania
            return true; // Domyślnie przyjmujemy, że plansza ma unikalne rozwiązanie
        }
    }
}
