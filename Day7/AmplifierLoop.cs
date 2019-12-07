using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Day7
{
    public class AmplifierLoop
    {
        public int AmplifierCount { get; }
        private IntCodeComputer[] _computers;

        public AmplifierLoop(IList<int[]> programs)
        {
            AmplifierCount = programs.Count;
            _computers = new IntCodeComputer[AmplifierCount];

            for(int i = 0; i < programs.Count; i++)
            {
                var computer = new IntCodeComputer(new StringInputProvider(), new QueueOutputSink());
                computer.Program = programs[i].Clone() as int[];
                _computers[i] = computer;
            }
        }

        public int ExecuteLoopOnce(IList<int> phaseSetting, int initialValue)
        {
            var lastOutputSignal = initialValue;

            for (int i = 0; i < phaseSetting.Count; i++)
            {
                var amplifier = _computers[i];
                var inputProvider = amplifier.InputProvider as StringInputProvider;
                inputProvider.AddInput($"{phaseSetting[i]}");
                inputProvider.AddInput($"{lastOutputSignal}");
                var outputSink = amplifier.OutputSink as QueueOutputSink;

                amplifier.ExecuteProgram();
                lastOutputSignal = outputSink.OutputQueue.Dequeue();
            }

            return lastOutputSignal;
        }

        public IEnumerable<int[]> GetAmplifierProgramStates()
        {
            for (int i = 0; i < _computers.Length; i++)
                yield return _computers[i].Program;
        }
    }
}
