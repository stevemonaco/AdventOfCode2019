using System.Collections.Generic;

namespace Day7
{
    public interface IOutputSink
    {
        Queue<int> OutputQueue { get; set; }
        void Output(int output);
    }
}
