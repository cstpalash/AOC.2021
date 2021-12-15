using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Text.RegularExpressions;

namespace AOC._2021
{
    public class Dec15
    {
        bool IsValid(Tuple<int, int> node, int rows, int cols)
        {
            return (node.Item1 >= 0 && node.Item1 < rows) && (node.Item2 >= 0 && node.Item2 < cols);
        }

        private List<Tuple<int, int>> GetAdjacentNodes(Tuple<int, int> node, int rows, int cols)
        {
            List<Tuple<int, int>> adjNodes = new List<Tuple<int, int>>();

            var left = new Tuple<int, int>(node.Item1, node.Item2 - 1);
            var right = new Tuple<int, int>(node.Item1, node.Item2 + 1);
            var top = new Tuple<int, int>(node.Item1 - 1, node.Item2);
            var bottom = new Tuple<int, int>(node.Item1 + 1, node.Item2);

            if (IsValid(left, rows, cols)) adjNodes.Add(left);
            if (IsValid(right, rows, cols)) adjNodes.Add(right);
            if (IsValid(top, rows, cols)) adjNodes.Add(top);
            if (IsValid(bottom, rows, cols)) adjNodes.Add(bottom);

            return adjNodes;
        }

        public int Puzzle1(string[] lines)
        {
            int rows = lines.Length;
            int cols = lines[0].Length;

            int[][] board = new int[rows][];

            for(int i=0; i< rows; i++)
            {
                board[i] = lines[i].ToCharArray().Select(item => int.Parse(item.ToString())).ToArray();
            }

            Tuple<int, int> source = new Tuple<int, int>(0, 0);

            int[][] distanceFromSource = new int[rows][];
            for (int i = 0; i < rows; i++)
            {
                distanceFromSource[i] = new int[cols];
            }

            bool[][] sptSet = new bool[rows][];
            for (int i = 0; i < rows; i++)
            {
                sptSet[i] = new bool[cols];
            }

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    distanceFromSource[i][j] = int.MaxValue;
                    sptSet[i][j] = false;
                }
            }

            CalculateRisks(board, source, distanceFromSource, sptSet, rows, cols);

            return distanceFromSource[rows - 1][cols - 1];
        }

        private void CalculateRisks(int[][] board, Tuple<int, int> source, int[][] distanceFromSource, bool[][] sptSet, int rows, int cols)
        {
            distanceFromSource[source.Item1][source.Item2] = 0; //distance from self is ZERO

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    // Pick the minimum distance node from the set of vertices not yet processed.
                    // minDistanceNode is always equal to source in first iteration.
                    Tuple<int, int> minDistanceNode = GetMinimumDistanceNode(distanceFromSource, sptSet);
                    sptSet[minDistanceNode.Item1][minDistanceNode.Item2] = true;

                    var adjNodes = GetAdjacentNodes(minDistanceNode, rows, cols);

                    foreach(var n in adjNodes)
                    {
                        // Update distanceFromSource only if is not in sptSet, there is an edge from minDistanceNode to n
                        // and total risk of path from src to n through minDistanceNode is smaller than current value of dist[n]

                        if (!sptSet[n.Item1][n.Item2] &&
                            distanceFromSource[minDistanceNode.Item1][minDistanceNode.Item2] != int.MaxValue &&
                            distanceFromSource[minDistanceNode.Item1][minDistanceNode.Item2] + board[n.Item1][n.Item2] < distanceFromSource[n.Item1][n.Item2])
                        {
                            distanceFromSource[n.Item1][n.Item2] = distanceFromSource[minDistanceNode.Item1][minDistanceNode.Item2] + board[n.Item1][n.Item2];
                        }
                    }

                }
            }

        }

        private Tuple<int, int> GetMinimumDistanceNode(int[][] distanceFromSource, bool[][] sptSet)
        {
            var rows = sptSet.Length;
            var cols = sptSet[0].Length;

            // Initialize min value
            int min = int.MaxValue;
            Tuple<int, int> minNode = null;

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    if(sptSet[i][j] == false && distanceFromSource[i][j] <= min)
                    {
                        min = distanceFromSource[i][j];
                        minNode = new Tuple<int, int>(i, j);
                    }
                }
            }

            return minNode;
        }

        public int Puzzle2(string[] lines)
        {
            int rows = lines.Length;
            int cols = lines[0].Length;

            int[][] board = new int[rows][];

            for (int i = 0; i < rows; i++)
            {
                board[i] = lines[i].ToCharArray().Select(item => int.Parse(item.ToString())).ToArray();
            }

            int newRows = rows * 5;
            int newCols = cols * 5;

            int[][] newBoard = new int[newRows][];
            for (int i = 0; i < newRows; i++)
            {
                newBoard[i] = new int[newCols];
            }

            for(int i=0; i < newRows; i++)
            {
                for (int j = 0; j < newCols; j++)
                {
                    var incrementRight = i / rows;
                    var incrementDown = j / cols;

                    var newValue = board[i % rows][j % cols] + incrementRight + incrementDown;
                    if (newValue > 9) newValue = newValue - 9;
                    newBoard[i][j] = newValue;
                }
            }

            Tuple<int, int> source = new Tuple<int, int>(0, 0);

            int[][] distanceFromSource = new int[newRows][];
            for (int i = 0; i < newRows; i++)
            {
                distanceFromSource[i] = new int[newCols];
            }

            bool[][] sptSet = new bool[newRows][];
            for (int i = 0; i < newRows; i++)
            {
                sptSet[i] = new bool[newCols];
            }

            for (int i = 0; i < newRows; i++)
            {
                for (int j = 0; j < newCols; j++)
                {
                    distanceFromSource[i][j] = int.MaxValue;
                    sptSet[i][j] = false;
                }
            }

            CalculateRisks(newBoard, source, distanceFromSource, sptSet, newRows, newCols);

            return distanceFromSource[newRows - 1][newCols - 1];
        }
    }
}
