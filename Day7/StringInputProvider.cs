using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Day7
{
    public class StringInputProvider : IInputProvider
    {
        public Queue<string> InputQueue { get; set; } = new Queue<string>();

        public StringInputProvider() { }

        public StringInputProvider(IEnumerable<string> inputs)
        {
            foreach (var input in inputs)
                InputQueue.Enqueue(input);
        }

        public string GetInput() => InputQueue.Dequeue();

        public void AddInput(string input) => InputQueue.Enqueue(input);
    }
}
