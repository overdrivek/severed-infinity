using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using HWEngine.BaseGeometry;
using HWEngine.Other;

namespace HWEngine.Graphics
{
    public enum ReturnValue
    {
        OK = 0,
        FileNotFound = 1,
        ErrorInFile = 2,
        ErrorWhileParsing = 3,
        InternalError = 4
    }

    public static class OBJModelLoader
    {
        public delegate void EventParser(Vector vec);
        public static ReturnValue ParseOBJFile (string path, EventParser vectorParser,
            EventParser texCoordParser, EventParser normalParser, EventParser commentParser = null)
        {
            if (!File.Exists(path)) return ReturnValue.FileNotFound;

            string[] data = File.ReadAllLines(path, Encoding.ASCII);
            foreach (string line in data)
            {
            }

            return ReturnValue.OK;
        }

        private struct LineData
        {
            public LineType type;
            public Vertex data;

            public LineData(LineType type, Vector data)
            {
                this.type = type;
                this.data = data;
            }
        }

        private static LineData ParseLine (string line)
        {
            string[] elements = line.Split(' ');
            switch (elements[0])
            {
                case "v":
                    new LineData(LineType.Vertex,
                        new Vector(float.Parse(elements[0]), float.Parse(elements[1]), float.Parse(elements[2]),
                            elements.Length < 4 ? 1.0f : float.Parse(elements[3])) );
                    break;
                case "vt":
                    new LineData(LineType.TexCoordinates,
                        new Vector(float.Parse(elements[0]), float.Parse(elements[1]),
                            elements.Length < 3 ? 1.0f : float.Parse(elements[2])) );
                    break;
                case "vn":
                    new LineData(LineType.TexCoordinates,
                        new Vector(float.Parse(elements[0]), float.Parse(elements[1]),
                            elements.Length < 3 ? 1.0f : float.Parse(elements[2])) );
                    break;
                case "g":
                case "o":
                    break;
            }
            return new LineData(LineType.Unknown, null);
        }

        private enum LineType
        {
            Comment = 0,
            Vertex = 1,
            Normal = 2,
            TexCoordinates = 3,
            Face = 4,
            Group = 5,
            Object = 6,
            UseMaterial = 7,
            MaterialReference = 8,
            SmoothShading = 9,
            Unknown = 10
        }
    }
}
