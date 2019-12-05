using System;
using System.Collections.Generic;
using System.IO;

namespace Day4
{
    public class Program
    {
        static void Main(string[] args)
        {
            string inputFile = "Day4.txt";

            var input = File.ReadAllText(inputFile).Split('-');
            var start = int.Parse(input[0]);
            var end = int.Parse(input[1]);

            int validCombinations1 = 0;
            int validCombinations2 = 0;

            for (int i = start; i <= end; i++)
            {
                if (IsValidPassword(i))
                    validCombinations1++;
                if (IsValidPassword2(i))
                    validCombinations2++;
            }

            Console.WriteLine($"Input range: [{start:D6}-{end:D6}]");
            Console.WriteLine($"The number of valid combinations for the first half is {validCombinations1}");
            Console.WriteLine($"The number of valid combinations for the second half is {validCombinations2}");
        }

        public static bool IsValidPassword(int numericPassword)
        {
            var text = numericPassword.ToString("D6");
            bool hasDouble = false;

            // Check for monotonically increasing series of digits
            for (int i = 1; i < text.Length; i++)
            {
                // Check for monotonically increasing series of digits
                if (text[i] < text[i - 1])
                    return false;

                if (text[i] == text[i - 1])
                    hasDouble = true;
            }

            return hasDouble;
        }

        public static bool IsValidPassword2(int numericPassword)
        {
            var text = numericPassword.ToString("D6");

            for (int i = 1; i < text.Length; i++)
            {
                // Check for monotonically increasing series of digits
                if (text[i] < text[i - 1])
                    return false;
            }

            var hasDouble = false;
            int index = 0;

            while(index < text.Length)
            {
                var repeatChar = text[index];
                var repeatLength = 1;
                index++;

                while(index < text.Length && repeatChar == text[index])
                {
                    repeatLength++;
                    index++;
                }

                if (repeatLength == 2)
                    hasDouble = true;
            }

            return hasDouble;
        }
    }
}
