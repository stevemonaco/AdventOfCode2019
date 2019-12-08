using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day7
{
    public class AmplifierLoop
    {
        public int AmplifierCount { get; }
        private IntCodeComputer[] _computers;
        private List<OneWayComputerLink> _links = new List<OneWayComputerLink>();

        public AmplifierLoop(IList<int[]> programs, IList<int> phaseSetting, int initialValue, bool createFeedback)
        {
            AmplifierCount = programs.Count;
            _computers = new IntCodeComputer[AmplifierCount];

            for (int i = 0; i < programs.Count; i++)
            {
                var computer = new IntCodeComputer();
                computer.Program = programs[i].Clone() as int[];
                computer.ComputerName = $"Amplifier-{char.ConvertFromUtf32('A' + i)}";
                _computers[i] = computer;
            }

            if (createFeedback)
            {
                var link = new OneWayComputerLink(_computers[programs.Count - 1], _computers[0]);
                link.LinkedCollection.Add(phaseSetting[0]);
                link.LinkedCollection.Add(initialValue);
                _links.Add(link);
            }
            else
                _computers[0].InputProvider = new IntInputProvider(new[] { phaseSetting[0], initialValue });

            for (int i = 1; i < programs.Count; i++)
            {
                var link = new OneWayComputerLink(_computers[i - 1], _computers[i]);
                link.LinkedCollection.Add(phaseSetting[i]);
                _links.Add(link);
            }
        }

        public async ValueTask<int> Execute(IList<int> phaseSetting, int initialValue)
        {
            var amplifierTasks = new List<Task>();

            foreach(var computer in _computers)
            {
                var task = Task.Run(() => computer.ExecuteProgram());
                amplifierTasks.Add(task);
            }

            await Task.WhenAll(amplifierTasks);

            return _computers.Last().ResultSink.OutputQueue.Last();
        }

        public int ExecuteSequential(IList<int> phaseSetting, int initialValue)
        {
            _computers[0].InputProvider = new IntInputProvider(new[] { phaseSetting[0], initialValue });

            for (int i = 1; i < phaseSetting.Count; i++)
            {
                var link = new OneWayComputerLink(_computers[i - 1], _computers[i]);
                link.LinkedCollection.Add(phaseSetting[i]);
                _links.Add(link);
            }

            for (int i = 0; i < phaseSetting.Count; i++)
            {
                var amplifier = _computers[i];
                amplifier.ExecuteProgram();
            }

            return _computers.Last().ResultSink.OutputQueue.Dequeue();
        }

        public IEnumerable<int[]> GetAmplifierProgramStates()
        {
            for (int i = 0; i < _computers.Length; i++)
                yield return _computers[i].Program;
        }
    }
}
