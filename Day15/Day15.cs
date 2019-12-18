using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Util;

namespace Day15
{
    class Day15 : AdventPuzzle
    {
        public override string SolveFirstPuzzle()
        {
            var code = ReadInputText<string>().Split(',').Select(long.Parse).ToArray();
            var exe = new Executable(null, code);

            exe.AddInput((int)Direction.North);

            var map = new Dictionary<(int x, int y), Status>();
            var path = new HashSet<(int x, int y)>();

            var found = FindOxygen((0, -1), map, new HashSet<(int, int)>(), exe, path, true);


            return path.Count().ToString();
        }
        public override string SolveSecondPuzzle()
        {
            var code = ReadInputText<string>().Split(',').Select(long.Parse).ToArray();
            var exe = new Executable(null, code);

            exe.AddInput((int)Direction.North);

            var map = new Dictionary<(int x, int y), Status>();
            var path = new HashSet<(int x, int y)>();
            FindOxygen((0, -1), map, new HashSet<(int, int)>(), exe, path, false);
            //DrawMap(map, null, null);
            var result = FillOxygen(path.First(), map);

            return result.ToString();
        }

        public int FillOxygen((int x, int y) pos, Dictionary<(int x, int y), Status> map)
        {
            var adjecant = new (int x, int y)[] { (0, 1), (0, -1), (1, 0), (-1, 0) };
            var q = new Queue<((int x, int y), int)>();
            var discovered = new HashSet<(int x, int y)>();

            var maxDist = 0;
            discovered.Add(pos);
            q.Enqueue((pos, 0));
            while(q.Count > 0)
            {
                var v = q.Dequeue();

                if (!map.ContainsKey(v.Item1) || map[v.Item1] == Status.Wall)
                    continue;

                map[v.Item1] = Status.Oxygen;
                DrawMap(map, null, null);

                if (v.Item2 > maxDist)
                    maxDist = v.Item2;
                
                foreach(var adj in adjecant )
                {
                    var w = (v.Item1.x + adj.x, v.Item1.y + adj.y);
                    if(!discovered.Contains(w))
                    {
                        discovered.Add(w);
                        q.Enqueue((w, v.Item2 + 1));
                    }
                }               
            }
            return maxDist;
        }

        public Status FindOxygen((int x, int y) pos, Dictionary<(int x, int y), Status> map, 
            HashSet<(int, int)> visited, Executable exe, HashSet<(int x, int y)> path, bool returnOxygen)
        {
            (long output, _) = exe.Execute();

            map[pos] = (Status)output;
            Status result;

            //DrawMap(map, pos, null);
            //Thread.Sleep(20);

            if ((Status)output == Status.Oxygen) {
                path.Add(pos);
                return (Status)output; 
            }

            if (map[pos] == Status.Wall || visited.Contains(pos))
                return (Status)output;

            visited.Add(pos);

            exe.AddInput((int)Direction.West);
            result = FindOxygen((pos.x - 1, pos.y), map, visited, exe, path, returnOxygen);
            if (result == Status.Oxygen)
            {
                path.Add(pos);
                if (returnOxygen)
                    return Status.Oxygen;
            }
            if (result != Status.Wall)
            {
                exe.AddInput((int)Direction.East);
                exe.Execute();
            }

            exe.AddInput((int)Direction.East);

            result = FindOxygen((pos.x + 1, pos.y), map, visited, exe, path, returnOxygen);
            if (result == Status.Oxygen)
            {
                path.Add(pos);
                if (returnOxygen)
                    return Status.Oxygen;
            }
            if (result != Status.Wall)
            { 
                exe.AddInput((int)Direction.West);
                exe.Execute();
            }
            exe.AddInput((int)Direction.North);

            result = FindOxygen((pos.x , pos.y - 1), map, visited, exe, path, returnOxygen);
            if (result == Status.Oxygen)
            {
                path.Add(pos);
                if (returnOxygen)
                    return Status.Oxygen;
            }
            if (result != Status.Wall)
            {
                exe.AddInput((int)Direction.South);
                exe.Execute();
            }
            exe.AddInput((int)Direction.South);

            result = FindOxygen((pos.x, pos.y + 1), map, visited, exe, path, returnOxygen);
            if (result == Status.Oxygen)
            {
                path.Add(pos);
                if(returnOxygen)
                    return Status.Oxygen;
            }
            if (result != Status.Wall)
            {
                exe.AddInput((int)Direction.North);
                exe.Execute();
            }

            return Status.Move;
        }
       
        private static void DrawMap(Dictionary<(int x, int y), Status> map, (int x, int y)? pos, 
            HashSet<(int x, int y)> oxygen)
        {
            int xMax = map.Keys.Max(pos => pos.x);
            int yMax = map.Keys.Max(pos => pos.y);
            int xMin = map.Keys.Min(pos => pos.x);
            int yMin = map.Keys.Min(pos => pos.y);

            Console.SetCursorPosition(0, 2);

            StringBuilder sb = new StringBuilder();
            for (int i = yMin; i <= yMax; i++)
            {
                for (int j = xMin; j <= xMax; j++)
                {   
                    if((j,i) == (0,0))
                        sb.Append("D");
                    
                    else if (map.ContainsKey((j, i)))
                    {
                        switch(map[(j, i)])
                        {
                            case Status.Move: sb.Append("."); break;
                            case Status.Oxygen: sb.Append("O"); break;
                            case Status.Wall: sb.Append("#"); break;
                        }
                    }
                    else
                        sb.Append(" ");
                }
                sb.AppendLine();
            }
            Console.WriteLine(sb.ToString());
        }

        public enum Status
        {
            Wall,
            Move,
            Oxygen
        }

        public enum Direction
        {
            North = 1,
            South,
            West,
            East
        }
    }
}
