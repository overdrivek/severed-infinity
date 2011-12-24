using System;
using System.Linq;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;
using SIEngine.GUI;
using System.Drawing;
using System.Drawing.Imaging;
using SIEngine.BaseGeometry;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Reflection;

namespace SIEngine.Graphics
{
    public class MD2Model
    {
        private const int HeaderEnd = 68;
        private FileStream Reader { get; set; }
        private List<Vertex> Vertices { get; set; }

        public MD2Model(string path)
        {
            //load model here
            if (!File.Exists(path))
                return;

            Header = new MD2Header();
            Reader = new FileStream(path, FileMode.Open);

            boxedHeader = ParseHeader();
            Header = (MD2Header)boxedHeader;

            Vertices = new List<Vertex>();
        }

        private int GetNextInt()
        {
            byte[] buffer = new byte[4];
            Reader.Read(buffer, 0, 4);

            return BitConverter.ToInt32(buffer, 0);
        }

        private object ParseHeader()
        {
            FieldInfo[] fields = Header.GetType().GetFields();
            object temp = (object)Header;
            foreach (FieldInfo field in fields)
                field.SetValue(temp, GetNextInt());
            return temp;
        
        }

        private void ParseVertices()
        {

        }

        private struct MD2Header
        {
            public int ident;
            public int version;

            public int texWidth;
            public int texHeight;

            public int frameSize;

            public int numSkins;
            public int numVertices;
            public int numTexCoordinates;
            public int numTriangles;
            public int numGLCommands;
            public int numFrames;

            public int offsetSkinData;
            public int offsetTexCoordData;
            public int offsetTriangleData;
            public int offsetFrameData;
            public int offsetGLCommands;
            public int offsetEnd;
        }
        private MD2Header Header;
        private object boxedHeader;

        private struct ModelData
        {
            public List<Vertex> Vertices;
            
        }

        private struct Frame
        {

        }
    }
}
