using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Util;

namespace Day06
{
    public class Day06 : AdventPuzzle
    {
        public override string SolveFirstPuzzle()
        {
            Dictionary<string, string> orbits = ReadInputArray<string>().Select(x => x.Split(')'))
                .ToDictionary(key => key[1], value => value[0]);
            int checksum = 0;

            foreach(var obj in orbits.Keys)
            {
                string currObj = obj;
                while(currObj != "COM")
                {
                    checksum++;
                    currObj = orbits[currObj];
                } 
            }

            return checksum.ToString() ;
        }

        public override string SolveSecondPuzzle()
        {
            Dictionary<string, string> orbits = ReadInputArray<string>().Select(x => x.Split(')'))
             .ToDictionary(key => key[1], value => value[0]);

            List<string> youPath = new List<string>();
            List<string> sanPath = new List<string>();

            string currObj = "YOU";

            while (currObj != "COM")
            {
                currObj = orbits[currObj];
                youPath.Add(currObj);
            }

            currObj = "SAN";
            while (currObj != "COM")
            {
                currObj = orbits[currObj];
                sanPath.Add(currObj);
            }

            var intersectionPoint = youPath.Intersect(sanPath).First();
            return (youPath.IndexOf(intersectionPoint) + sanPath.IndexOf(intersectionPoint)).ToString();

           

        }
    }
    
}
