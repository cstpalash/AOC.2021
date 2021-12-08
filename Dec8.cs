using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace AOC._2021
{
    public class Dec8
    {
        public int Puzzle1(string[] lines)
        {
            var count = 0;

            var uniqueLengths = new int[] { 2, 4, 3, 7 };

            foreach(var line in lines)
            {
                var data = line.Split("|", StringSplitOptions.RemoveEmptyEntries);
                List<string> signals = data[0].Split(" ", StringSplitOptions.RemoveEmptyEntries).ToList();
                List<string> inputs = data[1].Split(" ", StringSplitOptions.RemoveEmptyEntries).ToList();

                count += inputs.Count(item => uniqueLengths.Contains(item.Length));
            }

            return count;
        }

        public string Sort(string str)
        {
            return String.Concat(str.OrderBy(c => c));
        }

        public Dictionary<string, int> GetCountPerSegment(List<string> patterns)
        {
            Dictionary<string, int> map = new Dictionary<string, int>();
            map.Add("a", 0);
            map.Add("b", 0);
            map.Add("c", 0);
            map.Add("d", 0);
            map.Add("e", 0);
            map.Add("f", 0);
            map.Add("g", 0);

            foreach (var str in patterns)
            {
                if (str.Contains("a")) map["a"]++;
                if (str.Contains("b")) map["b"]++;
                if (str.Contains("c")) map["c"]++;
                if (str.Contains("d")) map["d"]++;
                if (str.Contains("e")) map["e"]++;
                if (str.Contains("f")) map["f"]++;
                if (str.Contains("g")) map["g"]++;
            }

            return map;
        }

        public int Puzzle2(string[] lines)
        {
            
            Dictionary<string, int> patterns = new Dictionary<string, int>();
            patterns.Add("abcefg", 0);
            patterns.Add("cf", 1);//2
            patterns.Add("acdeg", 2);
            patterns.Add("acdfg", 3);
            patterns.Add("bcdf", 4);//4
            patterns.Add("abdfg", 5);
            patterns.Add("abdefg", 6);
            patterns.Add("acf", 7);//3
            patterns.Add("abcdefg", 8);//7
            patterns.Add("abcdfg", 9);

            var orgCountPerSegment = GetCountPerSegment(patterns.Keys.ToList());
            var uniqueCountPerSegment = orgCountPerSegment.Where(kvp => orgCountPerSegment.Values.Count(item => item == kvp.Value) == 1).ToList();


            var sum = 0;

            foreach (var line in lines)
            {
                var data = line.Split("|", StringSplitOptions.RemoveEmptyEntries);
                List<string> signals = data[0].Split(" ", StringSplitOptions.RemoveEmptyEntries).ToList();
                List<string> inputs = data[1].Split(" ", StringSplitOptions.RemoveEmptyEntries).ToList();

                Dictionary<string, string> replacements = new Dictionary<string, string>();
                replacements.Add("a", "");
                replacements.Add("b", "");
                replacements.Add("c", "");
                replacements.Add("d", "");
                replacements.Add("e", "");
                replacements.Add("f", "");
                replacements.Add("g", "");

                var currentCountPerSegment = GetCountPerSegment(signals);

                foreach(var kvp in uniqueCountPerSegment)
                    replacements[kvp.Key] = currentCountPerSegment.Where(item => item.Value == kvp.Value).First().Key;

                var acf = signals.First(item => item.Length == 3); // Unique length 3 
                var cf = signals.First(item => item.Length == 2); // Unique length 2
                replacements["a"] = String.Concat(acf.ToCharArray().Except(cf.ToCharArray()));

                acf = signals.First(item => item.Length == 3); // Unique length 3
                var af = String.Concat(replacements["a"], replacements["f"]);
                replacements["c"] = String.Concat(acf.ToCharArray().Except(af.ToCharArray()));

                var bcdf = signals.First(item => item.Length == 4); // Unique length 4
                var bcf = String.Concat(replacements["b"], replacements["c"], replacements["f"]);
                replacements["d"] = String.Concat(bcdf.ToCharArray().Except(bcf.ToCharArray()));

                var abcdefg = signals.First(item => item.Length == 7); // Unique length 7
                var abcdef = String.Concat(replacements["a"], replacements["b"], replacements["c"], replacements["d"], replacements["e"], replacements["f"]);
                replacements["g"] = String.Concat(abcdefg.ToCharArray().Except(abcdef.ToCharArray())); ;

                Dictionary<string, int> replacedAndSortedPatterns = new Dictionary<string, int>();
                foreach(var kvp in patterns)
                {
                    var key = kvp.Key;
                    var replacedKey = Sort(Replace(key, replacements));
                    replacedAndSortedPatterns.Add(replacedKey, kvp.Value);
                }

                StringBuilder numberGenerator = new StringBuilder();
                foreach(var i in inputs)
                    numberGenerator.Append(replacedAndSortedPatterns[Sort(i)].ToString());

                int number = int.Parse(numberGenerator.ToString());

                sum += number;
            }

            return sum;
        }

        private string Replace(string str, Dictionary<string, string> replacementDict)
        {
            StringBuilder result = new StringBuilder();
            foreach(var c in str)
            {
                result.Append(replacementDict[c.ToString()]);
            }

            return result.ToString();
        }
    }
}
