using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Day10
{
    public class Asteroid
    {
        public int MapX { get; set; }
        public int MapY { get; set; }
        public int DeltaX { get; set; }
        public int DeltaY { get; set; }
        public double Distance { get; }
        public double Angle => Math.Atan2(DeltaY, DeltaX);

        public Asteroid() { }

        public Asteroid(int mapX, int mapY, int deltaX, int deltaY)
        {
            MapX = mapX;
            MapY = mapY;
            DeltaX = deltaX;
            DeltaY = deltaY;
            Distance = Math.Sqrt(DeltaX * DeltaX + DeltaY * DeltaY);
        }
    }
}
