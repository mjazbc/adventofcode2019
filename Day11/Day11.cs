using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Util;

namespace Day11
{
    public class Day11 : AdventPuzzle
    {
        public override string SolveFirstPuzzle()
        {
            var code = ReadInputText<string>().Split(',').Select(long.Parse).ToArray();
            var map = new Dictionary<(int y, int x), int>();
            bool halt = false; long output1; long output2;
            var exe = new Executable(null, code);

            (int x, int y) pos = (0, 0);
            map[pos] = 0;
            Direction dir = Direction.Up;

            while (!halt)
            {
                int color = map.GetValueOrDefault(pos, 0);

                exe.AddInput(color);
                (output1, halt) = exe.Execute();
                (output2, halt) = exe.Execute();

                map[pos] = (int)output1;

                dir = ChangeDirection(output2, dir);
                pos = Move(pos, dir);
            }

            return map.Keys.Count().ToString();
        }

        private static (int x, int y) Move((int x, int y) pos, Direction dir)
        {
            switch (dir)
            {
                case Direction.Up: pos = (pos.x, pos.y + 1); break;
                case Direction.Right: pos = (pos.x + 1, pos.y); break;
                case Direction.Down: pos = (pos.x, pos.y - 1); break;
                case Direction.Left: pos = (pos.x - 1, pos.y); break;
            }

            return pos;
        }

        private static Direction ChangeDirection(long output2, Direction dir)
        {
            if (output2 == 0)
            {
                if (((int)dir - 1) < 1)
                    dir = Direction.Left;
                else
                    dir -= 1;
            }
            else if (output2 == 1)
            {
                if (((int)dir + 1) > 4)
                    dir = Direction.Up;
                else
                    dir += 1;
            }

            return dir;
        }

        public override string SolveSecondPuzzle()
        {
            var code = ReadInputText<string>().Split(',').Select(long.Parse).ToArray();
            var map = new Dictionary<(int y, int x), int>();
            bool halt = false; long output1; long output2;
            var exe = new Executable(null, code);

            (int x, int y) pos = (0, 0);
            map[pos] = 1;
            Direction dir = Direction.Up;

            while (!halt)
            {
                int color = map.GetValueOrDefault(pos, 0);

                exe.AddInput(color);
                (output1, halt) = exe.Execute();
                (output2, halt) = exe.Execute();

                map[pos] = (int)output1;

                dir = ChangeDirection(output2, dir);
                pos = Move(pos, dir);
            }

            StringBuilder sb = new StringBuilder();
            sb.Append(Environment.NewLine);

            for (int j = map.Keys.Max(x => x.x); j >= map.Keys.Min(x => x.x); j--)
            {
                for (int i = map.Keys.Min(y=>y.y); i <= map.Keys.Max(y => y.y); i++)
                {
                    sb.Append(map.GetValueOrDefault((i,j), 0) == 0 ? " " : "#");
                }

                sb.Append(Environment.NewLine);
            }
            return sb.ToString();
        }

        public enum Direction
        {
            Up =1,
            Right,
            Down,
            Left
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
        public Executable(int? phase, long[] intCode)
        {
            _inputBuffer = new Queue<long>();
            if(phase.HasValue)
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
                            SetValue(code.Item3, 1, _inputBuffer.Dequeue());
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
                    throw new Exception("nekaj je odfukalo");

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
