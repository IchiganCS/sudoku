public class Solver
{
    private Board Board { get; init; }
    private List<int>[,] Valids { get; set; }

    /// <summary>
    /// Strike out valids with the most simple patterns (row, column and square) using information from the board at row, col
    /// </summary>
    /// <param name="row"></param>
    /// <param name="col"></param>
    private void StrikeValids(int row, int col)
    {
        int digit = Board.Digits[row, col];
        if (digit == -1)
            throw new ArgumentException($"the board doesn't have digit at {row}, {col}");

        for (int i = 0; i < 9; i++)
        {
            Valids[row, i].Remove(digit);
            Valids[i, col].Remove(digit);
        }

        foreach (var (x, y) in Board.GetSquareFields(row, col))
            Valids[x, y].Remove(digit);

        Valids[row, col] = new int[] { digit }.ToList();
    }

    public Solver(Board board)
    {
        Board = board;

        Valids = new List<int>[9, 9];

        for (int i = 0; i < 9; i++)
            for (int j = 0; j < 9; j++)
                Valids[i, j] = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 }.ToList();

        for (int i = 0; i < 9; i++)
            for (int j = 0; j < 9; j++)
                if (board.Digits[i, j] != -1)
                    StrikeValids(i, j);
    }

    /// <summary>
    /// looks for a row, column or square where a digit just has one position left and adjust Valids
    /// may need to be executed multiple times
    /// </summary>
    /// <returns>whether valids has been changed</returns>
    public bool LookForOnePlaceLeft()
    {
        bool hasChanged = false;
        Action<(int, int)[]> lookFor = squares =>
        {
            //if a digit is found, set index=digit to found square. if another is found, set to -2.
            //at the end, foundDigits contains certains
            int[] foundDigits = new int[9] { -1, -1, -1, -1, -1, -1, -1, -1, -1 };

            foreach (var ((row, col), index) in squares.Zip(Enumerable.Range(0, 9)))
            {
                Valids[row, col].ForEach(x =>
                {
                    if (foundDigits[x - 1] == -1)
                        foundDigits[x - 1] = index;
                    else if (foundDigits[x - 1] != -2)
                        foundDigits[x - 1] = -2;
                });
            }

            for (int i = 0; i < foundDigits.Length; i++)
                if (foundDigits[i] >= 0) {
                    hasChanged = true;
                    var (r, c) = squares[foundDigits[i]];
                    Valids[r, c].Clear();
                    Valids[r, c].Add(i + 1);
                }
        };


        foreach (int digit in Board.GetAllDigits())
        {
            foreach (int pos in Board.GetAllDigits())
            {
                lookFor(Board.GetRowFields(pos).ToArray());
                lookFor(Board.GetColumnFields(pos).ToArray());
                lookFor(Board.GetSquareFields(pos).ToArray());
            }
        }

        return hasChanged;
    }

    public IEnumerable<(int, int)> GetCertains()
    {
        return Board.GetAllFields().Where(tup => Board.Digits[tup.Item1, tup.Item2] == -1 && Valids[tup.Item1, tup.Item2].Count == 1);
    }

    public bool WriteToBoard() {
        bool hasChanged = false;
        foreach (var (r, c) in GetCertains()) {
            hasChanged = true;
            Board.Digits[r, c] = Valids[r, c][0];
        }
        return hasChanged;
    }
}