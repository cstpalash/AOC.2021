using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Numerics;
using System.Text;

namespace AOC._2021
{
    public class Dec21
    {
        private int P1, P2;
        public int Puzzle1(string[] lines)
        {
            Setup(lines.ToList());

            int p1 = P1 - 1;
            int p2 = P2 - 1;
            int p1Score = 0;
            int p2Score = 0;
            int dice = 1;
            int rolls = 0;
            while (p1Score < 1000 && p2Score < 1000)
            {
                rolls += 3;
                p1 = (RollDie(ref dice) + p1) % 10;
                p1Score += p1 + 1;
                if (p1Score >= 1000) { break; }
                rolls += 3;
                p2 = (RollDie(ref dice) + p2) % 10;
                p2Score += p2 + 1;
            }
            return (p1Score > p2Score ? p2Score : p1Score) * rolls;
        }

        public Int64 Puzzle2(string[] lines)
        {
            Dictionary<int, HashSet<Dirac>> pStates = new Dictionary<int, HashSet<Dirac>>();
            PlayDirac(pStates);

            long p1Wins = 0;
            long p2Wins = 0;
            foreach (HashSet<Dirac> states in pStates.Values)
            {
                foreach (Dirac dirac in states)
                {
                    if (dirac.Score1 < 21 && dirac.Score2 < 21) { continue; }

                    if (dirac.Score1 > dirac.Score2)
                    {
                        p1Wins += dirac.Count;
                    }
                    else
                    {
                        p2Wins += dirac.Count;
                    }
                }
            }

            long wins = p1Wins > p2Wins ? p1Wins : p2Wins;
            return wins;
        }

        private void Setup(List<string> items)
        {
            P1 = int.Parse(items[0].Substring(28));
            P2 = int.Parse(items[1].Substring(28));
        }

        private int RollDie(ref int dice)
        {
            int amount = dice++;
            if (dice > 100) { dice = 1; }
            amount += dice++;
            if (dice > 100) { dice = 1; }
            amount += dice++;
            if (dice > 100) { dice = 1; }
            return amount;
        }

        private void PlayDirac(Dictionary<int, HashSet<Dirac>> pStates)
        {
            Queue<Dirac> pTurn = new Queue<Dirac>();
            Dirac current = new Dirac() { Position1 = P1 - 1, Position2 = P2 - 1, Count = 1 };
            pTurn.Enqueue(current);
            pStates.Add(0, new HashSet<Dirac>() { current });
            int[] amounts = new int[] { 1, 3, 6, 7, 6, 3, 1 };

            while (pTurn.Count > 0)
            {
                current = pTurn.Dequeue();

                if (!pStates.TryGetValue(current.Steps + 1, out HashSet<Dirac> diracs))
                {
                    diracs = new HashSet<Dirac>();
                    pStates.Add(current.Steps + 1, diracs);
                }

                int index1 = current.Position1 + 2;
                for (int i = 0; i < amounts.Length; i++)
                {
                    index1 = (index1 + 1) % 10;
                    Dirac dirac1 = new Dirac()
                    {
                        Count = current.Count * amounts[i],
                        Position1 = index1,
                        Position2 = current.Position2,
                        Score1 = current.Score1 + index1 + 1,
                        Score2 = current.Score2,
                        Steps = current.Steps + 1
                    };

                    if (dirac1.Score1 < 21)
                    {
                        int index2 = current.Position2 + 2;
                        for (int j = 0; j < amounts.Length; j++)
                        {
                            index2 = (index2 + 1) % 10;
                            Dirac dirac2 = new Dirac()
                            {
                                Count = dirac1.Count * amounts[j],
                                Position1 = dirac1.Position1,
                                Position2 = index2,
                                Score1 = dirac1.Score1,
                                Score2 = current.Score2 + index2 + 1,
                                Steps = dirac1.Steps
                            };

                            if (diracs.TryGetValue(dirac2, out Dirac dirac))
                            {
                                dirac.Count += dirac2.Count;
                            }
                            else
                            {
                                diracs.Add(dirac2);
                                if (dirac2.Score2 < 21)
                                {
                                    pTurn.Enqueue(dirac2);
                                }
                            }
                        }
                    }
                    else if (diracs.TryGetValue(dirac1, out Dirac dirac))
                    {
                        dirac.Count += dirac1.Count;
                    }
                    else
                    {
                        diracs.Add(dirac1);
                    }
                }
            }
        }
    }

    public class Dirac : IEquatable<Dirac>
    {
        public int Steps;
        public int Position1;
        public int Position2;
        public int Score1;
        public int Score2;
        public long Count;

        public bool Equals(Dirac other)
        {
            return Position1 == other.Position1 && Position2 == other.Position2 && Score1 == other.Score1 && Score2 == other.Score2;
        }
        public override int GetHashCode()
        {
            return (((Position1 * 13) + Position2) * 13 + Score1) * 13 + Score2;
        }
        public override bool Equals(object obj)
        {
            return obj is Dirac dirac && Equals(dirac);
        }
        public override string ToString()
        {
            return $"Pos=({Position1},{Position2}) Score=({Score1},{Score2}) Count={Count}";
        }
    }
}
