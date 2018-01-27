using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _7seconds
{
    class MazeGenerator
    {
        public int[,] m_stage;
        public int[,] m_regions;

        public MazeInfo MapInformation;

        private int roomtries;
        private int ConnectorChance;
        private int roomExtraSize;
        private int windingPercent;

        public List<Rectangle> m_rooms = new List<Rectangle>();
        private Rectangle bounds;
        private int currentregion;

        protected Random RNG { get; set; }

        public MazeGenerator()
        {
            RNG = new Random();
        }
        public MazeGenerator(int seed)
        {
            RNG = new Random(seed);
        }

        private void SetValues(MazeInfo inf)
        {
            MapInformation = inf;
            m_stage = MapInformation.Map;
            m_rooms = MapInformation.Rooms;

            roomtries = MapInformation.Roomtries;
            ConnectorChance = MapInformation.ConnectorChance;
            roomExtraSize = MapInformation.RoomExtraSize;
            windingPercent = MapInformation.WindingPercent;
    }

        public void GenerateMaze(MazeInfo inf)
        {
            SetValues(inf);

            m_regions = new int[inf.Map.GetLength(0), inf.Map.GetLength(1)];
            bounds = new Rectangle(0, 0, inf.Map.GetLength(0), inf.Map.GetLength(1));

            for (int x = 0; x < m_stage.GetLength(0); x++)
                for (int y = 0; y < m_stage.GetLength(1); y++)
                    m_stage[x, y] = 1;

            AddRooms();
            for (var y = 1; y < bounds.Height; y += 2)
            {
                for (var x = 1; x < bounds.Width; x += 2)
                {
                    Point pos = new Point(x, y);
                    if (m_stage[x, y] != 1) continue;
                    _growMaze(pos);
                }
            }

            Connect();
            RemoveDeadEnds();
        }
    
        private void AddRooms()
        {
            for (int i = 0; i < roomtries; i++)
            {
                /* Pick a random room size. The funny math here does two things:
                 - It makes sure rooms are odd-sized to line up with maze.
                 - It avoids creating rooms that are too rectangular: too tall and
                   narrow or too wide and flat.
                TODO: This isn't very flexible or tunable. Do something better here.
                */
                int size = RNG.Next(3, 7 + roomExtraSize) * 2 + 1;
                int rectangularity = RNG.Next(0, 1 + size / 2) * 2;
                int width = size;
                int height = size;
                if (RNG.Next(0, 2) == 0)
                {
                    width += rectangularity;
                }
                else
                {
                    height += rectangularity;
                }
                int x = 0;
                int y = 0;
                try
                {
                    x = RNG.Next((bounds.Width - width) / 2) * 2 + 1;

                    y = RNG.Next((bounds.Height - height) / 2) * 2 + 1;
                }
                catch
                {
                    continue;
                }

                Rectangle room = new Rectangle(x, y, width, height);

                var overlaps = false;

                if (room.X + room.Width > m_stage.GetLength(0) ||
                    room.Y + room.Height > m_stage.GetLength(1))
                {
                    continue;
                }

                for (int curroom = 0; curroom < m_rooms.Count; curroom++)
                {
                    if (room.Intersects(m_rooms[curroom]))
                    {
                        overlaps = true;
                        break;
                    }
                }

                if (overlaps)
                    continue;

                m_rooms.Add(room);

                currentregion++;

                Carve(room);
            }
        }
        public void Carve(Point pos)
        {
            m_stage[pos.X, pos.Y] = 0;
            m_regions[pos.X, pos.Y] = currentregion;
        }
        public void Carve(Rectangle rect)
        {
            for (int x = 0; x < rect.Width; x++)
                for (int y = 0; y < rect.Height; y++)
                {
                    m_stage[rect.X + x, rect.Y + y] = 0;
                    m_regions[rect.X + x, rect.Y + y] = currentregion;
                }
        }
        private bool _canCarve(Point pos, Direction direction)
        {
            Point temp = SolveDirection(direction);
            // Must end in bounds.

            if (!bounds.Contains(new Point(pos.X + temp.X * 3, pos.Y + temp.Y * 3)))
            {

                return false;
            }

            return m_stage[pos.X + temp.X * 2, pos.Y + temp.Y * 2] == 1;
        }
        private void _growMaze(Point start)
        {
            List<Point> cells = new List<Point>();
            Direction lastDir = 0;

            currentregion++;

            Carve(start);

            cells.Add(start);
            while (cells.Count > 0)
            {
                Point cell = cells[cells.Count-1];

                // See which adjacent cells are open.
                List<Point> unmadeCells = new List<Point>();

                for (Direction i = 0; i <= Direction.West; i++)
                {
                    if (_canCarve(cell, i))
                        unmadeCells.Add(SolveDirection(i));
                }

                if (unmadeCells.Count > 0)
                {
                    Direction dir;
                    if (unmadeCells.Contains(SolveDirection(lastDir)) && RNG.Next(0, 101) > windingPercent)
                    {
                        dir = lastDir;
                    }
                    else
                    {
                        dir = SolvePoint(unmadeCells[RNG.Next(0, unmadeCells.Count)]);
                    }

                    Carve(cell + SolveDirection(dir));
                    Carve(new Point(cell.X + SolveDirection(dir).X * 2, cell.Y + SolveDirection(dir).Y * 2));

                    cells.Add(new Point(cell.X + SolveDirection(dir).X * 2, cell.Y + SolveDirection(dir).Y * 2));
                    lastDir = dir;
                }
                else
                {
                    // No adjacent uncarved cells.
                    cells.RemoveAt(cells.Count - 1);

                    // This path has ended.
                    lastDir = Direction.Null; 
                }
            }
        }

        private void ConnectRegions()
        {
            // Find all of the tiles that can connect two (or more) regions.
            Dictionary<Point, List<int>> connectorRegions = new Dictionary<Point, List<int>>();

            for (int x = 1; x < bounds.Width - 1; x++)
                for (int y = 1; y < bounds.Height - 1; y++)
                {
                    if (m_stage[x, y] != 1)
                        continue;

                    List<int> regions = new List<int>();
                    for (Direction dir = 0; dir <= Direction.West; dir++)
                    {
                        int region = m_regions[x + SolveDirection(dir).X, y + SolveDirection(dir).Y];
                        if (region != 0)
                            regions.Add(region);
                    }

                    if (regions.Count < 2 || regions[0] == regions[1])
                        continue;
                    connectorRegions.Add(new Point(x, y), regions);
                }

            List<Point> connectors = connectorRegions.Keys.ToList();

            // Keep track of which regions have been merged. This maps an original
            // region index to the one it has been merged to.
            int[] merged = new int[currentregion];
            List<int> openRegions = new List<int>();
            for (int i = 0; i < currentregion; i++)
            {
                merged[i] = i;
                openRegions.Add(i);
            }

            // Keep connecting regions until we're down to one.
            while (openRegions.Count > 1)
            {
                if (connectors.Count == 0)
                    break;

                Point connector = connectors[RNG.Next(0, connectors.Count)];
                
                // Carve the connection.
                AddJunction(connector);

                // Merge the connected regions. We'll pick one region (arbitrarily) and
                // map all of the other regions to its index.


                //List<int> regions = connectorRegions[connector].Where((region) =>
                //{
                //    if (region == merged[region])
                //        return false;
                //    return true;
                //}).ToList();

                List<int> regions = new List<int>();
                for (int i = 0; i < connectorRegions[connector].Count; i++)
                    regions.Add(connectorRegions[connector][i]);

                //region => merged[region];
                //.map((region) => merged[region]);

                int dest = regions[0];
                List<int> sources = regions.Skip(1).ToList();

                // Merge all of the affected regions. We have to look at *all* of the
                // regions because other regions may have previously been merged with
                // some of the ones we're merging now.
                for (int i = 0; i < currentregion; i++)
                {
                    if (sources.Contains(merged[i]))
                    {
                        merged[i] = dest;
                    }
                }

                // The sources are no longer in use.
                for (int i = 0; i < sources.Count; i++)
                    openRegions.Remove(sources[i]);


                List<Point> temp = connectors.Where((pos, value) =>
                {
                    var r = connectorRegions[pos].ToLookup(y => merged[y - 1]).ToList();

                    if (r.Count > 1) return false;

                    if (RNG.Next(0, ConnectorChance) == 0)
                        AddJunction(pos);

                    return true;
                }).ToList();

                for (int itemtorem = 0; itemtorem < temp.Count; itemtorem++)
                    connectors.Remove(temp[itemtorem]);

                

            }
        }

        private void Connect()
        {
            Dictionary<Point, List<int>> connectorRegions = new Dictionary<Point, List<int>>();

            for (int x = 1; x < bounds.Width - 1; x++)
                for (int y = 1; y < bounds.Height - 1; y++)
                {
                    if (m_stage[x, y] != 1)
                        continue;

                    List<int> regions = new List<int>();
                    for (Direction dir = 0; dir <= Direction.West; dir++)
                    {
                        int region = m_regions[x + SolveDirection(dir).X, y + SolveDirection(dir).Y];
                        if (region != 0)
                            regions.Add(region);
                    }

                    if (regions.Count < 2 || regions[0] == regions[1])
                        continue;
                    connectorRegions.Add(new Point(x, y), regions);
                }

            List<Point> connectors = connectorRegions.Keys.ToList();
 
            while (connectorRegions.Count > 0)
            {
                if (connectors.Count == 0)
                    break;

                Point connector = connectors[RNG.Next(0, connectors.Count)];

                // Carve the connection.
                AddJunction(connector);

                int targetRegion = connectorRegions[connector][0];
                int srcRegion = connectorRegions[connector][1];

                

                for (int x = 0; x < bounds.Width; x++)
                    for (int y = 0; y < bounds.Height; y++)
                    {
                        if (m_regions[x,y] == connectorRegions[connector][1])
                            m_regions[x,y] = targetRegion;
                    }

                Dictionary<Point, List<int>> tempregions = new Dictionary<Point, List<int>>();

                for (int i = 0; i < connectors.Count; i++)
                    tempregions.Add(connectors[i], connectorRegions[connectors[i]]);


                for (int i = 0; i < connectors.Count; i++)
                {
                    if (connectorRegions[connectors[i]][0] == targetRegion &&
                        connectorRegions[connectors[i]][1] == srcRegion)
                    {
                        if (RNG.Next(0, 100) < ConnectorChance)
                            AddJunction(connectors[i]);

                        tempregions.Remove(connectors[i]);
                        connectors.RemoveAt(i);
                        i--;
                    }
                }

                connectorRegions = tempregions;
            }

        }

        private void AddJunction(Point pos)
        {
                m_stage[pos.X, pos.Y] = 0;
        }

        private void RemoveDeadEnds()
        {
            var done = false;

            while (!done)
            {
                done = true;

                for (int x = 0; x < bounds.Width - 1; x++)
                    for (int y = 0; y < bounds.Height - 1; y++)
                    {
                        if (m_stage[x, y] == 1)
                            continue;

                        // If it only has one exit, it's a dead end.
                        var exits = 0;
                        for (Direction dir = 0; dir <= Direction.West; dir++)
                        {
                            if (m_stage[x + SolveDirection(dir).X, y + SolveDirection(dir).Y] != 1)
                                exits++;
                        }

                        if (exits != 1) continue;

                        done = false;
                        m_stage[x, y] = 1;
                    }
            }
        }

        private Point SolveDirection(Direction dir)
        {
            switch (dir)
            {
                case Direction.South:
                    return new Point(0, 1);
                case Direction.North:
                    return new Point(0, -1);
                case Direction.East:
                    return new Point(1, 0);
                case Direction.West:
                    return new Point(-1, 0);
            }
            return new Point(0, 0);
        }
        private Direction SolvePoint(Point dir)
        {
            if (dir.X > 0)
                return Direction.East;
            else if (dir.X < 0)
                return Direction.West;
            else if (dir.Y > 0)
                return Direction.South;
            else if (dir.Y < 0)
                return Direction.North;


            return Direction.Null;
        }
    }

    class TownGenerator : MazeGenerator
    {


        public TownGenerator(int seed)
            :base (seed)
        {
            m_stage = new int[128, 96];
            for (int x = 0; x < m_stage.GetLength(0); x++)
                for (int y = 0; y < m_stage.GetLength(1); y++)
                    m_stage[x, y] = 1;

            m_regions = new int[128, 96];

            base.Carve(new Rectangle(1, 1, m_stage.GetLength(0) - 2, m_stage.GetLength(1) - 2));
            
        }


    }


}

