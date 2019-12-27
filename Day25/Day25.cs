using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Util;

namespace Day25
{
    class Day25 : AdventPuzzle
    {
        public override string SolveFirstPuzzle()
        {

            var code = ReadInputText<string>().Split(',').Select(long.Parse).ToArray();
            var exe = new Executable(null, code);

            //Move to exit
            string instructions = "east\nsouth\nwest\nnorth\neast\nnorth\n";

            //Try all combinations of objects
            instructions += GenerateCombinations();

            foreach (var ch in instructions)
                exe.AddInput(ch);

            Regex r = new Regex(@"\d+");
            string text = "";
            while (true)
            {

                (long output, bool halt) = exe.Execute();

                text += (char)output;
                if ((char)output == '?')
                {
                    //Console.Write(text);
                    text = "";
                }

                if (halt)
                {
                    Console.Write(text);
                    var result = r.Match(text);
                    return result.Value;
                }               
            }
        }

        private static string GenerateCombinations()
        {
            List<string> objects = new List<string>
            {
                "pointer",
                "coin",
                "mug",
                "manifold",
                "hypercube",
                "easter egg",
                "astrolabe",
                "candy cane"
            };

            StringBuilder sb = new StringBuilder();
            sb.Append(string.Join("\n", objects.Select(o => $"drop {o}")));

            for (int i = 1; i < Math.Pow(2, 8) - 1; i++)
            {
                List<string> currentComb = new List<string>();
                string bin = Convert.ToString(i, 2).PadLeft(8,'0');
                for (int j = 0; j < bin.Count(); j++)
                {
                    if (bin[j] == '1')
                        currentComb.Add(objects[j]);
                }

                sb.Append(string.Join("", currentComb.Select(o => $"take {o}\n")));
                sb.Append("east\n");
                sb.Append(string.Join("", currentComb.Select(o => $"drop {o}\n")));
            }

            return sb.ToString();
        }

        public override string SolveSecondPuzzle()
        {
            throw new NotImplementedException();
        }
    }
}
