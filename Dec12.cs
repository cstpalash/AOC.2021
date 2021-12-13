using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace AOC._2021
{
    public class Dec12
    {
        private bool IsSmallCave(string point)
        {
            if (string.IsNullOrEmpty(point) || point == "start" || point == "end") return false;

            return Char.IsLower(point[0]);

        }

        private Dictionary<string, int> GetSmallCaveVisitMap(List<string> path)
        {
            Dictionary<string, int> map = new Dictionary<string, int>();

            foreach(var point in path)
            {
                if (IsSmallCave(point))
                {
                    if (!map.ContainsKey(point)) map.Add(point, 0);

                    map[point]++;
                }
            }

            return map;
        }

        private IEnumerable<List<string>> GetPaths(Queue<List<string>> tracker, Dictionary<string, List<string>> edges)
        {
            while (tracker.Any())
            {
                var path = tracker.Dequeue();

                var lastPoint = path.Last();

                if (lastPoint == "end")
                    yield return path;
                else
                {
                    foreach (var point in edges[lastPoint])
                    {
                        if (point != "start")
                        {
                            var smallCaveVisitMap = GetSmallCaveVisitMap(path);

                            bool allowedPath = true;

                            if (IsSmallCave(point) &&
                                smallCaveVisitMap.ContainsKey(point) &&
                                smallCaveVisitMap[point] == 1)
                            {
                                allowedPath = false;
                            }
                                
                            if (allowedPath)
                            {
                                var newpath = new List<string>(path);
                                newpath.Add(point);
                                tracker.Enqueue(newpath);
                            }
                            
                        }

                    }
                }

                
            }
        }

        private IEnumerable<List<string>> GetNewPath(Queue<List<string>> tracker, Dictionary<string, List<string>> edges)
        {
            while (tracker.Any())
            {
                var path = tracker.Dequeue();

                var lastPoint = path.Last();

                if (lastPoint == "end")
                    yield return path;
                else
                {
                    foreach (var point in edges[lastPoint])
                    {
                        if (point != "start")
                        {
                            bool allowed = true;

                            var smallCaveVisitMap = GetSmallCaveVisitMap(path);

                            if (IsSmallCave(point))
                            {
                                if (!smallCaveVisitMap.ContainsKey(point))
                                    smallCaveVisitMap[point] = 0;
                                smallCaveVisitMap[point]++;

                                if (smallCaveVisitMap[point] > 2)
                                    allowed = false;
                                else if (smallCaveVisitMap.Values.Count(item => item > 1) > 1) {
                                    allowed = false;
                                }
                            }

                            if (allowed)
                            {
                                var newpath = new List<string>(path);
                                newpath.Add(point);
                                tracker.Enqueue(newpath);
                            }

                        }

                    }
                }


            }
        }

        public int Puzzle1(string[] lines)
        {
            Dictionary<string, bool> points = new Dictionary<string, bool>();
            Dictionary<string, List<string>> edges = new Dictionary<string, List<string>>();

            foreach(var l in lines)
            {
                var pts = l.Split("-", StringSplitOptions.RemoveEmptyEntries);
                foreach(var p in pts)
                    if (points.ContainsKey(p)) points.Add(p, true);

                if (!edges.ContainsKey(pts[0]))
                    edges.Add(pts[0], new List<string>());
                edges[pts[0]].Add(pts[1]);

                if (!edges.ContainsKey(pts[1]))
                    edges.Add(pts[1], new List<string>());
                edges[pts[1]].Add(pts[0]);
            }

            Queue<List<string>> tracker = new Queue<List<string>>();
            List<string> paths = new List<string>();
            paths.Add("start");
            tracker.Enqueue(paths);

            var result = GetPaths(tracker, edges).ToList();

            

            return result.Count;
        }

        public int Puzzle2(string[] lines)
        {
            Dictionary<string, bool> points = new Dictionary<string, bool>();
            Dictionary<string, List<string>> edges = new Dictionary<string, List<string>>();

            foreach (var l in lines)
            {
                var pts = l.Split("-", StringSplitOptions.RemoveEmptyEntries);
                foreach (var p in pts)
                    if (points.ContainsKey(p)) points.Add(p, true);

                if (!edges.ContainsKey(pts[0]))
                    edges.Add(pts[0], new List<string>());
                edges[pts[0]].Add(pts[1]);

                if (!edges.ContainsKey(pts[1]))
                    edges.Add(pts[1], new List<string>());
                edges[pts[1]].Add(pts[0]);
            }

            Queue<List<string>> tracker = new Queue<List<string>>();
            List<string> paths = new List<string>();
            paths.Add("start");
            tracker.Enqueue(paths);

            var result = GetNewPath(tracker, edges).ToList();



            return result.Count;
        }
    }
}
