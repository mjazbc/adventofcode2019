using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Util;

namespace Day24
{
    class Day24 : AdventPuzzle
    {
        public override string SolveFirstPuzzle()
        {
            var input = ReadInputArray<string>()
                .SelectMany(x => x)
                .Select(x => x == '#' ? 1 : 0)
                .ToArray();

            var hist = new HashSet<int>();
            hist.Add(ToDec(input));

            while (true)
            {
                var mapCopy = (int[])input.Clone();

                for(int i = 0; i < input.Length; i++)
                {
                    int adj = CountAdjecant(input, i);

                    if (input[i] == 1 && adj != 1)
                        mapCopy[i] = 0;
                    
                    else if (input[i] == 0 && (adj == 2 || adj == 1))
                        mapCopy[i] = 1;
                }

                int dec = ToDec(mapCopy);
                if (hist.Contains(dec))
                    return dec.ToString();

                hist.Add(dec);
                input = mapCopy;
            }

        }

        public override string SolveSecondPuzzle()
        {
            var input = ReadInputArray<string>()
                .SelectMany(x => x)
                .Select(x => x == '#' ? 1 : 0)
                .ToArray();

            Dictionary<int, int[]> levels = new Dictionary<int, int[]>();
            levels.Add(0, input);

            for (int min = 0; min < 200; min++)
            {
                //Console.Clear();
                int depth = 0;
                var mapCopy = levels.ToDictionary(kvp => kvp.Key, kvp => (int[])kvp.Value.Clone());
                bool anyBug = ProcessLevel(levels, mapCopy, depth);

                depth = 1;
                while(anyBug || levels.ContainsKey(depth))
                {
                    if (!mapCopy.ContainsKey(depth))
                        mapCopy[depth] = new int[25];
                    anyBug = ProcessLevel(levels, mapCopy, depth);
                    depth++;
                }

                depth = -1;
                anyBug = true;
                while (anyBug || levels.ContainsKey(depth))
                {
                    if (!mapCopy.ContainsKey(depth))
                        mapCopy[depth] = new int[25];

                    anyBug = ProcessLevel(levels, mapCopy, depth);
                    depth--;
                }
                levels = mapCopy;
            }

            return levels.SelectMany(x => x.Value).Sum().ToString();
        }
        private int ToDec(int[] map)
        {
            int sum = 0;
            for(int i = 0; i < map.Length; i++)
                sum += map[i] * (int)Math.Pow(2,i);

            return sum;
        }


        private int CountAdjecant(int[] map, int idx)
        {
            var count = 0;
            count += (idx > 0 && idx % 5 > 0 && map[idx - 1] > 0) ? 1 : 0;
            count += (idx < map.Length -1 && (idx + 1) % 5 > 0 && map[idx + 1] > 0) ? 1 : 0;
            count += (idx - 5 >=  0 && map[idx - 5 ] > 0) ? 1 : 0;
            count += (idx + 5 <= map.Length - 1 && map[idx + 5] > 0) ? 1 : 0;

            return count;
        }

        private int CountAdjecant(Dictionary<int, int[]> levels, int idx, int level)
        {
            var count = 0;
            if (levels.ContainsKey(level))
            {
                var map = levels[level];
                count += (idx > 0 && idx % 5 > 0 && map[idx - 1] > 0) ? 1 : 0;
                count += (idx < map.Length - 1 && (idx + 1) % 5 > 0 && map[idx + 1] > 0) ? 1 : 0;
                count += (idx - 5 >= 0 && map[idx - 5] > 0) ? 1 : 0;
                count += (idx + 5 <= map.Length - 1 && map[idx + 5] > 0) ? 1 : 0;
            }
            int[] above = levels.GetValueOrDefault(level + 1, new int[25]);
            int[] below = levels.GetValueOrDefault(level - 1 , new int[25]);

            if (idx < 5) //upper row
            {
                count += below[7];
            }
            if(idx % 5 == 0 ) // left column
            {
                count += below[11];
            }
            if ((idx+1) % 5 == 0) // right column
            {
                count += below[13];
            }
            if (idx >= 20) // bottom row
            {
                count += below[17];
            }
            if(idx == 7)
            {
                count += above[0..5].Sum();
            }
            if( idx == 11)
            {
                count += above[0] + above[5] + above[10] + above[15] + above[20];
            }
            if (idx == 13)
            { 
                count += above[4] + above[9] + above[14] + above[19] + above[24];
            }
            if (idx == 17)
            {
                count += above[^5..].Sum();
            }

            return count;
        }

        private bool ProcessLevel(Dictionary<int, int[]> levels, Dictionary<int, int[]> mapCopy, int level)
        {
            var currLevel = levels.GetValueOrDefault(level, new int[25]);

            //Console.WriteLine("\nLEVEL " + level);
            for (int i = 0; i < currLevel.Length; i++)
            {
                //if (i % 5 == 0)
                //    Console.WriteLine();
                //Console.Write(currLevel[i] == 1 ? "# " : ". ");
                if (i == 12) 
                    continue;

                int adj = CountAdjecant(levels, i, level);

                if (currLevel[i] == 1 && adj != 1)
                    mapCopy[level][i] = 0;

                else if (currLevel[i] == 0 && (adj == 2 || adj == 1))
                    mapCopy[level][i] = 1;
            }

            return mapCopy[level].Any(x => x == 1);


        }
    }
}
