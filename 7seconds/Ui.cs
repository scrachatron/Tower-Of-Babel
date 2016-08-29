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

namespace _7seconds
{
    class Ui : Pixelclass
    {

        private Vector2 m_position;

        public Ui(Vector2 pos)
        {
            m_position = pos;
        }

        public void DrawMe(SpriteBatch sb)
        {
            sb.Draw(Pixel, m_position, null, Color.Gold, (float)Math.PI / 4, new Vector2(0.5f, 0.5f), 80f,SpriteEffects.None, 1f);

            sb.Draw(Pixel, new Vector2(m_position.X, m_position.Y + 100), null, Color.Gold, (float)Math.PI / 4, new Vector2(0.5f, 0.5f), 80f, SpriteEffects.None, 1f);
            sb.Draw(Pixel, new Vector2(m_position.X, m_position.Y - 100), null, Color.Gold, (float)Math.PI / 4, new Vector2(0.5f, 0.5f), 80f, SpriteEffects.None, 1f);
            sb.Draw(Pixel, new Vector2(m_position.X + 100, m_position.Y), null, Color.Gold, (float)Math.PI / 4, new Vector2(0.5f, 0.5f), 80f, SpriteEffects.None, 1f);
            sb.Draw(Pixel, new Vector2(m_position.X - 100, m_position.Y), null, Color.Gold, (float)Math.PI / 4, new Vector2(0.5f, 0.5f), 80f, SpriteEffects.None, 1f);
        }

    }
}