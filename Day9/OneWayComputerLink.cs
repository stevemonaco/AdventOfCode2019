using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Day9
{
    /// <summary>
    /// Links one computer's output to another's input
    /// </summary>
    public class OneWayComputerLink : IOutputSink, IInputProvider
    {
        public BlockingCollection<BigInteger> LinkedCollection { get; set; } = 
            new BlockingCollection<BigInteger>(new ConcurrentQueue<BigInteger>());

        public IntCodeComputer OutputComputer { get; }
        public IntCodeComputer InputComputer { get; }

        public OneWayComputerLink(IntCodeComputer outputComputer, IntCodeComputer inputComputer)
        {
            OutputComputer = outputComputer;
            InputComputer = inputComputer;

            outputComputer.AddOutputSink(this);
            inputComputer.InputProvider = this;
        }

        public BigInteger GetInput() => LinkedCollection.Take();

        public void SendOutput(BigInteger output) => LinkedCollection.Add(output);
    }
}
