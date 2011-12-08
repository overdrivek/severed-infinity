using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Graphics.OpenGL;

namespace SIEngine.BaseGeometry
{
    public class Vertex
    {
        public Vector Location { get; set; }
        public Normal Normal { get; set; }
        public TextureCoordinate TexCoord { get; set; }

        public Vertex(Vector location, Normal normal = null, TextureCoordinate texCoord = null)
        {
            Location = location;
            Normal = normal;
            TexCoord = texCoord;
        }
        public Vertex (float x, float y, float z = 0.0f, float w = 1.0f)
        {
            Location = new Vector(x, y, z, w);
            Normal = null;
            TexCoord = null;
        }

        public void Draw()
        {
            if (Normal != null) Normal.ApplyNormal();
            if (TexCoord != null) TexCoord.ApplyTextureCoordinates();
            if (Location != null) Location.Draw();
        }
    }
}
