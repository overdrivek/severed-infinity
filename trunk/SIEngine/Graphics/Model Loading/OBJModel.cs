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
using SIEngine.Graphics.Rendering;

using SIEngine.Graphics.Shaders;
namespace SIEngine.Graphics
{
    public class OBJModel
    {
        public Vector MinReach, MaxReach;

        public class Group
        {
            public string Name { get; set; }
            public int ListNumber { get; set; }
            public List<Polygon> Faces { get; set; }
            public Vector Location { get; set; }
            public Material Material { get; set; }
            public Color Color { get; set; }
            public OBJModel Parent { get; set; }

            public Group(string name)
            {
                Faces = new List<Polygon>();
                this.Color = Color.White;
                Name = name;
                ListNumber = -1;
            }

            public void Draw(Color? color)
            {
                if (color != null)
                    GL.Color4(color.Value);

                if (Material != null && Material.Image != null)
                {
                    GeneralGraphics.EnableTexturing();
                    Material.Image.SelectTexture();
                }
                else
                {
                    if (color == null)
                        GL.Color4(Color);
                    GeneralGraphics.DisableTexturing();
                }

                if (ListNumber == -1)
                {
                    GeneralGraphics.DrawFilled();
                    InternalDraw();
                    if (Parent.Stroke)
                    {
                        GeneralGraphics.DrawWireframe();
                        InternalDraw();
                        GeneralGraphics.DrawFilled();
                    }
                }
                else
                {
                    GeneralGraphics.DrawFilled();
                    GL.CallList(ListNumber);
                    if (Parent.Stroke)
                    {
                        GeneralGraphics.DrawWireframe();
                        GL.Color3(Color.Black);
                        GL.CallList(ListNumber);
                        GeneralGraphics.DrawFilled();
                    }
                }
            }

            public void Draw()
            {
                Draw(null);
            }

            public void Precompile()
            {
                ListNumber = GL.GenLists(1);
                GL.NewList(ListNumber, ListMode.Compile);
                    InternalDraw();
                GL.EndList();
            }

            public void InternalDraw()
            {
                foreach (Polygon face in Faces)
                    face.PureDraw();
            }
        }
        public class Material
        {
            public Texture Image { get; set; }
            public Color DiffuseColor { get; set; }
            public Color AmbientColor { get; set; }
            public Color SpecularColor { get; set; }
            public float Transparency { get; set; }
            public string Name { get; set; }

            public Material(string name)
            {
                Name = name;
                Transparency = 0.0f;
            }
        }

        private List<Vector> Vectors { get; set; }
        private List<TextureCoordinate> TexCoords { get; set; }
        private List<Normal> Normals { get; set; }
        public List<Group> Groups { get; set; }
        private List<Material> Materials { get; set; }
        private Random colorMizer;
        public Color Color { get; set; }
        public bool Stroke { get; set; }
        private Texture texture;
        public string Image
        {
            set
            {
                texture = new Texture(value);
            }
        }
        public float ScaleFactor { get; set; }
        public bool Rotate { get; set; }
        public Vector RotationVector { get; set; }
        private VBO VBORenderer { get; set; }

        public OBJModel()
        {
            Initialize();
        }

        public OBJModel(string path)
        {
            Initialize();
            ParseOBJFile(path);
        }

        public void Initialize()
        {
            Stroke = false;
            Color = Color.White;
            Vectors = new List<Vector>();
            TexCoords = new List<TextureCoordinate>();
            Normals = new List<Normal>();
            Groups = new List<Group>();
            Materials = new List<Material>();
            Rotate = false;
            colorMizer = new Random();
            RotationVector = new Vector(0.0f, 1.0f, 0.0f);
            VBORenderer = new VBO();

            ScaleFactor = 1.0f;
        }

        private void UploadModelToVBO()
        {
            List<Vertex> vertices = new List<Vertex>();

            foreach (Group group in Groups)
                foreach (Polygon face in group.Faces)
                    foreach (Vertex vert in face.Vertices)
                        vertices.Add(vert);

            VBORenderer.UploadData(vertices, BufferUsageHint.DynamicDraw);
        }

        public void PickDraw()
        {
            if (Rotate) GL.Rotate(x, RotationVector.X, RotationVector.Y, RotationVector.Z);
            GL.Scale(ScaleFactor, ScaleFactor, ScaleFactor);

            VBORenderer.Draw(BeginMode.Polygon);
        }

        public bool ApplyOriginalObjectColor = false;
        float x = 0.0f;
        public void Draw()
        {
            Draw(null, ApplyOriginalObjectColor);
        }

        public void Draw(Color? color, bool gColor = false)
        {

            if (Rotate) GL.Rotate(x++, RotationVector.X, RotationVector.Y, RotationVector.Z);
            GL.Scale(ScaleFactor, ScaleFactor, ScaleFactor);

            GeneralGraphics.DrawFilled();

            GeneralGraphics.EnableTexturing();
            GeneralGraphics.EnableAlphaBlending();
            //GL.Color4(Color.White);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.PushMatrix();
            {
                foreach (Group group in Groups)
                    if (!gColor)
                        group.Draw(color);
                    else
                        group.Draw(group.Material.DiffuseColor);
            }
            GL.PopMatrix();
            GeneralGraphics.DisableBlending();
            GeneralGraphics.DisableTexturing();
        }

        public void ParseOBJFile(string path)
        {
            if (!File.Exists(path)) return;

            ParseModel(path);
            BuildModel();
            UploadModelToVBO();
            CalculateReach();
        }

