using System;
using System.Collections.Generic;
using System.Linq;
using Util;

namespace Day07
{
    public class Day07 : AdventPuzzle
    {
        public override string SolveFirstPuzzle()
        {
            int[] code = ReadInputText<string>().Split(',').Select(int.Parse).ToArray();

            int maxOut = 0;
            foreach (var permutation in Permutate("12345"))
            {
                Executable[] executables = new Executable[5];

                for (int i = 0; i < permutation.Length; i++)
                {
                    executables[i] = new Executable((int)char.GetNumericValue(permutation[i]) -1 , (int[])code.Clone());
                }

                int output = 0;
                for (int i = 0; i < permutation.Length; i++)
                {
                    executables[i].AddInput(output);
                    (output, _) = executables[i].Execute();
                }

                if (output > maxOut)
                    maxOut = output;
            }

            return maxOut.ToString();
        }

        public override string SolveSecondPuzzle()
        {
            int[] code = ReadInputText<string>().Split(',').Select(int.Parse).ToArray();
            int maxOut = 0;
            foreach (var permutation in Permutate("98765"))
            {
                Executable[] executables = new Executable[5];
                int output = 0;
                bool halt = false;
                
                for (int i = 0; i < permutation.Length; i++)
                {             
                    executables[i] = new Executable((int)char.GetNumericValue(permutation[i]), (int[]) code.Clone());               
                }
                
                while (!halt)
                {
                    for (int i = 0; i < permutation.Length; i++)
                    {
                        executables[i].AddInput(output);
                        (output, halt) = executables[i].Execute();
                    }
                }

                if (output > maxOut)
                    maxOut = output;
            }

            return maxOut.ToString();
        }

        private static IEnumerable<string> Permutate(string source)
        {
            if (source.Length == 1) 
                return new List<string> { source };

            var permutations = from c in source
                               from p in Permutate(new String(source.Where(x => x != c).ToArray()))
                               select c + p;

            return permutations;
        }
    }

    public class Executable
    {
        private readonly int[] _intCode;
        private int _idx;
        public bool Initialized = false;
        private int _output = 0;
        private Queue<int> _inputBuffer;
        public Executable(int phase, int[] intCode)
        {
            _inputBuffer = new Queue<int>();
            AddInput(phase);
            _intCode = intCode;
            _idx = 0;
        }

        public void AddInput(int input)
        {
            _inputBuffer.Enqueue(input);
        }

        public (int, bool) Execute()
        {
            (int, int, int) code = ParseCommand(_intCode[_idx]);

            while (code.Item3 != 99)
            {
                switch (code.Item3)
                {
                    case 1:
                        {
                            int first = code.Item2 == 0 ? _intCode[_intCode[_idx + 1]] : _intCode[_idx + 1];
                            int second = code.Item1 == 0 ? _intCode[_intCode[_idx + 2]] : _intCode[_idx + 2];

                            _intCode[_intCode[_idx + 3]] = first + second;

                            _idx += 4;
                            break;
                        }
                    case 2:
                        {
                            int first = code.Item2 == 0 ? _intCode[_intCode[_idx + 1]] : _intCode[_idx + 1];
                            int second = code.Item1 == 0 ? _intCode[_intCode[_idx + 2]] : _intCode[_idx + 2];

                            _intCode[_intCode[_idx + 3]] = first * second;

                            _idx += 4;
                            break;
                        }
                    case 3:
                        {
                            _intCode[_intCode[_idx + 1]] = _inputBuffer.Dequeue();
                            _idx += 2;
                            break;
                        }
                    case 4:
                        {
                            _output = _intCode[_intCode[_idx + 1]];
                            _idx += 2;
                            return (_output, false);
                        }
                    case 5:
                        {
                            bool jump = (code.Item2 == 0 ? _intCode[_intCode[_idx + 1]] : _intCode[_idx + 1]) != 0;
                            if (jump)
                                _idx = code.Item1 == 0 ? _intCode[_intCode[_idx + 2]] : _intCode[_idx + 2];
                            else
                                _idx += 3;

                            break;
                        }
                    case 6:
                        {
                            bool jump = (code.Item2 == 0 ? _intCode[_intCode[_idx + 1]] : _intCode[_idx + 1]) == 0;
                            if (jump)
                                _idx = code.Item1 == 0 ? _intCode[_intCode[_idx + 2]] : _intCode[_idx + 2];
                            else
                                _idx += 3;
                            break;
                        }
                    case 7:
                        {
                            int first = code.Item2 == 0 ? _intCode[_intCode[_idx + 1]] : _intCode[_idx + 1];
                            int second = code.Item1 == 0 ? _intCode[_intCode[_idx + 2]] : _intCode[_idx + 2];

                            _intCode[_intCode[_idx + 3]] = first < second ? 1 : 0;

                            _idx += 4;
                            break;
                        }
                    case 8:
                        {
                            int first = code.Item2 == 0 ? _intCode[_intCode[_idx + 1]] : _intCode[_idx + 1];
                            int second = code.Item1 == 0 ? _intCode[_intCode[_idx + 2]] : _intCode[_idx + 2];

                            _intCode[_intCode[_idx + 3]] = first == second ? 1 : 0;

                            _idx += 4;
                            break;
                        }
                }
                code = ParseCommand(_intCode[_idx]);
            }

            return (_output, true);
        }
        private (int, int, int) ParseCommand (int code) => ((code / 1000) % 10, (code / 100) % 10, code % 100);
    }

}
