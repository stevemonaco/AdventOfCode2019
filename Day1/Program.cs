using System;
using System.IO;

namespace Day1
{
    class Program
    {
        static void Main(string[] args)
        {
            string inputFile = "Day1.txt";
            int fuelSum = 0;

            foreach (var line in File.ReadAllLines(inputFile))
            {
                if (int.TryParse(line, out int moduleMass))
                    fuelSum += CalculateModuleFuelConsumption2(moduleMass);
                else
                    Console.WriteLine($"Could not parse module weight: {line}");
            }

            Console.WriteLine($"Total fuel requirement is {fuelSum} units");
            Console.ReadKey();
        }

        static int CalculateModuleFuelConsumption(int moduleMass) => Math.Max((moduleMass / 3) - 2, 0);

        static int CalculateModuleFuelConsumption2(int moduleMass)
        {
            int fuelSum = 0;
            int mass = moduleMass;
            int fuelCost;

            while ((fuelCost = CalculateModuleFuelConsumption(mass)) > 0)
            {
                fuelSum += fuelCost;
                mass = fuelCost;
            }

            return fuelSum;
        }
    }
}
