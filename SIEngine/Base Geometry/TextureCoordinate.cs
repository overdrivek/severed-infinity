using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Graphics.OpenGL;
using SIEngine.Other;

namespace SIEngine.BaseGeometry
{
    public class TextureCoordinate
    {
        private float s;
        public float S
        {
            get { return s; }
            set { s = value;}// ExtensionMethods.Clamp<float>(value, 0.0f, 1.0f); }
        }

        private float t;
        public float T
        {
            get { return t; }
            set { t = value;}// ExtensionMethods.Clamp<float>(value, 0.0f, 1.0f); }
        }
        
        private float r;
        public float R
        {
            get { return r; }
            set { r = ExtensionMethods.Clamp<float>(value, 0.0f, 1.0f); }
        }

        private float q;
        public float Q
        {
            get { return q; }
            set { q = ExtensionMethods.Clamp<float>(value, 0.0f, 1.0f); }
        }

        public TextureCoordinate (float s, float t, float r = 0.0f, float q = 0.0f)
        {
            S = s;
            T = t;
            R = r;
            Q = q;
        }

        public void ApplyTextureCoordinates ()
        {
            GL.TexCoord3(S, T, R);//, Q);
        }
    }
}
