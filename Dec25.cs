using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Numerics;
using System.Text;

namespace AOC._2021
{
    public class Dec25
    {
        int rows, cols;
        string[][] board;

        private string[][] Clone(string[][] board)
        {
            var rows = board.Length;
            var cols = board[0].Length;

            string[][] clone = new string[rows][];
            for(int i = 0; i < rows; i++)
            {
                clone[i] = new string[cols];
            }

            for(int i=0; i< rows; i++)
            {
                for(int j=0; j< cols; j++)
                {
                    clone[i][j] = board[i][j];
                }
            }

            return clone;
        }

        private void Setup(string[] lines)
        {
            rows = lines.Length;
            cols = lines[0].Length;

            board = new string[rows][];

            for (int i = 0; i < rows; i++)
            {
                board[i] = new string[cols];
            }

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    board[i][j] = lines[i][j].ToString();
                }
            }
        }

        private (int, int) GetNextMove(string cucumber, int i, int j)
        {
            if (cucumber == ">")
                return (i, (j + 1) % cols);
            else if (cucumber == "v")
                return ((i + 1) % rows, j);
            else return (i, j);
        }

        public Int64 Puzzle1(string[] lines)
        {
            Setup(lines);

            int step;
            for(step = 1; step <= 1000; step++)
            {
                var clone = Clone(board);

                var totalMoves = 0;
                for (int i = 0; i < rows; i++)
                {
                    for (int j = cols - 1; j >= 0; j--)
                    {
                        if (board[i][j] == ">")
                        {
                            int nextI, nextJ;
                            (nextI, nextJ) = GetNextMove(board[i][j], i, j);
                            if (board[nextI][nextJ] == ".")
                            {
                                //move right
                                clone[nextI][nextJ] = board[i][j];
                                clone[i][j] = ".";
                                totalMoves++;
                            }
                        }
                    }
                }

                board = clone;
                clone = Clone(board);

                for (int j = 0; j < cols; j++)
                {
                    for (int i = rows - 1; i >= 0; i--)
                    {
                        if (board[i][j] == "v")
                        {
                            int nextI, nextJ;
                            (nextI, nextJ) = GetNextMove(board[i][j], i, j);
                            if (board[nextI][nextJ] == ".")
                            {
                                //move down
                                clone[nextI][nextJ] = board[i][j];
                                clone[i][j] = ".";
                                totalMoves++;
                            }
                        }
                    }
                }

                board = clone;

                if (totalMoves == 0) break;
            }

            

            return step;
        }

        private void PrintStep()
        {
            for(int i=0; i<rows; i++)
            {
                for(int j =0; j<cols; j++)
                {
                    Console.Write(board[i][j]);
                }
                Console.WriteLine();
            }
        }

        public Int64 Puzzle2(string[] lines)
        {
            return 0;
        }
    }
}
