using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Util;

namespace Day12
{
    public class Day12 : AdventPuzzle
    {
        public override string SolveFirstPuzzle()
        {
            var input = ReadInputArray<string>();
            var moons = input.Select(x => new Moon(x)).ToArray();

            for(int i = 0; i< 1000; i++)
                UpdateVelocityAndPosition(moons);
            
            return moons.Sum(x => x.TotalEnergy).ToString();
        }

        private void UpdateVelocityAndPosition(Moon[] moons)
        {
            for(int i = 0; i< moons.Length; i++)
            {
                var currMoon = moons[i];

                for (int j = 0; j < moons.Length; j++)
                {
                    if (j == i)
                        continue;

                    currMoon.Velocity.x -= Math.Sign(currMoon.Position.x - moons[j].Position.x);
                    currMoon.Velocity.y -= Math.Sign(currMoon.Position.y - moons[j].Position.y);
                    currMoon.Velocity.z -= Math.Sign(currMoon.Position.z - moons[j].Position.z);
                }
            }

            foreach(var moon in moons)
                moon.ApplyVelocity();
            
        }


        public override string SolveSecondPuzzle()
        {
            var input = ReadInputArray<string>();
            var moons = input.Select(x => new Moon(x)).ToArray();

            var moonsInitial = moons.Select(moon => (Moon)moon.Clone()).ToArray();
            int count = 0;
            (int x, int y, int z) loops = (0,0,0);

            do
            {
                UpdateVelocityAndPosition(moons);
                count++;

                if (loops.x == 0 && moons.Select(x => x.Position.x).SequenceEqual(moonsInitial.Select(x => x.Position.x)) &&
                moons.All(x => x.Velocity.x == 0))
                    loops.x = count;

                if (loops.y == 0 &&  moons.Select(x => x.Position.y).SequenceEqual(moonsInitial.Select(x => x.Position.y)) &&
                    moons.All(x => x.Velocity.y == 0))
                    loops.y = count;

                if (loops.z == 0 &&  moons.Select(x => x.Position.z).SequenceEqual(moonsInitial.Select(x => x.Position.z)) &&
                moons.All(x => x.Velocity.z == 0))
                    loops.z = count;
  
            }
            while (loops.x == 0 || loops.y == 0 || loops.z == 0);
            
            return LCM(new long[] { loops.x, loops.y, loops.z }).ToString();
        }

        static long LCM(IEnumerable<long> numbers) => numbers.Aggregate(lcm);
        static long lcm(long a, long b) => Math.Abs(a * b) / GCD(a, b);       
        static long GCD(long a, long b) => b == 0 ? a : GCD(b, a % b);
        
    }

    public class Moon : ICloneable
    {
        public Moon(string line)
        {
            Regex r = new Regex(@"<x=(?<x>-?\d+), y=(?<y>-?\d+), z=(?<z>-?\d+)>");
            var result = r.Match(line);
            Position = (int.Parse(result.Groups["x"].Value), int.Parse(result.Groups["y"].Value), int.Parse(result.Groups["z"].Value));
        }

        public Moon()
        {

        }
        public (int x, int y, int z) Position;
        public (int x, int y, int z) Velocity;

        public int TotalEnergy => (Math.Abs(Position.x) + Math.Abs(Position.y) + Math.Abs(Position.z)) *
            (Math.Abs(Velocity.x) + Math.Abs(Velocity.y) + Math.Abs(Velocity.z));
        public void ApplyVelocity()
        {
            Position.x += Velocity.x;
            Position.y += Velocity.y;
            Position.z += Velocity.z;
        }

        public object Clone()
        {
            return new Moon()
            {
                Position = this.Position,
                Velocity = this.Velocity
            };
        }
    }
}
