using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Util;

namespace Day19
{
    public class Day19 : AdventPuzzle
    {
        public override string SolveFirstPuzzle()
        {
            var code = ReadInputText<string>().Split(',').Select(long.Parse).ToArray();
            char[,] map = new char[50,50];

            int countBeam = 0;
            for(int i = 0; i < 50; i++)
            {
                for(int j = 0; j < 50; j++)
                {
                    int output = DeployDrone(code, i, j);
                    countBeam += output;
                    map[i, j] = output == 1 ? '#' : '.';
                }
            }

            return countBeam.ToString();
        }

        public override string SolveSecondPuzzle()
        {
            var code = ReadInputText<string>().Split(',').Select(long.Parse).ToArray();
            HashSet<(int x, int y)> map = new HashSet<(int x, int y)>();

            int countBeam = 0;
            int jstart = 20;
            bool newLine = true;

            for (int i = 500 ; i >= 0; i++)
            {
                countBeam = 0;
                newLine = true;
                for (int j = jstart - 10; j >= 0 ; j++)
                {
                    int output = DeployDrone(code, i, j);
                    if(output == 1) {

                        if (newLine) { 
                            jstart = j;
                            newLine = false;
                        }

                        countBeam++;
                        map.Add((j, i));
                    }

                    //Console.Write(output);

                    if (countBeam >= 100 && CheckArea(j, i, map))
                        return ((j - 99) * 10000 + (i -99)).ToString();

                    if (output == 0 & !newLine)
                        break;
                }
                //Console.Write("       " + countBeam);
                //Console.WriteLine();
            }

            return countBeam.ToString();
        }

        private static bool CheckArea(int x, int y, HashSet<(int x, int y)> map)
        {
            return map.Count(point => point.x <= x && point.y == y) >= 100 && map.Count(point => point.y <= y && point.x == x) >= 100;
        }

        private static int DeployDrone(long[] code, int i, int j)
        {
            var exe = new Executable(null, code);
            exe.AddInput(j);
            exe.AddInput(i);

            (long output, _) = exe.Execute();
            return (int)output;
        }
    }
}
