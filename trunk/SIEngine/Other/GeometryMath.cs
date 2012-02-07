using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIEngine.BaseGeometry;
using OpenTK.Graphics;
using OpenTK;
using System.Drawing;
using SIEngine.Graphics;

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

        public static Vector UnProjectMouse(Vector mouse)
        {
            double[] matrix = new double[16];
            GL.GetDouble(GetPName.ModelviewMatrix, matrix);
            double[] proj = new double[16];
            GL.GetDouble(GetPName.ProjectionMatrix, proj);
            int[] view = new int[4];
            GL.GetInteger(GetPName.Viewport, view);

            Matrix4 modelview = new Matrix4(
                (float)matrix[0], (float)matrix[1], (float)matrix[2], (float)matrix[3],
                (float)matrix[4], (float)matrix[5], (float)matrix[6], (float)matrix[7],
                (float)matrix[8], (float)matrix[9], (float)matrix[10], (float)matrix[11],
                (float)matrix[12], (float)matrix[13], (float)matrix[14], (float)matrix[15]);

            Matrix4 projection = new Matrix4(
                (float)proj[0], (float)proj[1], (float)proj[2], (float)proj[3],
                (float)proj[4], (float)proj[5], (float)proj[6], (float)proj[7],
                (float)proj[8], (float)proj[9], (float)proj[10], (float)proj[11],
                (float)proj[12], (float)proj[13], (float)proj[14], (float)proj[15]);

            var pos = new Vector4(2.0f * mouse.X / (float)view[2] - 1,
                -(2.0f * mouse.Y / (float)view[3] - 1), 0, 1);

            projection = Matrix4.Invert(projection);
            modelview = Matrix4.Invert(modelview);
            pos = Vector4.Transform(pos, projection);
            pos = Vector4.Transform(pos, modelview);
            Vector position = new Vector(pos.X * 50, pos.Y * 50);

            return position;
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

        public static Vector CrossProduct(Vector first, Vector second)
        {
            return new Vector(first.Y * second.Z - first.Z * second.Y,
                first.Z * second.X - first.X * second.Z,
                first.X * second.Y - first.Y * second.X);
        }

        public static float GetTriangleFace(Vector first, Vector second, Vector third)
        {
            return (third.X - first.X) * (second.X - first.X) -
                (third.Y - first.Y) * (second.Y - first.Y);
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
