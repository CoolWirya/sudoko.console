
using sudoku.consoleapp.Services;

int[] input = [1, 2, 3, 4, 5, 6, 7, 8, 9];
//int[] input = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16];

//SudokuLevel dificulty;
//while (true)
//{
//    Console.Write("\nDificulty:\n1.Easy\n2.Medium\n3.Hard\n4.Random\n\nChoose one:");
//    var consoleInput = Console.ReadLine();
//    _ = Enum.TryParse<SudokuLevel>(consoleInput.ToLower(), out var level);
//    if (level == SudokuLevel.none || (int)level < 0 || (int)level > 3 )
//    {
//        Display.WriteYellow("\nError ");
//        Console.Write("->");
//        Display.WriteRed(" Please enter the correct value, thanks\n");
//        continue;
//    }
//    dificulty = level;
//    break;
//}
//var board = await Api.GetSudokuBoard(dificulty);


int numberOfSquares = 9;
SudokuGridService2.Generate(numberOfSquares);
SudokuGridService2.ShowDesign(numberOfSquares);
//SudokuGridService2.ShowValues(input);
Console.WriteLine();
SudokuGridService2.PopulateFirstSqrRootRows(numberOfSquares);
SudokuGridService2.PopulateRemainingRows(numberOfSquares);
SudokuGridService2.ShowValues(numberOfSquares);

//Console.WriteLine();




/*


    (0,0) -- (0,1) -- (0,2)- ||--(0,3) -- (0,4) -- (0,5)- ||--(0,6) -- (0,7) -- (0,8)
    |   1  |   2    |   3    ||    4   |    5   |    6   ||    7    |    8    |   9   |
    (1,0) -- (1,1) -- (1,2)- ||--(1,3) -- (1,4) -- (1,5)- ||--(1,6) -- (1,7) -- (1,8)
    |   4  |   5    |   6    ||    7   |    8   |    9   ||    1    |    2    |   3   |
    (2,0) -- (2,1) -- (2,2)- ||--(2,3) -- (2,4) -- (2,5)- ||--(2,6) -- (2,7) -- (2,8)
    |   7  |   8    |   9    ||    1   |    2   |    3   ||    4    |    5    |   6   |
    (3,0) -- (3,1) -- (3,2)- ||--(3,3) -- (3,4) -- (3,5)- ||--(3,6) -- (3,7) -- (3,8)
    |   3  |   1    |   2    ||    6   |    4   |    5   ||    9    |    7    |   8   |
    (4,0) -- (4,1) -- (4,2)- ||--(4,3) -- (4,4) -- (4,5)- ||--(4,6) -- (4,7) -- (4,8)
    |   6  |   4    |   5    ||    9   |    7   |    8   ||    3    |    1    |   2   |
    (5,0) -- (5,1) -- (5,2)- ||--(5,3) -- (5,4) -- (5,5)- ||--(5,6) -- (5,7) -- (5,8)
    |   9  |   7    |   8    ||    3   |    1   |    2   ||    6    |    4    |   5   |
    (6,0) -- (6,1) -- (6,2)- ||--(6,3) -- (6,4) -- (6,5)- ||--(6,6) -- (6,7) -- (6,8)
    |   2  |   3    |   1    ||    5   |    6   |    4   ||    8    |    9    |   7   |
    (7,0) -- (7,1) -- (7,2)- ||--(7,3) -- (7,4) -- (7,5)- ||--(7,6) -- (7,7) -- (7,8)
    |   5  |   6    |   4    ||    8   |    9   |    7   ||    2    |    3    |   1   |
    (8,0) -- (8,1) -- (8,2)- ||--(8,3) -- (8,4) -- (8,5)- ||--(8,6) -- (8,7) -- (8,8)
    |   8  |   9    |   7    ||    2   |    3   |    1    ||    5    |    6    |   4   |
    -----------------------------------------------------------------------------------

    Patterns:
    first row is the list
    second row is first + 3
    third row is second + 3
    fourth row first digit is + 2 rest - 1 of the first row
    fifth       //                                second row
    sixth       //                                third row
    seventh row second digit is + 2 rest - 1 of the fourth row
    Eighth        //                              fifth row
    Nineth        //                              sixth row
     */

/*
(0,0) - (2,2)
(0,3) - (2,5)
(0,6) - (2,8)

Xmin = 0 Xmin = 2
Ymin = 0 Ymin = 2

Xmin = 0 Xmin = 2
Ymin = 3 Ymin = 5

Xmin = 0 Xmin = 2
Ymin = 6 Ymin = 8

Xmin = 3 Xmin = 5
Ymin = 0 Ymin = 2

Get Square Number Formula = Formula x + Formula y
Formula x  = (int) ( X / Root(max value)) * Root(max value)
Formula y = (int) (y / Root(max value)


*/







