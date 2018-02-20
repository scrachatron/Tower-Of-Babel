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
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Tower_Of_Babel
{
    class Town : Level
    {
        


        public Town(int townLvl)
            :base()
        {
            base.m_mazeGen = new TownGenerator(Android.OS.Build.Serial.GetHashCode() + townLvl);
            base.RegenTown();
            Map = m_mazeGen.m_stage;
            m_mazeGen.MapInformation.Map = Map;
            base.m_WinPos = new Point(5, 5);
            base.m_StartPos = new Point(3, 5);
        }
        public override void DrawMe(SpriteBatch sb, minimap minimap, Point playerpos)
        {
            sb.Draw(Pixel, new Rectangle(0, 0, LayerSize.X * minimap.ActiveArea.GetLength(0), LayerSize.X * minimap.ActiveArea.GetLength(1)), Color.Black);

            for (int x = 0; x < Map.GetLength(0); x++)
            {
                for (int y = 0; y < Map.GetLength(1); y++)
                {
                    if (Map[x, y] == 0)
                        sb.Draw(Pixel, new Rectangle(x * LayerSize.X, y * LayerSize.Y, LayerSize.X, LayerSize.Y), Color.White);
                }
            }

            
                sb.Draw(Pixel, new Rectangle(m_StartPos.X * LayerSize.X, m_StartPos.Y * LayerSize.Y, LayerSize.X, LayerSize.Y), Color.Red);
                sb.Draw(Pixel, new Rectangle(m_WinPos.X * LayerSize.X, m_WinPos.Y * LayerSize.Y, LayerSize.X, LayerSize.Y), Color.Green);
        


            //base.DrawMe(sb, minimap, playerpos);
        }



    }
}