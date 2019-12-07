using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Advent.UnitTests
{
    [TestFixture]
    public class Day6Tests
    {
        [TestCase(new string[] { "COM)B", "B)C", "C)D", "D)E", "E)F", "B)G", "G)H", "D)I", "E)J", "J)K", "K)L" }, 42)]
        public void CalculateOrbits_ReturnsExpected(string[] orbitLines, int expected)
        {
            var map = Day6.Program.CreateOrbitMap(orbitLines);
            var actual = Day6.Program.CalculateOrbits(map);
            Assert.AreEqual(expected, actual);
        }

        [TestCase(new string[] { "COM)B", "B)C", "C)D", "D)E", "E)F", "B)G", "G)H", "D)I", "E)J", "J)K", "K)L", "K)YOU", "I)SAN" }, "YOU", "SAN", 4)]
        public void CalculateOrbitTransfers_ReturnsExpected(string[] orbitLines, string a, string b, int expected)
        {
            var map = Day6.Program.CreateOrbitMap(orbitLines);
            var actual = Day6.Program.CalculateOrbitalTransfers(map, a, b);
            Assert.AreEqual(expected, actual);
        }
    }
}
