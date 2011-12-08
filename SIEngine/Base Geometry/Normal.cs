using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Graphics.OpenGL;
using SIEngine.Other;

namespace SIEngine.BaseGeometry
{
    public class Normal
    {
        #region XYZW coordinates
        private float x;
        public float X
        {
            get { return x; }
            set { x = ExtensionMethods.Clamp<float>(value, -1.0f, 1.0f); }
        }
        
        private float y;
        public float Y
        {
            get { return y; }
            set { y = ExtensionMethods.Clamp<float>(value, -1.0f, 1.0f); }
        }
        
        private float z;
        public float Z
        {
            get { return z; }
            set { z = ExtensionMethods.Clamp<float>(value, -1.0f, 1.0f); }
        }
        
        #endregion

        public Normal (float x, float y, float Z = 0.0f)
        {
            this.X = x;
            this.Y = y;
            this.Z = Z;
        }

        public static explicit operator Vector(Normal normal)
        {
            return new Vector(normal.X, normal.Y, normal.Z);
        }

        /// <summary>
        /// Applies the normal using OpenGL.
        /// </summary>
        public void ApplyNormal ()
        {
            GL.Normal3(X, Y, Z);
        }
    }
}
