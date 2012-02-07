using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIEngine.BaseGeometry;
using SIEngine.Graphics;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing;

namespace SIEngine
{
    namespace GUI
    {
        public class Polygon : GUIObject
        {

            public List<Vertex> Vertices { get; set; }
            /// <summary>
            /// The background image.
            /// </summary>
            private SIEngine.Graphics.Texture TextureImage { get; set; }
            public string Image
            {
                set
                {
                    this.TextureImage = new Graphics.Texture(value, TextureMinFilter.Linear,
                        TextureMagFilter.Linear);
                }
            }
            /// <summary>
            /// The background color.
            /// </summary>
            public Color BackgroundColor { get; set; }
            public PolygonMode DrawMode { get; set; }
            public BeginMode BeginMode { get; set; }
            public Vector ScaleFactor { get; set; }

            public Polygon()
            {
                Vertices = new List<Vertex>();
                BackgroundColor = Color.Black;
                DrawMode = PolygonMode.Line;
                BeginMode = BeginMode.Polygon;
                Location = new Vector(0, 0);
                Size = new Vector(0, 0);
                ScaleFactor = new Vector(1, 1, 1);
            }

            public void PureDraw()
            {
                GL.Begin(BeginMode);
                {
                    foreach (Vertex vertex in Vertices)
                        vertex.Draw();
                }
                GL.End();
            }

            public override void Draw()
            {
                GL.PushAttrib(AttribMask.AllAttribBits);
                {
                    GL.Color3(BackgroundColor);
                    GL.PolygonMode(MaterialFace.FrontAndBack, DrawMode);

                    GL.MatrixMode(MatrixMode.Modelview);
                    GL.PushMatrix();
                    {
                        GL.Translate((Vector3)Location);
                        GL.Scale((Vector3)ScaleFactor);
                        GL.Begin(BeginMode);
                        {
                            foreach (Vertex vertex in Vertices)
                            {
                                vertex.Draw();
                            }
                        }
                        GL.End();
                    }
                    GL.PopMatrix();
                }
                GL.PopAttrib();
            }
        }
    }
}
