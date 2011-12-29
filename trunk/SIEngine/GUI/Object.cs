using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIEngine.BaseGeometry;
using SIEngine.Graphics;
using SIEngine.Graphics.Shaders;
using OpenTK.Graphics.OpenGL;

namespace SIEngine.GUI
{
    public class Object : GUIObject
    {
        public OBJModel Body { get; set; }
        public ShaderProgram ShaderProgram { get; set; }
        
        public override void Draw ()
        {
            if (!Visible)
                return;
            if (ShaderProgram != null)
                ShaderProgram.UseProgram();

            GL.MatrixMode(MatrixMode.Modelview);
            GL.PushMatrix();
            {
                if (Location != null) Location.TranslateTo();
                Body.Draw();
            }
            GL.PopMatrix();

            GeneralGraphics.UseDefaultShaderProgram();
        }
    }
}
