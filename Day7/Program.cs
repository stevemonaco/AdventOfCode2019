using System;
using System.Linq;
using MoreLinq;
using System.Collections.Generic;
using System.IO;

namespace Day7
{
    public class Program
    {
        static void Main(string[] args)
        {
            string inputFile = "Day7.txt";
            int[][] phaseOptions1 = { new[] { 0, 1, 2, 3, 4 } };
            var programString = File.ReadAllText(inputFile);
            var program = IntCodeComputer.CreateProgramFromString(programString);
            var programs = Enumerable.Repeat(program, phaseOptions1.First().Length).ToList();

            // Part one
            var maxSequence = GetMaxThrusterOutput(phaseOptions1, programs, 0);
            Console.WriteLine($"Max thruster output signal was {maxSequence.output}" +
                $" with sequence ({string.Join(',', maxSequence.phaseSettings)})");

            // Part two
            int[][] phaseOptions2 = { new[] { 0, 1, 2, 3, 4 }, new[] { 5, 6, 7, 8, 9 } };
            var maxSequence2 = GetMaxThrusterOutput(phaseOptions2, programs, 0);
            Console.WriteLine($"Max thruster output signal was {maxSequence2.output}" +
                $" with sequence ({string.Join(',', maxSequence2.phaseSettings)})");
        }

        public static (int output, IList<int> phaseSettings)
            GetMaxThrusterOutput(int[][] phaseOptionsList, IList<int[]> programs, int initialValue)
        {
            var phasePerms = phaseOptionsList.Select(x => x.Permutations());
            var phaseSettingsList = phasePerms.CartesianProduct();

            var thrusterSimulations = new List<(int output, IList<int> phaseSettings)>();

            foreach(var phaseSettings in phaseSettingsList) // Each simulation
            {
                var loop = new AmplifierLoop(programs);
                var lastValue = initialValue;

                var flatSettings = new List<int>();

                foreach (var phaseSetting in phaseSettings) // Each feedback iteration
                {
                    var flatSetting = phaseSetting.ToList();
                    lastValue = GetThrusterOutput(loop, flatSetting, lastValue);
                    flatSettings.AddRange(flatSetting);
                }

                thrusterSimulations.Add((lastValue, flatSettings));
            }

            return thrusterSimulations.MaxBy(x => x.output).First();
        }

        public static int GetThrusterOutput(AmplifierLoop loop, IList<int> phaseSetting, int initialValue) =>
            loop.ExecuteLoopOnce(phaseSetting, initialValue);

        public static int GetThrusterOutput(IList<int> phaseSetting, int[] program)
        {
            var amplifiers = new List<IntCodeComputer>(phaseSetting.Count);
            var lastOutputSignal = 0;

            for (int i = 0; i < phaseSetting.Count; i++)
            {
                var inputProvider = new StringInputProvider(new[] { $"{phaseSetting[i]}", $"{lastOutputSignal}" });
                var outputSink = new QueueOutputSink();
                var amplifier = new IntCodeComputer(inputProvider, outputSink);
                amplifier.Program = program.Clone() as int[];

                amplifier.ExecuteProgram();
                lastOutputSignal = outputSink.OutputQueue.Dequeue();
            }

            return lastOutputSignal;
        }
    }
}
