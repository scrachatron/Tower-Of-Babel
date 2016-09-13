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

namespace Tower_Of_Babel.UiElements
{
    class GameButton : Pixelclass 
    {
        private bool m_isDown = false;
        private string m_name;

        public GameButton(string name)
        {
            m_name = name;
        }
        public void UpdateMe()
        {
            m_isDown = false;
        }


    }
}