        public void CalculateReach()
        {
            MinReach = new Vector(1000f, 1000f);
            MaxReach = new Vector(0f, 0f);
            foreach (var group in Groups)
                foreach (var triangle in group.Faces)
                    foreach (var vertex in triangle.Vertices)
                    {
                        if (vertex.Location == null)
                            continue;

                        if (vertex.Location.X < MinReach.X)
                            MinReach.X = vertex.Location.X;
                        if (vertex.Location.X > MaxReach.X)
                            MaxReach.X = vertex.Location.X;

                        if (vertex.Location.Y < MinReach.Y)
                            MinReach.Y = vertex.Location.Y;
                        if (vertex.Location.Y > MaxReach.Y)
                            MaxReach.Y = vertex.Location.Y;
                    }
            MinReach *= ScaleFactor;
            MaxReach *= ScaleFactor;
        }

        private void ParseModel(string path)
        {
            string[] data = File.ReadAllLines(path, Encoding.ASCII);
            int i = 0;
            foreach (string line in data)
            {
                ParseLine(line, path, i == 0 ? "" : data[i - 1]);
                i++;
            }
        }
        private void ParseLine(string line, string path, string previous)
        {
            line = Regex.Replace(line, @"\s+", " ");
            string[] elements = line.Split(' ');

            switch (elements[0])
            {
                case "#":
                    //Console.WriteLine(line);
                    break;
                case "v":
                    Vectors.Add(new Vector(float.Parse(elements[1]), float.Parse(elements[2]), float.Parse(elements[3]),
                        elements.Length <= 4 ? 1.0f : float.Parse(elements[4])));
                    //Console.WriteLine("{0} {1} {2}", float.Parse(elements[1]), float.Parse(elements[2]),float.Parse(elements[3]));
                    break;
                case "vt":
                    TexCoords.Add(new TextureCoordinate(float.Parse(elements[1]), float.Parse(elements[2]),
                        elements.Length <= 3 ? 0.0f : float.Parse(elements[3])));
                    break;
                case "vn":
                    Normals.Add(new Normal(float.Parse(elements[1]), float.Parse(elements[2]),
                        elements.Length <= 3 ? 0.0f : float.Parse(elements[3])));
                    break;
                case "f":
                    Polygon polygon = new Polygon();
                    Vertex vertex = new Vertex(null);

                    int k = 0;
                    for (int i = 1; i < elements.Length; ++i)
                    {
                        string[] components = elements[i].Split('/');
                        foreach (string component in components)
                        {
                            if (component == "")
                                continue;
                            switch (k)
                            {
                                case 0:
                                    vertex.Location = Vectors[int.Parse(component) - 1];
                                    break;
                                case 1:
                                    if (elements[i].Contains("//"))
                                    {
                                        vertex.Normal = Normals[int.Parse(component) - 1];
                                        k = 10;
                                    }
                                    else vertex.TexCoord = TexCoords[int.Parse(component) - 1];
                                    break;
                                case 2:
                                    vertex.Normal = Normals[int.Parse(component) - 1];
                                    break;
                            }
                            ++k;
                        }

                        k = 0;
                        polygon.Vertices.Add(vertex);
                        Groups.Last().Faces.Add(polygon);
                        
                        vertex = new Vertex(null);
                    }
                    break;
                case "g":
                case "o":
                    Groups.Add(new Group(elements[1]));
                    break;
                case "usemtl":
                    string[] prev = previous.Split(' ');
                    if (string.Compare(prev[0], "g") != 0)
                        Groups.Add(new Group("dummy"));
                    Groups.Last().Material = Materials.Find((material) => string.Compare (material.Name, elements[1]) == 0);
                    break;  
                case "mtllib":
                    string mtlpath = Path.Combine(Path.GetDirectoryName(path), elements[1]);
                    if (!File.Exists(mtlpath)) break;
                    ParseMaterial(mtlpath);
                    break;
            }
        }

        private void BuildModel()
        {
            foreach (Group group in Groups)
            {
                group.Color = Color;
                group.Parent = this;
                //group.Color = Color.FromArgb((byte)colorMizer.Next(0, 255), (byte)colorMizer.Next(0, 255), (byte)colorMizer.Next(0, 255));
                group.Precompile();
            }
        }

        private void ParseMaterial(string path)
        {
            string[] data = File.ReadAllLines(path, Encoding.ASCII);
            foreach (string line in data)
            {
                string formattedLine = Regex.Replace(line, @"\s+", " ");
                string[] elements = formattedLine.Split(' ');

                if (formattedLine == "")
                    continue;

                switch (elements[0])
                {
                    case "#":
                        //Console.WriteLine(formattedLine);
                        break;
                    case "newmtl":
                        Materials.Add(new Material(elements[1]));
                        break;
                    case "":
                        switch (elements[1])
                        {
                            case "Ks":
                                Materials.Last().SpecularColor = Color.FromArgb((byte)(255.0f * float.Parse(elements[2])),
                                    (byte)(255.0f * float.Parse(elements[3])), (byte)(255.0f * float.Parse(elements[4])));
                                break;
                            case "Ka":
                                Materials.Last().AmbientColor = Color.FromArgb((byte)(255.0f * float.Parse(elements[2])),
                                    (byte)(255.0f * float.Parse(elements[3])), (byte)(255.0f * float.Parse(elements[4])));
                                break;
                            case "Kd":
                                Materials.Last().DiffuseColor = Color.FromArgb((byte)(255.0f * float.Parse(elements[2])),
                                    (byte)(255.0f * float.Parse(elements[3])), (byte)(255.0f * float.Parse(elements[4])));
                                break;
                            case "map_Kd":
                                Materials.Last().Image = new Texture(
                                    Path.Combine(Path.GetDirectoryName(path), elements[2]));
                                break;
                            case "Tr":
                                float alpha = float.Parse(elements[2]);
                                if (alpha != 0.0f)
                                    Materials.Last().Transparency = alpha;
                                break;
                        }
                        break;
                }
            }
        }
    }
}
