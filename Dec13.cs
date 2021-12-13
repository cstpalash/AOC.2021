using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace AOC._2021
{
    public class FoldInstruction
    {
        public string Coordinate { get; set; }
        public int Value { get; set; }
    }

    

    public class Dec13
    {
        public int Puzzle1(string[] lines)
        {
            List<Coordinate> points = new List<Coordinate>();
            List<FoldInstruction> fi = new List<FoldInstruction>();

            var foldInstructionStartIndex = 0;

            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i] == "")
                {
                    foldInstructionStartIndex = i + 1;
                    break;
                }

                var segments = lines[i].Split(",", StringSplitOptions.RemoveEmptyEntries);

                points.Add(new Coordinate { X = int.Parse(segments[0]), Y = int.Parse(segments[1]) });
            }

            for (int i = foldInstructionStartIndex; i < lines.Length; i++)
            {
                if (lines[i] == "")
                {
                    break;
                }

                var instruction = lines[i].Replace("fold along", string.Empty);
                var segments = instruction.Split("=", StringSplitOptions.RemoveEmptyEntries);

                fi.Add(new FoldInstruction { Coordinate = segments[0].Trim(), Value = int.Parse(segments[1]) });

            }

            int cols = points.Max(item => item.X) + 1;
            int rows = points.Max(item => item.Y) + 1;

            int[][] board = new int[rows][];

            for (int i = 0; i < rows; i++)
            {
                board[i] = new int[cols];
            }

            foreach (var point in points)
            {
                board[point.Y][point.X] = 1;
            }

            int[][] newBoard = ApplyFold(board, fi.First());

            var count = 0;
            for (int i = 0; i < newBoard.Length; i++)
            {
                for (int j = 0; j < newBoard[0].Length; j++)
                {
                    if (newBoard[i][j] == 1) count++;
                }
            }

            return count;
        }

        private int[][] ApplyFold(int[][] board, FoldInstruction instruction)
        {
            int rows = board.Length;
            int cols = board[0].Length;

            int newRows = rows;
            int newCols = cols;

            if (instruction.Coordinate == "y")
            {
                if(instruction.Value >= (rows / 2)) newRows = instruction.Value;
                else newRows = rows - instruction.Value - 1;
            }
            else if(instruction.Coordinate == "x")
            {
                if (instruction.Value >= (cols / 2)) newCols = instruction.Value;
                else newCols = cols - instruction.Value - 1;
            }

            var newBoard = new int[newRows][];
            for (int i = 0; i < newRows; i++)
            {
                newBoard[i] = new int[newCols];
            }

            for (int i=0; i< rows; i++)
            {
                for(int j=0; j<cols; j++)
                {
                    if(board[i][j] == 1)
                    {
                        if (instruction.Coordinate == "y")
                        {
                            if (instruction.Value >= (rows / 2))
                            {
                                if (i <= instruction.Value)
                                {
                                    newBoard[i][j] = 1;
                                }
                                else
                                {
                                    newBoard[instruction.Value - (i - instruction.Value)][j] = 1;
                                }
                            }
                            else
                            {
                                newBoard[rows - i -1][j] = 1;
                            }
                        }
                        else
                        {
                            if (instruction.Value >= (cols / 2))
                            {
                                if (j <= instruction.Value)
                                {
                                    newBoard[i][j] = 1;
                                }
                                else
                                {
                                    newBoard[i][instruction.Value - (j - instruction.Value)] = 1;
                                }
                            }
                            else
                            {
                                newBoard[i][cols - j - 1] = 1;
                            }
                        }
                    }
                }
            }

            return newBoard;

        }

        public int Puzzle2(string[] lines)
        {
            List<Coordinate> points = new List<Coordinate>();
            List<FoldInstruction> fi = new List<FoldInstruction>();

            var foldInstructionStartIndex = 0;

            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i] == "")
                {
                    foldInstructionStartIndex = i + 1;
                    break;
                }

                var segments = lines[i].Split(",", StringSplitOptions.RemoveEmptyEntries);

                points.Add(new Coordinate { X = int.Parse(segments[0]), Y = int.Parse(segments[1]) });
            }

            for (int i = foldInstructionStartIndex; i < lines.Length; i++)
            {
                if (lines[i] == "")
                {
                    break;
                }

                var instruction = lines[i].Replace("fold along", string.Empty);
                var segments = instruction.Split("=", StringSplitOptions.RemoveEmptyEntries);

                fi.Add(new FoldInstruction { Coordinate = segments[0].Trim(), Value = int.Parse(segments[1]) });

            }

            int cols = points.Max(item => item.X) + 1;
            int rows = points.Max(item => item.Y) + 1;

            int[][] board = new int[rows][];

            for (int i = 0; i < rows; i++)
            {
                board[i] = new int[cols];
            }

            foreach (var point in points)
            {
                board[point.Y][point.X] = 1;
            }

            int[][] newBoard = board;

            foreach(var ins in fi)
            {
                newBoard = ApplyFold(newBoard, ins);
            }

            int totalCount = 0;
            for(int i=0; i< newBoard.Length; i++)
            {
                for(int j=0; j< newBoard[0].Length; j++)
                {
                    if (newBoard[i][j] == 1)
                    {
                        totalCount++;
                        Console.Write("#");
                    }
                    else Console.Write(" ");
                }
                Console.WriteLine();
            }

            return totalCount;
        }
    }
}
