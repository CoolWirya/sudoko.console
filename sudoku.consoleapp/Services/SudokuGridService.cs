using Newtonsoft.Json;
using sudoku.consoleapp.Models;
using sudoku.consoleapp.Utilities;



namespace sudoku.consoleapp.Services
{
    public class SudokuGridService
    {
        private static Dictionary<SudokuAxis, SudokuItem> _sudoku = [];
        private static List<Dictionary<SudokuAxis, SudokuItem>> _snapshots = [];
        private static int _counter = 0;
        public static void GenerateSudoku(int[][] rows, string message)
        {
            _sudoku = [];
            int x = 0;
            foreach (var row in rows)
            {
                for (int y = 0; y < row.Length; y++)
                {
                    _sudoku.Add(new(x, y), new()
                    {
                        SquareNo = (int)(x / Math.Sqrt(row.Length)) * (int)Math.Sqrt(row.Length) + (int)(y / Math.Sqrt(row.Length)),
                        CellValue = row[y],
                        FromInput = (row[y] != 0)
                    });
                }
                x++;
            }
            Display.ShowSudokuSquare(_sudoku, message);
        }
        public static (bool IsSuccess, double solvedCellsPercentage, string Solution) SolveSudoku()
        {
            bool nakedPairTried = false;
            do
            {
                _counter = 0;
                CheckNewPossibilities();
                var emptyCells = _sudoku.Where(n => n.Value.CellValue == 0).ToList();

                foreach (var empty in emptyCells)
                {
                    CheckByComparingPossibilities(empty);
                }
                if (_counter == 0 && !nakedPairTried)
                {
                    SolvedTwoNakedTwinsRandomlyAndCreateSnapshot();
                    nakedPairTried = true;
                    _counter++;
                }
            } while (_counter != 0);
           
            Display.ShowSudokuSquare(_sudoku, "[Attempt]:");
            Display.CheckIfFailedToCompleteThePuzzle(_sudoku);
            return (HasNoDuplicatesOrEmptyItems(), GetSudokuICellsSolvedPercentage(), ConvertArrayToTwoDimentionalArray());
        }
        private static bool CheckByComparingPossibilities(KeyValuePair<SudokuAxis, SudokuItem> sudokuItem)
        {
            var otherEmptyCellsPossibilities = _sudoku
                .Where(n => n.Key != sudokuItem.Key && n.Value.SquareNo == sudokuItem.Value.SquareNo && n.Value.CellValue == 0)
                .SelectMany(n => n.Value.PossibleValues).Distinct();
           
            var remainingPossibility = sudokuItem.Value.PossibleValues.Except(otherEmptyCellsPossibilities);
            if (remainingPossibility.Count() == 1)
            {
                sudokuItem.Value.CellValue = (int)remainingPossibility.FirstOrDefault();
                if(!HasNoDuplicates())
                {
                    sudokuItem.Value.CellValue = 0;
                    return false;
                }
                CheckNewPossibilities();
                _counter++;
                return true;
            }
            return false;
        }
        public static void SolvedTwoNakedTwinsRandomlyAndCreateSnapshot()
        {
            Display.ShowSudokuSquare(_sudoku, "[Attempt]:");
            var twoPossibilitiesList = _sudoku.Where(n => n.Value.CellValue == 0 && n.Value.PossibleValues?.Count == 2).ToList();
            if (twoPossibilitiesList is null)
            {
                return;
            }
            foreach (var cell in twoPossibilitiesList)
            {
                var twins = twoPossibilitiesList.Where(n => n.Key != cell.Key && n.Value.CellValue == 0
                && n.Value.SquareNo == cell.Value.SquareNo && (n.Key.X == cell.Key.X || n.Key.Y == cell.Key.Y)).ToList();
                if (twins is null || twins.Count == 0 || twins.Count > 1)
                {
                    continue;
                }
                var twin = twins.FirstOrDefault();
                HashSet<int> possibleValuesForCell = new HashSet<int>(cell.Value.PossibleValues);
                HashSet<int> possibleValuesForTwin = new HashSet<int>(twin.Value.PossibleValues);
                if(!possibleValuesForCell.SetEquals(possibleValuesForTwin))
                {
                  continue ;
                }
                cell.Value.CellValue = cell.Value.PossibleValues[0];
                twin.Value.CellValue = cell.Value.PossibleValues[1];
                
                if (!HasNoDuplicates())
                {

                    continue;
                }
                CheckNewPossibilities();

                // Snapshot to be captured here
            }
        }
        private static List<int> FindMissingPossibilities(List<int> list)
        {
            List<int> possibleValues = [1, 2, 3, 4, 5, 6, 7, 8, 9];
            return possibleValues.Except(list).ToList();
        }
        private static bool HasNoDuplicatesOrEmptyItems()
        {
            if ( _sudoku.Where(n => n.Value.CellValue == 0).Any())
            {
                return false;
            }
            foreach (var sudokuItem in _sudoku)
            {
                if (_sudoku.Where(n => n.Key.Y == sudokuItem.Key.Y && n.Value.CellValue != 0).
                Select(n => n.Value.CellValue).Distinct().Count() != 9)
                {
                    return false;
                }
                if (_sudoku.Where(n => n.Key.X == sudokuItem.Key.X && n.Value.CellValue != 0)
                .Select(n => n.Value.CellValue).Distinct().Count() != 9)
                {
                    return false;
                }
                if (_sudoku.Where(n => n.Value.SquareNo == sudokuItem.Value.SquareNo && n.Value.CellValue != 0)
                .Select(n => n.Value.CellValue).Distinct().Count() != 9)
                {
                    return false;
                }
            }
            return true;
        }
        private static bool HasNoDuplicates()
        {
            foreach (var sudokuItem in _sudoku)
            {
                var xValues = _sudoku.Where(n => n.Key.Y == sudokuItem.Key.Y && n.Value.CellValue != 0)
                    .Select(n => n.Value.CellValue);
                if (xValues.Distinct().Count() != xValues.Count())
                {
                    return false;
                }
                var yValues = _sudoku.Where(n => n.Key.X == sudokuItem.Key.X && n.Value.CellValue != 0)
                .Select(n => n.Value.CellValue);
                if (yValues.Distinct().Count() != yValues.Count() )
                {
                    return false;
                }
                var squareValues = _sudoku.Where(n => n.Value.SquareNo == sudokuItem.Value.SquareNo && n.Value.CellValue != 0)
                .Select(n => n.Value.CellValue);
                if (squareValues.Distinct().Count() != squareValues.Count())
                {
                    return false;
                }
            }
            return true;
        }
        private static double GetSudokuICellsSolvedPercentage()
        {
            double totalStartEmptyCells = (double)81 - _sudoku.Where(n => n.Value.FromInput).Count();
            double totalSolved = (double)_sudoku.Where(n => n.Value.CellValue != 0 && n.Value.FromInput == false).Count();
            return (totalSolved / totalStartEmptyCells) * 100;
        }
        private static string ConvertArrayToTwoDimentionalArray()
        {
            int[] sudokuValues = _sudoku.Select(n => n.Value.CellValue).ToArray();
            int chunkSize = 9;
            int[][] output = sudokuValues
            .Select((value, index) => new { value, index })
            .GroupBy(x => x.index / chunkSize)
            .Select(g => g.Select(x => x.value).ToArray())
            .ToArray();
            return JsonConvert.SerializeObject(output);
        }
        private static void CheckNewPossibilities()
        {
            foreach (var sudokuItem in _sudoku.Where(n => n.Value.CellValue == 0))
            {
                var xValues = _sudoku.Where(n => n.Key.Y == sudokuItem.Key.Y && n.Value.CellValue != 0).Select(n => n.Value.CellValue).ToList();
                var yValues = _sudoku.Where(n => n.Key.X == sudokuItem.Key.X && n.Value.CellValue != 0).Select(n => n.Value.CellValue).ToList();
                var sValues = _sudoku.Where(n => n.Value.SquareNo == sudokuItem.Value.SquareNo && n.Value.CellValue != 0).Select(n => n.Value.CellValue).ToList();
                var allNumbers = xValues.Concat(yValues).Concat(sValues).Distinct().ToList();
                sudokuItem.Value.PossibleValues = FindMissingPossibilities(allNumbers);
            }
            var onePossibility = _sudoku.Where(n => n.Value.PossibleValues?.Count == 1 && n.Value.CellValue == 0);
            foreach (var sudokuItem in onePossibility)
            {
                sudokuItem.Value.CellValue = sudokuItem.Value.PossibleValues[0];
                _counter++;
            }
        }
    }
}
