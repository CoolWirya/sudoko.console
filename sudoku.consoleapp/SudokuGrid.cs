

using System.Net.NetworkInformation;

namespace sudoku.consoleapp;

public record SudokuGrid
{
    public int SquareNo { get; set; }
    public Axis Position { get; set; }

    private static  Dictionary<SudokuGrid, int> _sudoko = [];
    public static Dictionary<SudokuGrid, int> Generate(int[] input)
    {
        for (int x = 0; x < input.Length; x++)
        {
            for (int y = 0; y < input.Length; y++)
            {
                _sudoko.Add(new()
                {
                    SquareNo = CalculateSquareNumber(x,y,input),
                    Position = new () { Y = y, X = x }
                }, 0
               );
            }
        }
        return _sudoko;
    }
    public static void ShowValues(int[] input)
    {
        int sqr = input.Length / (int)Math.Sqrt(input.Length);
        Console.WriteLine("Values:");
        foreach (var sudoko in _sudoko)
        {
            if (sudoko.Key.Position.Y == 0)
            {
                Console.WriteLine();
            }
            DisplayGridItem(sudoko.Key.Position.X, sudoko.Key.Position.Y, sudoko.Value, sqr);
        }
    }
    public static void ShowDesign(int[] input)
    {
        int sqr = input.Length / (int) Math.Sqrt(input.Length);
        Console.WriteLine("Design:");
        foreach (var sudoko in _sudoko)
        {
            if (sudoko.Key.Position.Y == 0)
            {
                Console.WriteLine();
            }
           DisplayGridItem(sudoko.Key.Position.X, sudoko.Key.Position.Y, sudoko.Key.SquareNo, sqr, true);
        }
    }
    public static void PopulateFirstSqrRootRows(int[] input)
    {
        for (int x = 0; x < input.Length; x++)
        {
            if (x == Math.Sqrt(input.Length))
            {
                break;
            }
            for (int y = 0; y < input.Length; y++)
            {
                var key = new SudokuGrid()
                {
                    SquareNo = CalculateSquareNumber(x, y, input),
                    Position = new Axis() { X = x, Y = y }
                };
                if(x == 0)
                {
                    _sudoko[key] = input[y];
                }
                else
                {
                    CalculateAboveItemPlusSqrRootValue(x, y, key, input);
                }
                
            }
        }
    }
    /*
    (3,0) = (0,2) => x - 3, y + 2
    (3,1) = (0,0) => x - 3, y - 1
     */
    public static void PopulateRemainingRows(int[] input)
    {
        foreach (var sqrNumber in _sudoko.Keys.Select(n=>n.SquareNo ).ToList())
        {
            if(sqrNumber < Math.Sqrt(input.Length))
            {
                continue;
            }
            var sqrItems = _sudoko.Keys.Where(n => n.SquareNo == sqrNumber).ToArray();
            int i = 0;
            foreach (var sqrItem in sqrItems)
            {
                if(i == Math.Sqrt(input.Length))
                {
                    i = 0;
                }
                if (i == 0)
                {
                    _sudoko.TryGetValue(new()
                    {
                        SquareNo = sqrNumber - (int) Math.Sqrt(input.Length),
                        Position = new Axis()
                        {
                            X = sqrItem.Position.X - (int)Math.Sqrt(input.Length),
                            Y = sqrItem.Position.Y + (int)Math.Sqrt(input.Length) - 1
                        }
                    }, out var item);
                    _sudoko[sqrItem] = item;
                    i++;
                }
                else
                {
                    _sudoko.TryGetValue(new()
                    {
                        SquareNo = sqrNumber - (int)Math.Sqrt(input.Length),
                        Position = new Axis()
                        {
                            X = sqrItem.Position.X - (int)Math.Sqrt(input.Length),
                            Y = sqrItem.Position.Y  - 1
                        }
                    }, out var item);
                    _sudoko[sqrItem] = item;
                    i++;
                }       
            }
        }
    }
    private static void CalculateAboveItemPlusSqrRootValue(int x, int y, SudokuGrid key, int[] input)
    {
        SudokuGrid aboveItemKey = new()
        {
            SquareNo = key.SquareNo,
            Position = new Axis()
            {
                X = key.Position.X - 1,
                Y = key.Position.Y
            }
        };
        _sudoko.TryGetValue(aboveItemKey, out var value);
        _sudoko[key] = (value + (int) Math.Sqrt(input.Length) > input.Length) 
            ? value + (int) Math.Sqrt(input.Length) - input.Length 
            : value + (int)Math.Sqrt(input.Length);
    }
    private static int CalculateSquareNumber(int x, int y, int[] input)
        => (int)(x / Math.Sqrt(input.Max())) * (int)Math.Sqrt(input.Max())+ (int)(y / Math.Sqrt(input.Max()));
    private static void DisplayGridItem(int x, int y, int value, int sqr, bool isDesign = false)
    {
        Console.Write("|(");
        Display.WriteYellow(x.ToString((sqr <= 3) ? "#0" : "#00"));
        Console.Write(",");
        Display.WriteYellow(y.ToString((sqr <= 3) ? "#0" : "#00"));
        Console.Write(") ");
        if (value == 0 && !isDesign)
        {
            Display.WriteRed(value.ToString((sqr <= 3) ? "#0" : "#00"));
        }
        else if (isDesign)
        {
            Display.WriteRandomColor(value.ToString((sqr <= 3) ? "#0" : "#00"));
        }
        else
        {
            Display.WriteGreen(value.ToString((sqr <= 3) ? "#0" : "#00"));
        }

    }











}
