using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIEngine.BaseGeometry;
using SIEngine.Graphics;
using SIEngine.Graphics.Shaders;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using SIEngine.Other;

namespace SIEngine.GUI
{
    public class Object : GUIObject
    {
        public OBJModel Body { get; set; }
        public ShaderProgram ShaderProgram { get; set; }
        public Color pickColor = GeneralMath.GetPickingColor();
        
        public void PickDraw()
        {
            GL.MatrixMode(MatrixMode.Modelview);
            GL.PushMatrix();
            {
                if (Location != null) Location.TranslateTo();
                Body.PickDraw();
            }
            GL.PopMatrix();
        }

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
