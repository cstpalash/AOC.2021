using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Numerics;
using System.Text;

namespace AOC._2021
{
    
    public class Dec24
    {
        private List<List<string>> blocks;
        int[] max = new int[14];
        int[] min = new int[14];

        private void Setup(string[] lines)
        {
            blocks = new List<List<string>>();

            for(int i=0; i<lines.Length; i++)
            {
                var line = lines[i];
                if(line == "inp w")
                {
                    var blk = new List<string>();
                    blocks.Add(blk);
                }
                else
                {
                    blocks[blocks.Count - 1].Add(line);
                }
            }
        }

        public Int64 Puzzle1(string[] lines)
        {
            Setup(lines);

            var stack = new Stack<(int, int)>();

            for(int i = 0; i < blocks.Count; i++)
            {
                var block = blocks[i];

                bool pop = int.Parse(block[3].Split(" ", StringSplitOptions.RemoveEmptyEntries)[2]) == 26;

                int xAdd = int.Parse(block[4].Split(" ", StringSplitOptions.RemoveEmptyEntries)[2]);
                int yAdd = int.Parse(block[14].Split(" ", StringSplitOptions.RemoveEmptyEntries)[2]);

                if (!pop)
                {
                    stack.Push((i, yAdd));
                }
                else
                {
                    int j;
                    (j, yAdd) = stack.Pop();

                    var difference = xAdd + yAdd;
                    if(difference < 0)
                    {
                        max[i] = 9 + difference;
                        max[j] = 9;
                        min[i] = 1;
                        min[j] = 1 - difference;
                    }
                    else if(difference > 0)
                    {
                        max[i] = 9;
                        max[j] = 9 - difference;
                        min[i] = 1 + difference;
                        min[j] = 1;
                    }
                    else
                    {
                        max[i] = max[j] = 9;
                        min[i] = min[j] = 1;
                    }
                }
            }

            return Int64.Parse(string.Join("", max.Select(item => item)));

        }

        public Int64 Puzzle2(string[] lines)
        {
            return Int64.Parse(string.Join("", min.Select(item => item)));
        }

    }
}
