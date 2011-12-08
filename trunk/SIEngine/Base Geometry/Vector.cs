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
        public class Vector
        {
            /// <summary>
            /// The X value,
            /// </summary>
            public float X { get; set; }
            /// <summary>
            /// The Y value;
            /// </summary>
            public float Y { get; set; }
            /// <summary>
            /// The Z value.
            /// </summary>
            public float Z { get; set; }
            /// <summary>
            /// The W value.
            /// </summary>
            public float W { get; set; }

            /// <summary>
            /// Default constructor. Sets the X and Y value correspondingly.
            /// </summary>
            /// <param name="x">X value.</param>
            /// <param name="y">Y value</param>
            public Vector (float x, float y, float Z = 1.0f, float W = 1.0f)
            {
                this.X = x;
                this.Y = y;
                this.Z = Z;
                this.W = W;
            }

            /// <summary>
            /// Draws the vector using OpenGL.
            /// </summary>
            public void Draw ()
            {
                GL.Vertex4(this.X, this.Y, this.Z, this.W);
            }

            public void TranslateTo ()
            {
                GL.Translate(X, Y, Z);
            }

            public static explicit operator OpenTK.Vector3(Vector vec)
            {
                return new OpenTK.Vector3(vec.X, vec.Y, vec.Z);
            }

            public static explicit operator Normal(Vector vec)
            {
                return new Normal(vec.X, vec.Y, vec.Z);
            }
        }
    }
}