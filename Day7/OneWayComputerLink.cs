using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Day7
{
    /// <summary>
    /// Links one computer's output to another's input
    /// </summary>
    public class OneWayComputerLink : IOutputSink, IInputProvider
    {
        public BlockingCollection<int> LinkedCollection { get; set; } = new BlockingCollection<int>(new ConcurrentQueue<int>());

        public IntCodeComputer OutputComputer { get; }
        public IntCodeComputer InputComputer { get; }

        public OneWayComputerLink(IntCodeComputer outputComputer, IntCodeComputer inputComputer)
        {
            OutputComputer = outputComputer;
            InputComputer = inputComputer;

            outputComputer.AddOutputSink(this);
            inputComputer.InputProvider = this;
        }

        public int GetInput() => LinkedCollection.Take();

        public void SendOutput(int output) => LinkedCollection.Add(output);
    }
}
