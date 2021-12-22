using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Numerics;
using System.Text;

namespace AOC._2021
{
    public class Dec20
    {
        public int Puzzle1(string[] lines)
        {
            Setup(lines.ToList());

            Step(algorithm[0] == 0 ? (byte)0 : algorithm[511]);
            Step(algorithm[0]);
            return image.Count();
        }

        public int Puzzle2(string[] lines)
        {
            //Setup(lines.ToList());

            for (int i = 2; i < 50; i += 2)
            {
                Step(algorithm[0] == 0 ? (byte)0 : algorithm[511]);
                Step(algorithm[0]);
            }

            return image.Count();
        }

        private byte[] algorithm;
        private BitGrid image, output;

        private void Setup(List<string> items)
        {

            algorithm = new byte[512];
            string algo = items[0];
            for (int i = 0; i < algo.Length; i++)
            {
                algorithm[i] = (byte)(algo[i] == '#' ? 1 : 0);
            }

            image = new BitGrid(items[2].Length + 110, items.Count + 108);
            output = new BitGrid(image.Width, image.Height);
            for (int i = 2; i < items.Count; i++)
            {
                string line = items[i];
                for (int j = 0; j < line.Length; j++)
                {
                    if (line[j] == '#')
                    {
                        image[j + 55, i + 53] = 1;
                    }
                }
            }
        }

        private void Step(byte spaceValue)
        {
            int height = output.Height - 1;
            int width = output.Width - 1;

            image.SetRow(0, spaceValue);
            image.SetRow(height, spaceValue);
            image.SetColumn(0, spaceValue);
            image.SetColumn(width, spaceValue);

            for (int i = 1; i < height; i++)
            {
                for (int j = 1; j < width; j++)
                {
                    uint index = (image.GetBits(j - 1, i - 1, 3) << 6) | (image.GetBits(j - 1, i, 3) << 3) | image.GetBits(j - 1, i + 1, 3);
                    output[j, i] = algorithm[index];
                }
            }

            BitGrid temp = image;
            image = output;
            output = temp;
        }
    }

    public class BitGrid
    {
        private ulong[] data;
        public int Width, Height;
        private int columns;
        public BitGrid(int width, int height)
        {
            Width = width;
            Height = height;
            columns = (width + 63) >> 6;
            data = new ulong[columns * height];
        }
        public void CopyTo(BitGrid other)
        {
            Array.Copy(data, other.data, data.Length);
        }
        public byte this[int x, int y]
        {
            get { return GetBit(x, y); }
            set { SetBit(x, y, value); }
        }
        public void SetBit(int x, int y, byte bit)
        {
            int pos = y * columns + (x >> 6);
            ulong bitPos = 1UL << (63 - (x & 63));
            if (bit == 0)
            {
                data[pos] &= ~bitPos;
            }
            else
            {
                data[pos] |= bitPos;
            }
        }
        public void SetRow(int y, int bit)
        {
            ulong temp = bit == 1 ? 0xffffffffffffffffUL : 0;
            int pos = y * columns;
            int max = pos + columns - 1;
            for (int i = pos; i < max; i++)
            {
                data[i] = temp;
            }
            temp <<= (64 - Width + ((columns - 1) << 6));
            data[max] = temp;
        }
        public void SetColumn(int x, int bit)
        {
            int pos = x >> 6;
            ulong bitPos = 1UL << (63 - (x & 63));
            for (int i = 0; i < Height; i++)
            {
                if (bit == 0)
                {
                    data[pos] &= ~bitPos;
                }
                else
                {
                    data[pos] |= bitPos;
                }
                pos += columns;
            }
        }
        public byte GetBit(int x, int y)
        {
            int pos = y * columns + (x >> 6);
            return (byte)((data[pos] >> (63 - (x & 63))) & 1);
        }
        public uint GetBits(int x, int y, int count)
        {
            int pos = y * columns + (x >> 6);
            int bitPos = x & 63;
            int invBit = 64 - bitPos;
            if (count > invBit && (x >> 6) + 1 < columns)
            {
                return (uint)((
                    (data[pos] << (count - invBit)) |
                    (data[pos + 1] >> (64 - count + invBit))
                ) & ((1UL << count) - 1));
            }
            return (uint)((data[pos] >> (invBit - count)) & ((1UL << count) - 1));
        }
        public int Count()
        {
            int count = 0;
            for (int i = 0; i < data.Length; i++)
            {
                count += BitOperations.PopCount(data[i]);
            }
            return count;
        }
        public int Count(int x, int y, int bits)
        {
            return BitOperations.PopCount(GetBits(x, y, bits));
        }
        public override string ToString()
        {
            return ToString('1', '0');
        }
        public string ToString(char trueValue = '1', char falseValue = '0')
        {
            StringBuilder sb = new StringBuilder();
            for (int j = 0, p = 0; j < Height; j++)
            {
                int bitWidth = 64;
                for (int i = 0; i < columns; i++)
                {
                    if (i == columns - 1) { bitWidth = Width & 63; }

                    ulong item = data[p++];
                    ulong mask = 1UL << 63;
                    for (int k = 0; k < bitWidth; k++)
                    {
                        sb.Append((item & mask) != 0 ? trueValue : falseValue);
                        mask >>= 1;
                    }
                }
                sb.AppendLine();
            }
            return sb.ToString();
        }
    }
}
