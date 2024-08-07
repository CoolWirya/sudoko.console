using Newtonsoft.Json;
using sudoku.consoleapp.Models;
using sudoku.consoleapp.Utilities;



namespace sudoku.consoleapp.Services
{
    public class SudokuGridService
    {
        private static Dictionary<SudokuAxis, SudokuItem> _sudoku = [];
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
            Display.ShowSudokuBox(_sudoku, message);
        }
        public static (bool IsSuccwss, string Solution) SolveSudoku()
        {
            do
            {
                _counter = 0;
                var emptyCells = _sudoku.Where(n => n.Value.CellValue == 0).ToList();
                foreach (var sudokuItem in emptyCells)
                {
                    if(CheckHorizontally(sudokuItem))
                    {
                        continue;
                    }

                    if (CheckVertically(sudokuItem))
                    {
                        continue;
                    }
                    if (CheckSquare(sudokuItem))
                    {
                        continue;
                    }
                    if (CheckHorizontallyVerticallyAndTheSquare(sudokuItem))
                    {
                        continue;
                    }
                    if(CheckByComparingPossibilities(sudokuItem))
                    {
                        continue;
                    }

                    /* Notes on other algorithms to be implemented*/
                    /*
                     If two cells in the same row, column, or block (also known as a box) contain exactly the same two candidates, 
                    those two candidates cannot appear in any other cells within that same row, column, or block
                     */
                }
            } while (_counter != 0);
            Display.ShowSudokuBox(_sudoku,"Attempt:");
            CheckIfFailedToCompleteThePuzzle();
            return (CheckForDuplicatesOrEmptyCells(), 
                JsonConvert.SerializeObject(ConvertArrayToTwoDimentionalArray(_sudoku.Select(n=>n.Value.CellValue).ToArray())));
        }
       
        private static bool CheckHorizontally(KeyValuePair<SudokuAxis,SudokuItem> sudokuItem)
        {
            // Get all x-axis cells values
            var xValues = _sudoku.Where(n => n.Key.Y == sudokuItem.Key.Y && n.Value.CellValue != 0).
                Select(n => n.Value.CellValue).ToList();
            if (xValues.Count == 8)
            {
                FindTheMissingNumber(xValues, sudokuItem);
                return true;
            }
            return false;
        }
        private static bool CheckVertically(KeyValuePair<SudokuAxis, SudokuItem> sudokuItem)
        {
            // Get all y-axis cells values
            var yValues = _sudoku.Where(n => n.Key.X == sudokuItem.Key.X && n.Value.CellValue != 0)
                .Select(n => n.Value.CellValue).ToList();

            if (yValues.Count == 8)
            {
                FindTheMissingNumber(yValues, sudokuItem);
                return true;
            }
            return false ;
        }
        private static bool CheckSquare(KeyValuePair<SudokuAxis, SudokuItem> sudokuItem)
        {
            // Get cell square values
            var sValues = _sudoku.Where(n => n.Value.SquareNo == sudokuItem.Value.SquareNo && n.Value.CellValue != 0)
                .Select(n => n.Value.CellValue).ToList();
            if (sValues.Count == 8)
            {
                FindTheMissingNumber(sValues, sudokuItem);
                return true;
            };
            return false ;
        }
        private static bool CheckHorizontallyVerticallyAndTheSquare(KeyValuePair<SudokuAxis, SudokuItem> sudokuItem)
        {
            var xValues = _sudoku.Where(n => n.Key.Y == sudokuItem.Key.Y && n.Value.CellValue != 0).Select(n => n.Value.CellValue).ToList();
            var yValues = _sudoku.Where(n => n.Key.X == sudokuItem.Key.X && n.Value.CellValue != 0).Select(n => n.Value.CellValue).ToList();
            var sValues = _sudoku.Where(n => n.Value.SquareNo == sudokuItem.Value.SquareNo && n.Value.CellValue != 0).Select(n => n.Value.CellValue).ToList();
            var allNumbers = xValues.Concat(yValues).Concat(sValues).Distinct().ToList();
            if (allNumbers.Count == 8)
            {
                FindTheMissingNumber(allNumbers, sudokuItem);
                return true;
            }
            sudokuItem.Value.PossibleValues = FindMissingPossibilities(allNumbers);
            _sudoku[sudokuItem.Key] = sudokuItem.Value;
            return false ;
        }
        private static bool CheckByComparingPossibilities(KeyValuePair<SudokuAxis, SudokuItem> sudokuItem)
        {
            var otherEmptyCellsPossibilities = _sudoku
                .Where(n => n.Key != sudokuItem.Key && n.Value.SquareNo == sudokuItem.Value.SquareNo && n.Value.CellValue == 0)
                .Select(n => n.Value.PossibleValues).ToList();
            List<int> combinePossibilities = [];
            foreach (var otherPossibilities in otherEmptyCellsPossibilities)
            {
                if (otherPossibilities is not null)
                {
                    combinePossibilities = combinePossibilities.Concat(otherPossibilities).Distinct().ToList();
                }
                else
                {
                    return false;
                }
            }
            var remainingPossibility = sudokuItem.Value.PossibleValues.Except(combinePossibilities);
            if (remainingPossibility.Count() == 1)
            {
                int value = (int)remainingPossibility.FirstOrDefault();
                _sudoku[sudokuItem.Key].CellValue = value;
                _counter++;
                return true;
            }
            return false;
        }
        public static void FindTheMissingNumber(List<int> otherNumbers, KeyValuePair<SudokuAxis, SudokuItem> sudoku)
        {
            List<int> possibleValues = [1, 2, 3, 4, 5, 6, 7, 8, 9];
            int missingValue = possibleValues.Except(otherNumbers).FirstOrDefault();
            sudoku.Value.CellValue = missingValue;
            _sudoku[sudoku.Key] = sudoku.Value;
            _counter++;
        }
        private static List<int> FindMissingPossibilities(List<int> list)
        {
            List<int> possibleValues = [1, 2, 3, 4, 5, 6, 7, 8, 9];
            return possibleValues.Except(list).ToList();
        }
        private static void CheckIfFailedToCompleteThePuzzle()
        {
            if (_sudoku.Where(n => n.Value.CellValue == 0).Count() > 0)
            {
                Display.WriteYellow("\n\n\nPossible values:\n");
                foreach (var sudoku in _sudoku.Where(n => n.Value.CellValue == 0))
                {

                    Console.WriteLine($"({sudoku.Key.X},{sudoku.Key.Y}): {JsonConvert.SerializeObject(sudoku.Value.PossibleValues)}");
                }
            }
        }
        private static bool CheckForDuplicatesOrEmptyCells()
        {
            if (_sudoku.Where(n => n.Value.CellValue == 0).Any())
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
        private static int[][] ConvertArrayToTwoDimentionalArray(int[] sudokuValues)
        {
            int chunkSize = 9;
            return sudokuValues
            .Select((value, index) => new { value, index })
            .GroupBy(x => x.index / chunkSize)
            .Select(g => g.Select(x => x.value).ToArray())
            .ToArray();
        }
    }
}
