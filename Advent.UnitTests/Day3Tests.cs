using Day3;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Advent.UnitTests
{
    [TestFixture]
    public class Day3Tests
    {
        [TestCase(new string[] { "R75,D30,R83,U83,L12,D49,R71,U7,L72", "U62,R66,U55,R34,D71,R55,D58,R83" }, 159)]
        [TestCase(new string[] { "R98,U47,R26,D63,R33,U87,L62,D20,R33,U53,R51", "U98,R91,D20,R16,D67,R40,U7,R15,U6,R7" }, 135)]
        public void ExecuteProgram_FindShortestDistance_ReturnsExpected(string[] textRoutes, int expected)
        {
            List<WireRoute> routes = new List<WireRoute>();

            foreach (var textRoute in textRoutes)
                routes.Add(Program.ParseRoute(textRoute));

            var intersections = Program.FindIntersections(routes[0], routes[1]);
            var actual = Program.FindShortestDistance(intersections);

            Assert.AreEqual(expected, actual);
        }

        [TestCase(new string[] { "R75,D30,R83,U83,L12,D49,R71,U7,L72", "U62,R66,U55,R34,D71,R55,D58,R83" }, 610)]
        [TestCase(new string[] { "R98,U47,R26,D63,R33,U87,L62,D20,R33,U53,R51", "U98,R91,D20,R16,D67,R40,U7,R15,U6,R7" }, 410)]
        public void ExecuteProgram_FindShortestRoutingTime_ReturnsExpected(string[] textRoutes, int expected)
        {
            List<WireRoute> routes = new List<WireRoute>();

            foreach (var textRoute in textRoutes)
                routes.Add(Program.ParseRoute(textRoute));

            var intersections = Program.FindIntersections(routes[0], routes[1]);
            var actual = Program.FindShortestRoutingTime(intersections, routes[0], routes[1]);

            Assert.AreEqual(expected, actual);
        }

        public static IReadOnlyList<TestCaseData> LineSegmentTests = new[]
        {
            new TestCaseData(new LineSegment(4, 16, 7, 3), new LineSegment(1, 17, 1, 5), new Point(13, 4))
        };

        [TestCaseSource(nameof(LineSegmentTests))]
        public void LineSegment_Intersects_AsExpected(LineSegment a, LineSegment b, Point expected)
        {
            if(a.FindIntersection(b, out var actual))
                Assert.AreEqual(expected, actual);
            else
                Assert.Fail();
        }
    }
}
