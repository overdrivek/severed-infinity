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
using SIEngine.Logging;

namespace SIEngine.Graphics.Shaders
{
    public abstract class AbstractShader
    {
        public int Shader { get; protected set; }
        public ShaderType Type { get; protected set; }

        public void AttachToProgram(ShaderProgram program)
        {
            program.AttachShader(this);
        }

        #region generate and compile
        public void LoadAndCompileShader(string path)
        {
            if (Shader == -1)
                return;

            if (!File.Exists(path))
            {
                LogManager.WriteError("Cannot open shader at:" + path);
                return;
            }

            StreamReader reader = new StreamReader(path);
            string @data = reader.ReadToEnd();
            CompileShader(@data);
            reader.Close();
            reader.Dispose();
            GC.Collect();
        }
        public void CompileShader(string @data)
        {
            GL.ShaderSource(Shader, @data);
            int compiled;
            GL.CompileShader(Shader);

            GL.GetShader(Shader, ShaderParameter.CompileStatus, out compiled);
            if (compiled == 0)
            {
                LogManager.WriteError("Cannot compile shader");
                LogManager.WriteError("\t Error message:" + GL.GetShaderInfoLog(Shader));
            }
        }
        #endregion
    }
}
