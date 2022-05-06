public class Solver
{
    private Board Board { get; init; }
    private List<int>[,] Markings { get; set; }

    /// <summary>
    /// Strike out Markings with the most simple patterns (row, column and square) using information from the board at row, col
    /// </summary>
    /// <param name="row"></param>
    /// <param name="col"></param>
    private void StrikeMarkings(int row, int col)
    {
        if (Markings[row, col].Count != 1)
            throw new ArgumentException($"there isn't exactly one digit at {row}, {col}");

        int digit = Markings[row, col][0];

        for (int i = 0; i < 9; i++)
        {
            Markings[row, i].Remove(digit);
            Markings[i, col].Remove(digit);
        }

        foreach (var (x, y) in Board.GetSquareFields(row, col))
            Markings[x, y].Remove(digit);

        //because digit has been removed
        Markings[row, col].Add(digit);
    }

    private void WriteMarking(int row, int col, int digit)
    {
        Markings[row, col].Clear();
        Markings[row, col].Add(digit);
        StrikeMarkings(row, col);
    }

    public Solver(Board board)
    {
        Board = board;

        Markings = new List<int>[9, 9];

        for (int i = 0; i < 9; i++)
            for (int j = 0; j < 9; j++)
                Markings[i, j] = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 }.ToList();

        for (int i = 0; i < 9; i++)
            for (int j = 0; j < 9; j++)
                if (board.Digits[i, j] != -1)
                    WriteMarking(i, j, board.Digits[i, j]);
    }

    /// <summary>
    /// looks for a row, column or square where a digit just has one position left and adjust Markings
    /// may need to be executed multiple times
    /// </summary>
    /// <returns>whether Markings has been changed</returns>
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
                Markings[row, col].ForEach(x =>
                {
                    if (foundDigits[x - 1] == -1)
                        foundDigits[x - 1] = index;
                    else if (foundDigits[x - 1] != -2)
                        foundDigits[x - 1] = -2;
                });
            }

            for (int i = 0; i < foundDigits.Length; i++)
            {
                if (foundDigits[i] == -1)
                    throw new UnsolvableException
                        ($"the sudoku is not solvable: {i} cannot be placed in the squares {squares.Aggregate("", (x, y) => x + $" {y.Item1},{y.Item2}")}");

                if (foundDigits[i] >= 0)
                {
                    var (r, c) = squares[foundDigits[i]];
                    if (Markings[r, c].Count == 1)
                        continue;

                    hasChanged = true;
                    WriteMarking(r, c, i + 1);
                }
            }
        };


        foreach (var comp in Board.GetAllCompounds())
        {
            lookFor(comp.ToArray());
        }

        return hasChanged;
    }

    public bool CrossCompoundReduction()
    {
        bool hasChanged = false;
        foreach (var compound in Board.GetAllCompounds())
        {
            foreach (int digit in Board.GetAllDigits().Select(x => x + 1))
            {
                IEnumerable<(int, int)> hasDigits = compound.Where(x => Markings[x.Item1, x.Item2].Contains(digit));
                if (hasDigits.Count() > 1 && hasDigits.Count() <= 3)
                {
                    var compoundsToReduce = Board.GetAllCompounds().Where(x => x.Intersect(hasDigits).Count() == hasDigits.Count());
                    foreach (var reduceComp in compoundsToReduce)
                    {
                        foreach (var (r, c) in reduceComp)
                        {
                            if (hasDigits.Contains((r, c)))
                                continue;

                            if (Markings[r, c].Remove(digit))
                                hasChanged = true;
                        }
                    }
                }

            }
        }

        return hasChanged;
    }

    public bool HiddenDigitsReduction()
    {
        bool hasChanged = false;

        foreach (var compound in Board.GetAllCompounds())
        {
            (int, int)[] comp = compound.ToArray();

            List<int>[] whereIsDigit = new List<int>[9];
            for (int i = 0; i < 9; i++)
                whereIsDigit[i] = new();

            foreach (var ((r, c), i) in comp.Zip(Board.GetAllDigits()))
            {
                foreach (int mark in Markings[r, c])
                    whereIsDigit[mark - 1].Add(i);
            }

            for (int digitToCheck = 0; digitToCheck < 9; digitToCheck++)
            {
                if (whereIsDigit[digitToCheck].Count == 2)
                {
                    IEnumerable<int> otherDigits = Board.GetAllDigits().Where(j => j != digitToCheck && whereIsDigit[j].OrderBy(x => x).SequenceEqual(whereIsDigit[digitToCheck].OrderBy(x => x)));
                    if (otherDigits.Count() != 1)
                        continue;

                    IEnumerable<int> allDigits = otherDigits.Append(digitToCheck);

                    for (int cellToRemove = 0; cellToRemove < 9; cellToRemove++)
                    {
                        List<int> markingToAdjust = Markings[comp[cellToRemove].Item1, comp[cellToRemove].Item2];
                        if (whereIsDigit[digitToCheck].Contains(cellToRemove))
                        {
                            markingToAdjust.Clear();
                            markingToAdjust.AddRange(allDigits.Select(x => x + 1));
                        }
                        else
                        {
                            if (markingToAdjust.RemoveAll(x => allDigits.Contains(x - 1)) != 0)
                                hasChanged = true;
                        }
                    }
                }
            }
        }

        return hasChanged;
    }



    public IEnumerable<(int, int)> GetCertains()
    {
        return Board.GetAllFields().Where(tup => Board.Digits[tup.Item1, tup.Item2] == -1 && Markings[tup.Item1, tup.Item2].Count == 1);
    }

    public bool WriteToBoard()
    {
        bool hasChanged = false;
        foreach (var (r, c) in GetCertains())
        {
            hasChanged = true;
            Board.Digits[r, c] = Markings[r, c][0];
        }
        return hasChanged;
    }
}