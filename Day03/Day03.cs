using System;
using System.Collections.Generic;
using System.Linq;
using Util;

namespace Day03
{
    class Day03 : AdventPuzzle
    {
        public override string SolveFirstPuzzle()
        {
            var wireDirections = ReadInputArray<string>().Select(line => line.Split(',').Select(x => new Direction(x)));
            var wires = GeneratePoints(wireDirections);

            var intersection = wires[0].Intersect(wires[1]);
            var point = intersection.Select(point => Math.Abs(point.Item1) + Math.Abs(point.Item2)).Min();

            return point.ToString();
        }

        public override string SolveSecondPuzzle()
        {
            var wireDirections = ReadInputArray<string>().Select(line => line.Split(',').Select(x => new Direction(x)));
            List<List<(int, int)>> wires = GeneratePoints(wireDirections);

            var intersections = wires[0].Intersect(wires[1]);

            var result = intersections
                .Select(intersection => wires[0].IndexOf(intersection) + wires[1].IndexOf(intersection) + 2)
                .Min();

            return result.ToString();
        }

        private static List<List<(int, int)>> GeneratePoints(IEnumerable<IEnumerable<Direction>> wireDirections)
        {
            var wires = new List<List<(int, int)>>();

            foreach (var wire in wireDirections)
            {
                var w = (0, 0);
                var wirePoints = new List<(int, int)>();

                foreach (var direction in wire)
                {
                    for (int i = 1; i <= direction.Distance; i++)
                    {
                        switch (direction.Dir)
                        {
                            case 'R': wirePoints.Add((++w.Item1, w.Item2)); break;
                            case 'L': wirePoints.Add((--w.Item1, w.Item2)); break;
                            case 'U': wirePoints.Add((w.Item1, ++w.Item2)); break;
                            case 'D': wirePoints.Add((w.Item1, --w.Item2)); break;
                        }
                    }
                }

                wires.Add(wirePoints);
            }

            return wires;
        }
    }

    public class Direction
    {
        public char Dir { get; set; }
        public int Distance { get; set; }

        public Direction(string dirString)
        {
            Dir = dirString[0];
            Distance = int.Parse(dirString.Substring(1));
        }

    }
}
