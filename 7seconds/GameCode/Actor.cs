using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Tower_Of_Babel
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
        protected Vector2 m_timer = new Vector2(0.1f, 0.1f);

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

            if (m_rect.X != m_virtualpos.X * level.LayerSize.X || m_rect.Y != m_virtualpos.Y * level.LayerSize.Y)
                m_position = Vector2.Lerp(m_position, m_targetPos.ToVector2(), 0.3f);
            if (m_rect.X == m_virtualpos.X * level.LayerSize.X)
            {
                m_position.X = m_virtualpos.X * level.LayerSize.X;
            }
            if (m_rect.Y == m_virtualpos.Y * level.LayerSize.Y)
            {
                m_position.Y = m_virtualpos.Y * level.LayerSize.Y;
            }

            //if (new Point(m_rect.X,m_rect.Y) != new Point(m_virtualpos.X * level.LayerSize.X,m_virtualpos.Y * level.LayerSize.Y))
            //    m_position = Vector2.Lerp(m_position, m_targetPos.ToVector2(), 0.3f);
            //else
            //{
            //    m_position = new Vector2(m_virtualpos.X * level.LayerSize.X, m_virtualpos.Y * level.LayerSize.Y);
            //}

            m_rect.X = (int)Math.Round(m_position.X);
            m_rect.Y = (int)Math.Round(m_position.Y);


            

            if (m_timer.X < 0)
            {
                Collision(level);
                m_timer.X = m_timer.Y;
            }
            m_timer.X -= (float)gt.ElapsedGameTime.TotalSeconds;
            
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
            :base(new Rectangle(0,0,Game1.TILESIZE,Game1.TILESIZE),Color.Red, 0)
        {

        }
        public void UpdateMe(GameTime gt, Level level, Ui input, TouchInputManager touchinput)
        {
            if (input.m_buttons[2].m_isDown)
                MoveHere.X += 1;
            if (input.m_buttons[3].m_isDown)
                MoveHere.X += -1;
            if (input.m_buttons[0].m_isDown)
                MoveHere.Y += 1;
            if (input.m_buttons[1].m_isDown)
                MoveHere.Y += -1;

            if (touchinput.m_Touches.Count > 1)
                m_timer.X -= (float)gt.ElapsedGameTime.TotalSeconds;


            base.UpdateMe(gt, level);

            MoveHere.X = 0;
            MoveHere.Y = 0;
        }
        public override void DrawMe(SpriteBatch sb)
        {
            base.DrawMe(sb);
#if DEBUG
            sb.DrawString(Font, VirtualPosition.X + "," + VirtualPosition.Y, Position, Color.White);
#endif
        }
    }
}
