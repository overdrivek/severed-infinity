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
    public class FragmentShader : AbstractShader
    {
        public FragmentShader(string path, ShaderProgram program)
        {
            Type = ShaderType.FragmentShader;
            Shader = GL.CreateShader(Type);
            LoadAndCompileShader(path);

            AttachToProgram(program);
        }
        public FragmentShader(string path)
        {
            Type = ShaderType.FragmentShader;
            Shader = GL.CreateShader(Type);
            LoadAndCompileShader(path);
            Console.WriteLine(GL.GetError());
        }
    }
}
