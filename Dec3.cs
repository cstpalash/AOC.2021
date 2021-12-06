using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace AOC._2021
{
    public class Dec3
    {
        public int Puzzle1(string[] input)
        {
            var length = input[0].Length;

            StringBuilder gama = new StringBuilder();
            StringBuilder eplison = new StringBuilder();

            for(int i=0; i<=length - 1; i++)
            {
                int count0 = 0;
                int count1 = 0;
                foreach(var record in input)
                {
                    if (record[i] == '1') count1++;
                    if (record[i] == '0') count0++;
                }

                if(count0 > count1)
                {
                    gama.Append("0");
                    eplison.Append("1");
                }
                else
                {
                    gama.Append("1");
                    eplison.Append("0");
                }

            }

            var g = Convert.ToInt32(gama.ToString(), 2);
            var e = Convert.ToInt32(eplison.ToString(), 2);

            return g * e;


        }

        public int LifeSupport(List<string> input, string type)
        {

            var length = input[0].Length;
            for (int i = 0; i <= length - 1; i++)
            {
                int count0 = 0;
                int count1 = 0;
                foreach (var record in input)
                {
                    if (record[i] == '1') count1++;
                    if (record[i] == '0') count0++;
                }

                if (count0 > count1)
                {
                    input = (type == "o2") ? input.Where(item => item[i] == '0').ToList() : input.Where(item => item[i] == '1').ToList();
                }
                else if(count1 > count0)
                {
                    input = (type == "o2") ? input.Where(item => item[i] == '1').ToList() : input.Where(item => item[i] == '0').ToList();
                }
                else
                {
                    input = (type == "o2") ? input.Where(item => item[i] == '1').ToList() : input.Where(item => item[i] == '0').ToList();
                }

                if(input.Count == 1)
                {
                    //stop
                    break;
                }

            }

            return Convert.ToInt32(input[0], 2);
        }

        public int Puzzle2(string[] input)
        {
            var o2 = LifeSupport(input.ToList(), "o2");
            var co2 = LifeSupport(input.ToList(), "co2");

            return o2 * co2;

        }
    }
}
