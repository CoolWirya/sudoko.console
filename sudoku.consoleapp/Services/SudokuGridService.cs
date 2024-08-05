using sudoku.consoleapp.Models;
using sudoku.consoleapp.Utilities;


namespace sudoku.consoleapp.Services
{
    public class SudokuGridService2
    {
        public int MyProperty { get; set; }
        private static Dictionary<SudokuAxis, SudokuItem> _sudoko = [];
        public static void Generate(int squares)
        {
            
            for (int x = 0; x < squares; x++)
            {
                for (int y = 0; y < squares; y++)
                {
                    _sudoko.Add(new (x,y), new ()
                    {
                        SquareNo = CalculateSquareNumber(x, y, squares),
                        CellValue = 0
                    });
                }
            }
        }
        public static void ShowValues(int boxes)
        {
            int sqr = boxes / (int)Math.Sqrt(boxes);
            Console.WriteLine("Values:");
            foreach (var sudoko in _sudoko)
            {
                if (sudoko.Key.Y == 0)
                {
                    Console.WriteLine();
                }
                Display.DisplayGridItem(sudoko.Key.X, sudoko.Key.Y, sudoko.Value.CellValue, sqr);
            }
        }
        public static void ShowDesign(int boxes)
        {
            int sqr = boxes / (int)Math.Sqrt(boxes);
            Console.WriteLine("Design:");
            foreach (var sudoko in _sudoko)
            {
                if (sudoko.Key.Y == 0)
                {
                    Console.WriteLine();
                }
                Display.DisplayGridItem(sudoko.Key.X, sudoko.Key.Y, sudoko.Value.SquareNo, sqr, true);
            }
        }
        public static void PopulateFirstSqrRootRows(int input)
        {
            for (int x = 0; x < input; x++)
            {
                if (x == Math.Sqrt(input))
                {
                    break;
                }
                for (int y = 0; y < input; y++)
                {
                    var key = new SudokuAxis (x, y);
                    
                    if (x == 0)
                    {
                      UpdateSudokuItem(key, y + 1);
                    }
                    else
                    {
                     CalculateAboveItemPlusSqrRootValue(key, input);
                    }

                }
            }
        }
        private static void UpdateSudokuItem(SudokuAxis key, int value)
        {
            _sudoko.TryGetValue(key, out var sudokuItem);
            sudokuItem.CellValue = value;
            _sudoko[key] = sudokuItem;
        }
        /*
        (3,0) = (0,2) => x - 3, y + 2
        (3,1) = (0,0) => x - 3, y - 1
         */
        public static void PopulateRemainingRows( int input)
        {
            
            foreach (var sqrNumber in _sudoko.Values.Select(n => n.SquareNo).ToList())
            {
                if (sqrNumber < Math.Sqrt(input))
                {
                    continue;
                }
                var items = _sudoko.Where(n=>n.Value.SquareNo == sqrNumber).ToList();
                int i = 0;


                foreach (var sudokoItem in items)
                {
                    if (i == Math.Sqrt(input))
                    {
                        i = 0;
                    }
                    if (i == 0)
                    {
                        _sudoko.TryGetValue( new(
                            sudokoItem.Key.X - (int)Math.Sqrt(input),
                            sudokoItem.Key.Y + (int)Math.Sqrt(input) - 1
                            ), out var item);
                        sudokoItem.Value.CellValue = item.CellValue;
                        _sudoko[sudokoItem.Key] = sudokoItem.Value;
                        i++;
                    }
                    else
                    {
                        _sudoko.TryGetValue(new(
                            sudokoItem.Key.X - (int)Math.Sqrt(input),
                            sudokoItem.Key.Y - 1
                            ), out var item);
                        sudokoItem.Value.CellValue = item.CellValue;
                        _sudoko[sudokoItem.Key] = sudokoItem.Value;
                        i++;
                    }
                }
            }
        }
        private static void CalculateAboveItemPlusSqrRootValue(SudokuAxis key, int squares)
        {
            _sudoko.TryGetValue(new(key.X - 1, key.Y), out var itemAbove);
            _sudoko.TryGetValue(key, out var item);
            item.CellValue = itemAbove.CellValue + (int)Math.Sqrt(squares) > squares 
                ? itemAbove.CellValue + (int)Math.Sqrt(squares) - squares
                : itemAbove.CellValue + (int)Math.Sqrt(squares);
            _sudoko[key] = item;
            
        }
        private static int CalculateSquareNumber(int x, int y, int input)
         => (int)(x / Math.Sqrt(input)) * (int)Math.Sqrt(input) + (int)(y / Math.Sqrt(input));
    }
}
