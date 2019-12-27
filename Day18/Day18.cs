using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Util;

namespace Day18
{
    public class Day18 : AdventPuzzle
    {
        public override string SolveFirstPuzzle()
        {
            var map = ReadInputArray<string>().Select(x => x.ToCharArray()).ToArray();

            var start = map.IndexOf('@');

            throw new NotImplementedException();
        }
 
        public override string SolveSecondPuzzle()
        {
            throw new NotImplementedException();
        }
    }

    public static class ArrayEx
    {
        public static (int x, int y) IndexOf(this char[][] matrix, char value)
        {
            int h = matrix.GetLength(1); // height
            int w = matrix.GetLength(0); // width

            for (int x = 0; x < w; ++x)
            {
                for (int y = 0; y < h; ++y)
                {
                    if (matrix[x][y].Equals(value))
                        return (x, y);
                }
            }
            return (-1, -1);
        } 
    }

}
