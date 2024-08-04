

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
                    SquareNo = (int)(x / Math.Sqrt(input.Max())) * (int) Math.Sqrt(input.Max())
                                + (int)(y / Math.Sqrt(input.Max())),
                    Position = new () { Y = y, X = x }
                }, 0
               );
            }
        }
        return _sudoko;
    }
    public static void ShowValues()
    {
        string output = string.Empty;
        Console.WriteLine("Values:");
        foreach (var sudoko in _sudoko)
        {
            if (sudoko.Key.Position.Y == 0)
            {
                output += "\n";
            }
            output += $"|({sudoko.Key.Position.X:#00},{sudoko.Key.Position.Y:#00}) {sudoko.Value:#00}";
        }
        Console.WriteLine(output);
    }
    public static void ShowDesign()
    {
        string output = string.Empty;
        Console.WriteLine("Design:");
        foreach (var sudoko in _sudoko)
        {
            if (sudoko.Key.Position.Y == 0)
            {
                output += "\n";
            }
            output += $"|({sudoko.Key.Position.X:#00},{sudoko.Key.Position.Y:#00}) {sudoko.Key.SquareNo:#00}";
        }
        Console.WriteLine(output);
    }









    
}
