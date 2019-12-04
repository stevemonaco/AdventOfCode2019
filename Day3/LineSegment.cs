using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using System.Text;
using static System.Math;

namespace Day3
{
    public struct LineSegment
    {
        public Point Start { get; set; }
        public Point End { get; set; }

        public int X1 => Start.X;
        public int X2 => End.X;
        public int Y1 => Start.Y;
        public int Y2 => End.Y;

        public LineSegment(int x1, int x2, int y1, int y2)
        {
            Start = new Point(x1, y1);
            End = new Point(x2, y2);
        }

        public LineSegment(Point start, Point end)
        {
            Start = start;
            End = end;
        }


        public static LineSegment FromMovement(LineSegment segment, Movement movement)
        {
            int x1 = segment.X2;
            int y1 = segment.Y2;
            int x2 = x1;
            int y2 = y1;

            switch (movement.Direction)
            {
                case Direction.Up:
                    y2 += movement.Distance;
                    break;
                case Direction.Down:
                    y2 -= movement.Distance;
                    break;
                case Direction.Left:
                    x2 -= movement.Distance;
                    break;
                case Direction.Right:
                    x2 += movement.Distance;
                    break;
            }

            return new LineSegment(x1, x2, y1, y2);
        }

        /// <summary>
        /// Detects intersections given line segments are horizontal or vertical
        /// </summary>
        /// <param name="segment"></param>
        /// <param name="intersection"></param>
        /// <returns></returns>
        /// <remarks>Based on math from https://bell0bytes.eu/intersection-of-line-segments/ </remarks>
        public bool FindIntersection(LineSegment segment, out Point intersection)
        {
            var v1 = new Vector2(End.X - Start.X, End.Y - Start.Y);
            var v2 = new Vector2(segment.End.X - segment.Start.X, segment.End.Y - segment.Start.Y);
            var p = new Vector2(segment.Start.X - Start.X, segment.Start.Y - Start.Y);

            var det = v1.X * -v2.Y - -v2.X * v1.Y;

            if (det == 0)
            {
                intersection = new Point(int.MinValue, int.MinValue);
                return false;
            }

            var t1 = (1 / det) * (-v2.Y * p.X + v2.X * p.Y);
            var t2 = (1 / det) * (-v1.Y * p.X + v1.X * p.Y);

            if (t1 < 0 || t1 > 1 || t2 < 0 || t2 > 1)
            {
                intersection = new Point(int.MinValue, int.MinValue);
                return false;
            }

            var xi = Start.X + t1 * v1.X;
            var yi = Start.Y + t1 * v1.Y;

            intersection = new Point((int)Round(xi), (int)Round(yi));
            if (intersection.X == 0 && intersection.Y == 0)
                return false;

            return true;
        }

        /// <summary>
        /// Checks if (an assumed) horizontal or vertical line segment contains a point
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public bool ContainsPoint(Point p)
        {           
            int minX = Min(Start.X, End.X);
            int maxX = Max(Start.X, End.X);
            int minY = Min(Start.Y, End.Y);
            int maxY = Max(Start.Y, End.Y);

            if (p.X == Start.X && p.Y >= minY && p.Y <= maxY)
                return true;
            else if (p.Y == Start.Y && p.X >= minX && p.X <= maxX)
                return true;

            return false;
        }

        public int Length()
        {
            var dx = End.X - Start.X;
            var dy = End.Y - Start.Y;
            return (int)Round(Sqrt(dx * dx + dy * dy));
        }
    }
}
