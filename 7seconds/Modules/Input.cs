using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;

namespace Tower_Of_Babel
{
    class InputManager
    {
        public GamePadState ThisPad { get { return pad_Curr; } }
        public KeyboardState ThisKeyboard { get { return key_Curr; } }
        public MouseState ThisMouse { get { return mouse_Curr; } }

        private GamePadState pad_Old, pad_Curr;
        private KeyboardState key_Old, key_Curr;
        private MouseState mouse_Old, mouse_Curr;
        private PlayerIndex index;
        private bool isGamePad = false;
        private float timer = 0;



        public GamePadState CurrPadState
        {
            get { return pad_Curr; }
        }
        public KeyboardState CurrKeyboardState
        {
            get { return key_Curr; }
        }

        public InputManager(PlayerIndex p)
        {
            index = p;
            isGamePad = true;
        }
        public InputManager()
        {

        }

        public void UpdateMe()
        {
            if (isGamePad)
            {
                pad_Old = pad_Curr;
                pad_Curr = GamePad.GetState(index);
            }
            else
            {
                key_Old = key_Curr;
                key_Curr = Keyboard.GetState();

            }

            mouse_Old = mouse_Curr;
            mouse_Curr = Mouse.GetState();

        }

        public bool WasLeftClickedFront()
        {
            return (mouse_Old.LeftButton == ButtonState.Released && mouse_Curr.LeftButton == ButtonState.Pressed);
        }
        public bool WasLeftClickedBack()
        {
            return (mouse_Curr.LeftButton == ButtonState.Released && mouse_Old.LeftButton == ButtonState.Pressed);
        }

        public bool WasRightClickedFront()
        {
            return (mouse_Old.RightButton == ButtonState.Released && mouse_Curr.RightButton == ButtonState.Pressed);
        }
        public bool WasRightClickedBack()
        {
            return (mouse_Curr.RightButton == ButtonState.Released && mouse_Old.RightButton == ButtonState.Pressed);
        }

        public bool WasMiddleClickedFront()
        {
            return (mouse_Old.MiddleButton == ButtonState.Released && mouse_Curr.MiddleButton == ButtonState.Pressed);
        }
        public bool WasMiddleClickedBack()
        {
            return (mouse_Curr.MiddleButton == ButtonState.Released && mouse_Old.MiddleButton == ButtonState.Pressed);
        }

        public Point MouseIsHere()
        {
            return mouse_Curr.Position;
        }
        public bool ClickedHere(Rectangle r)
        {
            return (WasLeftClickedFront() && r.Contains(MouseIsHere()));
        }

        public bool IsDown(Buttons b)
        {
            return pad_Curr.IsButtonDown(b);
        }
        public bool IsDown(Keys k)
        {
            return key_Curr.IsKeyDown(k);
        }

        public bool WasDown(Buttons b)
        {
            return pad_Old.IsButtonDown(b);
        }
        public bool WasDown(Keys k)
        {
            return key_Old.IsKeyDown(k);
        }

        public bool IsUp(Buttons b)
        {
            return pad_Curr.IsButtonUp(b);
        }
        public bool IsUp(Keys k)
        {
            return key_Curr.IsKeyUp(k);
        }

        public bool WasUp(Buttons b)
        {
            return pad_Old.IsButtonUp(b);
        }
        public bool WasUp(Keys k)
        {
            return key_Old.IsKeyUp(k);
        }

        public bool WasPressedFront(Buttons b)
        {
            return (pad_Curr.IsButtonDown(b) && pad_Old.IsButtonUp(b));
        }
        public bool WasPressedBack(Buttons b)
        {
            return (pad_Old.IsButtonDown(b) && pad_Curr.IsButtonUp(b));
        }

        public bool WasPressedFront(Keys k)
        {
            return (key_Curr.IsKeyDown(k) && key_Old.IsKeyUp(k));
        }
        public bool WasPressedBack(Keys k)
        {
            return (key_Old.IsKeyDown(k) && key_Curr.IsKeyUp(k));
        }

        public bool IsHeldFor(Buttons b, float t, GameTime gt)
        {
            if (IsDown(b) && timer >= t)
            {
                timer = 0;
                return true;
            }
            else
            {
                timer += (float)gt.ElapsedGameTime.TotalSeconds;
                return false;
            }
        }
        public bool IsHeldFor(Keys k, float t, GameTime gt)
        {
            if (IsDown(k) && timer >= t)
            {
                timer = 0;
                return true;
            }
            else
            {
                timer += (float)gt.ElapsedGameTime.TotalSeconds;
                return false;
            }
        }
        public bool HeldFor(Keys k, float t, GameTime gt)
        {
            if (WasPressedFront(k) || WasPressedBack(k))
                timer = 0;

            if (IsDown(k) && timer >= t)
            {
                return true;
            }
            else if (IsDown(k))
            {
                timer += (float)gt.ElapsedGameTime.TotalSeconds;
                return false;
            }
            return false;
        }


    }

    class TouchInputManager
    {
        public TouchCollection m_Touches;
        public TouchPanelCapabilities m_capability = TouchPanel.GetCapabilities();
        

        

        public void UpdateMe()
        {
            m_Touches = TouchPanel.GetState();
        }

        public bool WasTouchedFront()
        {
            for (int i = 0; i < m_Touches.Count; i++)
                if (m_Touches[i].State == TouchLocationState.Pressed)
                    return true;
            return false;
        }
        public bool WasTouchedBack()
        {
            for (int i = 0; i < m_Touches.Count; i++)
                if (m_Touches[i].State == TouchLocationState.Released)
                    return true;
            return false;
        }

        public bool ButtonTouchedFront(Rectangle rect)
        {
            for (int i = 0; i < m_Touches.Count; i++)
            if (rect.Contains(m_Touches[i].Position)&& m_Touches[i].State == TouchLocationState.Pressed)
                return true;
            return false;
        }
        public bool ButtonTouchedBack(Rectangle rect)
        {
            for (int i = 0; i < m_Touches.Count; i++)
                if (rect.Contains(m_Touches[i].Position) && m_Touches[i].State == TouchLocationState.Released)
                    return true;
            return false;
        }
    }
}