public struct MazeInfo
{
    public int Roomtries;
    public int ConnectorChance;
    public int RoomExtraSize;
    public int WindingPercent;
    public int NoOfChests;
    public int[,] Map;
    public List<Rectangle> Rooms;

    public MazeInfo(Point Size, int maxrooms, int chance, int roomsize, int wind,int chests)
    {
        Rooms = new List<Rectangle>();

        if (Size.X % 2 == 0)
            Size.X++;
        if (Size.Y % 2 == 0)
            Size.Y++;

        Map = new int[Size.X, Size.Y];

        if (maxrooms > 1)
        {
            Roomtries = maxrooms;
        } else
        { Roomtries = 1; }

        for (int x = 0; x < Map.GetLength(0); x++)
            for (int y = 0; y < Map.GetLength(1); y++)
                Map[x, y] = 1;

        if (wind >= 0 && wind <=100)
        {
            WindingPercent = wind;
        }
        else { WindingPercent = 30; }

        if (chance >= 0 && chance >= 100)
        {
            ConnectorChance = chance;
        } else { ConnectorChance = 3; }

        RoomExtraSize = roomsize;

        NoOfChests = chests;
    }
}



enum Direction
{
    North = 0,
    East = 1,
    South = 2,
    West = 3,
    Null = -1

}
