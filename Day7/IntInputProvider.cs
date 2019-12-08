using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Day7
{
    public class IntInputProvider : IInputProvider
    {
        public Queue<int> InputQueue { get; set; } = new Queue<int>();

        public IntInputProvider() { }

        public IntInputProvider(IEnumerable<int> inputs)
        {
            foreach (var input in inputs)
                InputQueue.Enqueue(input);
        }

        public int GetInput() => InputQueue.Dequeue();

        public void AddInput(int input) => InputQueue.Enqueue(input);
    }
}
