using System;

namespace Day5
{
    public class Program
    {
        static void Main(string[] args)
        {
            string inputFile = "Day5.txt";
            ExecuteProgram(inputFile);
        }

        public static void ExecuteProgram(string inputFile)
        {
            var provider = new TextFileInputProvider("Day5input2.txt");
            var sink = new ListOutputSink();
            var computer = new IntCodeComputer(provider, sink);
            computer.LoadProgramFromFile(inputFile);
            computer.ExecuteProgram();

            foreach (var s in sink.OutputHistory)
                Console.WriteLine($"Output: [{s}]");
        }
    }
}
