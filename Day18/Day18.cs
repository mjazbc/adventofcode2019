using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Util;

namespace Day18
{
    public class Day18 : AdventPuzzle
    {
        public override string SolveFirstPuzzle()
        {
            var map = ReadInputArray<string>().Select(x => x.ToCharArray()).ToArray();

            var start = map.IndexOf('@');
        }

        public int FillOxygen((int x, int y) pos, Dictionary<char,(int dist, List<char> keys)> distances,
            char[][] map)
        {

            var adjecant = new (int x, int y)[] { (0, 1), (0, -1), (1, 0), (-1, 0) };
            var q = new Queue<((int x, int y), int)>();
            var discovered = new HashSet<(int x, int y)>();

            var maxDist = 0;
            discovered.Add(pos);
            q.Enqueue((pos, 0));
            while (q.Count > 0)
            {
                var v = q.Dequeue();

                if (!map.ContainsKey(v.Item1) || map[v.Item1] == Status.Wall)
                    continue;

                map[v.Item1] = Status.Oxygen;
                DrawMap(map, null, null);

                if (v.Item2 > maxDist)
                    maxDist = v.Item2;

                foreach (var adj in adjecant)
                {
                    var w = (v.Item1.x + adj.x, v.Item1.y + adj.y);
                    if (!discovered.Contains(w))
                    {
                        discovered.Add(w);
                        q.Enqueue((w, v.Item2 + 1));
                    }
                }
            }
            return maxDist;
        }


        public override string SolveSecondPuzzle()
        {
            throw new NotImplementedException();
        }
    }

    public static class ArrayEx
    {
        public static (int x, int y) IndexOf(this char[][] matrix, char value)
        {
            int h = matrix.GetLength(1); // height
            int w = matrix.GetLength(0); // width

            for (int x = 0; x < w; ++x)
            {
                for (int y = 0; y < h; ++y)
                {
                    if (matrix[x][y].Equals(value))
                        return (x, y);
                }
            }
            return (-1, -1);
        } 
    }

}
