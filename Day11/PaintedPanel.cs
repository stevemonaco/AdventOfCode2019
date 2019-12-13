using System;
using System.Collections.Generic;
using System.Text;

namespace Day11
{
    public enum PanelColor { Black = 0, White = 1 }
    public class PaintedPanel
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Count { get; set; }
        public PanelColor Color { get; set; }
    }
}
