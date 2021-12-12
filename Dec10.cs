using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace AOC._2021
{
    public class Dec10
    {
        public int Puzzle1(string[] lines)
        {
            Dictionary<string, bool> openingChars = new Dictionary<string, bool>();
            openingChars.Add("(", true);
            openingChars.Add("{", true);
            openingChars.Add("[", true);
            openingChars.Add("<", true);

            Dictionary<string, string> closingChars = new Dictionary<string, string>();
            closingChars.Add(")", "(");
            closingChars.Add("}", "{");
            closingChars.Add("]", "[");
            closingChars.Add(">", "<");

            Dictionary<string, int> illigalPoints = new Dictionary<string, int>();
            illigalPoints.Add(")", 3);
            illigalPoints.Add("}", 1197);
            illigalPoints.Add("]", 57);
            illigalPoints.Add(">", 25137);

            var syntaxError = 0;
            List<string> illigalChars = new List<string>();

            foreach (string line in lines)
            {
                List<string> data = line.ToCharArray().Select(item => item.ToString()).ToList();

                Stack<string> stack = new Stack<string>();

                for(int i=0; i < data.Count; i++)
                {
                    if (openingChars.ContainsKey(data[i]))
                    {
                        stack.Push(data[i]);
                    }
                    else if (closingChars.ContainsKey(data[i]))
                    {
                        var lastItemPushed = stack.Pop();

                        if(string.Compare(closingChars[data[i]], lastItemPushed, StringComparison.OrdinalIgnoreCase) != 0)
                        {
                            //there is a problem
                            syntaxError += illigalPoints[data[i]];
                            illigalChars.Add(data[i]);
                        }
                    }
                }
                
            }

            return syntaxError;
        }

        public Int64 Puzzle2(string[] lines)
        {
            Dictionary<string, string> openingChars = new Dictionary<string, string>();
            openingChars.Add("(", ")");
            openingChars.Add("{", "}");
            openingChars.Add("[", "]");
            openingChars.Add("<", ">");

            Dictionary<string, string> closingChars = new Dictionary<string, string>();
            closingChars.Add(")", "(");
            closingChars.Add("}", "{");
            closingChars.Add("]", "[");
            closingChars.Add(">", "<");

            Dictionary<string, int> completionPoints = new Dictionary<string, int>();
            completionPoints.Add(")", 1);
            completionPoints.Add("}", 3);
            completionPoints.Add("]", 2);
            completionPoints.Add(">", 4);

            var points = new List<Int64>();
            List<string> illigalChars = new List<string>();

            foreach (string line in lines)
            {
                List<string> data = line.ToCharArray().Select(item => item.ToString()).ToList();

                Stack<string> stack = new Stack<string>();

                bool hasError = false;

                for (int i = 0; i < data.Count; i++)
                {
                    if (openingChars.ContainsKey(data[i]))
                    {
                        stack.Push(data[i]);
                    }
                    else if (closingChars.ContainsKey(data[i]))
                    {
                        var lastItemPushed = stack.Pop();

                        if (string.Compare(closingChars[data[i]], lastItemPushed, StringComparison.OrdinalIgnoreCase) != 0)
                        {
                            //there is a problem
                            //skip this line as it has error
                            hasError = true;
                            break;
                        }
                    }
                }

                if (!hasError)
                {
                    Int64 score = 0;
                    //incomplete -> pop to complete
                    while(stack.Count != 0)
                        score = (score * 5) + completionPoints[openingChars[stack.Pop()]];

                    points.Add(score);
                }

            }



            return points.OrderByDescending(item => item).ToList()[points.Count / 2];
        }
    }
}
