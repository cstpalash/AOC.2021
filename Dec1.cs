using System;
using System.Collections.Generic;
using System.Linq;

namespace AOC._2021
{
    public class Dec1
    {
        public int Puzzle1(int[] input)
        {
            int result = 0;
            if (input == null) return result;
            if (input.Length <= 1) return result;

            for(int i=1; i <= input.Length - 1; i++)
            {
                if (input[i] > input[i - 1])
                    result++;
            }

            return result;
            
        }

        public int Puzzle2(int[] input)
        {
            var windowSum = new List<int>();

            for(int i=0; i <= input.Length - 3; i++)
            {
                var window = input.Skip(i).Take(3);
                windowSum.Add(window.Sum());
            }

            return Puzzle1(windowSum.ToArray());

        }
    }
}
