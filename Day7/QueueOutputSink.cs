using System;
using System.Collections.Generic;
using System.Text;

namespace Day7
{
    public class QueueOutputSink : IOutputSink
    {
        public Queue<int> OutputQueue { get; set; } = new Queue<int>();

        public void Output(int output)
        {
            OutputQueue.Enqueue(output);
        }
    }
}
