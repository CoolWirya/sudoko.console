using Newtonsoft.Json;
using sudoku.consoleapp.Models;

namespace sudoku.consoleapp.Utilities;

public class Display
{
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
    public static void WriteBlue(string str)
    {
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.Write(str);
        Console.ResetColor();
    }
    public static void ShowSudokuSquare(Dictionary<SudokuAxis, SudokuItem> data, string source)
    {
        Console.Write($"\n{source}");
        foreach (var sudoku in data)
        {
            if (sudoku.Key.Y == 0)
            {
                Console.WriteLine();
            }
            if (sudoku.Key.X == 0 && sudoku.Key.Y == 0)
            {
                Display.WriteYellow("\n*-----------------*\n");

            }
            if ((sudoku.Key.X == 3 || sudoku.Key.X == 6) && sudoku.Key.Y == 0)
            {
                Display.WriteYellow("*-----------------*");
            }
            if (sudoku.Key.Y == 0 && (sudoku.Key.X == 3 || sudoku.Key.X == 6))
            {
                Console.WriteLine();
            }
            if (sudoku.Key.Y == 0)
            {
                Display.WriteYellow("|");
            }
            Display.ShowSudokuValues(sudoku.Key.X, sudoku.Key.Y, sudoku.Value.CellValue, sudoku.Value.FromInput);

            if (sudoku.Key.Y == 2 || sudoku.Key.Y == 5 || sudoku.Key.Y == 8)
            {
                Display.WriteYellow("|");
            }
            else
            {
                Console.Write(" ");
            }
        }
        Display.WriteYellow("\n*-----------------*");
        Console.WriteLine();
    }
    public static void ShowSudokuValues(int x, int y, int value, bool fromInput)
    {

        if (value == 0)
        {
            WriteRed($"{value}");
        }
        else if (fromInput)
        {
            Console.Write($"{value}");
        }
        else
        {
            WriteGreen($"{value}");
        }
    }
    public static void ShowRawData(string from, string data)
    {
        WriteYellow($"\n{from}");
        WriteRed(" -> ");
        Console.WriteLine(data);
    }
    public static void CheckIfFailedToCompleteThePuzzle(Dictionary<SudokuAxis, SudokuItem> sudoku)
    {
        if (sudoku.Where(n => n.Value.CellValue == 0).Count() > 0)
        {
            Display.WriteYellow("\n\n\nPossible values:\n");
            foreach (var sudokuItem in sudoku.Where(n => n.Value.CellValue == 0))
            {

                Console.WriteLine($"({sudokuItem.Key.X},{sudokuItem.Key.Y}): {JsonConvert.SerializeObject(sudokuItem.Value.PossibleValues)}");
            }
        }
    }
    public static void ShowStats(bool isSuccess, double success, int retries, double solvedSuccessRate, List<double> solvedPercentagesList)
    {
        Console.WriteLine("\n\n[Puzzle stats]:");
        WriteBlue("\nNo Duplicates:");
        if (isSuccess)
        {
            WriteYellow($" {isSuccess}");
        }
        else
        {
            WriteRed($" {isSuccess}");
        }
        WriteBlue("\nPuzzle Solved Rate:");
        WriteYellow($" {solvedSuccessRate:#0.00}% ");
        WriteBlue("\nAverage Success Rate:");
        WriteYellow($" {success:#0.00}%");
        WriteBlue("\nAverage Solved  Rate:");
        WriteYellow($" {(solvedPercentagesList.Sum() / solvedPercentagesList.Count):#0.00}% ");
    }
}

