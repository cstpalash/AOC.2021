using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace AOC._2021
{
    public class Dec9
    {
        List<Coordinate> GetAdjucent(int x, int y, int rows, int cols)
        {
            List<Coordinate> result = new List<Coordinate>();

            Coordinate left = new Coordinate { X = x - 1, Y = y };
            Coordinate right = new Coordinate { X = x + 1, Y = y };
            Coordinate top = new Coordinate { X = x, Y = y + 1 };
            Coordinate bottom = new Coordinate { X = x, Y = y - 1 };

            if (IsValid(left, rows, cols)) result.Add(left);
            if (IsValid(right, rows, cols)) result.Add(right);
            if (IsValid(top, rows, cols)) result.Add(top);
            if (IsValid(bottom, rows, cols)) result.Add(bottom);

            return result;
        }

        bool IsValid(Coordinate c, int rows, int cols)
        {
            return (c.X >= 0 && c.X < rows) && (c.Y >= 0 && c.Y < cols);
        }

        public int Puzzle1(string[] lines)
        {
            int rows = lines.Count();
            int cols = lines[0].Length;

            int[][] board = new int[rows][];

            for(int i=0; i< rows; i++)
            {
                board[i] = lines[i].ToCharArray().Select(item => int.Parse(item.ToString())).ToArray();
            }

            List<int> lowPoints = new List<int>();

            for(int i=0; i < rows; i++)
            {
                for(int j=0; j< cols; j++)
                {
                    var adjPoints = GetAdjucent(i, j, rows, cols);

                    var lowAdjPointCount = adjPoints.Where(p => board[i][j] < board[p.X][p.Y]).Count();

                    if (lowAdjPointCount == adjPoints.Count) lowPoints.Add(board[i][j]);

                }
            }

            return lowPoints.Sum(item => item + 1);
        }

        int GetBasinPointSizeAround(int value,int x, int y, int[][] board, int rows, int cols, Dictionary<string, bool> alreadyCounted)
        {
            List<Coordinate> result = new List<Coordinate>();

            Coordinate left = new Coordinate { X = x - 1, Y = y };
            Coordinate right = new Coordinate { X = x + 1, Y = y };
            Coordinate top = new Coordinate { X = x, Y = y + 1 };
            Coordinate bottom = new Coordinate { X = x, Y = y - 1 };

            if (!alreadyCounted.ContainsKey(string.Format("{0}-{1}", left.X, left.Y)) && IsValid(left, rows, cols) && (board[left.X][left.Y] > value && board[left.X][left.Y] < 9)) {
                result.Add(left);
                alreadyCounted.Add(string.Format("{0}-{1}", left.X, left.Y), true);
            }
            if (!alreadyCounted.ContainsKey(string.Format("{0}-{1}", right.X, right.Y)) && IsValid(right, rows, cols) && (board[right.X][right.Y] > value && board[right.X][right.Y] < 9)) {
                result.Add(right);
                alreadyCounted.Add(string.Format("{0}-{1}", right.X, right.Y), true);
            }
            if (!alreadyCounted.ContainsKey(string.Format("{0}-{1}", top.X, top.Y)) && IsValid(top, rows, cols) && (board[top.X][top.Y] > value && board[top.X][top.Y] < 9)) {
                result.Add(top);
                alreadyCounted.Add(string.Format("{0}-{1}", top.X, top.Y), true);
            }
            if (!alreadyCounted.ContainsKey(string.Format("{0}-{1}", bottom.X, bottom.Y)) && IsValid(bottom, rows, cols) && (board[bottom.X][bottom.Y] > value && board[bottom.X][bottom.Y] < 9)) {
                result.Add(bottom);
                alreadyCounted.Add(string.Format("{0}-{1}", bottom.X, bottom.Y), true);
            }

            if (result.Count == 0) return 1;
            else
            {
                var count = 0;
                foreach(var adj in result)
                {
                    count += GetBasinPointSizeAround(board[adj.X][adj.Y], adj.X, adj.Y, board, rows, cols, alreadyCounted);
                }

                return count + 1;
            }
        }

        public int Puzzle2(string[] lines)
        {
            int rows = lines.Count();
            int cols = lines[0].Length;

            int[][] board = new int[rows][];

            for (int i = 0; i < rows; i++)
            {
                board[i] = lines[i].ToCharArray().Select(item => int.Parse(item.ToString())).ToArray();
            }



            Dictionary<string, int> basinSizePerLowPoints = new Dictionary<string, int>();

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    var adjPoints = GetAdjucent(i, j, rows, cols);

                    var lowAdjPointCount = adjPoints.Where(p => board[i][j] < board[p.X][p.Y]).Count();

                    if (lowAdjPointCount == adjPoints.Count)
                    {
                        string key = string.Format("{0}-{1}", i, j);
                        if (!basinSizePerLowPoints.ContainsKey(key))
                            basinSizePerLowPoints.Add(key, GetBasinPointSizeAround(board[i][j], i, j, board, rows, cols, new Dictionary<string, bool>()));
                    }

                }
            }

            var top3Sizes = basinSizePerLowPoints.Values.OrderByDescending(item => item).Take(3).ToList();

            return top3Sizes[0] * top3Sizes[1] * top3Sizes[2];
        }
    }
}
