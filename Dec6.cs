using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace AOC._2021
{
    public class Dec6
    {
        public Int64 Puzzle1(string[] lines)
        {
            int noOfDays = 80;
            var fishes = lines[0].Split(",", StringSplitOptions.RemoveEmptyEntries).Select(item => int.Parse(item)).ToList();

            return Puzzle2(fishes, noOfDays);
        }

        public Int64 Puzzle2(string[] lines)
        {
            int noOfDays = 256;
            var fishes = lines[0].Split(",", StringSplitOptions.RemoveEmptyEntries).Select(item => int.Parse(item)).ToList();

            return Puzzle2(fishes, noOfDays);
        }

        private Int64 Puzzle2(List<int> fishes, int noOfDays)
        {
            Dictionary<int, Int64> map = new Dictionary<int, Int64>();
            foreach (var fish in fishes)
            {
                if (!map.ContainsKey(fish))
                    map.Add(fish, GetTotalReproduction(fish, noOfDays));
            }

            Int64 total = 0;
            foreach (int fish in fishes)
            {
                total += map[fish];
            }

            return total + fishes.Count;
        }

        private Int64 GetTotalReproduction(int fish, int days)
        {
            if (fish >= days) return 0;

            Int64 totalFishes = 0;

            for (int i = fish + 1; i <= days; i = i + 7) //first reproduce on day fish + 1
            {
                totalFishes += 1 + GetTotalReproduction(8, days - i);
            }

            return totalFishes;
        }
    }
}
