using System.Collections.Generic;
using System.Numerics;

namespace Day9
{
    public class QueueOutputSink : IOutputSink
    {
        public Queue<BigInteger> OutputQueue { get; set; } = new Queue<BigInteger>();

        public void SendOutput(BigInteger output)
        {
            OutputQueue.Enqueue(output);
        }
    }
}
