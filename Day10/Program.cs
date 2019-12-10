using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day10
{
    public class Program
    {
        static void Main(string[] args)
        {
            string inputFile = "Day10.txt";
            var map = ReadMap(File.ReadAllLines(inputFile));
            var bestLocation = FindBestLocation(map);

            Console.WriteLine($"The best location is ({bestLocation.x}, {bestLocation.y}) containing {bestLocation.asteroids} asteroids");

            var target = DestroyAsteroids(map, bestLocation.x, bestLocation.y, 200);
            Console.WriteLine($"Asteroid at ({target.MapX}, {target.MapY}) was the 200th to be destroyed");
        }

        public static char[,] ReadMap(string[] lines)
        {
            var width = lines[0].Length;
            var height = lines.Length;

            var map = new char[width, height];

            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                    map[x, y] = lines[y][x];

            return map;
        }

        public static (int x, int y, int asteroids) FindBestLocation(char[,] map)
        {
            int width = map.GetLength(0);
            int height = map.GetLength(1);

            var maxAsteroids = 0;
            var bestX = 0;
            var bestY = 0;

            for(int stationY = 0; stationY < height; stationY++)
            {
                for(int stationX = 0; stationX < width; stationX++)
                {
                    if (map[stationX, stationY] == '.')
                        continue;

                    int asteroids = DetectAsteroids(map, stationX, stationY);
                    if(asteroids > maxAsteroids)
                    {
                        maxAsteroids = asteroids;
                        bestX = stationX;
                        bestY = stationY;
                    }
                }
            }

            return (bestX, bestY, maxAsteroids);
        }

        public static int DetectAsteroids(char[,] map, int stationX, int stationY)
        {
            var set = new HashSet<(int, int)>();

            for(int y = 0; y < map.GetLength(1); y++)
            {
                for(int x = 0; x < map.GetLength(0); x++)
                {
                    if (map[x, y] == '.' || (x == stationX && y == stationY))
                        continue;

                    var deltaX = x - stationX;
                    var deltaY = y - stationY;

                    var gcd = GCD(Math.Abs(deltaX), Math.Abs(deltaY));
                    var vec = (deltaX / gcd, deltaY / gcd);
                    set.Add(vec);
                }
            }

            return set.Count;
        }

        public static Asteroid DestroyAsteroids(char[,] map, int stationX, int stationY, int count)
        {
            int destroyedAsteroids = 0;
            while (destroyedAsteroids < count)
            {
                var asteroids = ScanMap360(map, stationX, stationY);
                foreach (var target in asteroids)
                {
                    map[target.MapX, target.MapY] = '.';
                    Console.WriteLine($"Destroyed #{destroyedAsteroids} ({target.MapX}, {target.MapY})");
                    destroyedAsteroids++;
                    if (destroyedAsteroids == count)
                    {
                        return target;
                    }
                }
            }

            return null;
        }

        public static List<Asteroid> ScanMap360(char[,] map, int stationX, int stationY)
        {
            var dict = new Dictionary<(int x, int y), Asteroid>();

            for (int y = 0; y < map.GetLength(1); y++)
            {
                for (int x = 0; x < map.GetLength(0); x++)
                {
                    if (map[x, y] == '.' || (x == stationX && y == stationY))
                        continue;

                    var deltaX = x - stationX; // Right positive
                    var deltaY = stationY - y; // Down positive

                    var gcd = GCD(Math.Abs(deltaX), Math.Abs(deltaY));
                    var vec = (deltaX / gcd, deltaY / gcd);

                    var asteroid = new Asteroid(x, y, deltaX, deltaY);

                    if(dict.ContainsKey(vec))
                    {
                        if (asteroid.Distance < dict[vec].Distance)
                            dict[vec] = asteroid;
                    }
                    else
                        dict.Add(vec, asteroid);
                }
            }

            return dict.Values
                .OrderByDescending(a => a.Angle > (MathF.PI/2) ? a.Angle - 2 * MathF.PI : a.Angle)
                .ToList();
        }

        public static int GCD(int a, int b)
        {
            while (a != 0 && b != 0)
            {
                if (a > b)
                    a %= b;
                else
                    b %= a;
            }

            return a == 0 ? b : a;
        }
    }
}
