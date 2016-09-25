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
using Tower_Of_Babel.UiElements;

namespace _7seconds
{
    class Ui : Pixelclass
    {

        private Vector2 m_position;
        public List<GameButton> m_buttons = new List<GameButton>();
        private int m_buttonsize;


        public Ui(Vector2 pos, int buttonsize)
        {
            m_buttons.Add(new GameButton("W"));
            m_buttons.Add(new GameButton("A"));
            m_buttons.Add(new GameButton("S"));
            m_buttons.Add(new GameButton("D"));

            m_position = pos;
            m_buttonsize = buttonsize;
        }

        public void UpdateMe(TouchInputManager input)
        {
            BoundingSphere sphere = new BoundingSphere(new Vector3(m_position, 0), m_buttonsize * 2.7f);

            for (int dir = 0; dir < 4; dir ++)
            {
                m_buttons[dir].m_isDown = false;
            }
                

            if (input.m_Touches.Count > 0)
            {
                for (int i = 0; i < input.m_Touches.Count; i++)
                {
                    if (sphere.Contains(new Vector3(input.m_Touches[i].Position, 0)) == ContainmentType.Contains)
                    {
                        Vector2 dir = input.m_Touches[i].Position - m_position;

                        float gradient = dir.Y / dir.X;
                        gradient = (float)Math.Atan(gradient) * (180.0f / (float)Math.PI);
                        if (gradient < 45 && gradient > -45 && dir.X > 0)
                            m_buttons[2].m_isDown = true;
                        if (gradient < 45 && gradient > -45 && dir.X < 0)
                            m_buttons[3].m_isDown = true;
                        if (Math.Abs(gradient) >= 45 && dir.Y > 0)
                            m_buttons[0].m_isDown = true;
                        if (Math.Abs(gradient) >= 45 && dir.Y < 0)
                            m_buttons[1].m_isDown = true;

                    }
                }
            }

            //for (int dir = 0; dir < 4; dir++)
            //{
            //    if (new Rectangle((int)m_position.X, (int)m_position.Y + m_buttonsize, m_buttonsize, m_buttonsize).Contains(input.m_Touches[touchid].Position))
            //        m_buttons[0].m_isDown = true;
            //    if (new Rectangle((int)m_position.X, (int)m_position.Y - m_buttonsize, m_buttonsize, m_buttonsize).Contains(input.m_Touches[touchid].Position))
            //        m_buttons[1].m_isDown = true;
            //    if (new Rectangle((int)m_position.X + m_buttonsize, (int)m_position.Y, m_buttonsize, m_buttonsize).Contains(input.m_Touches[touchid].Position))
            //        m_buttons[2].m_isDown = true;
            //    if (new Rectangle((int)m_position.X - m_buttonsize, (int)m_position.Y, m_buttonsize, m_buttonsize).Contains(input.m_Touches[touchid].Position))
            //        m_buttons[3].m_isDown = true;
            //}


        }


        public void DrawMe(SpriteBatch sb)
        {
            //sb.Draw(Pixel, m_position + new Vector2(m_buttonsize / 2, m_buttonsize / 2), null, Color.Gold, (float)Math.PI / 4, new Vector2(0.5f, 0.5f), 80f, SpriteEffects.None, 1f);

            sb.Draw(Pixel, m_position, null, Color.Blue, 0, Vector2.One / 2, 50, SpriteEffects.None, 1);
            //sb.DrawString(Pixelclass.Font, m_grad + "", new Vector2(0, 100), Color.Blue);





        }

    }
}