using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace AOC._2021
{
    public class Line
    {
        public Coordinate Start { get; set; }
        public Coordinate End { get; set; }
    }
    public class Dec5
    {
        public int Puzzle1(string[] lines)
        {
            List<Line> hLines = new List<Line>();
            

            foreach(var line in lines)
            {
                var segments = line.Split("->", StringSplitOptions.RemoveEmptyEntries);

                var startCoordinates = segments[0].Trim().Split(",", StringSplitOptions.RemoveEmptyEntries);
                var endCoordinates = segments[1].Trim().Split(",", StringSplitOptions.RemoveEmptyEntries);

                var hLine = new Line();
                hLine.Start = new Coordinate { X = int.Parse(startCoordinates[0]), Y = int.Parse(startCoordinates[1]) };
                hLine.End = new Coordinate { X = int.Parse(endCoordinates[0]), Y = int.Parse(endCoordinates[1]) };

                hLines.Add(hLine);
            }

            var maxCoordinate = 0;
            foreach(var hLine in hLines)
            {
                if (hLine.Start.X > maxCoordinate) maxCoordinate = hLine.Start.X;
                if (hLine.Start.Y > maxCoordinate) maxCoordinate = hLine.Start.Y;

                if (hLine.End.X > maxCoordinate) maxCoordinate = hLine.End.X;
                if (hLine.End.Y > maxCoordinate) maxCoordinate = hLine.End.Y;
            }

            var dim = maxCoordinate + 1;

            var board = new int[dim][];

            for(int i=0; i<= dim - 1; i++)
            {
                board[i] = new int[dim];
                for(int j=0; j <= dim - 1; j++)
                {
                    board[i][j] = 0;
                }
            }

            foreach(var l in hLines)
            {
                if(l.Start.X == l.End.X)
                {
                    var minY = l.Start.Y > l.End.Y ? l.End.Y : l.Start.Y;
                    var maxY = l.Start.Y > l.End.Y ? l.Start.Y : l.End.Y;

                    for(int j = minY; j <= maxY; j++)
                    {
                        board[l.Start.X][j]++;
                    }

                }

                if (l.Start.Y == l.End.Y)
                {
                    var minX = l.Start.X > l.End.X ? l.End.X : l.Start.X;
                    var maxX = l.Start.X > l.End.X ? l.Start.X : l.End.X;

                    for (int i = minX; i <= maxX; i++)
                    {
                        board[i][l.Start.Y]++;
                    }

                }
            }

            var atLeastTwoLinesOverlap = 0;

            for (int i = 0; i <= dim - 1; i++)
            {
                for (int j = 0; j <= dim - 1; j++)
                {
                    if (board[i][j] >= 2) atLeastTwoLinesOverlap++;
                }
            }

            return atLeastTwoLinesOverlap;
        }

        public int Puzzle2(string[] lines)
        {
            List<Line> hLines = new List<Line>();


            foreach (var line in lines)
            {
                var segments = line.Split("->", StringSplitOptions.RemoveEmptyEntries);

                var startCoordinates = segments[0].Trim().Split(",", StringSplitOptions.RemoveEmptyEntries);
                var endCoordinates = segments[1].Trim().Split(",", StringSplitOptions.RemoveEmptyEntries);

                var hLine = new Line();
                hLine.Start = new Coordinate { X = int.Parse(startCoordinates[0]), Y = int.Parse(startCoordinates[1]) };
                hLine.End = new Coordinate { X = int.Parse(endCoordinates[0]), Y = int.Parse(endCoordinates[1]) };

                hLines.Add(hLine);
            }

            var maxCoordinate = 0;
            foreach (var hLine in hLines)
            {
                if (hLine.Start.X > maxCoordinate) maxCoordinate = hLine.Start.X;
                if (hLine.Start.Y > maxCoordinate) maxCoordinate = hLine.Start.Y;

                if (hLine.End.X > maxCoordinate) maxCoordinate = hLine.End.X;
                if (hLine.End.Y > maxCoordinate) maxCoordinate = hLine.End.Y;
            }

            var dim = maxCoordinate + 1;

            var board = new int[dim][];

            for (int i = 0; i <= dim - 1; i++)
            {
                board[i] = new int[dim];
                for (int j = 0; j <= dim - 1; j++)
                {
                    board[i][j] = 0;
                }
            }

            foreach (var l in hLines)
            {
                if (l.Start.X == l.End.X)
                {
                    var minY = l.Start.Y > l.End.Y ? l.End.Y : l.Start.Y;
                    var maxY = l.Start.Y > l.End.Y ? l.Start.Y : l.End.Y;

                    for (int j = minY; j <= maxY; j++)
                    {
                        board[l.Start.X][j]++;
                    }

                }
                else if (l.Start.Y == l.End.Y)
                {
                    var minX = l.Start.X > l.End.X ? l.End.X : l.Start.X;
                    var maxX = l.Start.X > l.End.X ? l.Start.X : l.End.X;

                    for (int i = minX; i <= maxX; i++)
                    {
                        board[i][l.Start.Y]++;
                    }

                }
                else
                {

                    if(l.Start.X < l.End.X)
                    {
                        for(int i=l.Start.X, j=l.Start.Y; i<=l.End.X; i++)
                        {
                            board[i][j]++;
                            if (l.Start.Y < l.End.Y) j++;
                            else j--;
                        }
                    }
                    else
                    {
                        for (int i = l.End.X, j = l.End.Y; i <= l.Start.X; i++)
                        {
                            board[i][j]++;
                            if (l.End.Y < l.Start.Y) j++;
                            else j--;
                        }
                    }

                }
            }

            var atLeastTwoLinesOverlap = 0;

            for (int i = 0; i <= dim - 1; i++)
            {
                for (int j = 0; j <= dim - 1; j++)
                {
                    if (board[i][j] >= 2) atLeastTwoLinesOverlap++;
                }
            }

            return atLeastTwoLinesOverlap;
        }
    }
}
