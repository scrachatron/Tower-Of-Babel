using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Tower_Of_Babel
{
    class minimap : Pixelclass
    {
        public bool[,] Terrain
        {
            get
            {
                return m_visibleterrain;
            }
        }
        public bool[,] ActiveArea
        {
            get
            {
                return Active_area;
            }
        }



        

        bool[,] m_visibleterrain;

        bool[,] Active_area;
        int mapsize = 16;

        //Level m_lvl;
        public minimap(int mapwidth)
        {
            mapsize = Game1.graphics.PreferredBackBufferWidth / 128;
        }
        public bool IsVisible(Point m_point)
        {
            return (m_visibleterrain[m_point.X, m_point.Y]);
        }
        public void ShowAll()
        {
            for (int x = 0; x < m_visibleterrain.GetLength(0); x++)
                for (int y = 0; y < m_visibleterrain.GetLength(1); y++)
                    m_visibleterrain[x, y] = true;
        }


        public void UpdateMap(Level lvl)
        {
            m_visibleterrain = new bool[lvl.Map.GetLength(0), lvl.Map.GetLength(1)];
            Active_area = new bool[lvl.Map.GetLength(0), lvl.Map.GetLength(1)];
            for (int x = 0; x < m_visibleterrain.GetLength(0); x++)
                for (int y = 0; y < m_visibleterrain.GetLength(1); y++)
                {
                    m_visibleterrain[x, y] = false;
                    Active_area[x, y] = false;
                }

            //m_lvl = lvl;


        }
        public void UpdateMe(GameTime gt,Player p, Level lvl)
        {
            for (int x = 0; x < Active_area.GetLength(0); x++)
                for (int y = 0; y < Active_area.GetLength(1); y++)
                {
                    Active_area[x, y] = false;
                }

            for (int x = -2; x < 3; x++)
                for (int y = -2; y < 3; y++)
                {
                    if (new Rectangle(0, 0, Active_area.GetLength(0), Active_area.GetLength(1)).Contains(new Point(p.VirtualPosition.X + x, p.VirtualPosition.Y + y)))
                    {
                        m_visibleterrain[p.VirtualPosition.X + x, p.VirtualPosition.Y + y] = true;
                        Active_area[p.VirtualPosition.X + x, p.VirtualPosition.Y + y] = true;
                    }

                }

            // fill fog of war for rooms
            for (int i = 0; i < lvl.m_mazeGen.m_rooms.Count; i++)
                if (lvl.m_mazeGen.m_rooms[i].Contains(p.VirtualPosition))
                {
                    for (int x= lvl.m_mazeGen.m_rooms[i].X - 1; x < lvl.m_mazeGen.m_rooms[i].Right + 1; x++)
                        for (int y= lvl.m_mazeGen.m_rooms[i].Y - 1; y < lvl.m_mazeGen.m_rooms[i].Bottom +1; y++)
                        {
                            m_visibleterrain[x, y] = true;
                            Active_area[x, y] = true;

                        }
                }
                

        }
        public void DrawMe(SpriteBatch sb,Level lvl,Point p)
        {
            sb.Draw(Pixel, new Rectangle(0, 0, Game1.graphics.PreferredBackBufferWidth, Game1.graphics.PreferredBackBufferHeight), Color.Black * 0.5f);

            for (int x = 0; x < m_visibleterrain.GetLength(0); x++)
                for (int y = 0; y < m_visibleterrain.GetLength(1); y++)
                    if (m_visibleterrain[x,y] == true)
                    {
                        if (lvl.Map[x,y] == 0)
                            sb.Draw(Pixel, new Rectangle(x * mapsize, y * mapsize, mapsize, mapsize), Color.White * 0.75f);

                    }

#if DEBUG
            sb.Draw(Pixel, new Rectangle(p.X * mapsize, p.Y * mapsize, mapsize, mapsize), Color.Blue * 0.75f);
            sb.Draw(Pixel, new Rectangle(lvl.m_StartPos.X * mapsize, lvl.m_StartPos.Y * mapsize, mapsize, mapsize), Color.Red * 0.75f);
            sb.Draw(Pixel, new Rectangle(lvl.m_WinPos.X * mapsize, lvl.m_WinPos.Y * mapsize, mapsize, mapsize), Color.Green * 0.75f);
#endif
        }

        public void DrawFogOfWar(SpriteBatch sb, Level lvl)
        {
            //sb.Draw(Pixel, new Rectangle(0, 0, Game1.graphics.PreferredBackBufferWidth, Game1.graphics.PreferredBackBufferHeight), Color.Black * 0.5f);

            for (int x = 0; x < m_visibleterrain.GetLength(0); x++)
                for (int y = 0; y < m_visibleterrain.GetLength(1); y++)
                    if (m_visibleterrain[x, y] == false)
                    {
                        if (lvl.Map[x, y] == 0)
                            sb.Draw(Pixel, new Rectangle(x * Game1.TILESIZE, y * Game1.TILESIZE, Game1.TILESIZE, Game1.TILESIZE), Color.Black * 0.75f);

                    }

        }



    }
}