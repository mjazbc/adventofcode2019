using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Util;

namespace Day08
{
    public class Day08 : AdventPuzzle
    {
        public override string SolveFirstPuzzle()
        {
            int[] input = ReadInputText<string>().Select(x=> (int)char.GetNumericValue(x)).ToArray();
            int width = 25;
            int height = 6;

            List<int[]> layers = new List<int[]>();

            while(input.Length > 0)
            {
                int[] layer = input[0..(width * height)];

                layers.Add(layer);
                input = input[(width * height)..^0];              
            }

            int[] minZeros = layers.Single(x => layers.Min(x => x.Count(y => y == 0)) == x.Count(y => y == 0));

            return (minZeros.Count(x => x == 1) * minZeros.Count(x => x == 2)).ToString();
        }

        public override string SolveSecondPuzzle()
        {
            int[] input = ReadInputText<string>().Select(x => (int)char.GetNumericValue(x)).ToArray();
            int width = 25;
            int height = 6;

            Stack<int[,]> layers = new Stack<int[,]>();

            while (input.Length > 0)
            {
                int[,] layer = new int[height, width];
                for (int i = 0; i < height; i++)
                    for (int j = 0; j < width; j++)
                        layer[i, j] = input[i * width + j];


                layers.Push(layer);
                input = input[(width * height)..^0];
            }

            var combined = CombineLayers(width, height, layers);

            return WriteImage(width, height, combined);
        }

        private static string WriteImage(int width, int height, int[,] combined)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Environment.NewLine);

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                    sb.Append(combined[i, j] == 1 ? "#" : " ");

                sb.Append(Environment.NewLine);
            }
            return sb.ToString();
        }

        private static int[,] CombineLayers(int width, int height, Stack<int[,]> layers)
        {
            var combined = layers.Pop();
            while (layers.Any())
            {
                var newLayer = layers.Pop();

                for (int i = 0; i < height; i++)
                    for (int j = 0; j < width; j++)
                    {
                        int pixel = newLayer[i, j];
                        if (pixel < 2)
                            combined[i, j] = pixel;

                    }
            }

            return combined;
        }
    }
}
