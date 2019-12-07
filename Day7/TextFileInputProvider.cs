using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Day7
{
    public class TextFileInputProvider : IInputProvider
    {
        public Queue<string> InputQueue { get; set; } = new Queue<string>();

        public TextFileInputProvider(string fileName)
        {
            foreach (var line in File.ReadAllLines(fileName))
                InputQueue.Enqueue(line);
        }

        public string GetInput() => InputQueue.Dequeue();
    }
}
