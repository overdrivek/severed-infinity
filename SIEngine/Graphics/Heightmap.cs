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
using Object = SIEngine.GUI.Object;

namespace SIEngine.Graphics
{
    /// <summary>
    /// A class to load and draw a heightmap.
    /// Doesn't use VBO to facilitate change in
    /// terrain.
    /// </summary>
    public class Heightmap : Object
    {
        public static Gradient HeightmapGradient { get; set; }

        private byte[] ActualMap { get; set; }
        /// <summary>
        /// Defines how big the map actually is when drawn.
        /// </summary>
        public int SquareSize { get; set; }
        public Color LowColor { get; set; }
        public Color HighColor { get; set; }

        public Vector MapScale { get; set; }
        /// <summary>
        /// Loads a heightmap from a given file.
        /// </summary>
        /// <param name="path">The path to the RAW file with the heightmap
        /// values.</param>
        /// 
        public Heightmap(string path, Color lowColor, Color hightColor, Vector mapScale)
        {
            HeightmapGradient = new Gradient(Color.LightBlue, Color.SandyBrown,
                Color.Green, Color.DarkGray, Color.White);

            SquareSize = 1;
            MapScale = mapScale;
            LowColor = lowColor;
            HighColor = hightColor;
            FromFile(path);
        }

        public void FromFile(string path)
        {
            if (!File.Exists(path))
                return;
            FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read);
            int size = (int)Math.Sqrt(stream.Length / sizeof(byte));
            Console.WriteLine(size);

            Size.X = size;
            Size.Y = size;
            ActualMap = new byte[size * size];

            stream.Read(ActualMap, 0, size * size);

            list = GL.GenLists(1);
            GL.NewList(list, ListMode.Compile);
            {
                GL.Begin(BeginMode.Quads);
                {
                    for (int x = 0; x < Size.X - SquareSize; x += SquareSize)
                        for (int y = 0; y < Size.Y - SquareSize; y += SquareSize)
                        {
                            SetVertexColor(x, y);
                            GL.Vertex3(x, GetHeight(x, y), y);

                            SetVertexColor(x, y + SquareSize);
                            GL.Vertex3(x, GetHeight(x, y + SquareSize), y + SquareSize);

                            SetVertexColor(x + SquareSize, y + SquareSize);
                            GL.Vertex3(x + SquareSize, GetHeight(x + SquareSize, y + SquareSize), y + SquareSize);

                            SetVertexColor(x + SquareSize, y);
                            GL.Vertex3(x + SquareSize, GetHeight(x + SquareSize, y), y);
                        }
                }
                GL.End();
            }
            GL.EndList();
        }

        private float GetHeight(int x, int y)
        {
            return (float)ActualMap[x + y * (int)Size.X];// / 255f;
        }

        int list;
        private void SetVertexColor(int x, int y)
        {
            GL.Color3(HeightmapGradient.GetColor((1f - GetHeight(x, y) / 200f)));
            //GL.Color3(GeneralMath.Interpolate(LowColor, HighColor, GetHeight(x, y) / 255f));
        }

        public override void Draw()
        {
            GeneralGraphics.UseSimulatedLighting();
            GL.PushMatrix();
            {
                GL.Translate(Location.X, Location.Y, Location.Z);
                GL.Scale(MapScale.X, MapScale.Y, MapScale.Z);
                GL.Rotate(-160f, 1f, 0f, 0f);

                GL.CallList(list);
            }
            GL.PopMatrix();
            GeneralGraphics.UseDefaultShaderProgram();
        }
    }
}
