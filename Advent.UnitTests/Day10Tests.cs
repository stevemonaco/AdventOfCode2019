using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Advent.UnitTests
{
    [TestFixture]
    class Day10Tests
    {
        public static IReadOnlyList<TestCaseData> AsteroidTestData = new[]
        {
            new TestCaseData(new[]
            {
                "......#.#.",
                "#..#.#....",
                "..#######.",
                ".#.#.###..",
                ".#..#.....",
                "..#....#.#",
                "#..#....#.",
                ".##.#..###",
                "##...#..#.",
                ".#....####",
            }, 33),
            new TestCaseData(new[]
            {
                ".#..#",
                ".....",
                "#####",
                "....#",
                "...##",
            }, 8)
        };

        public static IReadOnlyList<TestCaseData> DestroyTestData = new[]
{
            new TestCaseData(new[]
            {
                ".#..##.###...#######",
                "##.############..##.",
                ".#.######.########.#",
                ".###.#######.####.#.",
                "#####.##.#.##.###.##",
                "..#####..#.#########",
                "####################",
                "#.####....###.#.#.##",
                "##.#################",
                "#####.##.###..####..",
                "..######..##.#######",
                "####.##.####...##..#",
                ".#####..#.######.###",
                "##...#.##########...",
                "#.##########.#######",
                ".####.#.###.###.#.##",
                "....##.##.###..#####",
                ".#.#.###########.###",
                "#.#.#.#####.####.###",
                "###.##.####.##.#..##",
            }, 11, 13, 200, 8, 2)
        };

        [TestCaseSource(nameof(AsteroidTestData))]
        public void FindBestLocation_AsExpected(string[] lines, int expected)
        {
            var map = Day10.Program.ReadMap(lines);
            var actual = Day10.Program.FindBestLocation(map);

            Assert.AreEqual(expected, actual.asteroids);
        }

        [TestCaseSource(nameof(DestroyTestData))]
        public void DestroyAsteroids(string[] lines, int stationX, int stationY, int count, int expectedX, int expectedY)
        {
            var map = Day10.Program.ReadMap(lines);
            var location = Day10.Program.FindBestLocation(map);
            var actual = Day10.Program.DestroyAsteroids(map, stationX, stationY, count);

            Assert.Multiple(() =>
            {
                Assert.AreEqual(expectedX, actual.MapX);
                Assert.AreEqual(expectedY, actual.MapY);
            });
        }
    }
}
