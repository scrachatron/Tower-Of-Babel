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

namespace Tower_Of_Babel.GameCode.Graphics
{
    enum TileType
    {
        EmptyRegion,
        TLCornerWall,
        TRCornerWall,
        BLCornerWall,
        BrCornerWall,
        LWall,
        RWall,
        TWall,
        BWall,
        Floor
    }


    class Tile
    {
        public static Texture2D m_base;
        public static Point SpriteCount;
        private TileType m_tiletype;
        private Point m_sourcePoint;
        private Rectangle m_sourcerect;
        private static Point internalSpriteSize { get { return new Point(m_base.Width / SpriteCount.X, m_base.Height / SpriteCount.Y); } }


        public Tile(TileType type)
        {
            switch(type)
            {
                case TileType.EmptyRegion:
                    m_sourcePoint = new Point(1, 1);
                    break;
                case TileType.Floor:
                    m_sourcePoint = new Point(6 + Game1.RNG.Next(0, 6), 0 + Game1.RNG.Next(0, 4));
                    break;
                case TileType.LWall:
                    m_sourcePoint = new Point(0,4 + Game1.RNG.Next(0,3));
                    break;
                case TileType.RWall:
                    m_sourcePoint = new Point(1, 4 + Game1.RNG.Next(0, 3));
                    break;
                case TileType.TWall:
                    m_sourcePoint = new Point(0 + Game1.RNG.Next(0, 3), 3);
                    break;
                case TileType.BWall:
                    m_sourcePoint = new Point(2 + Game1.RNG.Next(0, 3), 4);
                    break;
                case TileType.TLCornerWall:
                    m_sourcePoint = new Point(3 , 0);
                    break;
                case TileType.TRCornerWall:
                    m_sourcePoint = new Point(4, 0);
                    break;
                case TileType.BLCornerWall:
                    m_sourcePoint = new Point(3, 1);
                    break;
                case TileType.BrCornerWall:
                    m_sourcePoint = new Point(4, 1);
                    break;


                default:
                    break;
            }

            m_sourcerect = new Rectangle(m_sourcePoint.X * internalSpriteSize.X, m_sourcePoint.Y * internalSpriteSize.Y, internalSpriteSize.X, internalSpriteSize.Y);
        }

        public void Draw(SpriteBatch sb, Rectangle rect)
        {
            sb.Draw(m_base, rect, m_sourcerect,Color.White);
        }



    }
}