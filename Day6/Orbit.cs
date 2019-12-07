using System;
using System.Collections.Generic;
using System.Text;

namespace Day6
{
    public class Orbit
    {
        public string Name { get; set; }
        public string ParentName { get; set; }

        public Orbit(string name)
        {
            Name = name;
        }

        public Orbit(string name, string parentName)
        {
            Name = name;
            ParentName = parentName;
        }
    }
}
