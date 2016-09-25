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

namespace Tower_Of_Babel.UiElements
{
    class GameButton : Pixelclass 
    {
        public bool m_isDown = false;
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