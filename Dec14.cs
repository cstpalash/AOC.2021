using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Text.RegularExpressions;

namespace AOC._2021
{
    public class Rule
    {
        public string Find { get; set; }
        public string Replace { get; set; }
    }
    public class Dec14
    {
        public Int64 Puzzle1(string[] lines)
        {
            var totalSteps = 10;
            var poly = lines[0];

            var rules = new Dictionary<string, string>();

            for (int i = 2; i < lines.Length; i++)
            {
                var segments = lines[i].Split("->", StringSplitOptions.RemoveEmptyEntries);

                rules.Add(segments[0].Trim(), segments[1].Trim());
            }


            Dictionary<string, Int64> stepSegments = new Dictionary<string, Int64>();

            for (int i = 0; i <= poly.Length - 2; i++)
            {
                var find = string.Concat(poly[i].ToString(), poly[i + 1].ToString());

                if (!stepSegments.ContainsKey(find))
                    stepSegments.Add(find, 0);
                stepSegments[find]++;
            }

            return GetCount(poly, stepSegments, rules, totalSteps);
        }

        private Int64 GetCount(string poly, Dictionary<string, Int64> stepSegments, Dictionary<string, string> rules, int totalSteps)
        {
            var map = new Dictionary<string, Int64>();

            for (int step = 1; step <= totalSteps; step++)
            {
                var newStepSegments = new Dictionary<string, long>();
                foreach (var kvp in stepSegments)
                {
                    if (rules.ContainsKey(kvp.Key))
                    {
                        var left = string.Concat(kvp.Key[0].ToString(), rules[kvp.Key]);
                        var right = string.Concat(rules[kvp.Key], kvp.Key[1].ToString());

                        if (!newStepSegments.ContainsKey(left))
                            newStepSegments.Add(left, 0);
                        newStepSegments[left] += kvp.Value;

                        if (!newStepSegments.ContainsKey(right))
                            newStepSegments.Add(right, 0);
                        newStepSegments[right] += kvp.Value;
                    }
                }

                stepSegments = newStepSegments;
            }

            foreach (var kvp in stepSegments)
            {
                if (!map.ContainsKey(kvp.Key[0].ToString()))
                    map.Add(kvp.Key[0].ToString(), 0);
                map[kvp.Key[0].ToString()] += kvp.Value;

                if (!map.ContainsKey(kvp.Key[1].ToString()))
                    map.Add(kvp.Key[1].ToString(), 0);
                map[kvp.Key[1].ToString()] += kvp.Value;
            }

            foreach (var key in new List<string>(map.Keys))
            {
                map[key] = map[key] / 2;
                if (poly.StartsWith(key) || poly.EndsWith(key))
                {
                    map[key]++;
                }
            }

            var high = map.Values.Max();
            var low = map.Values.Min();

            return high - low;
        }

        public Int64 Puzzle2(string[] lines)
        {
            var totalSteps = 40;
            var poly = lines[0];

            var rules = new Dictionary<string, string>();

            for (int i = 2; i < lines.Length; i++)
            {
                var segments = lines[i].Split("->", StringSplitOptions.RemoveEmptyEntries);

                rules.Add(segments[0].Trim(), segments[1].Trim());
            }

            
            Dictionary<string, Int64> stepSegments = new Dictionary<string, Int64>();

            for (int i = 0; i <= poly.Length - 2; i++)
            {
                var find = string.Concat(poly[i].ToString(), poly[i + 1].ToString());

                if (!stepSegments.ContainsKey(find))
                    stepSegments.Add(find, 0);
                stepSegments[find]++;
            }

            return GetCount(poly, stepSegments, rules, totalSteps);
        }
    }
}
