using System;
using System.Collections.Generic;
using System.Linq;
using Util;

namespace Day16
{
    class Day16 : AdventPuzzle
    {
        public override string SolveFirstPuzzle()
        {
            int[] input = ReadInputText<string>().Select(x => (int)char.GetNumericValue(x)).ToArray();

            var result = ExecuteFft(input);
            return string.Join("", result[0..8]);
        }

        public override string SolveSecondPuzzle()
        {
            var input = ReadInputText<string>().Select(x => (int)char.GetNumericValue(x)).ToList();

            var longer = new List<int>();
            for(int i = 0; i< 10000; i++)
                longer.AddRange(input);
            
            int offset = int.Parse(string.Join("", input.Take(7)));

            longer = longer.Skip(offset).ToList();

            var result = Execute2(longer.ToArray());

            return string.Join("",result).ToString();
        }

        private int[] Execute2(int[] input)
        {
            for (int phase = 0; phase < 100; phase++)
            {
                int prev = 0;
                for (int j = input.Length -1; j >= 0; j--)
                {
                    input[j] = (input[j] + prev) % 10;
                    prev = input[j];
                }
            }

            return input.Take(8).ToArray();
                
        }

        private int[] ExecuteFft(int[] input)
        {
            int[] originalPattern = new[] { 0, 1, 0, -1 };
            int[] newInput = new int[input.Length];

            for (int phase = 0; phase < 100; phase++)
            {
                int patternRepeat = 1;
                var pattern = new[] { 0, 1, 0, -1 };
                for (int k = 0; k < input.Length; k++)
                {
                    int sum = 0;
                    for (int i = 0; i < input.Length; i++)
                        sum += pattern[(i + 1) % pattern.Length] * input[i];

                    newInput[k] = Math.Abs(sum) % 10;
                    pattern = ExtendPattern(originalPattern, ++patternRepeat);
                }

                input = (int[])newInput.Clone();
                Console.WriteLine(string.Join("", input));
            }

            return input;
        }

        public int[] ExtendPattern(int[] pattern, int repeat)
        {
            var newPattern = new List<int>();
            foreach (int i in pattern)
            {
                for (int j = 0; j < repeat; j++)
                {
                    newPattern.Add(i);
                }
            }

            return newPattern.ToArray();
        }

       
    }
}
