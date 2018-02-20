using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Graphics;
using System.Threading;
using Tower_Of_Babel;

namespace Tower_Of_Babel
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
        public List<Enemy> Enemies
        {
            get { return m_enemys; }
        }


        public Point m_StartPos;
        public Point m_WinPos;
        private Point m_LayerSize;
        public MazeGenerator m_mazeGen;

        private MazeInfo m_inf;
        protected List<Point> m_chests = new List<Point>();
        protected List<Enemy> m_enemys = new List<Enemy>();

        public Level()
        {
            m_mazeGen = new MazeGenerator();
            m_inf = new MazeInfo(new Point(128,96), 40, 30, 7, 15, 10);
            RegenMaze(m_inf);
        }
        public Level(int seed)
        {
            m_mazeGen = new MazeGenerator(seed);
            m_inf = new MazeInfo(new Point(128, 96), 40, 30, 7, 15, 10);
            RegenMaze(m_inf);
        }
        public Level(MazeInfo inf)
        {
            m_mazeGen = new MazeGenerator();
            m_inf = inf;
            RegenMaze(inf);
        }

        public virtual void UpdateMe(GameTime gt, Player p, TouchInputManager input)
        {
            if(input.m_Touches.Count == 5)
            {
                m_enemys.Add(new Enemy(this, p));
            }

            if (Game1.PlayerTurn == false)
            {
                for (int i = 0; i < m_enemys.Count; i++)
                {
                    m_enemys[i].TakeTurn();
                }
                Game1.PlayerTurn = true;
            }

            for (int i = 0; i < m_enemys.Count; i++)
            {
                m_enemys[i].UpdateMe(gt, this);
            }




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

        public void RegenTown()
        {
            Map = m_mazeGen.MapInformation.Map;

            m_LayerSize = new Point(Game1.TILESIZE, Game1.TILESIZE);
        }
        
        public void DrawMe(SpriteBatch sb,minimap MiniMap)
        {
            Map = m_mazeGen.MapInformation.Map;
            for (int x = 0; x < Map.GetLength(0); x++)
            {
                for (int y = 0; y < Map.GetLength(1); y++)
                {
                    if (Map[x, y] == 1)
                        sb.Draw(Pixel, new Rectangle(x * m_LayerSize.X, y * m_LayerSize.Y, m_LayerSize.X, m_LayerSize.Y), Color.Black);
                    //else if (Map[x, y] == 2 && MiniMap.IsVisible(new Point(x,y)))
                    //    sb.Draw(Pixel, new Rectangle(x * m_LayerSize.X, y * m_LayerSize.Y, m_LayerSize.X, m_LayerSize.Y), Color.Green);
                    //else if (Map[x, y] == 3 && MiniMap.IsVisible(new Point(x, y)))
                    //    sb.Draw(Pixel, new Rectangle(x * m_LayerSize.X, y * m_LayerSize.Y, m_LayerSize.X, m_LayerSize.Y), Color.Red);
                }
            }

            for (int i = 0; i < m_chests.Count; i++)
            {
                if (MiniMap.IsVisible(new Point(m_chests[i].X, m_chests[i].Y)))
                    sb.Draw(Pixel, new Rectangle(m_chests[i].X * m_LayerSize.X, m_chests[i].Y * m_LayerSize.Y, m_LayerSize.X, m_LayerSize.Y), Color.Blue);
            }

            if (MiniMap.IsVisible(m_StartPos))
                sb.Draw(Pixel, new Rectangle(m_StartPos.X * m_LayerSize.X, m_StartPos.Y * m_LayerSize.Y, m_LayerSize.X, m_LayerSize.Y), Color.Red);
            else if (MiniMap.IsVisible(m_WinPos))
                sb.Draw(Pixel, new Rectangle(m_WinPos.X * m_LayerSize.X, m_WinPos.Y * m_LayerSize.Y, m_LayerSize.X, m_LayerSize.Y), Color.Green);
        }

        public virtual void DrawMe(SpriteBatch sb, minimap minimap, Point playerpos)
        {


         

            sb.Draw(Pixel, new Rectangle(0, 0, m_LayerSize.X * minimap.ActiveArea.GetLength(0), m_LayerSize.X * minimap.ActiveArea.GetLength(1)), Color.Black);



            for (int x = 0; x < Map.GetLength(0); x++)
            {
                for (int y = 0; y < Map.GetLength(1); y++)
                {
                    if (minimap.Terrain[x, y] == true)
                    {

                        if (Map[x, y] == 0)
                            sb.Draw(Pixel, new Rectangle(x * m_LayerSize.X, y * m_LayerSize.Y, m_LayerSize.X, m_LayerSize.Y), Color.White);
                        else if (Map[x, y] == 2)
                            sb.Draw(Pixel, new Rectangle(x * m_LayerSize.X, y * m_LayerSize.Y, m_LayerSize.X, m_LayerSize.Y), Color.Green);
                        else if (Map[x, y] == 3)
                            sb.Draw(Pixel, new Rectangle(x * m_LayerSize.X, y * m_LayerSize.Y, m_LayerSize.X, m_LayerSize.Y), Color.Red);
                    }

                }
            }

            for (int x = 0; x < minimap.ActiveArea.GetLength(0); x++)
                for (int y = 0; y < minimap.ActiveArea.GetLength(1); y++)
                {
                    if (minimap.ActiveArea[x, y] == false)
                        sb.Draw(Pixel, new Rectangle(x * m_LayerSize.X, y * m_LayerSize.Y, m_LayerSize.X, m_LayerSize.Y), Color.Black * 0.75f);
                }

            for (int i = 0; i < m_chests.Count; i++)
            {
                if (minimap.ActiveArea[m_chests[i].X, m_chests[i].Y])
                    sb.Draw(Pixel, new Rectangle(m_chests[i].X * m_LayerSize.X, m_chests[i].Y * m_LayerSize.Y, m_LayerSize.X, m_LayerSize.Y), Color.Blue);
            }


            for (int i = 0; i < m_enemys.Count; i++)
            {
                if (minimap.ActiveArea[m_enemys[i].VirtualPosition.X, m_enemys[i].VirtualPosition.Y])
                    m_enemys[i].DrawMe(sb);

            }

                sb.Draw(Pixel, new Rectangle(m_StartPos.X * m_LayerSize.X, m_StartPos.Y * m_LayerSize.Y, m_LayerSize.X, m_LayerSize.Y), Color.Red);
                sb.Draw(Pixel, new Rectangle(m_WinPos.X * m_LayerSize.X, m_WinPos.Y * m_LayerSize.Y, m_LayerSize.X, m_LayerSize.Y), Color.Green);
        }
    }

}
