using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using static System.Math;

namespace Day3
{
    public class Program
    {
        static Dictionary<string, Direction> directionMap = new Dictionary<string, Direction> 
        {
            { "U", Direction.Up }, { "D", Direction.Down }, 
            { "L", Direction.Left }, { "R", Direction.Right }
        };

        static void Main(string[] args)
        {
            string inputFile = "Day3.txt";
            var wireRoutes = new List<WireRoute>();

            foreach (var line in File.ReadAllLines(inputFile))
            {
                wireRoutes.Add(ParseRoute(line));
            }

            var intersections = FindIntersections(wireRoutes[0], wireRoutes[1]);
            int distance = FindShortestDistance(intersections);
            int time = FindShortestRoutingTime(intersections, wireRoutes[0], wireRoutes[1]);

            Console.WriteLine($"Closest intersection was {distance} units away");
            Console.WriteLine($"Fastest intersection was {time} units in transit");
        }

        public static IList<Point> FindIntersections(WireRoute firstRoute, WireRoute secondRoute)
        {
            var list = new List<Point>();

            foreach(var firstSegment in firstRoute.Segments)
            {
                foreach(var secondSegment in secondRoute.Segments)
                {
                    if (firstSegment.FindIntersection(secondSegment, out var point))
                        list.Add(point);
                }
            }

            return list;
        }

        public static WireRoute ParseRoute(string line)
        {
            var items = line.Split(",", StringSplitOptions.RemoveEmptyEntries);
            var movementList = new List<Movement>(items.Length);

            foreach (var item in items)
            {
                var direction = directionMap[item.Substring(0, 1)];
                var distance = int.Parse(item.Substring(1));

                movementList.Add(new Movement(direction, distance));
            }

            return new WireRoute(movementList);
        }

        public static int FindShortestDistance(IList<Point> points)
        {
            int minDistance = int.MaxValue;

            for(int i = 0; i < points.Count; i++)
            {
                int distance = Abs(points[i].X) + Abs(points[i].Y);
                minDistance = Min(minDistance, distance);
            }

            return minDistance;
        }

        public static int FindShortestRoutingTime(IList<Point> points, WireRoute a, WireRoute b)
        {
            int minTime = int.MaxValue;

            foreach(var point in points)
            {
                int timea = a.GetRoutingTime(point);
                int timeb = b.GetRoutingTime(point);
                int time = a.GetRoutingTime(point) + b.GetRoutingTime(point);
                minTime = Min(minTime, time);
            }

            return minTime;
        }
    }
}
