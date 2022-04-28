public class Board
{
    //-1 for not filled in
    public int[,] Digits = new int[9, 9] {
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