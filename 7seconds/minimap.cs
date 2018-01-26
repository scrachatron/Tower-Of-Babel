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
using _7seconds;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Tower_Of_Babel
{
    class minimap : Pixelclass
    {

        

        bool[,] m_visibleterrain;
        bool[,] m_fogOfWar;
        //Level m_lvl;
        public minimap()
        {
           
        }
        public bool IsVisible(Point m_point)
        {
            return (m_fogOfWar[m_point.X, m_point.Y]);
        }

        public void UpdateMap(Level lvl)
        {
            m_visibleterrain = new bool[lvl.Map.GetLength(0), lvl.Map.GetLength(1)];
            m_fogOfWar = new bool[lvl.Map.GetLength(0), lvl.Map.GetLength(1)];

            for (int x = 0; x < m_visibleterrain.GetLength(0); x++)
                for (int y = 0; y < m_visibleterrain.GetLength(1); y++)
                {
                    m_fogOfWar[x, y] = false;
                    m_visibleterrain[x, y] = false;
                }
            //m_lvl = lvl;


        }
        public void UpdateMe(GameTime gt,Player p, Level lvl)
        {
            // Reset Fog Of War
            for (int x = 0; x < m_visibleterrain.GetLength(0); x++)
                for (int y = 0; y < m_visibleterrain.GetLength(1); y++)
                {
                    m_fogOfWar[x, y] = false;
                }

            // Fill visible map and fog of war
            for (int x = -1; x < 2; x++)
                for (int y = -1; y < 2; y++)
                {
                    m_visibleterrain[p.VirtualPosition.X + x, p.VirtualPosition.Y + y] = true;
                    m_fogOfWar[p.VirtualPosition.X + x, p.VirtualPosition.Y + y] = true;
                }

            // fill fog of war for rooms
            for (int i = 0; i < lvl.m_mazeGen.m_rooms.Count; i++)
                if (lvl.m_mazeGen.m_rooms[i].Contains(p.VirtualPosition))
                {
                    for (int x= lvl.m_mazeGen.m_rooms[i].X - 1; x < lvl.m_mazeGen.m_rooms[i].Right + 1; x++)
                        for (int y= lvl.m_mazeGen.m_rooms[i].Y - 1; y < lvl.m_mazeGen.m_rooms[i].Bottom +1; y++)
                        {
                            m_visibleterrain[x, y] = true;
                            m_fogOfWar[x, y] = true;
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
                            sb.Draw(Pixel, new Rectangle(x * 16, y * 16, 16, 16), Color.White * 0.75f);

                    }

#if DEBUG
            sb.Draw(Pixel, new Rectangle(p.X * 16, p.Y * 16, 16, 16), Color.Blue * 0.75f);
            sb.Draw(Pixel, new Rectangle(lvl.m_StartPos.X * 16, lvl.m_StartPos.Y * 16, 16, 16), Color.Red * 0.75f);
            sb.Draw(Pixel, new Rectangle(lvl.m_WinPos.X * 16, lvl.m_WinPos.Y * 16, 16, 16), Color.Green * 0.75f);
#endif
        }

        public void DrawFogOfWar(SpriteBatch sb, Level lvl)
        {
            //sb.Draw(Pixel, new Rectangle(0, 0, Game1.graphics.PreferredBackBufferWidth, Game1.graphics.PreferredBackBufferHeight), Color.Black * 0.5f);

            for (int x = 0; x < m_visibleterrain.GetLength(0); x++)
                for (int y = 0; y < m_visibleterrain.GetLength(1); y++)
                    if (m_fogOfWar[x, y] == false)
                    {
                        if (lvl.Map[x, y] == 0)
                            sb.Draw(Pixel, new Rectangle(x * Game1.TILESIZE, y * Game1.TILESIZE, Game1.TILESIZE, Game1.TILESIZE), Color.Black * 0.75f);

                    }

        }



    }
}