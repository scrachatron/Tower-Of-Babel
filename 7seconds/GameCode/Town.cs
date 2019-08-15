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

namespace Tower_Of_Babel
{
    class Town : Level
    {
        
        public Town(int townLvl)
            :base()
        {
            base.m_mazeGen = new TownGenerator(townLvl);
            Map = m_mazeGen.m_stage;
            m_mazeGen.MapInformation.Map = Map;
            base.m_WinPos = new Point(5, 5);
            base.m_StartPos = new Point(3, 5);



        }



    }
}