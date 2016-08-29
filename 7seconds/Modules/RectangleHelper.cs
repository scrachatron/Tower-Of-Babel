using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace _7seconds
{
    static class RectangleHelper
    {
        public static bool TouchTopOf(this Rectangle r1, Rectangle r2)
        {
            return (r1.Bottom >= r2.Top - 1 &&
                        r1.Bottom <= r2.Top + (r2.Height / 2) &&
                        r1.Right >= r2.Left + (r2.Width / 6) &&
                        r1.Left <= r2.Right - (r2.Width / 6));
        }
        public static bool TouchBottomOf(this Rectangle r1, Rectangle r2)
        {
            return (r1.Top <= r2.Bottom - 1 &&
                        r1.Top >= r2.Bottom - (r2.Height / 2) &&
                        r1.Right >= r2.Left + (r2.Width / 6) &&
                        r1.Left <= r2.Right - (r2.Width / 6));
        }
        public static bool TouchLeftOf(this Rectangle r1, Rectangle r2)
        {
            return (r1.Right <= r2.Right &&
                        r1.Right >= r2.Left + 1 &&
                        r1.Top <= r2.Bottom - (r2.Width / 4) &&
                        r1.Bottom >= r2.Top + (r2.Width / 4));
        }
        public static bool TouchRightOf(this Rectangle r1, Rectangle r2)
        {
            return (r1.Left >= r2.Left &&
                        r1.Left <= r2.Right - 1 &&
                        r1.Top <= r2.Bottom - (r2.Width / 4) &&
                        r1.Bottom >= r2.Top + (r2.Width / 4));
        }

        public static Point ReturnRandom(this Rectangle r1)
        {
            return new Point(r1.X + 1 + Game1.RNG.Next(0, r1.Width - 2), r1.Y + 1 + Game1.RNG.Next(0, r1.Height - 2));
        }
        public static Point ReturnRandom(this Rectangle r1, int margin)
        {
            return new Point(r1.X + margin + Game1.RNG.Next(0, r1.Width - (margin + 1)), r1.Y + 1 + Game1.RNG.Next(0, r1.Height - (margin + 1)));
        }
    }
}
