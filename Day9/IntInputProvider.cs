using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Numerics;

namespace Day9
{
    public class IntInputProvider : IInputProvider
    {
        public Queue<BigInteger> InputQueue { get; set; } = new Queue<BigInteger>();

        public IntInputProvider() { }

        public IntInputProvider(IEnumerable<BigInteger> inputs)
        {
            foreach (var input in inputs)
                InputQueue.Enqueue(input);
        }

        public BigInteger GetInput() => InputQueue.Dequeue();

        public void AddInput(int input) => InputQueue.Enqueue(input);
    }
}
