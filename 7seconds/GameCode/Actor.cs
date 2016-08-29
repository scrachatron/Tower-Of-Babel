using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace _7seconds
{
    class Actor : Pixelclass
    {
        private Rectangle Rect
        {
            get { return m_rect; }
            set
            {
                m_rect = value;
                m_position.X = value.X;
                m_position.Y = value.Y;
            }

        }
        public Point VirtualPosition
        {
            get { return m_virtualpos; }
            set
            {
                m_virtualpos = value;
                m_position.X = value.X * m_rect.Width;
                m_position.Y = value.Y * m_rect.Height;
                m_targetPos = m_position.ToPoint();
                m_velocity = Vector2.Zero;
            }
        }
        public Vector2 Position
        {
            get { return m_position; }
            set { m_position = value;
                m_targetPos = value.ToPoint();
                m_velocity = Vector2.Zero;
            }
        }

        private Color m_tint;
        private Rectangle m_rect;

        private Vector2 m_position;
        private Vector2 m_velocity;
        private Point m_targetPos;

        private Point m_virtualpos;

        protected Point MoveHere = new Point(0, 0);

        public Actor(Rectangle rect, Color tint, int layer)
        {
            Rect = rect;
            m_position = new Vector2(rect.X, rect.Y);
            m_targetPos = m_position.ToPoint();
            m_tint = tint;
        }
        public virtual void UpdateMe(GameTime gt, Level level)
        {
            m_rect.Width = level.LayerSize.X;
            m_rect.Height = level.LayerSize.Y;
            m_rect.X = (int)Math.Round(m_position.X);
            m_rect.Y = (int)Math.Round(m_position.Y);


            m_position = Vector2.Lerp(m_position, m_targetPos.ToVector2(), 0.3f);

            Collision(level);
        }
        public virtual void DrawMe(SpriteBatch sb)
        {
            sb.Draw(Pixel, m_rect , m_tint);
        }
        private void Collision(Level lvl)
        {
            if (MoveHere.X != 0 && MoveHere.Y != 0)
            {
                if (lvl.Map[VirtualPosition.X + MoveHere.X,VirtualPosition.Y] == 0
                    && lvl.Map[VirtualPosition.X, VirtualPosition.Y + MoveHere.Y] == 0)
                    if (lvl.Map[VirtualPosition.X + MoveHere.X, VirtualPosition.Y + MoveHere.Y] == 0)
                    {
                        m_virtualpos += MoveHere;
                        m_targetPos = (m_virtualpos * lvl.LayerSize);
                        
                        return;
                    }
            }

            if (lvl.Map[VirtualPosition.X + MoveHere.X, VirtualPosition.Y + MoveHere.Y] == 0)
            {
                m_virtualpos += MoveHere;
                m_targetPos = (m_virtualpos * lvl.LayerSize);

                return;
            }
        }
    }

    class Player : Actor
    {

        public Player()
            :base(new Rectangle(0,0,32,32),Color.Red, 0)
        {

        }
        public void UpdateMe(GameTime gt, Level level, InputManager input)
        {
            if (input.WasPressedFront(Keys.Left) || input.HeldFor(Keys.Left, 0.7f, gt))
                MoveHere.X += -1;
            if (input.WasPressedFront(Keys.Right) || input.HeldFor(Keys.Right, 0.7f, gt))
                MoveHere.X += 1;
            if (input.WasPressedFront(Keys.Up) || input.HeldFor(Keys.Up, 0.7f, gt))
                MoveHere.Y += -1;
            if (input.WasPressedFront(Keys.Down) || input.HeldFor(Keys.Down, 0.7f, gt))
                MoveHere.Y += 1;

            base.UpdateMe(gt, level);

            MoveHere.X = 0;
            MoveHere.Y = 0;
        }
        public override void DrawMe(SpriteBatch sb)
        {
            base.DrawMe(sb);
            sb.DrawString(Font, VirtualPosition.X + "," + VirtualPosition.Y, Position, Color.White);
        }
    }
}
