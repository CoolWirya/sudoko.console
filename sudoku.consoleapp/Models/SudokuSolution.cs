using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sudoku.consoleapp.Models;

public class SudokuBoard
{
    public string Difficulty { get; set; }
    public int[][] Solution { get; set; }
}
