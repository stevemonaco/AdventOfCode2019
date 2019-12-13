using MoreLinq;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Day11
{
    public enum ProgramState { AwaitPaint, AwaitTurn };
    public enum TurnDirection { Left90 = 0, Right90 = 1 }
    public enum Orientation { North = 0, East = 1, South = 2, West = 3 }
    public class GridNavigator : IInputProvider, IOutputSink
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Orientation Orientation { get; set; } = Orientation.North;
        public Dictionary<(int, int), PaintedPanel> Panels { get; } =
            new Dictionary<(int, int), PaintedPanel>();

        private ProgramState _state = ProgramState.AwaitPaint;
        private IntCodeComputer _computer;
        private Dictionary<Orientation, (int, int)> _moveVectors =
            new Dictionary<Orientation, (int, int)>
            {
                { Orientation.North, (0, 1) },
                { Orientation.South, (0, -1) },
                { Orientation.East, (1, 0) },
                { Orientation.West, (-1, 0) },
            };

        public GridNavigator(string program)
        {
            _computer = new IntCodeComputer(this);
            _computer.OutputSinks.Add(this);
            _computer.LoadProgramFromString(program);
            Panels[(0, 0)] = new PaintedPanel()
            {
                X = 0,
                Y = 0,
                Color = PanelColor.White,
                Count = 1
            };
        }

        public void Execute()
        {
            _computer.ExecuteProgram();
        }

        public BigInteger GetInput()
        {
            if (Panels.ContainsKey((X, Y)))
                return new BigInteger((int)Panels[(X, Y)].Color);
            return 0;
        }

        public void SendOutput(BigInteger output)
        {
            if (_state == ProgramState.AwaitPaint)
            {
                Paint((PanelColor)(int)output);
                _state = ProgramState.AwaitTurn;
            }
            else if (_state == ProgramState.AwaitTurn)
            {
                TurnAndAdvance((TurnDirection)(int)output);
                _state = ProgramState.AwaitPaint;
            }
        }

        public char[,] CreatePrintableGrid()
        {
            int minX = Panels.Values.MinBy(p => p.X).First().X;
            int minY = Panels.Values.MinBy(p => p.Y).First().Y;
            int maxX = Panels.Values.MaxBy(p => p.X).First().X;
            int maxY = Panels.Values.MaxBy(p => p.Y).First().Y;

            var grid = new char[maxX - minX + 1, maxY - minY + 1];
            for (int y = 0; y < grid.GetLength(1); y++)
                for (int x = 0; x < grid.GetLength(0); x++)
                    grid[x, y] = ' ';

            foreach(var panel in Panels.Values)
            {
                int x = panel.X - minX;
                int y = (grid.GetLength(1) - 1) - (panel.Y - minY);
                grid[x, y] = panel.Color == PanelColor.Black ? ' ' : 'X';
            }

            return grid;
        }

        private void Paint(PanelColor color)
        {
            if (Panels.ContainsKey((X, Y)))
            {
                Panels[(X, Y)].Color = color;
                Panels[(X, Y)].Count += 1;
            }
            else
            {
                Panels[(X, Y)] = new PaintedPanel()
                {
                    X = X,
                    Y = Y,
                    Count = 1,
                    Color = color
                };
            }
        }

        private void TurnAndAdvance(TurnDirection direction)
        {
            switch(Orientation)
            {
                case Orientation.North:
                    Orientation = direction == TurnDirection.Left90 ? Orientation.West : Orientation.East;
                    break;
                case Orientation.West:
                    Orientation = direction == TurnDirection.Left90 ? Orientation.South : Orientation.North;
                    break;
                case Orientation.South:
                    Orientation = direction == TurnDirection.Left90 ? Orientation.East : Orientation.West;
                    break;
                case Orientation.East:
                    Orientation = direction == TurnDirection.Left90 ? Orientation.North : Orientation.South;
                    break;
            }

            X += _moveVectors[Orientation].Item1;
            Y += _moveVectors[Orientation].Item2;
        }
    }
}
