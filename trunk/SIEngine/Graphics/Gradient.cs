using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using OpenTK.Graphics.OpenGL;
using SIEngine.Other;

namespace SIEngine.Graphics
{
    public class Gradient
    {
        private List<Color> Colors { get; set; }
        public int ColorCount
        {
            get
            {
                return Colors.Count;
            }
        }

        public Gradient(params Color[] colors)
        {
            Colors = new List<Color>();
            foreach (var color in colors)
                Colors.Add(color);
        }

        /// <summary>
        /// Gets the color for a position.
        /// </summary>
        /// <param name="coef">The position (from 0 to 1 in floating point).</param>
        /// <returns></returns>
        public Color GetColor(float coef)
        {
            int interval = (int)Math.Ceiling(coef * (ColorCount - 2));
            float intCoef = coef;
            if(interval != 0)
                intCoef = coef - 1 / ColorCount * interval;

            return GeneralMath.Interpolate(Colors[interval], Colors[interval + 1], intCoef);
        }
    }
}
