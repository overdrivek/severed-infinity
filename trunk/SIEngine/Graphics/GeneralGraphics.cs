using System;
using System.Collections.Generic;
using OpenTK;
using System.Linq;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using SIEngine.Other;
using SIEngine.BaseGeometry;

namespace SIEngine.Graphics
{
    public static class GeneralGraphics
    {
        public static void UseDefaultShaderProgram()
        {
            GL.UseProgram(0);
        }

        public static void EnableAlphaBlending()
        {
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
        }

        public static void BlendWhite()
        {
            GL.Enable(EnableCap.Blend);
            GL.BlendColor(1, 1, 1, 1);
            GL.BlendFunc(BlendingFactorSrc.DstColor, BlendingFactorDest.Zero);
        }

        public static void DisableBlending()
        {
            GL.Disable(EnableCap.Blend);
        }

        public static void EnableTexturing()
        {
            GL.Enable(EnableCap.Texture2D);
        }
        public static void DisableTexturing()
        {
            GL.Disable(EnableCap.Texture2D);
        }

        public static void DrawRectangle(BaseGeometry.Vector Position, BaseGeometry.Vector Size)
        {
            GL.Begin(BeginMode.Quads);
            {
                GL.TexCoord2(0, 0);
                GL.Vertex2(Position.X, Position.Y);

                GL.TexCoord2(0, 1);
                GL.Vertex2(Position.X, Position.Y + Size.Y);

                GL.TexCoord2(1, 1);
                GL.Vertex2(Position.X + Size.X, Position.Y + Size.Y);

                GL.TexCoord2(1, 0);
                GL.Vertex2(Position.X + Size.X, Position.Y);
            }
            GL.End();
        }
    }
}
