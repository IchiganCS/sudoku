using System;

public class Sudoku 
{
    public static void Main(string[] args) 
    {
        //board 63 in SUPERSCHWER
        Board exampleBoard = new(
             new int[9, 9] {
                {9, -1, 8, -1, -1, 1, -1, 4, -1},
                {-1, -1, 5, 8, -1, 4, -1, -1, -1},
                {-1, -1, -1, -1, -1, 7, -1, 6, -1},
                {-1, -1, -1, 7, -1, -1, -1, -1, 3},
                {-1, 6, -1, 9, -1, -1, -1, -1, 1},
                {5, 2, 7, 6, -1, -1, -1, -1, 9},
                {-1, -1, -1, -1, 5, -1, -1, -1, -1},
                {-1, -1, 6, -1, -1, -1, 8, 2, -1},
                {8, 3, 2, -1, -1, -1, 9, -1, -1}
            });

        Console.WriteLine(exampleBoard);

        Solver sol = new(exampleBoard);
        sol.LookForOnePlaceLeft();
        sol.WriteToBoard();
        Console.WriteLine(exampleBoard);

    }
}