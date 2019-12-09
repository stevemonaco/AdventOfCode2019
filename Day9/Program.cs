using System;
using System.IO;
using System.Numerics;

namespace Day9
{
    class Program
    {
        static void Main(string[] args)
        {
            string inputFile = "Day9.txt";
            string program = File.ReadAllText(inputFile);
            var testResult = RunBoostTest(program);
            Console.WriteLine($"The test result was {testResult}");

            var boostResult = BoostSensors(program);
            Console.WriteLine($"The sensor result was {boostResult}");
        }

        public static BigInteger RunBoostTest(string program)
        {
            var provider = new IntInputProvider(new BigInteger[] { 1 });
            var computer = new IntCodeComputer(provider);
            computer.LoadProgramFromString(program);
            computer.ExecuteProgram();
            return computer.ResultSink.OutputQueue.Dequeue();
        }

        public static BigInteger BoostSensors(string program)
        {
            var provider = new IntInputProvider(new BigInteger[] { 2 });
            var computer = new IntCodeComputer(provider);
            computer.LoadProgramFromString(program);
            computer.ExecuteProgram();
            return computer.ResultSink.OutputQueue.Dequeue();
        }
    }
}
