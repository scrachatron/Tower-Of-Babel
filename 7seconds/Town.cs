﻿using System;
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

namespace Tower_Of_Babel
{
    class Town : Level
    {
        


        public Town(int townLvl)
            :base()
        {
            base.m_mazeGen = new TownGenerator(townLvl);

            



        }



    }
}