using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Util;

namespace Day14
{
    class Day14 : AdventPuzzle
    {
        public override string SolveFirstPuzzle()
        {
            var input = ReadInputArray<string>();
            var reactions = input.Select(line => new Reaction(line)).ToArray();

            var reqs = reactions.ToDictionary(x => x.Result.Name, x => (long) 0);
            reqs["FUEL"] = 1;
            reqs["ORE"] = 0;
            //var reqs = new Dictionary<string, long>() { { "FUEL", 1 } };

            return ProduceFuel(reactions, reqs).ToString();

            throw new NotImplementedException();

        }

        private long ProduceFuel(Reaction[] reactions, Dictionary<string, long> reqs)
        {
            while (true)
            {
                var chem = reqs.FirstOrDefault(x => x.Key != "ORE" && x.Value > 0);

                if (chem.Key == null)
                    return reqs["ORE"];

                var reaction = reactions.First(r => r.Result.Name == chem.Key);

                var mul = (long)Math.Ceiling((double)chem.Value / reaction.Result.Quantity);
                reqs[chem.Key] -= (mul * reaction.Result.Quantity);

                foreach (var inputChemical in reaction.Input)
                {
                    reqs[inputChemical.Name] +=  mul*(inputChemical.Quantity);
                }
            }
        }

        public override string SolveSecondPuzzle()
        {
            var input = ReadInputArray<string>();
            var reactions = input.Select(line => new Reaction(line)).ToArray();

            long treshold = 1000000000000;
            long step = 10000000;
            long guess = step;

            while (step > 0)
            {
                var reqs = reactions.ToDictionary(x => x.Result.Name, x => (long)0);
                reqs["FUEL"] = guess;
                reqs["ORE"] = 0;
                var fuel = ProduceFuel(reactions, reqs);

                if (fuel > treshold) {
                    guess -= step;
                    step /= 10;
                }

                guess += step;
            }

            return guess.ToString();
        }
    }

    class Reaction
    {
        public Chemical[] Input;
        public Chemical Result;
        private readonly string print;

        public Reaction(string line)
        {
            var splitLine = line.Split(" => ");
            Input = splitLine[0].Split(", ").Select(ParseChemical).ToArray();

            Result = ParseChemical(splitLine[1]);
            print = line;
        }

        private Chemical ParseChemical(string c)
        {
            var tmp = c.Split(' ');
            return new Chemical
            {
                Name = tmp[1].Trim(),
                Quantity = int.Parse(tmp[0].Trim())
            };
        }
        public override string ToString() => print;
    }

    class Chemical
    {
        public int Quantity;
        public string Name;

        public override string ToString()
        {
            return $"{Quantity} {Name}";
        }
    }
}
