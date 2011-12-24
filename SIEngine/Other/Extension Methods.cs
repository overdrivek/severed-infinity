using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using SIEngine.BaseGeometry;

namespace SIEngine
{
    namespace Other
    {
        public static class ExtensionMethods
        {
            public static void Assign (this Color baseColor, Color target)
            {
                target = Color.FromArgb(baseColor.A, baseColor);
            }

            public static T Clamp<T>(this T val, T min, T max) where T : IComparable<T>
            {
                if (val.CompareTo(min) < 0) return min;
                else if (val.CompareTo(max) > 0) return max;
                else return val;
            }

        }
    }
}