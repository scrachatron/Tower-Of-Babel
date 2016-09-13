using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Graphics;
using System.Threading;

namespace _7seconds
{

    class Level : Pixelclass
    {
        
        public int[,] Map
        {
            get; set; 
        }
        public Point LayerSize
        {
            get { return m_LayerSize; }
        }

        public Point m_StartPos;
        public Point m_WinPos;
        private Point m_LayerSize;
        public MazeGenerator m_mazeGen;
        private MazeInfo m_inf;
        protected List<Point> m_chests = new List<Point>();

        public Level()
        {
            m_mazeGen = new MazeGenerator();
            m_inf = new MazeInfo(new Point(128,96), 40, 30, 10, 20,10);
            RegenMaze(m_inf);
        }
        public Level(MazeInfo inf)
        {
            m_mazeGen = new MazeGenerator();
            m_inf = inf;
            RegenMaze(inf);
        }


        public void RegenMaze(MazeInfo inf)
        {
            m_mazeGen.GenerateMaze(inf);
            Map = m_mazeGen.MapInformation.Map;

            m_LayerSize = new Point(Game1.TILESIZE, Game1.TILESIZE);
            Rectangle temprect = m_mazeGen.m_rooms[Game1.RNG.Next(0, m_mazeGen.m_rooms.Count)];
            m_StartPos = temprect.ReturnRandom(1);

            Rectangle tmprect = temprect;
            do
            {
                temprect = m_mazeGen.m_rooms[Game1.RNG.Next(0, m_mazeGen.m_rooms.Count)];
            } while (tmprect == temprect);

            m_WinPos = temprect.ReturnRandom(1);

            for (int i = 0; i < inf.NoOfChests; i++)
            {
                Rectangle room = m_mazeGen.m_rooms[Game1.RNG.Next(0, m_mazeGen.m_rooms.Count)];
                Point possiblechest = room.ReturnRandom(1);
                if (possiblechest != m_StartPos && possiblechest != m_WinPos)
                {
                    if (m_chests.Contains(possiblechest))
                        i--;
                    else
                        m_chests.Add(possiblechest);
                }



            }

        }

        public void DrawMe(SpriteBatch sb)
        {
            Map = m_mazeGen.MapInformation.Map;
            for (int x = 0; x < Map.GetLength(0); x++)
            {
                for (int y = 0; y < Map.GetLength(1); y++)
                {
                    if (Map[x, y] == 1)
                        sb.Draw(Pixel, new Rectangle(x * m_LayerSize.X, y * m_LayerSize.Y, m_LayerSize.X, m_LayerSize.Y), Color.Black);
                    else if (Map[x, y] == 2)
                        sb.Draw(Pixel, new Rectangle(x * m_LayerSize.X, y * m_LayerSize.Y, m_LayerSize.X, m_LayerSize.Y), Color.Green);
                    else if (Map[x, y] == 3)
                        sb.Draw(Pixel, new Rectangle(x * m_LayerSize.X, y * m_LayerSize.Y, m_LayerSize.X, m_LayerSize.Y), Color.Red);
                }
            }

            for (int i = 0; i < m_chests.Count; i++)
            {
                sb.Draw(Pixel, new Rectangle(m_chests[i].X * m_LayerSize.X, m_chests[i].Y * m_LayerSize.Y, m_LayerSize.X, m_LayerSize.Y), Color.Blue);
            }

            sb.Draw(Pixel, new Rectangle(m_StartPos.X * m_LayerSize.X, m_StartPos.Y * m_LayerSize.Y, m_LayerSize.X, m_LayerSize.Y), Color.Red);
            sb.Draw(Pixel, new Rectangle(m_WinPos.X * m_LayerSize.X, m_WinPos.Y * m_LayerSize.Y, m_LayerSize.X, m_LayerSize.Y), Color.Green);
        }
    }
}
