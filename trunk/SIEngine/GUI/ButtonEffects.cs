using System;
using System.Collections.Generic;
using OpenTK;
using System.Linq;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using SIEngine.Other;
using SIEngine.Graphics;

namespace SIEngine.GUI
{
    public class ButtonEffects
    {
        public bool OverShadow { get; set; }
        public bool BorderEffect { get; set; }
        private Button ParentButton { get; set; }
        private Texture shadow;

        public ButtonEffects(Button parent)
        {
            this.ParentButton = parent;
            shadow = new Texture("data/img/shadow.bmp");
        }

        /// <summary>
        /// Call this to apply effects. Precautions included!
        /// </summary>
        public void ApplyAllEffects()
        {
            ApplyBorderEffect();
            ApplyShadowEffect();
        }
        
        public void ApplyBorderEffect()
        {
            if (!this.BorderEffect)
                return;
            
            if (ParentButton.State == GUIObject.ObjectState.Clicked) GL.Color3(Color.White);
            else GL.Color3(Color.Black);
            GL.Begin(BeginMode.LineStrip);
            {
                GL.Vertex2(ParentButton.Location.X + ParentButton.Size.X, ParentButton.Location.Y);
                GL.Vertex2(ParentButton.Location.X + ParentButton.Size.X, ParentButton.Location.Y + ParentButton.Size.Y);
                GL.Vertex2(ParentButton.Location.X, ParentButton.Location.Y + ParentButton.Size.Y);

                if (ParentButton.State == GUIObject.ObjectState.Clicked) GL.Color3(Color.Black);
                else GL.Color3(Color.White);
                GL.Vertex2(ParentButton.Location.X, ParentButton.Location.Y + ParentButton.Size.Y);
                GL.Vertex2(ParentButton.Location.X, ParentButton.Location.Y);
                GL.Vertex2(ParentButton.Location.X + ParentButton.Size.X, ParentButton.Location.Y);
            }
            GL.End();
        }

        public void ApplyShadowEffect()
        {
            if (!OverShadow || ParentButton.State == GUIObject.ObjectState.Normal)
                return;

            GeneralGraphics.EnableAlphaBlending();
            GeneralGraphics.EnableTexturing();
            shadow.SelectTexture();

            GL.MatrixMode(MatrixMode.Modelview);
            GL.PushMatrix();
            {
                GL.Color4(1.0f, 1.0f, 1.0f, 0.2f);
                GeneralGraphics.DrawRectangle(ParentButton.Location, ParentButton.Size);
            }
            GL.PopMatrix();

            GeneralGraphics.DisableBlending();
        }
    }
}
