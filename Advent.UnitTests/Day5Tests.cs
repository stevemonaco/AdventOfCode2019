using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Day5;

namespace Advent.UnitTests
{
    [TestFixture]
    public class Day5Tests
    {
        [TestCase("1,0,0,0,99", new int[] { 2, 0, 0, 0, 99 })]
        [TestCase("2,3,0,3,99", new int[] { 2, 3, 0, 6, 99 })]
        [TestCase("2,4,4,5,99,0", new int[] { 2, 4, 4, 5, 99, 9801 })]
        [TestCase("1,1,1,4,99,5,6,0,99", new int[] { 30, 1, 1, 4, 2, 5, 6, 0, 99 })]
        public void ExecuteProgram_AsExpected(string programString, int[] expected)
        {
            var provider = new StringInputProvider(new List<string>());
            var computer = new IntCodeComputer(provider, new ListOutputSink());
            computer.LoadProgramFromString(programString);
            computer.ExecuteProgram();

            CollectionAssert.AreEqual(expected, computer.Program);
        }

        [TestCase("3,12,6,12,15,1,13,14,13,4,13,99,-1,0,1,9", new[] { "0" }, new[] { 0 })]
        [TestCase("3,12,6,12,15,1,13,14,13,4,13,99,-1,0,1,9", new[] { "5" }, new[] { 1 })]
        [TestCase("3,21,1008,21,8,20,1005,20,22,107,8,21,20,1006,20,31,1106, 0, 36, 98, 0, 0, 1002, 21, 125, 20, 4, 20, 1105, 1, 46, 104,999, 1105, 1, 46, 1101, 1000, 1, 20, 4, 20, 1105, 1, 46, 98, 99",
            new[] { "7" }, new[] { 999 })]
        [TestCase("3,21,1008,21,8,20,1005,20,22,107,8,21,20,1006,20,31,1106, 0, 36, 98, 0, 0, 1002, 21, 125, 20, 4, 20, 1105, 1, 46, 104,999, 1105, 1, 46, 1101, 1000, 1, 20, 4, 20, 1105, 1, 46, 98, 99",
            new[] { "8" }, new[] { 1000 })]
        [TestCase("3,21,1008,21,8,20,1005,20,22,107,8,21,20,1006,20,31,1106, 0, 36, 98, 0, 0, 1002, 21, 125, 20, 4, 20, 1105, 1, 46, 104,999, 1105, 1, 46, 1101, 1000, 1, 20, 4, 20, 1105, 1, 46, 98, 99",
            new[] { "9" }, new[] { 1001 })]
        [TestCase("3,21,1008,21,8,20,1005,20,22,107,8,21,20,1006,20,31,1106, 0, 36, 98, 0, 0, 1002, 21, 125, 20, 4, 20, 1105, 1, 46, 104,999, 1105, 1, 46, 1101, 1000, 1, 20, 4, 20, 1105, 1, 46, 98, 99",
            new[] { "15" }, new[] { 1001 })]
        public void ExecuteProgram_AsExpected(string programString, string[] inputs, int[] expected)
        {
            var provider = new StringInputProvider(inputs);
            var sink = new ListOutputSink();
            var computer = new IntCodeComputer(provider, sink);
            computer.LoadProgramFromString(programString);
            computer.ExecuteProgram();

            CollectionAssert.AreEqual(expected, sink.OutputHistory);
        }
    }
}
