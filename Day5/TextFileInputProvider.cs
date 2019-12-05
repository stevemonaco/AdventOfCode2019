using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Day5
{
    public class TextFileInputProvider : IInputProvider
    {
        public string[] Text { get; set; }
        private int Position { get; set; }

        public TextFileInputProvider(string fileName)
        {
            Text = File.ReadAllLines(fileName);
        }

        public string GetInput() => Text[Position++];
    }
}
