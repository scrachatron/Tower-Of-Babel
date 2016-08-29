using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _7seconds
{
    class Lee
    {
        public List<LeeNode> OpenSet;
        public List<LeeNode> ClosedSet;
        private List<LeeNode> CurNodeNeighbors;
        public List<LeeNode> CompleatedPath = new List<LeeNode>();

        private LeeNode curr;

        LeeNode[,] Map;
        Point Start, End;
        //Point[] Path;

        public bool ValidPath;
        bool IsComplete;

        public double LargestGScore;
        public void ReloadPathfinding(int[,] map, Point start, Point end)
        {
            Map = new LeeNode[map.GetLength(0), map.GetLength(1)];
            for (int x = 0; x < map.GetLength(0); x++)
                for (int y = 0; y < map.GetLength(1); y++)
                {
                    Map[x, y] = new LeeNode(new Point(x, y));
                    Map[x, y].type = map[x, y];
                }

            Start = start;
            curr = new LeeNode(start);
            End = end;

            ValidPath = false;
            IsComplete = false;

            OpenSet = new List<LeeNode>();
            ClosedSet = new List<LeeNode>();
            CurNodeNeighbors = new List<LeeNode>();
            CompleatedPath = new List<LeeNode>();

            RunPathfinding();
        }

        private void RunPathfinding()
        {
            curr.G = 0;

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

                    if (!OpenSet.Contains(CurNodeNeighbors[i]))
                    {
                        CurNodeNeighbors[i].Previous = curr;
                        //CurNodeNeighbors[i].G = curr.G + 1;

                        OpenSet.Add(CurNodeNeighbors[i]);
                    }
                }
                CurNodeNeighbors.Clear();
            }
            if (OpenSet.Count == 0)
            {
                IsComplete = true;
                ValidPath = false;
            }
        }
        private LeeNode GetLowestInList(List<LeeNode> allnodes)
        {
            double currvalue = (int)Math.Pow(Map.GetLength(0) * Map.GetLength(1), 2);
            LeeNode low = allnodes[0];

            for (int i = 0; i < allnodes.Count; i++)
            {
                if (allnodes[i].G < currvalue)
                {
                    low = allnodes[i];
                    currvalue = allnodes[i].G;
                }
            }
            return low;
        }
        private void GetNeighborstonode(LeeNode curr, List<LeeNode> Nnodes)
        {

            #region oldcode
            // int Xpos = curr.Location.X < Map.GetLength(0) - 1 ? curr.Location.X + 1 : curr.Location.X;
            // int Xneg = curr.Location.X > 0 ? curr.Location.X - 1 : curr.Location.X;
            // int Ypos = curr.Location.Y < Map.GetLength(1) - 1 ? curr.Location.Y + 1 : curr.Location.Y;
            // int Yneg = curr.Location.Y > 0 ? curr.Location.Y - 1 : curr.Location.Y;
            //
            // if (!OpenSet.Contains(Map[curr.Location.X, Yneg]) &&
            //     (Map[curr.Location.X, Yneg].type != 1))
            //     Nnodes.Add(Map[curr.Location.X, Yneg]);
            //
            // if (!OpenSet.Contains(Map[Xpos, curr.Location.Y]) &&
            //     (Map[Xpos, curr.Location.Y].type != 1))
            //     Nnodes.Add(Map[Xpos, curr.Location.Y]);
            //
            //
            // if (!OpenSet.Contains(Map[curr.Location.X, Ypos]) &&
            //     (Map[curr.Location.X, Ypos].type != 1))
            //     Nnodes.Add(Map[curr.Location.X, Ypos]);
            //
            // if (!OpenSet.Contains(Map[Xneg, curr.Location.Y]) &&
            //     (Map[Xneg, curr.Location.Y].type != 1))
            //     Nnodes.Add(Map[Xneg, curr.Location.Y]);
            #endregion

            // Retrieve 8 in a square
            for (int x = -1; x <= 1; x++)
                for (int y = -1; y <= 1; y++)
                {
                    if (x == 0 && y == 0)
                        continue;

                    if (curr.Location.X + x > -1 && curr.Location.X + x < Map.GetLength(0) &&
                        curr.Location.Y + y > -1 && curr.Location.Y + y < Map.GetLength(1))
                        if (!OpenSet.Contains(Map[curr.Location.X + x, curr.Location.Y + y]) &&
                            (Map[curr.Location.X + x, curr.Location.Y + y].type != 1))
                        {
                            if (Map[curr.Location.X + x, curr.Location.Y].type != 1
                                && Map[curr.Location.X, curr.Location.Y + y].type != 1)
                            {
                                Nnodes.Add(Map[curr.Location.X + x, curr.Location.Y + y]);

                                //Nnodes[Nnodes.Count - 1].G

                                if (x != 0 && y != 0)
                                {
                                    Nnodes[Nnodes.Count - 1].G = curr.G + 1.41421356237;
                                    //Math.Sqrt((Math.Pow(Start.X - Nnodes[Nnodes.Count - 1].Location.X, 2) + Math.Pow(Start.Y - Nnodes[Nnodes.Count - 1].Location.Y, 2)));
                                }
                                else
                                {
                                    Nnodes[Nnodes.Count - 1].G = curr.G + 1;
                                    //Math.Sqrt((Math.Pow(Start.X - Nnodes[Nnodes.Count - 1].Location.X, 2) + Math.Pow(Start.Y - Nnodes[Nnodes.Count - 1].Location.Y, 2)));
                                }
                                //Math.Sqrt((Math.Pow(Start.X - Nnodes[Nnodes.Count - 1].Location.X, 2) + Math.Pow(Start.Y - Nnodes[Nnodes.Count - 1].Location.Y, 2)));
                            }
                        }
                }

        }

        private List<LeeNode> ConstructPath(LeeNode curr)
        {
            List<LeeNode> final = new List<LeeNode>();

            final.Add(curr);
            while (curr.Previous != null)
            {
                curr = curr.Previous;
                final.Add(curr);
            }
            return final;
        }
        private double MaxGscore(List<LeeNode> allnodes)
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
    }

    class LeeNode
    {
        public double G;
        public Point Location;
        public LeeNode Previous;
        public int type;

        public LeeNode(Point l)
        {
            Location = l;
        }
    }
}
