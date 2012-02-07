using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using SIEngine.BaseGeometry;
using SIEngine.Other;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using SIEngine.Graphics.Rendering;
using Object = SIEngine.GUI.Object;

namespace SIEngine.Graphics
{
    /// <summary>
    /// A basic class to draw a laser. It fires two beams
    /// that are at length/2 distance from Location.
    /// </summary>
    public class Laser : Object
    {
        public Vector Destination { get; set; }
        public Color MainColor { get; set; }
        /// <summary>
        /// THe length between the two beams.
        /// </summary>
        public float Length { get; set; }
        private List<Vertex> vertices;

        public Laser(Vector location, Vector destination)
        {
            Location = location;
            Destination = destination;
            MainColor = Color.Red;
        }

        public override void Draw()
        {
            if (!Visible)
                return;

            GL.MatrixMode(MatrixMode.Modelview);
            GL.PushMatrix();
            {
                GeneralGraphics.EnableAlphaBlending();
                GL.Color4(MainColor.R, MainColor.G, MainColor.B, (byte)128);
                GL.Begin(BeginMode.Lines);
                {
                    GL.Vertex3(Location.X - Length / 2, Location.Y, Location.Z);
                    Destination.Draw();

                    GL.Vertex3(Location.X + Length / 2, Location.Y, Location.Z);
                    Destination.Draw();
                }
                GL.End();
            }
            GL.PopMatrix();
        }
    }
}
