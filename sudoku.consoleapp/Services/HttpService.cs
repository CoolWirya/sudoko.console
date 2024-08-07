using Newtonsoft.Json;
using sudoku.consoleapp.Models;
using System.Net.Http.Headers;
using System.Net.Http.Json;


namespace sudoku.consoleapp.Services;


public class HttpService
{
    private static readonly HttpClient _client = new HttpClient();
    public static async Task<SudokuPuzzle> GetSudokuPuzzle(SudokuLevel level)
    {
        try
        {
            return await _client.GetFromJsonAsync<SudokuPuzzle>($"https://sugoku.onrender.com/board?difficulty={level}");
        }
        catch { }
        return null;
    }
    public static async Task<SudokuBoard> GetSudokuSolution(int[][] board)
    {
        try
        {

            var content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("board", JsonConvert.SerializeObject(board))
                });
            content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
            var response = await _client.PostAsync("https://sugoku.onrender.com/solve", content);
            return await response.Content.ReadFromJsonAsync<SudokuBoard>();
        }
        catch { }
        return null;
    }
}
