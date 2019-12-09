using NUnit.Framework;
using System.Collections.Generic;
using Day9;
using System.Numerics;

namespace Advent.UnitTests
{
    [TestFixture]
    public class Day9Tests
    {
        public static IReadOnlyList<TestCaseData> ExecuteProgramTests = new[]
        {
            new TestCaseData("104,1125899906842624,99", new[] { BigInteger.Parse("1125899906842624") }),
            new TestCaseData("109,1,204,-1,1001,100,1,100,1008,100,16,101,1006,101,0,99", new[]
            {
                BigInteger.Parse("109"), BigInteger.Parse("1"), BigInteger.Parse("204"), BigInteger.Parse("-1"),
                BigInteger.Parse("1001"), BigInteger.Parse("100"), BigInteger.Parse("1"), BigInteger.Parse("100"),
                BigInteger.Parse("1008"), BigInteger.Parse("100"), BigInteger.Parse("16"), BigInteger.Parse("101"),
                BigInteger.Parse("1006"), BigInteger.Parse("101"), BigInteger.Parse("0"), BigInteger.Parse("99")
            }),
            new TestCaseData("1102,34915192,34915192,7,4,7,99,0", new[] { BigInteger.Parse("1219070632396864") }),
        };

        [TestCaseSource(nameof(ExecuteProgramTests))]
        public void ExecuteProgram_AsExpected(string programString, BigInteger[] expected)
        {
            var computer = new IntCodeComputer();
            computer.LoadProgramFromString(programString);
            computer.ExecuteProgram();
            var actual = computer.ResultSink.OutputQueue;

            CollectionAssert.AreEqual(expected, actual);
        }
    }
}
