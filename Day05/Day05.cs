using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Util;

namespace Day05
{
    public class Day05 : AdventPuzzle
    {
        public override string SolveFirstPuzzle()
        {
            int[] input = ReadInputText<string>().Split(',').Select(int.Parse).ToArray();
            int output = ExecuteProgram(input, 1);

            return output.ToString();
        }

        public override string SolveSecondPuzzle()
        {
            int[] input = ReadInputText<string>().Split(',').Select(int.Parse).ToArray();
            int output = ExecuteProgram(input, 5);

            return output.ToString();
        }

        private int ExecuteProgram(int[] intCode, int input)
        {
            int output = 0;
            int idx = 0;

            (int, int, int) code = ParseOpcode(intCode[idx]);

            while (code.Item3 != 99)
            {
                switch(code.Item3)
                {
                    case 1: 
                        {
                            int first = code.Item2 == 0 ? intCode[intCode[idx + 1]] : intCode[idx + 1];
                            int second = code.Item1 == 0 ? intCode[intCode[idx + 2]] : intCode[idx + 2];

                            intCode[intCode[idx + 3]] = first + second;

                            idx += 4;
                            break;
                        }
                    case 2:
                        {
                            int first = code.Item2 == 0 ? intCode[intCode[idx + 1]] : intCode[idx + 1];
                            int second = code.Item1 == 0 ? intCode[intCode[idx + 2]] : intCode[idx + 2];

                            intCode[intCode[idx + 3]] = first * second;

                            idx += 4;
                            break;
                        }
                    case 3:
                        {
                            intCode[intCode[idx + 1]] = input;
                            idx += 2;
                            break;
                        }
                    case 4:
                        {
                            output = intCode[intCode[idx + 1]];
                            idx += 2;
                            break;
                        }
                    case 5:
                        {
                            bool jump = (code.Item2 == 0 ? intCode[intCode[idx + 1]] : intCode[idx + 1]) != 0;
                            if (jump)
                                idx = code.Item1 == 0 ? intCode[intCode[idx + 2]] : intCode[idx + 2];
                            else
                                idx += 3;
                            
                            break;
                        }
                    case 6:
                        {
                            bool jump = (code.Item2 == 0 ? intCode[intCode[idx + 1]] : intCode[idx + 1]) == 0;
                            if (jump)
                                idx = code.Item1 == 0 ? intCode[intCode[idx + 2]] : intCode[idx + 2];
                            else
                                idx += 3;
                            break;
                        }
                    case 7:
                        {
                            int first = code.Item2 == 0 ? intCode[intCode[idx + 1]] : intCode[idx + 1];
                            int second = code.Item1 == 0 ? intCode[intCode[idx + 2]] : intCode[idx + 2];

                            intCode[intCode[idx + 3]] = first < second ? 1 : 0;

                            idx += 4;
                            break;
                        }
                    case 8:
                        {
                            int first = code.Item2 == 0 ? intCode[intCode[idx + 1]] : intCode[idx + 1];
                            int second = code.Item1 == 0 ? intCode[intCode[idx + 2]] : intCode[idx + 2];

                            intCode[intCode[idx + 3]] = first == second ? 1 : 0;

                            idx += 4;
                            break;
                        }
                }

                code = ParseOpcode(intCode[idx]);

            }

            return output;
        }

        private (int, int, int) ParseOpcode(int code) => ((code / 1000) % 10, (code / 100) % 10, code % 100);

    }
    
}
