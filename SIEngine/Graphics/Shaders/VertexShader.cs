using System;
using System.Collections.Generic;
using OpenTK;
using System.Linq;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using SIEngine.Other;
using SIEngine.Graphics;
using SIEngine.BaseGeometry;
using System.IO;

namespace SIEngine.Graphics.Shaders
{
    public class VertexShader : AbstractShader
    {
        public VertexShader(string path, ShaderProgram program)
        {
            Type = ShaderType.VertexShader;
            Shader = GL.CreateShader(Type);
            LoadAndCompileShader(path);

            AttachToProgram(program);
        }
        public VertexShader(string path)
        {
            Type = ShaderType.VertexShader;
            Shader = GL.CreateShader(Type);
            Console.WriteLine("vs error:" + GL.GetError());
            LoadAndCompileShader(path);   
        }
    }
}
