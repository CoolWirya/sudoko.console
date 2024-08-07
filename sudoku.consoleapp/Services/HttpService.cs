using Newtonsoft.Json;
using sudoku.consoleapp.Models;
using System.Net.Http.Headers;
using System.Net.Http.Json;


namespace sudoku.consoleapp.Services;


public class HttpService
{
    public static async Task<SudokuPuzzle> GetSudokuPuzzle(SudokuLevel level)
    {
        try
        {
            using HttpClient client = new();
            return await client.GetFromJsonAsync<SudokuPuzzle>($"https://sugoku.onrender.com/board?difficulty={level}");
        }
        catch { }
        return null;
    }
    public static async Task<SudokuBoard> GetSudokuSolution(int[][] board)
    {
        try
        {

            using HttpClient client = new();
            var content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("board", JsonConvert.SerializeObject(board))
                });
            content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
            var response = await client.PostAsync("https://sugoku.onrender.com/solve", content);
            return await response.Content.ReadFromJsonAsync<SudokuBoard>();
        }
        catch { }
        return null;
    }
}
