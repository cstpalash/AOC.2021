using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Text.RegularExpressions;

namespace AOC._2021
{
    public class Target
    {
        public int MinX { get; set; }
        public int MaxX { get; set; }
        public int MinY { get; set; }
        public int MaxY { get; set; }
    }

    public class Dec17
    {
        private bool IsInTarget(Tuple<int, int> step, Target tgt)
        {
            var withinX = step.Item1 >= tgt.MinX && step.Item1 <= tgt.MaxX;
            var withinY = step.Item2 >= tgt.MinY && step.Item2 <= tgt.MaxY;

            return withinX && withinY;
        }

        private Tuple<Tuple<int, int>, Tuple<int, int>> GetNextStepAndVelocity(Tuple<Tuple<int, int>, Tuple<int, int>> currentStepAndVelocity)
        {
            var currentStep = currentStepAndVelocity.Item1;
            var currentVelocity = currentStepAndVelocity.Item2;

            var nextStep = new Tuple<int, int>(currentStep.Item1 + currentVelocity.Item1, currentStep.Item2 + currentVelocity.Item2);

            var nextVelocityX = 0;
            if (currentVelocity.Item1 > 0) nextVelocityX = currentVelocity.Item1 - 1;
            else if (currentVelocity.Item1 < 0) nextVelocityX = currentVelocity.Item1 + 1;
            var nextVelocityY = currentVelocity.Item2 - 1;

            var nextVelocity = new Tuple<int, int>(nextVelocityX, nextVelocityY);

            return new Tuple<Tuple<int, int>, Tuple<int, int>>(nextStep, nextVelocity);
        }

        public int Puzzle1(string[] lines)
        {
            var tgt = lines[0].Replace("target area: ", string.Empty);

            var segments = tgt.Split(",", StringSplitOptions.RemoveEmptyEntries);

            var segmentX = segments[0].Trim().Replace("x=", string.Empty).Split("..", StringSplitOptions.RemoveEmptyEntries);
            var segmentY = segments[1].Trim().Replace("y=", string.Empty).Split("..", StringSplitOptions.RemoveEmptyEntries);

            var minX = Math.Min(int.Parse(segmentX[0]), int.Parse(segmentX[1]));
            var maxX = Math.Max(int.Parse(segmentX[0]), int.Parse(segmentX[1]));

            var minY = Math.Min(int.Parse(segmentY[0]), int.Parse(segmentY[1]));
            var maxY = Math.Max(int.Parse(segmentY[0]), int.Parse(segmentY[1]));

            var target = new Target { MinX = minX, MaxX = maxX, MinY = minY, MaxY = maxY };

            

            var highestYVelocity = 0;

            for(int x = 0; x <= 300; x++)
            {
                for(int y = 0; y <= 300; y++)
                {
                    var currentStepAndVelocity = new Tuple<Tuple<int, int>, Tuple<int, int>>(
                        new Tuple<int, int>(0, 0),
                        new Tuple<int, int>(x, y));

                    for (int step = 1; step <= 300; step++)
                    {
                        currentStepAndVelocity = GetNextStepAndVelocity(currentStepAndVelocity);

                        if (IsInTarget(currentStepAndVelocity.Item1, target))
                        {
                            if (y > highestYVelocity) highestYVelocity = y;
                        }
                    }

                }
            }

            return highestYVelocity * (highestYVelocity + 1) / 2;

        }

        public int Puzzle2(string[] lines)
        {
            var tgt = lines[0].Replace("target area: ", string.Empty);

            var segments = tgt.Split(",", StringSplitOptions.RemoveEmptyEntries);

            var segmentX = segments[0].Trim().Replace("x=", string.Empty).Split("..", StringSplitOptions.RemoveEmptyEntries);
            var segmentY = segments[1].Trim().Replace("y=", string.Empty).Split("..", StringSplitOptions.RemoveEmptyEntries);

            var minX = Math.Min(int.Parse(segmentX[0]), int.Parse(segmentX[1]));
            var maxX = Math.Max(int.Parse(segmentX[0]), int.Parse(segmentX[1]));

            var minY = Math.Min(int.Parse(segmentY[0]), int.Parse(segmentY[1]));
            var maxY = Math.Max(int.Parse(segmentY[0]), int.Parse(segmentY[1]));

            var target = new Target { MinX = minX, MaxX = maxX, MinY = minY, MaxY = maxY };



            Dictionary<string, bool> distinctVelocities = new Dictionary<string, bool>();

            for (int x = -300; x <= 300; x++)
            {
                for (int y = -300; y <= 300; y++)
                {
                    var currentStepAndVelocity = new Tuple<Tuple<int, int>, Tuple<int, int>>(
                        new Tuple<int, int>(0, 0),
                        new Tuple<int, int>(x, y));

                    for (int step = 1; step <= 300; step++)
                    {
                        currentStepAndVelocity = GetNextStepAndVelocity(currentStepAndVelocity);

                        if (IsInTarget(currentStepAndVelocity.Item1, target))
                        {
                            var key = string.Format("{0}-{1}", x, y);
                            if (!distinctVelocities.ContainsKey(key))
                                distinctVelocities.Add(key, true);
                        }
                    }

                }
            }

            return distinctVelocities.Count;
        }
    }
}
