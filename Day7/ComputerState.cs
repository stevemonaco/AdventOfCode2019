using System;
using System.Collections.Generic;
using System.Text;

namespace Day7
{
    public class ComputerState
    {
        public int[] Program { get; set; }
        public int IP { get; set; }
        public IInputProvider InputProvider { get; set; }
        public IOutputSink OutputSink { get; set; }
    }
}
