using System;
using System.Collections.Generic;
using System.Drawing;
using static System.Math;

namespace Day3
{
    public class WireRoute
    {
        public List<Movement> Route { get; } = new List<Movement>();
        public List<LineSegment> Segments { get; } = new List<LineSegment>();

        public WireRoute(IEnumerable<Movement> movements)
        {
            Route = new List<Movement>(movements);

            Segments.Capacity = Route.Count - 1;

            var segment = new LineSegment(0, 0, 0, 0);

            foreach (var movement in movements)
            {
                segment = LineSegment.FromMovement(segment, movement);
                Segments.Add(segment);
            }
        }

        public int GetRoutingTime(Point p)
        {
            int time = 0;

            foreach(var segment in Segments)
            {
                if (segment.ContainsPoint(p))
                {
                    var dx = Abs(p.X - segment.Start.X);
                    var dy = Abs(p.Y - segment.Start.Y);
                    time += dx + dy;
                    return time;
                }
                else
                    time += segment.Length();
            }

            return int.MaxValue;
        }

        public int GetManhattanDistance()
        {
            int x = 0;
            int y = 0;

            foreach(var movement in Route)
            {
                switch (movement.Direction)
                {
                    case Direction.Up:
                        y += movement.Distance;
                        break;
                    case Direction.Down:
                        y -= movement.Distance;
                        break;
                    case Direction.Left:
                        x -= movement.Distance;
                        break;
                    case Direction.Right:
                        x += movement.Distance;
                        break;
                }
            }

            return Abs(x) + Abs(y);
        }
    }
}
