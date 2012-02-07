using System;
using System.Collections.Generic;
using System.Drawing;
using OpenTK.Graphics.OpenGL;

namespace SIEngine
{
    namespace BaseGeometry
    {
        /// <summary>
        /// A two dimensional vector.
        /// </summary>
        public class DecimalVector
        {
            /// <summary>
            /// The X value,
            /// </summary>
            public decimal X { get; set; }
            /// <summary>
            /// The Y value;
            /// </summary>
            public decimal Y { get; set; }
            /// <summary>
            /// The Z value.
            /// </summary>
            public decimal Z { get; set; }
            /// <summary>
            /// The W value.
            /// </summary>
            public decimal W { get; set; }

            /// <summary>
            /// Default constructor. Sets the X and Y value correspondingly.
            /// </summary>
            /// <param name="x">X value.</param>
            /// <param name="y">Y value</param>
            public DecimalVector(decimal x, decimal y, decimal Z = 1.0m, decimal W = 1.0m)
            {
                this.X = x;
                this.Y = y;
                this.Z = Z;
                this.W = W;
            }

            /// <summary>
            /// Draws the vector using OpenGL.
            /// </summary>
            public void Draw()
            {
                GL.Vertex4((float)X, (float)Y, (float)Z, (float)W);
            }

            public void TranslateTo()
            {
                GL.Translate((float)X, (float)Y, (float)Z);
            }

            #region operator overloading
            
            public static explicit operator Vector(DecimalVector vec)
            {
                return new Vector((float)vec.X, (float)vec.Y, (float)vec.Z, (float)vec.W);
            }
            public static explicit operator OpenTK.Vector3(DecimalVector vec)
            {
                return new OpenTK.Vector3((float)vec.X, (float)vec.Y, (float)vec.Z);
            }

            public static explicit operator Normal(DecimalVector vec)
            {
                return new Normal((float)vec.X, (float)vec.Y, (float)vec.Z);
            }

            public static DecimalVector operator *(DecimalVector vec, decimal coef)
            {
                return new DecimalVector(vec.X * coef, vec.Y * coef, vec.Z * coef, vec.W * coef);
            }
            public static DecimalVector operator *(decimal coef, DecimalVector vec)
            {
                return new DecimalVector(vec.X * coef, vec.Y * coef, vec.Z * coef, vec.W * coef);
            }
            public static DecimalVector operator +(DecimalVector first, DecimalVector second)
            {
                return new DecimalVector(first.X + second.X, first.Y + second.Y, first.Z + second.Z, first.W + second.W);
            }
            public static DecimalVector operator -(DecimalVector first, DecimalVector second)
            {
                return new DecimalVector(first.X - second.X, first.Y - second.Y, first.Z - second.Z, first.W - second.W);
            }
            #endregion
        }
    }
}