using System;
using System.Linq;
using MoreLinq;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Day7
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            string inputFile = "Day7.txt";
            int[] phaseOptions1 = new[] { 0, 1, 2, 3, 4 };
            var programString = File.ReadAllText(inputFile);
            var program = IntCodeComputer.CreateProgramFromString(programString);
            var programs = Enumerable.Repeat(program, phaseOptions1.Length).ToList();

            // Part one
            var maxSequence = await GetMaxThrusterOutput(phaseOptions1, programs, 0, false);
            Console.WriteLine($"Max thruster output signal was {maxSequence.maxOutput}" +
                $" with sequence ({string.Join(',', maxSequence.bestPhaseSettings)})");

            // Part two
            int[] phaseOptions2 = new[] { 5, 6, 7, 8, 9 };
            var maxSequence2 = await GetMaxThrusterOutput(phaseOptions2, programs, 0, true);
            Console.WriteLine($"Max thruster output signal was {maxSequence2.maxOutput}" +
                $" with sequence ({string.Join(',', maxSequence2.bestPhaseSettings)})");
        }

        public static async ValueTask<(int maxOutput, IList<int> bestPhaseSettings)>
            GetMaxThrusterOutput(IEnumerable<int> phaseOptionsList, IList<int[]> programs, int initialValue, bool useFeedback)
        {
            var phasePerms = phaseOptionsList.Permutations();

            var thrusterSimulations = new List<(int output, IList<int> phaseSettings)>();

            foreach(var phaseSettings in phasePerms) // Each simulation
            {
                var settings = phaseSettings.ToList();
                var loop = new AmplifierLoop(programs, settings, initialValue, useFeedback);

                int thrustResult;
                if (useFeedback)
                    thrustResult = await loop.Execute(settings, initialValue);
                else
                    thrustResult = loop.ExecuteSequential(settings, initialValue);

                thrusterSimulations.Add((thrustResult, settings));
            }

            return thrusterSimulations.MaxBy(x => x.output).First();
        }
    }
}
