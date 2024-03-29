﻿using System;
using System.Collections.Generic;
using OpenTK;
using System.Linq;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using SIEngine.GUI;
using SIEngine.Other;
using SIEngine.Graphics;

using Object = SIEngine.GUI.Object;

namespace SIEngine.Graphics
{
    public class Skybox : Object
    {
        private Texture bottom, top, front, back, left, right;
        public float Angle { get; set; }

        public Skybox()
        {
            bottom = new Texture("data/img/3.bmp");
            top = new Texture("data/img/1.bmp");
            front = new Texture("data/img/6.bmp");
            back = new Texture("data/img/4.bmp");
            left = new Texture("data/img/2.bmp");
            right = new Texture("data/img/5.bmp");
            Angle = 90;
        }

        public override void Draw()
        {
            GL.MatrixMode(MatrixMode.Modelview);
            GeneralGraphics.DisableBlending();
            GeneralGraphics.EnableTexturing();

            GL.PushMatrix();
            {
                GL.Translate(0f, 0f, -(float)Camera.Zoom - 50);
                GL.Scale(300.0f, 300.0f, 300.0f);
                GL.Rotate(Angle, 0.0f, 1.0f, 0.0f);
                GL.Color3(Color.White);

                bottom.SelectTexture();
                GL.Begin(BeginMode.Quads);
                {
                    //bottom
                    GL.TexCoord2(0.0f, 1.0f);
                    GL.Vertex3(-.5f, -.5f, .5f);
                    GL.TexCoord2(1.0f, 1.0f);
                    GL.Vertex3(.5f, -.5f, .5f);
                    GL.TexCoord2(1.0f, 0.0f);
                    GL.Vertex3(.5f, -.5f, -.5f);
                    GL.TexCoord2(0.0f, 0.0f);
                    GL.Vertex3(-.5f, -.5f, -.5f);
                }
                GL.End();

                top.SelectTexture();
                GL.Begin(BeginMode.Quads);
                {
                    //top
                    GL.TexCoord2(0.0f, 1.0f);
                    GL.Vertex3(-.5f, .5f, .5f);
                    GL.TexCoord2(1.0f, 1.0f);
                    GL.Vertex3(.5f, .5f, .5f);
                    GL.TexCoord2(1.0f, 0.0f);
                    GL.Vertex3(.5f, .5f, -.5f);
                    GL.TexCoord2(0.0f, 0.0f);
                    GL.Vertex3(-.5f, .5f, -.5f);
                }
                GL.End();

                back.SelectTexture();
                GL.Begin(BeginMode.Quads);
                {
                    //back
                    GL.TexCoord2(1.0f, 0.0f);
                    GL.Vertex3(-.5f, .5f, -.5f);
                    GL.TexCoord2(0.0f, 0.0f);
                    GL.Vertex3(.5f, .5f, -.5f);
                    GL.TexCoord2(0.0f, 1.0f);
                    GL.Vertex3(.5f, -.5f, -.5f);
                    GL.TexCoord2(1.0f, 1.0f);
                    GL.Vertex3(-.5f, -.5f, -.5f);
                }
                GL.End();

                front.SelectTexture();
                GL.Begin(BeginMode.Quads);
                {
                    //front
                    GL.TexCoord2(0.0f, 0.0f);
                    GL.Vertex3(-.5f, .5f, .5f);
                    GL.TexCoord2(1.0f, 0.0f);
                    GL.Vertex3(.5f, .5f, .5f);
                    GL.TexCoord2(1.0f, 1.0f);
                    GL.Vertex3(.5f, -.5, .5f);
                    GL.TexCoord2(0.0f, 1.0f);
                    GL.Vertex3(-.5f, -.5f, .5f);
                }
                GL.End();
                
                left.SelectTexture();
                GL.Begin(BeginMode.Quads);
                {
                    //left
                    GL.TexCoord2(0.0f, 0.0f);
                    GL.Vertex3(-.5f, .5f, -.5f);
                    GL.TexCoord2(0.0f, 1.0f);
                    GL.Vertex3(-.5f, -.5f, -.5f);
                    GL.TexCoord2(1.0f, 1.0f);
                    GL.Vertex3(-.5f, -.5f, .5f);
                    GL.TexCoord2(1.0f, 0.0f);
                    GL.Vertex3(-.5f, .5f, .5f);
                }
                GL.End();

                right.SelectTexture();
                GL.Begin(BeginMode.Quads);
                {
                    //right
                    GL.TexCoord2(1.0f, 0.0f);
                    GL.Vertex3(.5f, .5f, -.5f);
                    GL.TexCoord2(1.0f, 1.0f);
                    GL.Vertex3(.5f, -.5f, -.5f);
                    GL.TexCoord2(0.0f, 1.0f);
                    GL.Vertex3(.5f, -.5f, .5f);
                    GL.TexCoord2(0.0f, 0.0f);
                    GL.Vertex3(.5f, .5f, .5f);
                }
                GL.End();
            }

            GL.PopMatrix();
        }
    }
}
