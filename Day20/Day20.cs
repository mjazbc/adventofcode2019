﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Util;

namespace Day20
{
    public class Day20 : AdventPuzzle
    {
        public override string SolveFirstPuzzle()
        {
            var maze = ReadInputArray<string>().Select(x => x.ToCharArray()).ToArray();
            var portals = FindPortals(maze);

            var tmp = portals.Single(x => x.Name == "AA");
            portals.Remove(tmp);
            var startPos = (tmp.X, tmp.Y, 0);

            tmp = portals.Single(x => x.Name == "ZZ");
            portals.Remove(tmp);
            var endPos = (tmp.X, tmp.Y, 0);

            int result = Bfs(startPos, endPos, maze, portals, Adjecant);

            return result.ToString();
        }

        public override string SolveSecondPuzzle()
        {
            var maze = ReadInputArray<string>().Select(x => x.ToCharArray()).ToArray();
            var portals = FindPortals(maze);

            var tmp = portals.Single(x => x.Name == "AA");
            portals.Remove(tmp);
            var startPos = (tmp.X, tmp.Y, 0);

            tmp = portals.Single(x => x.Name == "ZZ");
            portals.Remove(tmp);
            var endPos = (tmp.X, tmp.Y, 0);

            int result = Bfs(startPos, endPos, maze, portals, AdjecantRecursive);

            return result.ToString();
        }

        private HashSet<Portal> FindPortals(char[][] maze)
        {
            var portals = new HashSet<Portal>();
            for (int i = 2; i < maze.Length - 2; i++)
            {
                for (int j = 2; j < maze.First().Length - 2; j++)
                {
                    if (maze[i][j] != '.')
                        continue;
                    
                    var portal = FindPortal(j, i, maze);

                    if (portal == null)                    
                        continue;
                    
                    var existing = portals.FirstOrDefault(x => x.Name == portal.Name);
                    if (existing != null)
                    {
                        existing.Other = portal;
                        portal.Other = existing;
                    }
                    portals.Add(portal);
                }
            }

            return portals;
        }
        private Portal FindPortal(int x, int y, char[][] map)
        {
            int maxX = map[0].Length -1;
            int maxY = map.Length - 1;

            var first = map[y - 1][x];
            var second = map[y - 2][x];
            if (char.IsLetter(first) && char.IsLetter(second))
            {
                return CreatePortal(x, y, "" + second + first, maxX, maxY);
            }

            first = map[y + 1][x];
            second = map[y + 2][x];
            if (char.IsLetter(first) && char.IsLetter(second))
            {
                return CreatePortal(x, y, "" +first + second ,maxX, maxY);
            }

            first = map[y][x - 1];
            second = map[y][x - 2];
            if (char.IsLetter(first) && char.IsLetter(second))
            {
                return CreatePortal(x, y, "" +second + first , maxX, maxY);
            }

            first = map[y][x + 1];
            second = map[y][x + 2];
            if (char.IsLetter(first) && char.IsLetter(second))
            {
                return CreatePortal(x, y, "" + first + second, maxX, maxY);
            }

            return null;

        }

        private static Portal CreatePortal(int x, int y, string name, int maxX, int maxY)
        {
            return  new Portal
            {
                X = x,
                Y = y,
                Name = name,
                Dir = (x <= 2 || x >= maxX - 2 || y <= 2 || y >= maxY - 2) ? Portal.Direction.Out : Portal.Direction.In      
            };
        }

        public int Bfs((int x, int y, int level) pos, (int x, int y, int level) end, char[][] map, HashSet<Portal> portals,
            Func<(int x, int y, int level), HashSet<Portal>, IEnumerable<(int x, int y, int level)>> generateAdjecant)
        {
            var q = new Queue<((int x, int y, int level), int)>();
            var discovered = new HashSet<(int x, int y, int level)>();

            discovered.Add(pos);
            q.Enqueue((pos, 0));
            while (q.Count > 0)
            {
                int dist;
                (pos, dist) = q.Dequeue();

                if (!IsValid(pos.x, pos.y, map))
                    continue;

                if (pos == end)
                    return dist;

                foreach (var w in generateAdjecant(pos, portals))
                {
                    if (discovered.Contains(w))
                        continue;
                    
                    discovered.Add(w);
                    q.Enqueue((w, dist + 1));
                }
            }

            throw new Exception("not found");
        }

        private IEnumerable<(int x, int y, int level)> Adjecant((int x, int y, int level) pos, HashSet<Portal> portals)
        {
            var currentPortal = portals.SingleOrDefault(portal => portal.X == pos.x && portal.Y == pos.y);

            if (currentPortal != null)
                yield return (currentPortal.Other.X, currentPortal.Other.Y, pos.level);

            foreach (var adj in new (int x, int y)[] { (0, 1), (0, -1), (1, 0), (-1, 0) })
                yield return (pos.x + adj.x, pos.y + adj.y, pos.level);

        }

        private IEnumerable<(int x, int y, int level)> AdjecantRecursive((int x, int y, int level) pos, HashSet<Portal> portals)
        {
            var currentPortal = portals.SingleOrDefault(portal => portal.X == pos.x && portal.Y == pos.y);

            //No out portals on outmost level
            if (pos.level == 0 && currentPortal?.Dir == Portal.Direction.Out)
                currentPortal = null;

            if (currentPortal != null) 
                yield return (currentPortal.Other.X, currentPortal.Other.Y, 
                    pos.level + (currentPortal.Dir == Portal.Direction.Out ? -1 : 1));

            foreach (var adj in new (int x, int y)[] { (0, 1), (0, -1), (1, 0), (-1, 0) })
                yield return (pos.x + adj.x, pos.y + adj.y, pos.level);

        }

        private bool IsValid(int x, int y, char[][] map)
        {
            return !(x < 2 || x > map.First().Length - 2 || y < 2 || y > map.Length - 2 || map[y][x] != '.');
        }
    }

    public class Portal
    {
        public int X { get; set; }
        public int Y { get; set; }

        public string Name { get; set; }

        public Direction Dir { get; set; }
        public Portal Other { get; set; }

        public override string ToString() => $"{Name}\t({X},{Y})";

        public enum Direction
        {
            In,
            Out
        }
    }
}
