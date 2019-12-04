using System;
using System.IO;
using System.Linq;

namespace Day2
{
    public class Program
    {
        static void Main(string[] args)
        {
            string inputFile = "Day2.txt";

            var program = File.ReadAllText(inputFile).Split(",").Select(x => int.Parse(x)).ToArray();

            // First problem
            //program[1] = 12;
            //program[2] = 2;
            //ExecuteProgram(program);
            //Console.WriteLine($"Value at Position 0 is {program[0]}");

            // Second problem
            GreedyResultSearch(program, 19690720);
        }

        public static void ExecuteProgram(int[] program)
        {
            int ip = 0;
            bool halt = false;

            while(ip < program.Length && !halt)
            {
                var instruction = program[ip];

                if (instruction == 1 && ip + 3 < program.Length)
                {
                    int operand1Address = program[ip + 1];
                    int operand2Address = program[ip + 2];
                    int resultAddress = program[ip + 3];
                    program[resultAddress] = program[operand1Address] + program[operand2Address];
                    ip += 4;
                }
                else if (instruction == 2 && ip + 3 < program.Length)
                {
                    int operand1Address = program[ip + 1];
                    int operand2Address = program[ip + 2];
                    int resultAddress = program[ip + 3];
                    program[resultAddress] = program[operand1Address] * program[operand2Address];
                    ip += 4;
                }
                else if (instruction == 99)
                {
                    halt = true;
                    ip++;
                }
            }
        }

        public static void GreedyResultSearch(int[] program, int answer)
        {
            for(int noun = 0; noun <= 99; noun++)
            {
                for(int verb = 0; verb <= 99; verb++)
                {
                    var testProgram = program.Clone() as int[];
                    testProgram[1] = noun;
                    testProgram[2] = verb;

                    ExecuteProgram(testProgram);
                    if(testProgram[0] == answer)
                    {
                        Console.WriteLine($"Output {answer} can be achieved with (noun: {noun}) (verb: {verb})");
                        return;
                    }
                }
            }
        }
    }
}
