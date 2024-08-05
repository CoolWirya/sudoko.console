

namespace sudoku.consoleapp.Models
{
    public class SudokuItem
    {
        public int SquareNo { get; set; }
        public int CellValue { get; set; }
        public List<int> PossibleValues { get; set; }
    }
}
