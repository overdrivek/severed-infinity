using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIEngine.BaseGeometry;

namespace SIEngine.Other
{
    public static class GeneralMath
    {
        private static Random Generator = new Random();

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
    }
}
