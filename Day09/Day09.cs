using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Util;

namespace Day09
{
    public class Day09 : AdventPuzzle
    {
        public override string SolveFirstPuzzle()
        {
            var code = ReadInputText<string>().Split(',').Select(int.Parse).ToArray();
            var exe = new Executable(1, (int[])code.Clone());

            (long output, bool halt) = exe.Execute();

            return output.ToString();
             
        }

        public override string SolveSecondPuzzle()
        {
            var code = ReadInputText<string>().Split(',').Select(int.Parse).ToArray();
            var exe = new Executable(2, (int[])code.Clone());

            (long output, bool halt) = exe.Execute();

            return output.ToString();
        }
    }
    public class Executable
    {
        private Dictionary<long, long> _intCode;
        private long _idx;
        public bool Initialized = false;
        private long _output = 0;
        private Queue<long> _inputBuffer;
        private long _relativeBase = 0;
        public Executable(int phase, int[] intCode)
        {
            _inputBuffer = new Queue<long>();
            AddInput(phase);

            _intCode = new Dictionary<long, long>();
            for (int i = 0; i < intCode.Length; i++)
            {
                _intCode[i] = intCode[i];
            }

            _idx = 0;
        }

        public void AddInput(int input)
        {
            _inputBuffer.Enqueue(input);
        }

        public (long, bool) Execute()
        {
            var code = ParseCommand(_intCode[_idx]);

            while (code.Item4 != 99)
            {
                switch (code.Item4)
                {
                    case 1:
                        {
                            long first = GetValue(code.Item3, 1);
                            long second = GetValue(code.Item2, 2);

                            SetValue(code.Item1, 3, first + second);

                            _idx += 4;
                            break;
                        }
                    case 2:
                        {
                            long first = GetValue(code.Item3, 1);
                            long second = GetValue(code.Item2, 2);

                            SetValue(code.Item1, 3, first * second);

                            _idx += 4;
                            break;
                        }
                    case 3:
                        {
                            SetValue(code.Item3, 1, _inputBuffer.Dequeue());
                            _idx += 2;
                            break;
                        }
                    case 4:
                        {
                            _output = GetValue(code.Item3, 1);
                            _idx += 2;
                            //Console.WriteLine(_output);
                            break;
                            //return (_output, false);
                        }
                    case 5:
                        {
                            bool jump = GetValue(code.Item3, 1) != 0;
                            if (jump)
                                _idx = GetValue(code.Item2, 2);
                            else
                                _idx += 3;

                            break;
                        }
                    case 6:
                        {
                            bool jump = GetValue(code.Item3, 1) == 0;
                            if (jump)
                                _idx = GetValue(code.Item2, 2);
                            else
                                _idx += 3;
                            break;
                        }
                    case 7:
                        {
                            long first = GetValue(code.Item3, 1);
                            long second = GetValue(code.Item2, 2);

                            SetValue(code.Item1, 3, first < second ? 1 : 0);

                            _idx += 4;
                            break;
                        }
                    case 8:
                        {
                            long first = GetValue(code.Item3, 1);
                            long second = GetValue(code.Item2, 2);

                            SetValue(code.Item1, 3, first == second ? 1 : 0);

                            _idx += 4;
                            break;
                        }
                    case 9:
                        {
                            long offset = GetValue(code.Item3, 1);
                            _relativeBase += offset;

                            _idx += 2;
                            break;
                        }
                }
                code = ParseCommand(_intCode[_idx]);
            }

            return (_output, true);
        }

        private void SetValue(long mode, long element, long value)
        {
            switch (mode)
            {
                case 0:
                    _intCode.GetValueOrDefault(_intCode[_idx + element]);
                    _intCode[_intCode[_idx + element]] = value;
                    break;
                case 2:
                    {
                        long addr = _relativeBase + _intCode[_idx + element];
                        _intCode[_relativeBase + _intCode[_idx + element]] = value;
                        break;
                    }
                default:
                    throw new Exception("nekaj je odfukalo");

            }
        }

        private long GetValue(long mode, long element)
        {
            switch(mode)
            {
                case 0:
                    return _intCode.GetValueOrDefault(_intCode[_idx + element]);
                case 1:
                    return _intCode.GetValueOrDefault(_idx + element);
                case 2:
                    {   
                        return _intCode.GetValueOrDefault(_relativeBase + _intCode[_idx + element]);
                    }
                default:
                    throw new Exception("nekaj je odfukalo");
                   
            }
        }

        private (long, long, long, long) ParseCommand(long code) => ((code / 10000) % 10, (code / 1000) % 10, (code / 100) % 10, code % 100);
    }
}
