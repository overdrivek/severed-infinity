using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Graphics.OpenGL;
using SIEngine.BaseGeometry;
using System.Runtime.InteropServices;

namespace SIEngine.Graphics.Rendering
{
    public class VBO
    {
        private int address;
        private int size;

        public VBO()
        {
            GL.GenBuffers(1, out address);
        }
        
        [StructLayout(LayoutKind.Sequential)]
        struct VBOVertex
        {
            //coordinates
            public float x, y, z;
            //normal coordinates
            public float nx, ny, nz;
            //texcoords0
            public float s0, t0;

            public static readonly int Stride = Marshal.SizeOf(default(VBOVertex));
        }

        public void Draw(BeginMode beginMode)
        {
            Activate();
            {
                GL.DrawArrays(beginMode, 0, size);
            }
            Deactivate();
        }

        public void Activate()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, address);
            GL.EnableClientState(ArrayCap.VertexArray);
            GL.EnableClientState(ArrayCap.TextureCoordArray);
            GL.EnableClientState(ArrayCap.NormalArray);
        }

        public static void Deactivate()
        {
            GL.DisableClientState(ArrayCap.VertexArray);
            GL.DisableClientState(ArrayCap.TextureCoordArray);
            GL.DisableClientState(ArrayCap.NormalArray);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }

        public void UploadData(List<Vertex> vertices, BufferUsageHint usageHint)
        {
            this.size = vertices.Count;
            VBOVertex[] data = new VBOVertex[vertices.Count];
            for (int i = 0; i < vertices.Count; ++i)
            {
                if (vertices[i].Location != null)
                {
                    data[i].x = vertices[i].Location.X;
                    data[i].y = vertices[i].Location.Y;
                    data[i].z = vertices[i].Location.Z;
                }

                if (vertices[i].Normal != null)
                {
                    data[i].nx = vertices[i].Normal.X;
                    data[i].ny = vertices[i].Normal.Y;
                    data[i].nz = vertices[i].Normal.Z;
                }

                if (vertices[i].TexCoord != null)
                {
                    data[i].s0 = vertices[i].TexCoord.S;
                    data[i].t0 = vertices[i].TexCoord.T;
                }
            }

            Activate();
            {
                GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(data.Length * VBOVertex.Stride),
                   data, usageHint);
                GL.VertexPointer(3, VertexPointerType.Float, VBOVertex.Stride, 0);
                GL.NormalPointer(NormalPointerType.Float, VBOVertex.Stride, sizeof(float) * 3);
                GL.TexCoordPointer(2, TexCoordPointerType.Float, VBOVertex.Stride, sizeof(float) * 3);
            }
            Deactivate();

        }
    }
}
