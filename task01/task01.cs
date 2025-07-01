﻿namespace task01;
public static class StringExtensions
{
    public static bool IsPalindrome(this string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return false;
        }
        else
        {
            string line = "";
            input = input.ToLower();
            foreach (char chr in input )
            {
                if (!char.IsPunctuation(chr) && !char.IsWhiteSpace(chr))
                {
                    line += chr;
                }
            }
            
            for (var i = 0; i < input.Length / 2; i++)
            {
                if (line[i] != line[line.Length - i - 1])
                {
                    return false;
                }
            }

            return true;
        }  
    }
}
