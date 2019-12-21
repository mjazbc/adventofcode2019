using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Util;

namespace Day17
{
    public class Day17 : AdventPuzzle
    {
        public override string SolveFirstPuzzle()
        {
            var code = ReadInputText<string>().Split(',').Select(long.Parse).ToArray();
            var exe = new Executable(null, code);

            var map = BuildMap(exe);

            int sum = 0;
            for(int i = 1; i < map.Length -1; i++)
            {
                for(int j = 1; j < map[0].Length -1; j++)
                {
                    Console.Write(map[i][j]);
                }
                Console.WriteLine();
            }

            return sum.ToString();
        }

        private bool IsIntersection(int x, int y, char[][] map)
        {
            return map[y][x] == '#' && map[y + 1][x] == '#' && map[y - 1][x] == '#' && map[y][x + 1] == '#' && map[y][x - 1] == '#';
        }

        private char[][] BuildMap(Executable exe)
        {
            long output;
            string line = "";
            var tmpMap = new List<char[]>();

            while (true)
            {
                bool halt;
                (output, halt) = exe.Execute();
                if (halt)
                    break;

                char outChar = (char)output;
                if (outChar == '\n' && line.Length > 0)
                {
                    tmpMap.Add(line.ToCharArray());
                    line = "";
                }
                else
                    line += outChar;
            }

            return tmpMap.ToArray();
        }

        public override string SolveSecondPuzzle()
        {
            var code = ReadInputText<string>().Split(',').Select(long.Parse).ToArray();
            code[0] = 2;
            var exe = new Executable(null, code);
            long output;
            while (true)
            {
                bool halt;
                (output, halt) = exe.Execute();
                if (halt)
                    break;

                Console.Write((char)output);
            }


           return output.ToString();
        }
    }
}
