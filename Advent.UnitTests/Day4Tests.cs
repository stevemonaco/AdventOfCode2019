using NUnit.Framework;
using Day4;
using System;
using System.Collections.Generic;
using System.Text;

namespace Advent.UnitTests
{
    [TestFixture]
    public class Day4Tests
    {
        [TestCase(111111, true)]
        [TestCase(123389, true)]
        [TestCase(223450, false)]
        [TestCase(123789, false)]
        public void IsValidPassword_AsExpected(int password, bool expected)
        {
            var actual = Program.IsValidPassword(password);
            Assert.AreEqual(expected, actual);
        }

        [TestCase(111111, false)]
        [TestCase(112233, true)]
        [TestCase(113444, true)]
        [TestCase(113334, true)]
        [TestCase(123389, true)]
        [TestCase(123488, true)]
        [TestCase(223450, false)]
        [TestCase(123789, false)]
        public void IsValidPassword2_AsExpected(int password, bool expected)
        {
            var actual = Program.IsValidPassword2(password);
            Assert.AreEqual(expected, actual);
        }
    }
}
