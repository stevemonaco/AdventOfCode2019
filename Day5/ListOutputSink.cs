using System;
using System.Collections.Generic;
using System.Text;

namespace Day5
{
    public class ListOutputSink : IOutputSink
    {
        public List<int> OutputHistory { get; set; } = new List<int>();

        public void Output(int output)
        {
            OutputHistory.Add(output);
        }
    }
}
