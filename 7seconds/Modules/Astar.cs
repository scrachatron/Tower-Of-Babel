using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tower_Of_Babel
{
    class Astar
    {

        public  List<StarNode> OpenSet;
        public  List<StarNode> ClosedSet;
        private List<StarNode> CurNodeNeighbors;
        public  List<StarNode> CompleatedPath = new List<StarNode>();

        private StarNode curr;

        bool Isrun = false;

        StarNode[,] Map;
        Point Start, End;
        //Point[] Path;

        public bool ValidPath;
        bool IsComplete;

        public double LargestGScore;
        public void ReloadPathfinding(int[,] map,Point start, Point end)
        {
            Map = new StarNode[map.GetLength(0), map.GetLength(1)];
            for (int x = 0; x < map.GetLength(0); x++)
                for (int y = 0; y < map.GetLength(1); y++)
                {
                    Map[x, y] = new StarNode(new Point(x, y));
                    Map[x, y].type = map[x, y];
                }

            Start = start;
            curr = new StarNode(start);
            End = end;

            ValidPath = false;
            IsComplete = false;

            OpenSet = new List<StarNode>();
            ClosedSet = new List<StarNode>();
            CurNodeNeighbors = new List<StarNode>();
            CompleatedPath = new List<StarNode>();
            //StepFinding();

            RunPathfinding();
        } 

        private void RunPathfinding()
        {
            curr.G = 0;
            curr.H = HcostEstimate(curr.Location,End);


            OpenSet.Add(curr);

            while (OpenSet.Count != 0)
            {
                curr = GetLowestInList(OpenSet);

                if (curr.Location.X == End.X && curr.Location.Y == End.Y)
                {
                    CompleatedPath = ConstructPath(curr);
                    LargestGScore = MaxGscore(ClosedSet);
                    IsComplete = true;
                    ValidPath = true;
                    break;
                }

                OpenSet.Remove(curr);
                ClosedSet.Add(curr);

                GetNeighborstonode(curr, CurNodeNeighbors);

                for (int i = 0; i < CurNodeNeighbors.Count; i++)
                {
                    if (ClosedSet.Contains(CurNodeNeighbors[i]))
                        continue;

                    double tentative_g_score = curr.G + DistBetween(curr.Location, CurNodeNeighbors[i].Location);

                    if (!OpenSet.Contains(CurNodeNeighbors[i]))
                    {
                        OpenSet.Add(CurNodeNeighbors[i]);
                    }
                    else if (tentative_g_score >= CurNodeNeighbors[i].G)
                    {
                        continue;
                    }
                    CurNodeNeighbors[i].Previous = curr;
                    CurNodeNeighbors[i].H = HcostEstimate(CurNodeNeighbors[i].Location, End);
                    CurNodeNeighbors[i].G = tentative_g_score;
                }

                CurNodeNeighbors.Clear();

            }
            if (OpenSet.Count == 0)
            {
                IsComplete = true;
                ValidPath = false;
            }
        }
        private double DistBetween(Point start, Point end)
        {
            double dx = start.X - end.X;
            double dy = start.Y - end.Y;

            return Math.Sqrt((dx * dx) + (dy * dy));

            //return Math.Sqrt((dx * dx) + (dy * dy));
        }
        public void StepFinding(Point start, Point end, int[,]map)
        {
            if (Isrun == false)
            {
                Map = new StarNode[map.GetLength(0), map.GetLength(1)];
                for (int x = 0; x < map.GetLength(0); x++)
                    for (int y = 0; y < map.GetLength(1); y++)
                    {
                        Map[x, y] = new StarNode(new Point(x, y));
                        Map[x, y].type = map[x, y];
                    }

                Start = start;
                curr = new StarNode(start);
                End = end;

                OpenSet.Clear();
                ClosedSet.Clear();
                CurNodeNeighbors.Clear();
                CompleatedPath.Clear();
                curr.G = 0;
                curr.H = HcostEstimate(curr.Location, End);


                OpenSet.Add(curr);

                Isrun = true;
            }
            
            if (OpenSet.Count != 0)
            {
                curr = GetLowestInList(OpenSet);

                if (curr.Location.X == End.X && curr.Location.Y == End.Y)
                {
                    CompleatedPath = ConstructPath(curr);
                    //LargestGScore = MaxGscore(ClosedSet);
                    IsComplete = true;
                    ValidPath = true;
                    Isrun = false;
                    return;
                }

                OpenSet.Remove(curr);
                ClosedSet.Add(curr);

                GetNeighborstonode(curr, CurNodeNeighbors);

                for (int i = 0; i < CurNodeNeighbors.Count; i++)
                {
                    if (ClosedSet.Contains(CurNodeNeighbors[i]))
                        continue;

                    double tentative_g_score = curr.G + DistBetween(curr.Location, CurNodeNeighbors[i].Location);

                    if (!OpenSet.Contains(CurNodeNeighbors[i]))
                    {
                        OpenSet.Add(CurNodeNeighbors[i]);
                    }
                    else if (tentative_g_score >= CurNodeNeighbors[i].G)
                    {
                        continue;
                    }
                    CurNodeNeighbors[i].Previous = curr;
                    CurNodeNeighbors[i].H = HcostEstimate(CurNodeNeighbors[i].Location, End);
                    CurNodeNeighbors[i].G = tentative_g_score;
                }

                CurNodeNeighbors.Clear();

            }
            if (OpenSet.Count == 0)
            {
                IsComplete = true;
                ValidPath = false;
            }
        }

        private StarNode GetLowestInList(List<StarNode> allnodes)
        {
            double currvalue = (int)Math.Pow(Map.GetLength(0) *  Map.GetLength(1), 2); 
            StarNode low = allnodes[0];

            for (int i = 0; i < allnodes.Count; i++)
            {
                if (allnodes[i].F < currvalue)
                {
                    low = allnodes[i];
                    currvalue = allnodes[i].F;
                }
            }
            return low;
        }
        private double HcostEstimate(Point current, Point goal)
        {
            double final = (Math.Abs(goal.X - current.X) +
                                    Math.Abs(goal.Y - current.Y));

            //final *= (1.0 + (1 / 1000));

            return final;
        }
        private void GetNeighborstonode(StarNode curr, List<StarNode> Nnodes)
        {
            // Retrieve 8 in a square
            for (int x = -1; x <= 1; x++)
                for (int y = -1; y <= 1; y++)
                {
                    if (x == 0 && y == 0)
                        continue;

                    if (curr.Location.X + x > -1 && curr.Location.X + x < Map.GetLength(0) &&
                        curr.Location.Y + y > -1 && curr.Location.Y + y < Map.GetLength(1))

                        if ((Map[curr.Location.X + x, curr.Location.Y + y].type != 1))
                        {
                            if (/*!OpenSet.Contains(Map[curr.Location.X + x, curr.Location.Y + y]) &&*/
                                !ClosedSet.Contains(Map[curr.Location.X + x, curr.Location.Y + y]))
                            {
                                if (Map[curr.Location.X + x, curr.Location.Y].type != 1
                                    && Map[curr.Location.X, curr.Location.Y + y].type != 1)
                                {
                                    Nnodes.Add(Map[curr.Location.X + x, curr.Location.Y + y]);
                                }
                            }
                            else
                                continue;

                        }
                }
        }

        private List<StarNode> ConstructPath(StarNode curr)
        {
            List<StarNode> final = new List<StarNode>();

            final.Add(curr);
            while (curr.Previous != null)
            {
                curr = curr.Previous;
                final.Add(curr);
            }
            return final;
        }

        private double MaxGscore(List<StarNode> allnodes)
        {
            double currvalue = 0;
            double High = allnodes[0].G;

            for (int i = 0; i < allnodes.Count; i++)
            {
                if (allnodes[i].G > currvalue)
                {
                    High = allnodes[i].G;
                    currvalue = allnodes[i].G;
                }
            }
            return High;
        }

        private double HeuristicCost_Euclidean(Point current, Point goal)
        {
            // x^2 + y^2 square root
            double H = Math.Sqrt(
                    Math.Pow(Math.Abs(goal.X - current.X), 2) +
                    Math.Pow(Math.Abs(goal.Y - current.Y), 2)
                );
            return H;
        }
    }
    class StarNode
    {
        public double F
        {
            get { return G + H; }
        }

        public double G;
        public double H;
        public Point Location;
        public StarNode Previous;
        public int type;
        public StarNode(Point l)
        {
            Location = l;
        }
        public double HcostEstimate(Point a, Point b)
        {
            double h = ((double)Math.Abs(a.X - b.X) +
                (double)Math.Abs(a.Y - b.Y));
            return h;
        }
        //public double HcostEstimate(Point goal)
        //{
        //    double h = (Math.Abs(goal.X - Location.X) +
        //        Math.Abs(goal.Y - Location.Y));
        //    return h;
        //}
        //public int GuristicEstimate(Point start)
        //{
        //    int g = (Math.Abs(start.X - Location.X) +
        //        Math.Abs(start.Y - Location.Y));
        //    return g;
        //}
    }
}
