using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Util;

namespace Day21
{
    public class Day21 : AdventPuzzle
    {
        public override string SolveFirstPuzzle()
        {

            var code = ReadInputText<string>().Split(',').Select(long.Parse).ToArray();
            var exe = new Executable(null, code);

            string instructions = File.ReadAllText("../../../springscript1.txt").Replace("\r", "");

            return ExecuteSpringscript(exe, instructions).ToString();
        }

        public override string SolveSecondPuzzle()
        {
            var code = ReadInputText<string>().Split(',').Select(long.Parse).ToArray();
            var exe = new Executable(null, code);

            string instructions = File.ReadAllText("../../../springscript2.txt").Replace("\r", "");

            return ExecuteSpringscript(exe, instructions).ToString();
        }

        private long ExecuteSpringscript(Executable exe, string instructions)
        {
            foreach (var ch in instructions)
                exe.AddInput(ch);

            while (true)
            {
                (long output, bool halt) = exe.Execute();
                if (halt)
                    break;

                if (output > 500)
                    return output;
                else
                    Console.Write((char)output);
            }

            throw new Exception("Something went wrong");
        }
    }
}
