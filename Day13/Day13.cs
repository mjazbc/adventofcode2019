using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Util;

namespace Day13
{
    public class Day13 : AdventPuzzle
    {
        private char[] _tiles = { ' ', '#', 'X', '_', 'o' };
        public static Dictionary<(long x, long y), char> _map = new Dictionary<(long x, long y), char>();
        public static long _score = 0;
        public override string SolveFirstPuzzle()
        {
            var code = ReadInputText<string>().Split(',').Select(long.Parse).ToArray();
            var exe = new Executable(null, code);
            bool halt = false;

            int idx = 0;
            int blocksCount = 0;
            while (!halt)
            {
                long output;
                (output, halt) = exe.Execute();
                idx++;

                if (idx % 3 == 0 && output == 2)
                    blocksCount++;
            }

            return blocksCount.ToString();
        }

        public override string SolveSecondPuzzle()
        {
            var code = ReadInputText<string>().Split(',').Select(long.Parse).ToArray();
            code[0] = 2;
            var exe = new Executable(null, code);
            bool halt = false;

            long x;
            long y;
            long tileId;

            long paddleX = 0;
            while (true)
            {
                (x, halt) = exe.Execute();
                (y, halt) = exe.Execute();
                (tileId, halt) = exe.Execute();

                if (halt)
                    break;

                if (x == -1 && y == 0)
                {
                    _score = tileId;
                    continue;
                }

                if(tileId == 3)
                    paddleX = x;
                
                if(tileId == 4)
                {
                    var delta = Math.Sign(x - paddleX);
                    exe.AddInput(delta);
                }

                _map[(x, y)] = _tiles[tileId];
            }
            Console.SetCursorPosition(0,30);
            return _score.ToString();
        }

        private static void PrintGame(Dictionary<(long x, long y), char> map, long score)
        {
            long xSize = map.Keys.Max(pos => pos.x);
            long ySize = map.Keys.Max(pos => pos.y);

            Console.SetCursorPosition(0,5);

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("SCORE: " + score);
            for (int i = 0; i <= ySize; i++)
            {
                for (int j = 0; j <= xSize; j++)
                {
                    sb.Append(map[(j, i)]);
                }
                sb.AppendLine();
            }

            Console.Write(sb.ToString());
        }

        public class Executable
        {
            private Dictionary<long, long> _intCode;
            private long _idx;
            public bool Initialized = false;
            private long _output = 0;
            private Queue<long> _inputBuffer;
            private long _relativeBase = 0;
            public Executable(int? phase, long[] intCode)
            {
                _inputBuffer = new Queue<long>();
                if (phase.HasValue)
                    AddInput(phase.Value);

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
                                PrintGame(_map, _score);
                                Thread.Sleep(10);
                                SetValue(code.Item3, 1, _inputBuffer.Dequeue() );
                                _idx += 2;
                                break;
                            }
                        case 4:
                            {
                                _output = GetValue(code.Item3, 1);
                                _idx += 2;
                                return (_output, false);
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
                        throw new Exception("Unsupported mode "+ mode);

                }
            }
            private long GetValue(long mode, long element)
            {
                switch (mode)
                {
                    case 0:
                        return _intCode.GetValueOrDefault(_intCode[_idx + element]);
                    case 1:
                        return _intCode.GetValueOrDefault(_idx + element);
                    case 2:
                            return _intCode.GetValueOrDefault(_relativeBase + _intCode[_idx + element]);                        
                    default:
                        throw new Exception("Unsupported mode "+ mode);

                }
            }

            private (long, long, long, long) ParseCommand(long code) => ((code / 10000) % 10, (code / 1000) % 10, (code / 100) % 10, code % 100);
        }
    }
}
