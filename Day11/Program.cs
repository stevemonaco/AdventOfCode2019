using System;
using System.Collections.Generic;
using System.IO;

namespace Day11
{
    class Program
    {
        static void Main(string[] args)
        {
            string inputFile = "Day11.txt";

            var navigator = new GridNavigator(File.ReadAllText(inputFile));
            navigator.Execute();
            Console.WriteLine($"There were {navigator.Panels.Count} panels painted");

            var grid = navigator.CreatePrintableGrid();

            for(int y = 0; y < grid.GetLength(1); y++)
            {
                for(int x = 0; x < grid.GetLength(0); x++)
                {
                    Console.Write(grid[x, y]);
                }
                Console.WriteLine();
            }
        }
    }
}
