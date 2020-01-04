using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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
            map[start.y][start.x] = '.';

            int allKeys = map.Sum(x => x.Count(c => char.IsLower(c)));
            int allKeysBin = (int)Math.Pow(2, allKeys) - 1;

            var startPos = new NodeWithDist<Bot>
            {
                node = new Bot
                {
                    X = start.x,
                    Y = start.y,
                    Keys = 0,
                },

                Dist = 0
            };

            int pathLen = Bfs(startPos, map, allKeysBin);

            return pathLen.ToString();
        }
 
        public override string SolveSecondPuzzle()
        {
            var map = ReadInputArray<string>().Select(x => x.ToCharArray()).ToArray();
            var nodes = ModifyMapAndGetStartNodes(map);

            var keys = map.SelectMany(c => c)
                .Where(c => char.IsLower(c))
                .Select(c => map.IndexOf(c));

            int allKeysBin = (int)Math.Pow(2, keys.Count()) - 1;
            var keysForBot = KeysForBot(map, keys);

            int pathLen = Bfs(nodes, map, allKeysBin, keysForBot);

            return pathLen.ToString();

        }

        private int[] KeysForBot(char[][] map, IEnumerable<(int x, int y)> keys)
        {
            int w = map[0].Length / 2;
            int h = map.Length / 2;

            var keysForBot = new int[4];

            foreach(var (x, y) in keys)
            {
                if (x < w && y < h)
                    keysForBot[0]++;
                else if (x >= w && y < h)
                    keysForBot[1]++;
                else if (x < w && y >= h)
                    keysForBot[2]++;
                else if (x >= w && y >= h)
                    keysForBot[3]++;
            }

            return keysForBot;
        }

        private NodeWithDist<Bots> ModifyMapAndGetStartNodes(char[][] map)
        {
            var start = map.IndexOf('@');
            map[start.y][start.x] = '#';
            foreach (var (x, y) in new (int x, int y)[] { (0, 1), (0, -1), (1, 0), (-1, 0) })
                map[start.y + y][start.x + x] = '#';

            var bots = new Bots
            {
                BotPos = new (int y, int x)[4],
                k = new int[4]
            };

            int i = 0;
            foreach (var (x, y) in new (int x, int y)[] { (-1, -1), (1, -1), (-1, 1), (1, 1) })
            {
                bots.BotPos[i++] = (start.y + y, start.x + x);        
            }
            
            return new NodeWithDist<Bots>
            {
                Dist = 0,
                lastMoved = 0,
                node = bots
            };

        }

        public int Bfs(NodeWithDist<Bots> start, char[][] map, int allKeys, int[] keysForBot)
        {
            var discovered = new HashSet<Bots>(new BotsComparer()) 
            { 
                start.node
            };

            var q = new Queue<NodeWithDist<Bots>>();
            q.Enqueue(start);

            while (q.Count > 0)
            {
                var currNode = q.Dequeue();
                //var currBot = ((int x, int y))currNode.GetType().GetProperty($"Bot{currNode.lastMoved}").GetValue(this);
                var currBot = currNode.node.BotPos[currNode.lastMoved];
                var currChar = map[currBot.y][currBot.x];

              
                //Check if on key and pick it up
                if (char.IsLower(currChar)) {
                    int keyMask = (1 << (currChar - 97));
                    if((currNode.node.Keys & keyMask) == 0) { 
                        currNode.node.Keys |= keyMask ;
                        currNode.node.k[currNode.lastMoved]++;
                    };
                }

                //PrintMap(map, currNode);
                //Stop if all keys are found
                if (currNode.node.Keys == allKeys)
                    return currNode.Dist;

                //Add all undiscovered adjecant nodes to queue
                foreach (var w in Adjecant(currNode.node, map, keysForBot))
                {
                    if (discovered.Contains(w.Item1))
                        continue;

                    discovered.Add(w.Item1);

                    var n = new NodeWithDist<Bots>
                    {
                        node = w.Item1,
                        Dist = currNode.Dist + 1,
                        lastMoved = w.Item2

                    };

                    q.Enqueue(n);
                }
            }

            throw new Exception("not found");
        }

        private static void PrintMap(char[][] map, NodeWithDist<Bots> currNode)
        {
            for (int i = 0; i < map.Length; i++)
            {
                for (int j = 0; j < map[0].Length; j++)
                {
                    if (currNode.node.BotPos.Any(b => b.y == i && b.x == j))
                        Console.Write('@');
                    else
                        Console.Write(map[i][j]);
                }
                Console.WriteLine();
            }
     
            Console.WriteLine();
        }

        public int Bfs(NodeWithDist<Bot> start, char[][] map, int allKeys)
        {
            var discovered = new HashSet<Bot>
            {
                start.node
            };

            var q = new Queue<NodeWithDist<Bot>>();
            q.Enqueue(start);

            while (q.Count > 0)
            {
                var currNode = q.Dequeue();
                var currChar = map[currNode.node.Y][currNode.node.X];

                //Skip if on wall or locked door
                if (!IsValid(currNode.node.X, currNode.node.Y, currNode.node.Keys, map))
                    continue;

                //Check if on key and pick it up
                if (char.IsLower(currChar))
                    currNode.node.Keys |=  (1 << (currChar - 97));

                //Stop if all keys are found
                if (currNode.node.Keys == allKeys)
                    return currNode.Dist;

                //Add all undiscovered adjecant nodes to queue
                foreach (var w in Adjecant(currNode.node))
                {
                    if (discovered.Contains(w))
                        continue;

                    discovered.Add(w);

                    var n = new NodeWithDist<Bot>
                    {
                        node = w,
                        Dist = currNode.Dist + 1,
                    };

                    q.Enqueue(n);
                }
            }

            throw new Exception("not found");
        }

        private IEnumerable<Bot> Adjecant(Bot pos)
        {
            foreach (var (x, y) in new (int x, int y)[] { (0, 1), (0, -1), (1, 0), (-1, 0) })
                yield return new Bot
                {
                    X = pos.X + x,
                    Y = pos.Y + y,
                    Keys = pos.Keys
                };
        }

        private IEnumerable<(Bots, int)> Adjecant(Bots pos, char[][]map, int[] keysForBot)
        {
            for (int i = 0; i < 4; i++) {

                if (pos.k[i] == keysForBot[i])
                    continue;

                foreach (var (y, x) in new (int y, int x)[] { (0, 1), (0, -1), (1, 0), (-1, 0) })
                {       
                    var newPos = ((int y, int x)[])pos.BotPos.Clone();
                    newPos[i] = (newPos[i].y + y, newPos[i].x + x);

                    if (!IsValid(newPos[i].x, newPos[i].y, pos.Keys, map))
                        continue;

                    yield return (new Bots
                    {
                        BotPos = newPos,
                        Keys = pos.Keys,
                        k = (int[])pos.k.Clone()
                        
                    }, i);
                }
            }
        }

        private bool IsValid(int x, int y, int keys, char[][] map)
        {
            //Check if inside map
            if (x < 0 || x >= map[0].Length || y < 0 || y >= map.Length)
                return false;

            char current = map[y][x];
            
            //Check if current position is open passage, key or door can be unlocked with current set of keys
            return current == '.' || char.IsLower(current) || (char.IsUpper(current) && ((keys &  (1 << (current-65))) > 0));
        }
    }

    public struct Bot
    {
        public int X;
        public int Y;
        public int Keys;
    }

    public struct NodeWithDist<T>
    {
        public T node;
        public int Dist;
        public int lastMoved;
    }

    public struct Bots
    {
        public (int y, int x)[] BotPos;
        public int[] k;
        public int Keys;
    }

    public class BotsComparer : IEqualityComparer<Bots>
    {
        public bool Equals([AllowNull] Bots x, [AllowNull] Bots y)
        {
            return x.Keys == y.Keys && x.BotPos.SequenceEqual(y.BotPos);
        }

        public int GetHashCode([DisallowNull] Bots obj)
        {
            unchecked {
                int hash = obj.Keys.GetHashCode();
                hash = hash * 23 + obj.BotPos[0].GetHashCode();
                hash = hash * 23 + obj.BotPos[1].GetHashCode();
                hash = hash * 23 + obj.BotPos[2].GetHashCode();
                hash = hash * 23 + obj.BotPos[3].GetHashCode();

                return hash;
            }
        }
    }

    public static class ArrayEx
    {
        public static (int x, int y) IndexOf(this char[][] matrix, char value)
        {
            int w = matrix[0].Length; // height
            int h = matrix.Length; // width

            for (int x = 0; x < w; ++x)
            {
                for (int y = 0; y < h; ++y)
                {
                    if (matrix[y][x].Equals(value))
                        return (x, y);
                }
            }
            return (-1, -1);
        } 
    }

}
