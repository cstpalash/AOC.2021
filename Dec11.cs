using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace AOC._2021
{
    public class Dec11
    {
        bool IsValid(Coordinate c, int rows, int cols)
        {
            return (c.X >= 0 && c.X < rows) && (c.Y >= 0 && c.Y < cols);
        }

        void Flash(int[][] board, int rows, int cols, int i, int j, Dictionary<string, bool> tracker)
        {
            board[i][j] = 0; //set zero

            List<Coordinate> adjucent = new List<Coordinate>();
            adjucent.Add(new Coordinate { X = i - 1, Y = j }); //left
            adjucent.Add(new Coordinate { X = i + 1, Y = j }); //right
            adjucent.Add(new Coordinate { X = i, Y = j - 1 }); //top
            adjucent.Add(new Coordinate { X = i, Y = j + 1 }); //bottom

            adjucent.Add(new Coordinate { X = i - 1, Y = j - 1 }); //top-left
            adjucent.Add(new Coordinate { X = i + 1, Y = j - 1 }); //top - right
            adjucent.Add(new Coordinate { X = i - 1, Y = j + 1 }); //bottom - left
            adjucent.Add(new Coordinate { X = i + 1, Y = j + 1 }); //bottom - right

            foreach(var adj in adjucent)
            {
                if (IsValid(adj, rows, cols))
                {
                    if(board[adj.X][adj.Y] == 9)
                    {
                        string key = string.Format("{0}-{1}", adj.X, adj.Y);
                        if (!tracker.ContainsKey(key))
                        {
                            tracker.Add(key, true);
                            Flash(board, rows, cols, adj.X, adj.Y, tracker);
                        }
                    }
                    else
                    {
                        string key = string.Format("{0}-{1}", adj.X, adj.Y);
                        if (!tracker.ContainsKey(key))
                        {
                            board[adj.X][adj.Y]++;
                        }
                        
                    }
                        
                }
            }

        }

        public int Puzzle1(string[] lines)
        {
            int rows = lines.Length;
            int cols = lines[0].Length;

            int[][] octopusBoard = new int[rows][];

            for(int i = 0; i < cols; i++)
            {
                octopusBoard[i] = lines[i].ToCharArray().Select(item => int.Parse(item.ToString())).ToArray();
            }

            int totalFlashes = 0;

            Dictionary<string, bool> flashTracker = new Dictionary<string, bool>();

            for (int step=1; step <= 100; step++)
            {
                flashTracker.Clear();

                for (int i=0; i<rows; i++)
                {
                    for(int j=0; j<cols; j++)
                    {
                        if(octopusBoard[i][j] == 9)
                        {
                            //Flash
                            string key = string.Format("{0}-{1}", i, j);
                            if (!flashTracker.ContainsKey(key))
                            {
                                flashTracker.Add(key, true);
                                Flash(octopusBoard, rows, cols, i, j, flashTracker);
                            }
                        }
                        else
                        {
                            string key = string.Format("{0}-{1}", i, j);
                            if (!flashTracker.ContainsKey(key))
                            {
                                octopusBoard[i][j]++;
                            }
                        }                            
                    }
                }

                totalFlashes += flashTracker.Count;
            }

            return totalFlashes;
        }

        private bool IsAllFlashing(int[][] board, int rows, int cols)
        {

            for(int i=0; i< rows; i++)
            {
                for(int j=0; j< cols; j++)
                {
                    if (board[i][j] != 0) return false;
                }
            }
            return true;
        }

        public int Puzzle2(string[] lines)
        {
            int rows = lines.Length;
            int cols = lines[0].Length;

            int[][] octopusBoard = new int[rows][];

            for (int i = 0; i < cols; i++)
            {
                octopusBoard[i] = lines[i].ToCharArray().Select(item => int.Parse(item.ToString())).ToArray();
            }

            int totalFlashes = 0;

            Dictionary<string, bool> flashTracker = new Dictionary<string, bool>();

            for (int step = 1; step <= 1000; step++)
            {
                flashTracker.Clear();

                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < cols; j++)
                    {
                        if (octopusBoard[i][j] == 9)
                        {
                            //Flash
                            string key = string.Format("{0}-{1}", i, j);
                            if (!flashTracker.ContainsKey(key))
                            {
                                flashTracker.Add(key, true);
                                Flash(octopusBoard, rows, cols, i, j, flashTracker);
                            }
                        }
                        else
                        {
                            string key = string.Format("{0}-{1}", i, j);
                            if (!flashTracker.ContainsKey(key))
                            {
                                octopusBoard[i][j]++;
                            }
                        }
                    }
                }

                totalFlashes += flashTracker.Count;

                if (flashTracker.Count == rows * cols) return step;
            }

            return 0;
        }
    }
}
