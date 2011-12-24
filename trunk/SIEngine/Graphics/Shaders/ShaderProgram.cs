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

namespace SIEngine.Graphics.Shaders
{
    public class ShaderProgram
    {
        public int Program { get; private set; }
        public FragmentShader FragmentShader { get; private set; }
        public VertexShader VertexShader { get; private set; }

        #region constructors
        public ShaderProgram()
        {
            Program = -1;
        }
        public ShaderProgram(string vertexShader, string fragmentShader)
        {
            GenerateProgram();
            AttachShader(new VertexShader(vertexShader));
            AttachShader(new FragmentShader(fragmentShader));
            LinkProgram();
        }
        public ShaderProgram(VertexShader vShader, FragmentShader fShader)
        {
            GenerateProgram();
            AttachShader(vShader);
            AttachShader(fShader);
            LinkProgram();
        }
        #endregion

        public virtual void Dispose()
        {
            GL.DeleteProgram(Program);
        }

        #region program management
        public void GenerateProgram()
        {
            Program = GL.CreateProgram();
        }

        public void LinkProgram()
        {
            GL.LinkProgram(Program);
        }
        public void UseProgram()
        {
            GL.UseProgram(Program);
        }
        public void LinkAndUseProgram()
        {
            LinkProgram();
            UseProgram();
        }
        #endregion

        #region shader management
        public void AttachShader(AbstractShader shader)
        {
            GL.AttachShader(Program, shader.Shader);

            switch(shader.Type)
            {
                case ShaderType.VertexShader:
                    VertexShader = (VertexShader)shader;
                    break;
                case ShaderType.FragmentShader:
                    FragmentShader = (FragmentShader)shader;
                    break;
                case ShaderType.GeometryShader:
                    //TODO:implement
                    break;
            }
        }
        public void DetachShader(AbstractShader shader)
        {
            GL.DetachShader(Program, shader.Shader);

            switch (shader.Type)
            {
                case ShaderType.VertexShader:
                    VertexShader = null;
                    break;
                case ShaderType.FragmentShader:
                    FragmentShader = null;
                    break;
                case ShaderType.GeometryShader:
                    //TODO:implement
                    break;
            }
        }
        #endregion

        public void WriteUniform(string name, float value)
        {
            int loc = GL.GetUniformLocation(Program, name);
            GL.Uniform1(loc, value);
        }

        public void WriteSampler(string name, Texture sampler)
        {
            int loc = GL.GetUniformLocation(Program, name);
            GL.Uniform1(loc, sampler.Location);
        }
    }
}
