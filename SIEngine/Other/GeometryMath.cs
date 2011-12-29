using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIEngine.BaseGeometry;

namespace SIEngine.Other
{
    public static class GeometryMath
    {
        /// <summary>
        /// Gets the magnitude of a vector.
        /// </summary>
        /// <param name="vec">The vector to get the acceleration of.</param>
        /// <returns></returns>
        public static float GetAccel(Vector vec)
        {
            return (float)Math.Sqrt(Math.Pow(vec.X, 2) + Math.Pow(vec.Y, 2));
        }

        /// <summary>
        /// Returns the product of adding the current vector with the provided.
        /// </summary>
        /// <param name="vec">The vector to add to the current vector.</param>
        /// <returns></returns>
        public static Vector Add(Vector first, Vector second)
        {
            return new Vector(first.X + second.X, first.Y + second.Y, first.Z + second.Z);
        }

        /// <summary>
        /// Gets the angle between the vector and the X axis.
        /// </summary>
        /// <param name="vec">The vector to get the angle of</param>
        /// <returns></returns>
        public static float GetZAngle(Vector vec)
        {
            return (float)Math.Asin(vec.Y
                / Math.Sqrt(Math.Pow(vec.X, 2) + Math.Pow(vec.Y, 2)));
        }
        public static float GetYAngle(Vector vec)
        {
            return (float)Math.Asin(vec.X
                / Math.Sqrt(Math.Pow(vec.Z, 2) + Math.Pow(vec.X, 2)));
        }


        public static double DegreeToRadian(double angle)
        {
            return Math.PI * angle / 180.0;
        }

        public static double RadianToDegree(double angle)
        {
            return angle * (180.0 / Math.PI);
        }
    }
}
