using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace ASPNet_Sudoku.Controllers
{
    public class SudokuController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
