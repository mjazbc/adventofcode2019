using System;
using System.Linq;
using Util;

namespace Day01
{
    public class Day01 : AdventPuzzle
    {
        public override string SolveFirstPuzzle()
        {
            int[] input = ReadInputArray<int>();
            return input.Sum(CalculateFuel).ToString();
        }

        public override string SolveSecondPuzzle()
        {
            int[] input = ReadInputArray<int>();
            return input.Sum(CalculateFuelReqs).ToString();
        }

        private int CalculateFuel(int mass) => (int) Math.Floor(mass / 3.0) - 2;
        private int CalculateFuelReqs(int mass)
        {
            int fuel = CalculateFuel(mass);

            if (fuel <= 0)
                return 0;

            return fuel + CalculateFuelReqs(fuel);
        }
    }
}
