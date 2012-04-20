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

using TextPrinter = OpenTK.Graphics.TextPrinter;
using TextQuality = OpenTK.Graphics.TextQuality;

namespace SIEngine
{
    namespace GUI
    {
        public class Button : GUIObject
        {
            /// <summary>
            /// The background image.
            /// </summary>
            private SIEngine.Graphics.Texture TextureImage { get; set; }
            public string Image
            {
                set
                {
                    this.TextureImage = new Graphics.Texture(value, TextureMinFilter.Linear,
                        TextureMagFilter.Linear);
                }
            }

            /// <summary>
            /// The text of the button.
            /// </summary>
            public string Text { get; set; }
            /// <summary>
            /// The color of the text;
            /// </summary>
            public Color ForegroundColor { get; set; }
            public Color SecondaryForegroundColor { get; set; }
            /// <summary>
            /// The background color.
            /// </summary>
            public Color BackgroundColor { get; set; }
            public Color SecondaryBackgroundColor { get; set; }
            /// <summary>
            /// The font of the button text;
            /// </summary>
            public Font TextFont { get; set; }
            //private TextPrinter printer;
            public ButtonEffects ButtonEffect { get; set; }

            public Button()
            {
                //printer = new TextPrinter(TextQuality.Default);
                TextFont = TextPrinter.DefaultFont;
                this.ForegroundColor = Color.Black;
                this.BackgroundColor = Color.LightGray;

                this.SecondaryForegroundColor = Color.Black;
                this.SecondaryBackgroundColor = Color.LightGray;

                ButtonEffect = new ButtonEffects(this);
                ButtonEffect.OverShadow = false;
                ButtonEffect.BorderEffect = false;

                //this.Image = new Graphics.Texture("data/new.bmp", TextureMinFilter.Linear, TextureMagFilter.Linear);
                //this.SecondaryImage;
            }

            #region Experimental
            public void ApplyStylishEffect()
            {
                ButtonEffect.OverShadow = true;
                ButtonEffect.BorderEffect = false;
                BackgroundColor = Color.White;
                SecondaryBackgroundColor = Color.White;
                ForegroundColor = Color.FromArgb(81, 45, 14) ;
                SecondaryForegroundColor = ForegroundColor;
                Size = new Vector(130, 25);
            }
            #endregion

            #region overrides
            public override void Draw()
            {
                if (this.Location == null || this.Size == null)
                    return;

                if (ButtonEffect.OverShadow)
                    GeneralGraphics.BlendWhite();

                GL.MatrixMode(MatrixMode.Modelview);
                GL.PushMatrix();
                {
                    GL.Color4(this.BackgroundColor);
                    GL.Enable(EnableCap.Texture2D);
                    if (this.TextureImage != null)
                        this.TextureImage.SelectTexture();
                    else GL.Disable(EnableCap.Texture2D);

                    GL.Begin(BeginMode.Quads);
                    {
                        if (this.State == ObjectState.Clicked) GL.TexCoord2(-0.01f, -0.01f);
                        else GL.TexCoord2(0, 0);
                        GL.Vertex2(this.Location.X, this.Location.Y);

                        if (this.State == ObjectState.Clicked) GL.TexCoord2(-0.01f, 1.01f);
                        else GL.TexCoord2(0, 1);
                        GL.Vertex2(this.Location.X, this.Location.Y + this.Size.Y);

                        if (this.State == ObjectState.Clicked) GL.TexCoord2(1.01f, 1.01f);
                        else GL.TexCoord2(1, 1);
                        GL.Vertex2(this.Location.X + this.Size.X, this.Location.Y + this.Size.Y);

                        if (this.State == ObjectState.Clicked) GL.TexCoord2(1.01f, -0.01f);
                        else GL.TexCoord2(1, 0);
                        GL.Vertex2(this.Location.X + this.Size.X, this.Location.Y);
                    }
                    GL.End();

                    ButtonEffect.ApplyBorderEffect();

                    GL.Translate(this.Location.X + this.Size.X / 2 - (this.TextFont.Height / 2) * (this.Text.Length / 2),
                        this.Location.Y + (this.Size.Y - this.TextFont.Height) / 2 - 4 , 0);

                    if (this.State == ObjectState.Clicked)
                        GL.Translate(1.0f, 1.0f, 0.0f);
                    TextPrinter.Print(this.Text, this.ForegroundColor);

                }
                GL.PopMatrix();

                ButtonEffect.ApplyShadowEffect();
                GeneralGraphics.DisableBlending();
            }

            private void SwitchState()
            {
                Color middle;
                middle = this.SecondaryBackgroundColor;
                this.SecondaryBackgroundColor = this.BackgroundColor;
                this.BackgroundColor = middle;

                middle = this.SecondaryForegroundColor;
                this.SecondaryForegroundColor = this.ForegroundColor;
                this.ForegroundColor = middle;

                //SIEngine.Graphics.Texture iMiddle = this.Image;
                //this.Image = this.SecondaryImage;
                //this.SecondaryImage = iMiddle;

                //GC.Collect();
            } 

            public override void InternalMouseOut(Vector mousePos)
            {
                this.State = ObjectState.Normal;
                SwitchState();
            }

            public override void InternalMouseOver(Vector mousePos)
            {
                this.State = ObjectState.Hover;
                SwitchState();
            }

            #endregion
        }
    }
}