using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace AOC._2021
{
    public class Dec7
    {
        
        public int Puzzle1(string[] lines)
        {
            var positions = lines[0].Split(",", StringSplitOptions.RemoveEmptyEntries).Select(item => int.Parse(item)).ToList();

            int min = positions.Min();
            int max = positions.Max();

            Dictionary<int, int> posAndCost = new Dictionary<int, int>();

            for (int i = min; i <= max; i++)
            {
                if (!posAndCost.ContainsKey(i))
                {
                    int cost = 0;
                    foreach (int pos in positions)
                    {
                        cost += Math.Abs(i - pos);
                    }
                    posAndCost.Add(i, cost);
                }
                
            }

            var leastCost = posAndCost.Values.Min();
            var highestCost = posAndCost.Values.Max();

            return leastCost;
        }

        public int Puzzle2(string[] lines)
        {
            var positions = lines[0].Split(",", StringSplitOptions.RemoveEmptyEntries).Select(item => int.Parse(item)).ToList();

            int min = positions.Min();
            int max = positions.Max();

            Dictionary<int, int> posAndCost = new Dictionary<int, int>();

            for (int i = min; i <= max; i++)
            {
                if (!posAndCost.ContainsKey(i))
                {
                    int cost = 0;
                    foreach (int pos in positions)
                    {
                        var diff = Math.Abs(i - pos);
                        cost += diff * (diff + 1) / 2;
                    }
                    posAndCost.Add(i, cost);
                }

            }

            var leastCost = posAndCost.Values.Min();
            var highestCost = posAndCost.Values.Max();

            return leastCost;
        }
    }
}
