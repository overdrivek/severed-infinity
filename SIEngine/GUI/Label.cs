using System;
using System.Collections.Generic;
using OpenTK;
using System.Linq;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using SIEngine.Other;

using TextPrinter = OpenTK.Graphics.TextPrinter;
using TextQuality = OpenTK.Graphics.TextQuality;

namespace SIEngine
{
    namespace GUI
    {
        public class Label : GUIObject
        {
            public string Text { get; set; }
            public Font TextFont { get; set; }
            public Color ForegroundColor { get; set; }

            public Label ()
            {
                this.TextFont = new Font("Comic", 14, FontStyle.Bold, GraphicsUnit.Pixel);
                this.ForegroundColor = Color.Black;
                this.Text = "";
            }

            public override void Draw()
            {
                if (this.Location == null || this.Size == null)
                    return;

                GL.MatrixMode(MatrixMode.Modelview);
                GL.PushMatrix();
                {
                    GL.Translate(this.Location.X + this.Size.X / 2 - (this.TextFont.Height / 2) * (this.Text.Length / 2),
                          this.Location.Y + (this.Size.Y - this.TextFont.Height) / 2, 0);
                    TextPrinter.Print(Text, ForegroundColor);
                }
                GL.PopMatrix();
            }
        }
    }
}