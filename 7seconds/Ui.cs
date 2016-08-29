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
        private bool[] m_active;
        private int m_buttonsize;


        public Ui(Vector2 pos,int buttonsize)
        {
            m_active = new bool[4];
            m_position = pos - new Vector2(buttonsize/2,buttonsize/2);
            m_buttonsize = buttonsize;
        }

        public void UpdateMe(TouchInputManager input)
        {
            for (int i = 0; i < m_active.Length; i++)
                m_active[i] = false;

            for (int touchid = 0; touchid < input.m_Touches.Count; touchid++)
            {
                for (int dir = 0; dir < 4; dir++)
                {
                    if (new Rectangle((int)m_position.X, (int)m_position.Y + m_buttonsize, m_buttonsize, m_buttonsize).Contains(input.m_Touches[touchid].Position))
                        m_active[0] = true;
                    if (new Rectangle((int)m_position.X, (int)m_position.Y - m_buttonsize, m_buttonsize, m_buttonsize).Contains(input.m_Touches[touchid].Position))
                        m_active[1] = true;
                    if (new Rectangle((int)m_position.X + m_buttonsize, (int)m_position.Y, m_buttonsize, m_buttonsize).Contains(input.m_Touches[touchid].Position))
                        m_active[2] = true;
                    if (new Rectangle((int)m_position.X - m_buttonsize, (int)m_position.Y, m_buttonsize, m_buttonsize).Contains(input.m_Touches[touchid].Position))
                        m_active[3] = true;
                }
                for (int button = 0; button < 3; button++)
                {

                }


            }
        }

        public void DrawMe(SpriteBatch sb)
        {
            sb.Draw(Pixel, m_position + new Vector2(m_buttonsize/2,m_buttonsize/2), null, Color.Gold, (float)Math.PI / 4, new Vector2(0.5f, 0.5f), 80f,SpriteEffects.None, 1f);

            if (m_active[0] == false)
                sb.Draw(Pixel, new Rectangle((int)m_position.X, (int)m_position.Y + m_buttonsize, m_buttonsize, m_buttonsize), Color.Goldenrod);
            else
                sb.Draw(Pixel, new Rectangle((int)m_position.X, (int)m_position.Y + m_buttonsize, m_buttonsize, m_buttonsize), Color.GhostWhite);

            if (m_active[1] == false)
                sb.Draw(Pixel, new Rectangle((int)m_position.X, (int)m_position.Y - m_buttonsize, m_buttonsize, m_buttonsize), Color.Goldenrod);
            else
                sb.Draw(Pixel, new Rectangle((int)m_position.X, (int)m_position.Y - m_buttonsize, m_buttonsize, m_buttonsize), Color.GhostWhite);

            if (m_active[2] == false)
                sb.Draw(Pixel, new Rectangle((int)m_position.X + m_buttonsize, (int)m_position.Y, m_buttonsize, m_buttonsize), Color.Goldenrod);
            else
                sb.Draw(Pixel, new Rectangle((int)m_position.X + m_buttonsize, (int)m_position.Y, m_buttonsize, m_buttonsize), Color.GhostWhite);

            if (m_active[3] == false)
                sb.Draw(Pixel, new Rectangle((int)m_position.X - m_buttonsize, (int)m_position.Y, m_buttonsize, m_buttonsize), Color.Goldenrod);
            else
                sb.Draw(Pixel, new Rectangle((int)m_position.X - m_buttonsize, (int)m_position.Y, m_buttonsize, m_buttonsize), Color.GhostWhite);





        }

    }
}