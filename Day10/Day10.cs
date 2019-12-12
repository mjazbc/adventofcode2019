using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Util;

namespace Day10
{
    public class Day10 : AdventPuzzle
    {
        private char[][] _map;
        private (int, int) _bestAsteroid;
        public override string SolveFirstPuzzle()
        {
            var input = ReadInputArray<string>();
            _map = input.Select(line => line.ToCharArray()).ToArray();

            int max = 0;
            for(int y = 0; y < _map.Count(); y++)
            {
                for (int x = 0; x < input.First().Length; x++)
                {
                    char current = _map[y][x];
                    if (current != '#')
                        continue;

                    var angles = GetVectors(_map, (y, x)).Select(vec => vec.Item1);
                    int asteroids = angles.Distinct().Count();

                    if (asteroids > max) { 
                        max = asteroids;
                        _bestAsteroid = (y, x);
                    }
                }
            }

            return max.ToString();
        }

        private List<(double, double, (int y, int x))> GetVectors(char[][] map, (int,int) current)
        {
            var vectors = new List<(double, double, (int y, int x))>();
            for (int y = 0; y < map.Count(); y++)
            {
                for (int x = 0; x < map.First().Length; x++)
                {
                    char asteroid = map[y][x];
                    if (asteroid != '#' || (y,x) == current)
                        continue;

                    var vector = (current.Item1 - y, current.Item2 - x);
                    vectors.Add((Math.Atan2(vector.Item1, vector.Item2), 
                        Math.Sqrt(Math.Pow(vector.Item1, 2) + Math.Pow(vector.Item2, 2)), (y,x)));
                }
            }

            return vectors;
        }

        public override string SolveSecondPuzzle()
        {
            if(_bestAsteroid == (0,0))
                SolveFirstPuzzle();

            var vectors = GetVectors(_map, _bestAsteroid);

            var circle = vectors.Where(vec => vec.Item1 >= Math.PI / 2)
                .OrderBy(vec => vec.Item1)
                .ThenBy(vec => vec.Item2).ToList();
            circle.AddRange(vectors.Where(vec => vec.Item1 < Math.PI / 2)
                .OrderBy(vec => vec.Item1).ThenBy(vec => vec.Item2).ToList());

            int shotAsteroids = 0;

            double prevAngle = 0;
            while (true)
            {
                var forRemoval = new List<(double, double, (int, int))>();
                foreach(var asteroid in circle)
                {
                    if (asteroid.Item1 == prevAngle)
                        continue;

                    prevAngle = asteroid.Item1;
                    shotAsteroids++;
                    forRemoval.Add(asteroid);

                    if (shotAsteroids == 200)
                        return (asteroid.Item3.x * 100 + asteroid.Item3.y).ToString();

                }

                circle = circle.Except(forRemoval).ToList();
            }
        }
    }
}
