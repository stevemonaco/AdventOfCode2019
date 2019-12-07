using System.Collections.Generic;

namespace Day7
{
    public interface IInputProvider
    {
        Queue<string> InputQueue { get; set; }
        string GetInput();
    }
}