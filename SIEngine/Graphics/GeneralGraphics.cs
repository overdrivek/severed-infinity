using System;
using System.Collections.Generic;
using OpenTK;
using System.Linq;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using SIEngine.Other;
using SIEngine.BaseGeometry;
using SIEngine.Graphics.Shaders;

namespace SIEngine.Graphics
{
    public static class GeneralGraphics
    {
        public static float ShadeLight = 128.0f;
        public static float Angle = 0.0f;
        public static float AmbientLight = 32;
        public static ShaderProgram SimulatedLighting { get; set; }
        public static Texture ExclamationTexture { get; set; }
        public static Texture ExclamationMask { get; set; }
        public static Texture InfoBoxFrame { get; set; }
        
        static GeneralGraphics()
        {
            SimulatedLighting = new ShaderProgram("data/effects/SimulatedLighting.vert",
                "data/effects/SimulatedLighting.frag");
            ExclamationTexture = new Texture("data/img/icons/exclamation.png");
            ExclamationMask = new Texture("data/img/icons/mask.png");
            InfoBoxFrame = new Texture("data/img/icons/frame.png");
        }

        public static void UseSimulatedLighting()
        {
            SimulatedLighting.UseProgram();
        }

        public static void UseDefaultShaderProgram()
        {
            GL.UseProgram(0);
        }

        public static int RenderMode()
        {
            return GL.RenderMode(RenderingMode.Render);
        }
        public static void PickingMode()
        {
            GL.RenderMode(RenderingMode.Select);
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

        public static void DrawFilled()
        {
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
        }
        public static void DrawWireframe()
        {
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
        }

        public static void DrawRectangle(BaseGeometry.Vector Position, BaseGeometry.Vector Size)
        {
            GL.Begin(BeginMode.Quads);
            {
                GL.TexCoord2(0, 0);
                GL.Vertex3(Position.X, Position.Y, Position.Z);

                GL.TexCoord2(0, 1);
                GL.Vertex3(Position.X, Position.Y + Size.Y, Position.Z);

                GL.TexCoord2(1, 1);
                GL.Vertex3(Position.X + Size.X, Position.Y + Size.Y, Position.Z);

                GL.TexCoord2(1, 0);
                GL.Vertex3(Position.X + Size.X, Position.Y, Position.Z);
            }
            GL.End();
        }
    }
}
