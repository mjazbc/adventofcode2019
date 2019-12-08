using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Util;

namespace Day04
{
    class Day04 : AdventPuzzle
    {
        public override string SolveFirstPuzzle()
        {
            int[] range = ReadInputText<string>().Split('-').Select(int.Parse).ToArray();
            int count = 0;

            Regex r = new Regex(@"(.)\1");

            for(int pwd = range[0]; pwd <= range[1]; pwd++)
            {

                if (r.IsMatch(pwd.ToString()) && string.Join("", pwd.ToString().OrderBy(a => a)) == pwd.ToString())
                    count++;
            }

            return count.ToString();

        }

        public override string SolveSecondPuzzle()
        {     
            int[] range = ReadInputText<string>().Split('-').Select(int.Parse).ToArray();
            int count = 0;

            Regex r = new Regex(@"(.)\1{1,}");

            for (int pwd = range[0]; pwd <= range[1]; pwd++)
            {
                if (r.Matches(pwd.ToString()).Any(m => m.Length == 2) && string.Join("", pwd.ToString().OrderBy(a => a)) == pwd.ToString())
                    count++;
            }

            return count.ToString();
        }
    }
    
}
