using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Day5
{
    public class StringInputProvider : IInputProvider
    {
        public string[] Text { get; set; }
        public int Position { get; set; }

        public StringInputProvider(IEnumerable<string> inputs)
        {
            Text = inputs.ToArray();
        }

        public string GetInput() => Text[Position++];
    }
}
