using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Day2;

namespace Advent.UnitTests
{
    [TestFixture]
    public class Day2Tests
    {
        [TestCase(new int[] { 1, 0, 0, 0, 99 }, new int[] { 2, 0, 0, 0, 99 })]
        [TestCase(new int[] { 2, 3, 0, 3, 99 }, new int[] { 2, 3, 0, 6, 99 })]
        [TestCase(new int[] { 2, 4, 4, 5, 99, 0 }, new int[] { 2, 4, 4, 5, 99, 9801 })]
        [TestCase(new int[] { 1, 1, 1, 4, 99, 5, 6, 0, 99 }, new int[] { 30, 1, 1, 4, 2, 5, 6, 0, 99 })]
        public void ExecuteProgram_ReturnsExpected(int[] program, int[] expected)
        {
            Day2.Program.ExecuteProgram(program);
            CollectionAssert.AreEqual(expected, program);
        }
    }
}
