public class Board
{
    //-1 for not filled in
    //row first
    public int[,] Digits { get; init; }

    public Board()
    {
        Digits = new int[9, 9] {
            {-1, -1, -1, -1, -1, -1, -1, -1, -1},
            {-1, -1, -1, -1, -1, -1, -1, -1, -1},
            {-1, -1, -1, -1, -1, -1, -1, -1, -1},
            {-1, -1, -1, -1, -1, -1, -1, -1, -1},
            {-1, -1, -1, -1, -1, -1, -1, -1, -1},
            {-1, -1, -1, -1, -1, -1, -1, -1, -1},
            {-1, -1, -1, -1, -1, -1, -1, -1, -1},
            {-1, -1, -1, -1, -1, -1, -1, -1, -1},
            {-1, -1, -1, -1, -1, -1, -1, -1, -1},
        };
    }

    /// <summary>
    /// Get an array with the positions of all square fields (row, col) lies in
    /// </summary>
    /// <param name="row"></param>
    /// <param name="col"></param>
    /// <returns></returns>
    public static IEnumerable<(int, int)> GetSquareFields(int row, int col)
    {
        Func<int, int[]> getThreePart = x =>
        {
            if (x < 0 || x > 8)
                throw new ArgumentException($"invalid index for {x}");

            if (x < 3)
                return new int[] { 0, 1, 2 };
            else if (x < 6)
                return new int[] { 3, 4, 5 };
            else
                return new int[] { 6, 7, 8 };
        };

        return getThreePart(row).SelectMany(x => getThreePart(col).Select(y => (x, y)));
    }
    public static IEnumerable<(int, int)> GetRowFields(int row)
    {
        return Enumerable.Range(0, 9).Select(x => (row, x));
    }
    public static IEnumerable<(int, int)> GetColumnFields(int col)
    {
        return Enumerable.Range(0, 9).Select(x => (x, col));
    }
    public static IEnumerable<(int, int)> GetAllFields()
    {
        return Enumerable.Range(0, 9).SelectMany(x => Enumerable.Range(0, 9).Select(y => (x, y)));
    }
    public static IEnumerable<int> GetAllDigits()
        => Enumerable.Range(0, 9);
    public static IEnumerable<(int, int)> GetSquareFields(int squareId)
    {
        return squareId switch
        {
            0 => GetSquareFields(0, 0),
            1 => GetSquareFields(3, 0),
            2 => GetSquareFields(6, 0),
            3 => GetSquareFields(0, 3),
            4 => GetSquareFields(3, 3),
            5 => GetSquareFields(6, 3),
            6 => GetSquareFields(0, 6),
            7 => GetSquareFields(3, 6),
            8 => GetSquareFields(6, 6),
            _ => throw new ArgumentException("invalid square id"),
        };
    }

    public static IEnumerable<IEnumerable<(int, int)>> GetAllCompounds()
    {
        return Enumerable.Range(0, 9).SelectMany(x => new IEnumerable<(int, int)>[] { GetRowFields(x), GetColumnFields(x), GetSquareFields(x) });
    }

    public Board(int[,] digits)
    {
        if (digits.GetLength(0) != 9 || digits.GetLength(1) != 9)
            throw new ArgumentException($"{nameof(digits)} may not have a dimension other than 9");

        Digits = digits;
    }



    public override string ToString()
    {
        string separatorLine = "+-------+-------+-------+";
        string digitSeparator = "| ";
        string res = "";

        for (int i = 0; i < 9; i++)
        {
            if (i % 3 == 0)
                res += separatorLine + "\n";

            for (int j = 0; j < 9; j++)
            {
                int digit = Digits[i, j];

                if (j % 3 == 0)
                    res += digitSeparator;

                if (digit == -1)
                    res += ". ";
                else
                    res += digit.ToString() + " ";
            }

            res += "|\n";
        }

        return res + separatorLine;
    }
}