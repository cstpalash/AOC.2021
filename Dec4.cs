using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace AOC._2021
{
    public class Coordinate
    {
        public int X { get; set; }
        public int Y { get; set; }
    }

    

    public class Dec4
    {

        private int GetSumOfUnmarkedNumbers(int[][] board, List<Coordinate> matches)
        {
            int sum = 0;
            for (int i= 0; i < 5; i++)
            {
                for(int j=0; j<5; j++)
                {
                    if(!matches.Any(item => item.X == i && item.Y == j))
                    {
                        sum += board[i][j];
                    }
                }
            }
            return sum;
        }

        private bool IsComplete(List<Coordinate> matches, int rows, int columns)
        {
            //any row is complete
            for (int i = 0; i <= rows - 1; i++)
            {
                if (matches.Count(item => item.X == i) == columns)
                {
                    return true;
                }
            }

            //any col is complete
            for (int j = 0; j <= columns - 1; j++)
            {
                if (matches.Count(item => item.Y == j) == rows)
                {
                    return true;
                }
            }

            return false;
        }

        public int Puzzle1(int[] numbersToCall, List<int[][]> boards)
        {
            Dictionary<int, List<Coordinate>> marker = new Dictionary<int, List<Coordinate>>();
            foreach(var number in numbersToCall)
            {
                for(int boardNo = 0; boardNo <= boards.Count -1; boardNo++)
                {
                    if (!marker.ContainsKey(boardNo)) marker.Add(boardNo, new List<Coordinate>());

                    var currentBoard = boards[boardNo];

                    for(int i=0; i<= currentBoard.Length -1; i++)
                    {
                        for(int j=0; j<= currentBoard[i].Length -1; j++)
                        {
                            if(currentBoard[i][j] == number)
                            {
                                //match
                                marker[boardNo].Add(new Coordinate { X = i, Y = j });
                                if(IsComplete(marker[boardNo], 5, 5))
                                {
                                    var sumOfUnmarkedNumbers = GetSumOfUnmarkedNumbers(currentBoard, marker[boardNo]);

                                    return sumOfUnmarkedNumbers * number;
                                }
                            }
                        }
                    }
                }
            }
            return 0;
        }

        public int Puzzle2(int[] numbersToCall, List<int[][]> boards)
        {
            Dictionary<int, List<Coordinate>> marker = new Dictionary<int, List<Coordinate>>();
            List<int> remainingBoards = new List<int>();
            for(int i=0; i<= boards.Count - 1; i++)
            {
                remainingBoards.Add(i);
            }

            foreach (var number in numbersToCall)
            {
                for (int boardNo = 0; boardNo <= boards.Count - 1; boardNo++)
                {
                    if (!marker.ContainsKey(boardNo)) marker.Add(boardNo, new List<Coordinate>());

                    var currentBoard = boards[boardNo];

                    for (int i = 0; i <= currentBoard.Length - 1; i++)
                    {
                        for (int j = 0; j <= currentBoard[i].Length - 1; j++)
                        {
                            if (currentBoard[i][j] == number)
                            {
                                //match
                                marker[boardNo].Add(new Coordinate { X = i, Y = j });
                                if (IsComplete(marker[boardNo], 5, 5))
                                {
                                    remainingBoards.Remove(boardNo);
                                    if(remainingBoards.Count == 0)
                                    {
                                        var sumOfUnmarkedNumbers = GetSumOfUnmarkedNumbers(currentBoard, marker[boardNo]);
                                        return sumOfUnmarkedNumbers * number;
                                    }

                                }
                            }
                        }
                    }
                }
            }
            return 0;
        }
    }
}
