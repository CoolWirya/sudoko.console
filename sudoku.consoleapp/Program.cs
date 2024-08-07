
using Newtonsoft.Json;
using sudoku.consoleapp.Models;
using sudoku.consoleapp.Services;
using sudoku.consoleapp.Utilities;



/* Stress test */

int retries = 0;
int success = 0;
while (true)
{
    SudokuLevel dificulty = SudokuLevel.easy;
    bool isSuccess = false;
    string attemptOutput = string.Empty;
    string solutionOutput = string.Empty;
    Console.WriteLine("Getting the puzzle ...");
    Display.WriteGreen("It may take time to load\n");
    var sudoku = await HttpService.GetSudokuPuzzle(dificulty);
    if (sudoku is not null)
    {
        retries++;
        Display.WriteYellow("\nPuzzle Input");
        Display.WriteRed(" -> ");
        Console.WriteLine($"{JsonConvert.SerializeObject(sudoku.Board)}");
        SudokuGridService.GenerateSudoku(sudoku.Board,"Puzzle");
        (isSuccess, attemptOutput) = SudokuGridService.SolveSudoku();
        Display.WriteYellow("\nAttempt Output");
        Display.WriteRed(" -> ");
        Console.WriteLine(attemptOutput);
        Console.WriteLine("\nGetting solution to the puzzle ...");
        Display.WriteGreen("It may take time to load\n");
        var result = await HttpService.GetSudokuSolution(sudoku.Board);
        if (result is not null)
        {
            SudokuGridService.GenerateSudoku(result.Solution, $"Solution: Level {result.Difficulty}");
            solutionOutput = JsonConvert.SerializeObject(result.Solution);
        }
        else
        {
            Console.WriteLine("No response from the api");
        }
        
    }
    else
    {
        Console.WriteLine("No response from the api");
    }
    if (isSuccess)
    {
        success++;
    }
    Display.WriteYellow("\nSolution Output");
    Display.WriteRed(" -> ");
    Console.WriteLine(solutionOutput);
    Display.WriteGreen("\nSuccess Rate:");
    Display.WriteYellow($" %{(success / retries) * 100:#0.00}");
    Display.WriteYellow("\n\n\nPress Enter to get a new puzzle");
    Console.ReadLine();
}


/* Manual Solver */

//int[][] data = [[0, 0, 0, 0, 6, 0, 4, 0, 8], [0, 3, 0, 0, 5, 8, 6, 0, 9], [5, 0, 8, 0, 7, 0, 1, 0, 0], [0, 0, 0, 0, 0, 6, 8, 9, 0], [4, 0, 0, 0, 0, 0, 0, 3, 0], [7, 0, 9, 0, 1, 2, 5, 0, 0], [0, 2, 1, 7, 0, 4, 9, 0, 5], [8, 0, 0, 0, 0, 1, 3, 6, 4], [0, 4, 0, 6, 0, 5, 0, 0, 2]];
////int[][] data = [[0, 0, 0, 9, 0, 5, 0, 0, 0], [0, 3, 0, 1, 0, 0, 0, 0, 0], [0, 0, 0, 2, 0, 0, 1, 0, 0], [1, 0, 0, 4, 0, 6, 0, 0, 0], [4, 5, 6, 7, 8, 0, 0, 0, 3], [0, 9, 0, 3, 1, 2, 5, 4, 0], [3, 0, 0, 0, 9, 1, 6, 0, 7], [5, 6, 0, 8, 7, 0, 9, 0, 4], [0, 7, 8, 0, 0, 0, 0, 5, 1]];
//SudokuGridService.GenerateSudoku(data,"Puzzle:");
//SudokuGridService.SolveSudoku();
//Console.WriteLine("\nGetting solution to the puzzle ...");
//Display.WriteGreen("It may take time to load\n");
//var result = await HttpService.GetSudokuSolution(data);
//SudokuGridService.GenerateSudoku(result.Solution, $"Solution: Level {result.Difficulty}");









