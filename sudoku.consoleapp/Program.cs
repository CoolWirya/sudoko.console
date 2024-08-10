
using Newtonsoft.Json;
using sudoku.consoleapp.Models;
using sudoku.consoleapp.Services;
using sudoku.consoleapp.Utilities;



///* Stress test */

int retries = 0;
int success = 0;
List<double> avgSolvedPercentageList = [];
SudokuLevel dificulty = SudokuLevel.easy;

while (true)
{
    // Get data from sudoku api
    Console.WriteLine("Getting the puzzle ...");
    Display.WriteGreen("It may take time to load\n");

    var sudoku = await HttpService.GetSudokuPuzzle(dificulty);
    if (sudoku == null)
    {
        Console.WriteLine("No response from the api, double check your internet");
        break;
    }

    // Try to solve the puzzle
    retries++;
    Display.ShowRawData("Puzzle Input", $"{JsonConvert.SerializeObject(sudoku.Board)}");
    SudokuGridService.GenerateSudoku(sudoku.Board, "[Puzzle]:");
    (bool isSuccess, double SolvedSuccessRate, string AttemptOutput) = SudokuGridService.SolveSudoku();
    Display.ShowRawData("Attempt Output", AttemptOutput);

    // Get solution from sudoku api
    Console.WriteLine("\nGetting solution to the puzzle ...");
    Display.WriteGreen("It may take time to load\n");
    var result = await HttpService.GetSudokuSolution(sudoku.Board);
    if (result == null)
    {
        Console.WriteLine("No response from the api, double check your internet");
        break;
    }
    SudokuGridService.GenerateSudoku(result.Solution, $"[Solution]: Level {result.Difficulty}");
    Display.ShowRawData("Solution Output", JsonConvert.SerializeObject(result.Solution));

    // Show stats
    if (isSuccess)
    {
        success++;
    }
    double successRatePercentage = ((double)success / (double)retries) * 100;
    avgSolvedPercentageList.Add(SolvedSuccessRate);
    Display.ShowStats(isSuccess, successRatePercentage, retries, SolvedSuccessRate, avgSolvedPercentageList);

    // Retry
    Display.WriteYellow("\n\n\nPress Enter to get a new puzzle");
    Console.ReadLine();
}










/* Manual Solver */

//int[][] data = [[0, 0, 0, 0, 0, 1, 9, 0, 5], [0, 0, 4, 0, 0, 0, 0, 7, 0], [0, 0, 0, 2, 0, 7, 0, 0, 4], [2, 1, 0, 0, 7, 0, 8, 0, 0], [4, 5, 7, 0, 0, 8, 0, 1, 0], [6, 0, 0, 0, 2, 3, 4, 5, 0], [3, 0, 0, 5, 0, 2, 7, 0, 6], [0, 7, 0, 9, 3, 0, 5, 0, 1], [0, 6, 0, 0, 0, 0, 0, 0, 2]];
//SudokuGridService.GenerateSudoku(data, "Puzzle:");
//(bool isSuccess, double SolvedSuccessRate, string AttemptOutput)  = SudokuGridService.SolveSudoku();
//Console.WriteLine("\nGetting solution to the puzzle ...");
//Display.WriteGreen("It may take time to load\n");
//var result = await HttpService.GetSudokuSolution(data);
//SudokuGridService.GenerateSudoku(result.Solution, $"Solution: Level {result.Difficulty}");









