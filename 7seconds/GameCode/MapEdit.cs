using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Xml.Serialization;
using System.IO;

namespace _7seconds
{
    [Serializable]
    public struct LevelData
    {
        public List<int[]> SolvedLayer;
        public Point sizeXbyY;
        public string songtitle;

        public LevelData(List<int[,]> numbers)
        {
            sizeXbyY = new Point(numbers[0].GetLength(0), numbers[0].GetLength(1));
            SolvedLayer = new List<int[]>();
            songtitle = "Track1";

            for (int i = 0; i < numbers.Count; i++)
            {
                SolvedLayer.Add(new int[numbers[i].GetLength(0) * numbers[i].GetLength(1)]);
                System.Buffer.BlockCopy(numbers[i], 0, SolvedLayer[i], 0, numbers[i].GetLength(0) * numbers[i].GetLength(1));
            }
        }
    }

    class MapEdit : Pixelclass
    {
        private Level levelCreate;
        public bool help;
        public bool MapChanged;
        public MazeGenerator m_maze;

        public Level Level
        {
            get { return levelCreate; }
        }

        public MapEdit()
        {
            levelCreate = new Level();

            help = true;
        }
        //public bool CreateLayer(int[,] ColMap)
        //{
        //    if (new Point(ColMap.GetLength(1), ColMap.GetLength(0)) == m_LayerSize)
        //    {
        //        m_SimpleLayer.Add(ColMap);
        //        return true;
        //    }
        //    else
        //        throw new SystemException("The current map does not have the same dimentions it excpects");
        //    //return false;
        //}
        //public void AddLayer()
        //{
        //    m_SimpleLayer.Add(new int[m_LayerSize.X, m_LayerSize.Y]);
        //}

        public void Reset()
        {
            levelCreate = new Level();
            MapChanged = true;
        }
        public void Reset(int Tilewidth)
        { 
            MapChanged = true;
        }
        public void RandomFill(int Percent)
        {
            m_maze = new MazeGenerator();

            List<Rectangle> m_exitRooms = new List<Rectangle>();
            List<Point> m_exits = new List<Point>();

            if (m_maze.m_rooms.Count > 1)
            {
                do
                {
                    m_exitRooms.Add(m_maze.m_rooms[Game1.RNG.Next(0, m_maze.m_rooms.Count)]);
                    m_exitRooms.Add(m_maze.m_rooms[Game1.RNG.Next(0, m_maze.m_rooms.Count)]);

                    if (m_exitRooms[0] != m_exitRooms[1])
                    {
                        break;
                    }
                    m_exitRooms.Clear();
                } while (true);

                m_exits.Add(new Point(m_exitRooms[0].X + Game1.RNG.Next(1, m_exitRooms[0].Width - 1), m_exitRooms[0].Y +Game1.RNG.Next(1, m_exitRooms[0].Height - 1)));
                m_exits.Add(new Point(m_exitRooms[1].X +Game1.RNG.Next(1, m_exitRooms[1].Width - 1), m_exitRooms[1].Y +Game1.RNG.Next(1, m_exitRooms[1].Height - 1)));

            }
            else if (m_maze.m_rooms.Count == 1)
            {

                m_exitRooms.Add(m_maze.m_rooms[0]);
                do
                {
                    m_exits.Add(new Point(m_exitRooms[0].X +Game1.RNG.Next(1, m_exitRooms[0].Width - 1), m_exitRooms[0].Y +Game1.RNG.Next(1, m_exitRooms[0].Height - 1)));
                    m_exits.Add(new Point(m_exitRooms[0].X +Game1.RNG.Next(1, m_exitRooms[0].Width - 1), m_exitRooms[0].Y +Game1.RNG.Next(1, m_exitRooms[0].Height - 1)));
                    if (m_exits[0] != m_exits[1])
                    {
                        break;
                    }

                } while (true);
            }
            else
            {
                m_exits.Add(new Point(0,0));
                m_exits.Add(new Point(0, 1));
            }

            //m_exits.Clear();
            //m_exits.Add(new Point(0, 0));
            //m_exits.Add(new Point(0, 1));

            
            levelCreate.Map[m_exits[0].X, m_exits[0].Y] = 2;
            levelCreate.Map[m_exits[1].X, m_exits[1].Y] = 3;

            MapChanged = true;
        }
        public void UpdateMe(GameTime gt, InputManager Input)
        {
            MapChanged = false;
            if (Input.WasPressedBack(Keys.H))
            {
                help = !help;
            }
            for (int x = 0; x < levelCreate.Map.GetLength(1); x++)
            {
                for (int y = 0; y < levelCreate.Map.GetLength(0); y++)
                {
                    if (Input.ThisMouse.LeftButton == ButtonState.Pressed && new Rectangle(x * Game1.TILESIZE, y * Game1.TILESIZE, Game1.TILESIZE, Game1.TILESIZE).Contains(Input.ThisMouse.Position))
                    {
                        if (levelCreate.Map[y, x] != 2 && levelCreate.Map[y, x] != 3 && levelCreate.Map[y, x] != 1)
                        {
                            levelCreate.Map[y, x] = 1;
                            MapChanged = true;
                            
                        }
                    }
                    else if (Input.ThisMouse.RightButton == ButtonState.Pressed && new Rectangle(x * Game1.TILESIZE, y * Game1.TILESIZE, Game1.TILESIZE, Game1.TILESIZE).Contains(Input.ThisMouse.Position))
                    {
                        if (levelCreate.Map[y, x] != 2 && levelCreate.Map[y, x] != 3 && levelCreate.Map[y, x] != 0)
                        {
                            levelCreate.Map[y, x] = 0;
                            MapChanged = true;
                        }
                    }
                    else if (Input.WasPressedBack(Keys.A) && new Rectangle(x * Game1.TILESIZE, y * Game1.TILESIZE, Game1.TILESIZE, Game1.TILESIZE).Contains(Input.ThisMouse.Position))
                    {
                        if (levelCreate.Map[y, x] == 0)
                        {
                            for (int x2 = 0; x2 < levelCreate.Map.GetLength(1); x2++)
                                for (int y2 = 0; y2 < levelCreate.Map.GetLength(0); y2++)
                                    if (levelCreate.Map[y2, x2] == 2)
                                        levelCreate.Map[y2, x2] = 0;

                            levelCreate.Map[y, x] = 2;
                            levelCreate.m_StartPos = new Point(y, x);
                            MapChanged = true;
                        }
                    }
                    else if (Input.WasPressedBack(Keys.D) && new Rectangle(x * Game1.TILESIZE, y * Game1.TILESIZE, Game1.TILESIZE, Game1.TILESIZE).Contains(Input.ThisMouse.Position))
                    {
                        if (levelCreate.Map[y, x] == 0)
                        {
                            for (int x2 = 0; x2 < levelCreate.Map.GetLength(1); x2++)
                                for (int y2 = 0; y2 < levelCreate.Map.GetLength(0); y2++)
                                    if (levelCreate.Map[y2, x2] == 3)
                                        levelCreate.Map[y2, x2] = 0;

                            levelCreate.Map[y, x] = 3;
                            levelCreate.m_WinPos = new Point(y, x);
                            MapChanged = true;
                        }
                    }
                }
            }
        }
        public void DrawMe(SpriteBatch sb)
        {
            levelCreate.DrawMe(sb);
        }



    }

}
