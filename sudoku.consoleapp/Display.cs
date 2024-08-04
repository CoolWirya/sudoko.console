

namespace sudoku.consoleapp;

public class Display
{

    private static Dictionary<int,ConsoleColor?> _colors = [];   
    public static void WriteYellow(string str)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write(str);
        Console.ResetColor();
    }
    public static void WriteRed(string str)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write(str);
        Console.ResetColor();
    }
    public static void WriteGreen(string str)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write(str);
        Console.ResetColor();
    }
    public static void WriteRandomColor(string str)
    {
        _ = int.TryParse(str, out int sqrNo);
        var color = GetColor(sqrNo);
        if (color is null)
        {
            color = GenerateRandomColor();
            _colors.Add(sqrNo, color);
        }
        Console.ForegroundColor = color ?? ConsoleColor.Yellow;
        Console.Write(str);
        Console.ResetColor();
    }
    public static ConsoleColor GenerateRandomColor()
    {
        Random ran = new ();
        return (ConsoleColor)ran.Next(1, 13);
    }
    public static ConsoleColor? GetColor(int sqrNo)
    {
        _ = _colors.TryGetValue(sqrNo, out var color);
        return color;
    }
}

