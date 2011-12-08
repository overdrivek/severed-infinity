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
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Text.RegularExpressions;

namespace SIEngine.Graphics
{
    public class OBJModel
    {
        private class Group
        {
            public string Name { get; set; }
            public int ListNumber { get; set; }
            public List<Polygon> Faces { get; set; }
            public Vector Location { get; set; }
            public Material Material { get; set; }
            public Color Color { get; set; }

            public Group(string name)
            {
                Faces = new List<Polygon>();
                this.Color = Color.Gray;
                Name = name;
                ListNumber = -1;
            }

            public void Draw()
            {
                if (Material != null && Material.Image != null)
                    Material.Image.SelectTexture();
                
                if (ListNumber == -1) InternalDraw();
                else GL.CallList(ListNumber);
            }

            public void Precompile()
            {
                ListNumber = GL.GenLists(1);
                GL.NewList(ListNumber, ListMode.Compile);
                    InternalDraw();
                GL.EndList();
            }

            private void InternalDraw()
            {
                foreach (Polygon face in Faces)
                {
                    GL.Begin(BeginMode.Polygon);
                        face.PureDraw();
                    GL.End();
                }
            }
        }
        private class Material
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
        private List<Group> Groups { get; set; }
        private List<Material> Materials { get; set; }
        private Random colorMizer;

        public OBJModel()
        {
            Vectors = new List<Vector>();
            TexCoords = new List<TextureCoordinate>();
            Normals = new List<Normal>();
            Groups = new List<Group>();
            Materials = new List<Material>();

            colorMizer = new Random();

            ScaleFactor = 1.0f;
        }

        private Texture texture;
        public string Image
        {
            set
            {
                texture = new Texture(value);
            }
        }

        public float ScaleFactor { get; set; }

        float x = 0.0f;
        public void Draw()
        {
            GL.Rotate(x++, 1.0, 0.0, 0.0);
            GL.Scale(ScaleFactor, ScaleFactor, ScaleFactor);

            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);

            GeneralGraphics.EnableTexturing();
            //GeneralGraphics.EnableAlphaBlending();
            GL.Color3(Color.White);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.PushMatrix();
            {
                foreach (Group group in Groups)
                    group.Draw();
            }
            GL.PopMatrix();
            //GeneralGraphics.DisableBlending();
            GeneralGraphics.DisableTexturing();
        }

        public void ParseOBJFile(string path)
        {
            if (!File.Exists(path)) return;

            ParseModel(path);
            BuildModel();
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
                group.Color = Color.FromArgb((byte)colorMizer.Next(0, 255), (byte)colorMizer.Next(0, 255), (byte)colorMizer.Next(0, 255));
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
                                Console.WriteLine(path);
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
