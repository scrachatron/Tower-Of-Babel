using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Tower_Of_Babel
{
    class Camera
    {
        private Matrix m_transform;
        public Matrix Transform
        {
            get { return m_transform; }
        }

        private Vector2 m_centre;
        private Viewport m_viewport;

        public Camera(Viewport newVp)
        {
            m_viewport = newVp;
        }

        public void UpdateMe(Vector2 Pos, Point Offset)
        {
            if (Pos.X < m_viewport.Width / 2)
                m_centre.X = m_viewport.Width / 2;
            else if (Pos.X > Offset.X - (m_viewport.Width / 2))
                m_centre.X = Offset.X - (m_viewport.Width / 2);
            else m_centre.X = Pos.X;

            if (Pos.Y < m_viewport.Height / 2)
                m_centre.Y = m_viewport.Height / 2;
            else if (Pos.Y > Offset.Y - (m_viewport.Height / 2))
                m_centre.Y = Offset.Y - (m_viewport.Height / 2);
            else m_centre.Y = Pos.Y;


            m_transform = Matrix.CreateTranslation(new Vector3(-m_centre.X + (m_viewport.Width / 2),
                                                               -m_centre.Y + (m_viewport.Height / 2),
                                                               0));
                                
        }

    }
}
