using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Text.RegularExpressions;
namespace AOC._2021
{
    public class Dec18
    {
        private string ProcessExplode(string input)
        {
            var matchExplode = GetFirstMatchCanExplode(input, @"\[\d+,\d+\]");
            if(matchExplode != null)
            {
                input = Explode(input, matchExplode.Index, matchExplode.Value.Length);
            }
            
            return input;
        }

        private string ProcessSplit(string input)
        {
            var matchSplit = GetFirstMatchCanSplit(input);
            if (matchSplit.Success)
            {
                input = Split(input, matchSplit);
            }

            return input;
        }

        private string Process(string input)
        {
            var exploded = ProcessExplode(input);

            if (exploded == input)
            {
                var splitted = ProcessSplit(input);

                if (splitted == input)
                    return input;
                else
                    return Process(splitted);
            }
            else
                return Process(exploded);
        }

        private bool CanExplode(string input, int matchPairIndex)
        {

            var result = false;

            Stack<char> tracker = new Stack<char>();

            for(int index = 0; index < matchPairIndex; index++)
            {
                if (input[index] == '[') tracker.Push('[');
                else if (input[index] == ']') tracker.Pop();
            }

            if (tracker.Count(item => item == '[') >= 4)
                result = true;

            return result;
        }

        private string Explode(string input, int matchPairIndex, int matchPairLength)
        {

            var explodingPair = input.Substring(matchPairIndex + 1, matchPairLength - 2).Split(",", StringSplitOptions.RemoveEmptyEntries);

            var left = int.Parse(explodingPair[0]);
            var right = int.Parse(explodingPair[1]);

            var numberIndexLeft = FindNumberIndexLeft(input, matchPairIndex);

            if(numberIndexLeft.Item1 != -1)
            {
                var newValue = left + numberIndexLeft.Item2;

                input = input.Remove(numberIndexLeft.Item1, numberIndexLeft.Item2.ToString().Length);

                string toBeInserted = newValue.ToString();
                
                input = input.Insert(numberIndexLeft.Item1, toBeInserted);

                matchPairIndex += toBeInserted.Length - numberIndexLeft.Item2.ToString().Length;
            }

            var numberIndexRight = FindNumberIndexRight(input, matchPairIndex + matchPairLength - 1);
            if (numberIndexRight.Item1 != -1)
            {
                var newValue = right + numberIndexRight.Item2;

                input = input.Remove(numberIndexRight.Item1, numberIndexRight.Item2.ToString().Length);

                string toBeInserted = newValue.ToString();
                
                input = input.Insert(numberIndexRight.Item1, toBeInserted);
            }

            input = string.Concat(input.Substring(0, matchPairIndex), "0", input.Substring(matchPairIndex + matchPairLength)); //explode

            return input;
        }

        private Tuple<int, int> FindNumberIndexLeft(string input, int index)
        {
            var match = GetLastMatch(input.Substring(0, index), @"\d+");
            if (match != null && match.Success)
                return new Tuple<int, int>(match.Index, int.Parse(match.Value));

            return new Tuple<int, int>(-1, -1);
        }

        private Tuple<int, int> FindNumberIndexRight(string input, int index)
        {
            if (index == input.Length - 1) new Tuple<int, int>(-1, -1);
            var match = GetFistMatch(input.Substring(index), @"\d+");
            if (match != null && match.Success)
                return new Tuple<int, int>(index + match.Index, int.Parse(match.Value));

            return new Tuple<int, int>(-1, -1);
        }

        private Match GetFistMatch(string input, string pattern)
        {
            var reg = new Regex(pattern);

            var matches = reg.Matches(input);
            return matches.FirstOrDefault();
        }

        private Match GetLastMatch(string input, string pattern)
        {
            var reg = new Regex(pattern);
            var matches = reg.Matches(input);
            return matches.LastOrDefault();
        }

        private Match GetFirstMatchCanExplode(string input, string pattern)
        {
            var reg = new Regex(pattern);

            var allMatches = reg.Matches(input);
            foreach(Match m in allMatches)
            {
                if (CanExplode(input, m.Index)) return m;
            }
            return null;
        }

        private Match GetFirstMatchCanSplit(string input)
        {
            var patternSplit = @"\d{2}";
            var reg = new Regex(patternSplit);

            return reg.Match(input);
        }

        private string Split(string input, Match match)
        {
            var number = int.Parse(input.Substring(match.Index, 2));
            var left = number / 2;
            var right = number - left;
            var toBeInserted = string.Format("[{0},{1}]", left, right);

            input = input.Remove(match.Index, 2);
            input = input.Insert(match.Index, toBeInserted);

            return input;
        }

        private int CalculateMagnitude(string input)
        {
            var pairPattern = @"\[\d+,\d+\]";
            Regex reg = new Regex(pairPattern);

            var match = reg.Match(input);

            while (match.Success)
            {
                var pair = input.Substring(match.Index + 1, match.Value.Length - 2).Split(",", StringSplitOptions.RemoveEmptyEntries);

                var left = int.Parse(pair[0]);
                var right = int.Parse(pair[1]);

                var mag = (3 * left) + (2 * right);

                input = input.Remove(match.Index, match.Value.Length);
                input = string.Concat(input.Substring(0, match.Index), mag.ToString(), input.Substring(match.Index));

                match = reg.Match(input);
            }

            return int.Parse(input);
        }

        public int Puzzle1(string[] lines)
        {

            var result = lines[0];
            for(int i =1; i < lines.Length; i++)
            {
                var combinedPair = string.Format("[{0},{1}]", result, lines[i]);
                result = Process(combinedPair);
            }
            return CalculateMagnitude(result);
        }


        public int Puzzle2(string[] lines)
        {
            var highestMagnitude = 0;

            for(int i=0; i<lines.Length; i++)
            {
                for(int j=0; j<lines.Length; j++)
                {
                    var combinedPair = string.Format("[{0},{1}]", lines[i], lines[j]);
                    var result = Process(combinedPair);
                    var mag = CalculateMagnitude(result);

                    if (mag > highestMagnitude)
                        highestMagnitude = mag;

                    combinedPair = string.Format("[{0},{1}]", lines[j], lines[i]);
                    result = Process(combinedPair);
                    mag = CalculateMagnitude(result);

                    if (mag > highestMagnitude)
                        highestMagnitude = mag;

                }
            }

            return highestMagnitude;
        }
    }
}
