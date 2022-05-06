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


        exampleBoard = new(
            new int[9, 9] {
                {-1, -1, 6, 5, -1, 9, -1, -1, 3},
                {4, -1, -1, -1, -1, -1, 6, -1, -1},
                {-1, -1, -1, -1, 2, 3, 8, -1, -1},
                {-1, -1, -1, -1, -1, -1, -1, -1, 7},
                {-1, -1, -1, -1, -1, -1, 2, 1, -1},
                {-1, -1, 9, 8, -1, -1, 5, -1, -1},
                {2, -1, 8, -1, 1, -1, -1, -1, -1},
                {-1, -1, 7, -1, -1, -1, 9, -1, 4},
                {-1, -1, -1, -1, -1, -1, -1, -1, -1}
        });

        //has unique solution
        exampleBoard = new(
            new int[9, 9] {
                {-1, 5, -1, -1, -1, 9, -1, -1, -1},
                {1, -1, -1, -1, -1, -1, -1, -1, -1},
                {3, -1, 7, 2, -1, 8, -1, 4, -1},
                {-1, 8, -1, -1, 3, -1, -1, 9, 6},
                {-1, -1, -1, 4 , -1, -1, 8, -1, -1},
                {-1, -1, -1, -1, -1, -1, -1, -1, 4},
                {-1, -1, 3, -1, 9, 7, -1, 1, 8},
                {6, -1, 1, -1, -1, -1, -1, 3, -1},
                {-1, 2, 8, -1, 1, 3, -1, -1, -1},
        });

        Console.WriteLine(exampleBoard);

        Solver sol = new(exampleBoard);
        for (int i = 0; i < 15; i++)
        {
            sol.LookForOnePlaceLeft();
            sol.WriteToBoard();
            Console.WriteLine(exampleBoard);
            sol.CrossCompoundReduction();
            sol.WriteToBoard();
            Console.WriteLine(exampleBoard);
            sol.HiddenDigitsReduction();
            sol.WriteToBoard();
            Console.WriteLine(exampleBoard);
        };
        sol.WriteToBoard();
        Console.WriteLine(exampleBoard);

    }
}