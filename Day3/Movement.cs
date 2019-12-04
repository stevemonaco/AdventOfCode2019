using System;
using System.Collections.Generic;
using System.Text;

namespace Day3
{
    public enum Direction { Up, Down, Left, Right };
    public class Movement
    {
        public Direction Direction { get; }
        public int Distance { get; }

        public Movement(Direction direction, int distance)
        {
            Direction = direction;
            Distance = distance;
        }
    }
}
