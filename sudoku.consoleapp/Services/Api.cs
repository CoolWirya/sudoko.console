using sudoku.consoleapp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace sudoku.consoleapp.Services;


public class Api
{
    public static async Task<SudokuBoard> GetSudokuBoard(SudokuLevel level)
    {
        try
        {
            using HttpClient client = new();
            return await client.GetFromJsonAsync<SudokuBoard>($"https://sugoku.onrender.com/board?difficulty={level}");
        }
        catch {}
        return null;
    }
}
