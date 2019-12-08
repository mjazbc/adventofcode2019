using System;
using System.Linq;
using Util;

namespace Day02
{
    class Day02 : AdventPuzzle
    {
        public override string SolveFirstPuzzle()
        {
            int[] input = ReadInputText<string>().Split(',').Select(int.Parse).ToArray();
            int result = ExecuteProgram(input, 12, 2);

            return result.ToString();
        }
        public override string SolveSecondPuzzle()
        {
            int[] input = ReadInputText<string>().Split(',').Select(int.Parse).ToArray();
            
            for(int n = 0; n < 100; n++)
            {
                for(int v = 0; v < 100; v++)
                {
                    int[] inputCopy = (int[]) input.Clone();
                    int result = ExecuteProgram(inputCopy, n, v);

                    if (result == 19690720)
                        return (100 * n + v).ToString();
                }
            }

            return null;
        }

        private int ExecuteProgram(int[] intCode, int noun, int verb)
        {
            intCode[1] = noun;
            intCode[2] = verb;

            int idx = 0;
            int current = intCode[idx];

            while (current != 99)
            {
                if (current == 1)
                    intCode[intCode[idx + 3]] = intCode[intCode[idx + 1]] + intCode[intCode[idx + 2]];
                else if (current == 2)
                    intCode[intCode[idx + 3]] = intCode[intCode[idx + 1]] * intCode[intCode[idx + 2]];

                idx += 4;
                current = intCode[idx];
            }

            return intCode[0];
        }   
    }
}
