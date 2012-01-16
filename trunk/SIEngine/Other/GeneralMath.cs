using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Text;
using SIEngine.BaseGeometry;
using SIEngine.Logging;
using Timer = System.Windows.Forms.Timer;

namespace SIEngine.Other
{
    public static class GeneralMath
    {
        private static Random Generator = new Random();
        private static Color currentColor = Color.FromArgb(1, 0, 0, 0);

        public static Color GetPickingColor()
        {
            byte a = currentColor.A;
            byte r = currentColor.R;
            byte g = currentColor.G;
            byte b = currentColor.B;

            if (a == 255)
                if (r == 255)
                    if (g == 255)
                        if (b < 255)
                            b++;
                        else LogManager.WriteError("Too many objects created for color picking to handle!");
                    else g++;
                else r++;
            else a++;
            currentColor = Color.FromArgb(a, r, g, b);

            return currentColor;
        }

        public static int RandomInt()
        {
            return Generator.Next();
        }
        
        public static float RandomFloat(float min, float max)
        {
            return min + (float)Generator.NextDouble() * (max - min);
        }

        public static float Interpolate(float beginning, float end, float coef)
        {
            return coef * end + (1.0f - coef) * beginning;
        }

        public static int Interpolate(int beginning, int end, float coef)
        {
            return (int)(coef * (float)end + (1.0f - coef) * (float)beginning);
        }

        public static Vector Interpolate(this Vector beginning, Vector end, float coef)
        {
            return coef * end + (1.0f - coef) * beginning;
        }

        public static Color Interpolate(Color beginning, Color end, float coef)
        {
            float rest = 1 - coef;
            return Color.FromArgb(
                (byte)(end.A * coef + beginning.A * rest),
                (byte)(end.R * coef + beginning.R * rest),
                (byte)(end.G * coef + beginning.G * rest),
                (byte)(end.B * coef + beginning.B * rest));
        }
    }
}
