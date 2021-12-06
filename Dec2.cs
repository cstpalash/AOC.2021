using System;
using System.Collections.Generic;

namespace AOC._2021
{
    public class InstructionDec2
    {
        public string Direction { get; set; }
        public int Unit { get; set; }
    }

    public class Dec2
    {
        public int Puzzle1(List<InstructionDec2> input)
        {
            int position = 0;
            int depth = 0;

            foreach(var instruction in input)
            {
                switch (instruction.Direction)
                {
                    case "forward":
                        position = position + instruction.Unit;
                        break;
                    case "up":
                        depth = depth - instruction.Unit;
                        break;
                    case "down":
                        depth = depth + instruction.Unit;
                        break;
                }
            }

            return position * depth;

        }

        public int Puzzle2(List<InstructionDec2> input)
        {
            int position = 0;
            int depth = 0;
            int aim = 0;

            foreach (var instruction in input)
            {
                switch (instruction.Direction)
                {
                    case "forward":
                        position = position + instruction.Unit;
                        depth = depth + aim * instruction.Unit;
                        break;
                    case "up":
                        aim = aim - instruction.Unit;
                        break;
                    case "down":
                        aim = aim + instruction.Unit;
                        break;
                }
            }

            return position * depth;

        }

    }
}
