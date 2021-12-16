using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Text.RegularExpressions;

namespace AOC._2021
{
    public class Packet
    {
        public int Version { get; set; }
        public int TypeId { get; set; }
        public Int64 Value { get; set; }
    }

    public class Dec16
    {

        private Int64 GetValue(int typeId, List<Int64> values)
        {
            switch (typeId)
            {
                case 0:
                    return values.Sum();
                case 1:
                    Int64 mult = 1;
                    foreach (var v in values) mult *= v;
                    return mult;
                case 2:
                    return values.Min();
                case 3:
                    return values.Max();
                case 5:
                    return values[0] > values[1] ? 1 : 0;
                case 6:
                    return values[0] < values[1] ? 1 : 0;
                case 7:
                    return values[0] == values[1] ? 1 : 0;
                default:
                    return 0;
            }
        }

        private Tuple<int, Int64> ParsePacket(string original, List<Packet> allPackets)
        {
            int lengthParsed = 0;
            var version = GetVersion(original);
            var typeId = GetTypeId(original);
            lengthParsed += 6;

            Int64 value = -1;
            if(typeId == 4)
            {
                var valueAndLengthParsed = ParseLiteral(original);
                value = valueAndLengthParsed.Item1;
                lengthParsed += valueAndLengthParsed.Item2;

                allPackets.Add(new Packet { Version = version, TypeId = typeId, Value = value });

                return new Tuple<int, Int64>(lengthParsed, value);
            }
            else
            {
                allPackets.Add(new Packet { Version = version, TypeId = typeId });
                //operator
                var lengthType = int.Parse(original[6].ToString());
                if (lengthType == 0)
                {
                    var totalLengthInBits = Convert.ToInt32(original.Substring(7, 15), 2);

                    var l = 0;
                    List<Int64> values = new List<long>();
                    while(l < totalLengthInBits)
                    {
                        var res = ParsePacket(original.Substring(22 + l), allPackets);
                        l += res.Item1;
                        values.Add(res.Item2);
                    }

                    var v = GetValue(typeId, values);

                    return new Tuple<int, Int64>(lengthParsed + 1 + 15 + totalLengthInBits, v);
                }
                else
                {
                    var totalNoOfSubpackets = Convert.ToInt64(original.Substring(7, 11), 2);

                    var l = 0;
                    List<Int64> values = new List<long>();
                    for (int counter = 1; counter <= totalNoOfSubpackets; counter++)
                    {
                        var res = ParsePacket(original.Substring(18 + l), allPackets);
                        l += res.Item1;
                        values.Add(res.Item2);
                    }

                    var v = GetValue(typeId, values);

                    return new Tuple<int, Int64>(lengthParsed + 1 + 11 + l, v);
                }
            }
        }

        private string BinaryFromHex(string hex)
        {
            return String.Join(String.Empty, hex.Select(c => Convert.ToString(Convert.ToInt64(c.ToString(), 16), 2).PadLeft(4, '0')));
        }

        private int GetVersion(string bin)
        {
            return Convert.ToInt32(bin.Substring(0, 3), 2); 
        }

        private int GetTypeId(string bin)
        {
            return Convert.ToInt32(bin.Substring(3, 3), 2);
        }

        private Tuple<Int64, int> ParseLiteral(string bin)
        {
            int lengthParsed = 0;
            int startIndex = 6;
            string lit = string.Empty;
            while (bin[startIndex] != '0')
            {
                lit = string.Concat(lit, bin.Substring(startIndex + 1, 4));
                startIndex += 5;
                lengthParsed += 5;
            }
            lit = string.Concat(lit, bin.Substring(startIndex + 1, 4));
            lengthParsed += 5;

            var litValue = Convert.ToInt64(lit, 2);

            return new Tuple<Int64, int>(litValue, lengthParsed);

        }

        public int Puzzle1(string[] lines)
        {
            string originalHex = lines[0];
            string originalBinary = BinaryFromHex(originalHex);

            List<Packet> allPackets = new List<Packet>();

            ParsePacket(originalBinary, allPackets);

            return allPackets.Sum(item => item.Version);
        }

        public Int64 Puzzle2(string[] lines)
        {
            string originalHex = lines[0];
            string originalBinary = BinaryFromHex(originalHex);

            List<Packet> allPackets = new List<Packet>();

            var r = ParsePacket(originalBinary, allPackets);

            return r.Item2;
        }
    }
}
