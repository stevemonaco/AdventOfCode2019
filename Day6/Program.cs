using System;
using System.Collections.Generic;
using System.IO;

namespace Day6
{
    public class Program
    {
        static void Main(string[] args)
        {
            string inputFile = "Day6.txt";

            var orbitMap = CreateOrbitMap(File.ReadAllLines("Day6.txt"));

            var orbits = CalculateOrbits(orbitMap);
            Console.WriteLine($"There are {orbits} orbits");

            var transfers = CalculateOrbitalTransfers(orbitMap, "YOU", "SAN");
            Console.WriteLine($"There are {transfers} orbital transfers between 'YOU' and 'SAN'");
        }

        public static int CalculateOrbits(Dictionary<string, Orbit> orbitMap)
        {
            var orbitCount = 0;
            foreach(var orbit in orbitMap.Values)
            {
                var visitor = orbit;
                while(visitor.ParentName != null)
                {
                    orbitCount++;
                    visitor = orbitMap[visitor.ParentName];
                }
            }

            return orbitCount;
        }

        public static int CalculateOrbitalTransfers(Dictionary<string, Orbit> orbitMap,
            string orbitNameA, string orbitNameB)
        {
            var orbitA = orbitMap[orbitNameA];
            var orbitB = orbitMap[orbitNameB];

            var visitor = orbitA;
            var distanceMap = new Dictionary<string, int>();
            var distance = 0;
            while(visitor.ParentName != null)
            {
                distanceMap[visitor.ParentName] = distance;
                visitor = orbitMap[visitor.ParentName];
                distance++;
            }

            visitor = orbitB;
            distance = 0;
            while(visitor.ParentName != null)
            {
                if(distanceMap.TryGetValue(visitor.ParentName, out var distance2))
                {
                    return distance + distance2;
                }
                visitor = orbitMap[visitor.ParentName];
                distance++;
            }

            throw new Exception("Orbits are not connected");
        }

        public static Dictionary<string, Orbit> CreateOrbitMap(string[] orbitLines)
        {
            var orbitMap = new Dictionary<string, Orbit>();

            foreach (var line in orbitLines)
            {
                var orbitPair = line.Split(")");
                var centerName = orbitPair[0];
                var orbiterName = orbitPair[1];

                Orbit center;
                Orbit orbiter;

                if (!orbitMap.TryGetValue(centerName, out center))
                {
                    center = new Orbit(centerName);
                    orbitMap[centerName] = center;
                }
                if (!orbitMap.TryGetValue(orbiterName, out orbiter))
                {
                    orbiter = new Orbit(orbiterName);
                    orbitMap[orbiterName] = orbiter;
                }

                orbiter.ParentName = center.Name;
            }

            return orbitMap;
        }
    }
}
