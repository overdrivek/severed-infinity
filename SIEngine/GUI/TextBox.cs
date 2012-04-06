using System;
using System.Collections.Generic;
using OpenTK;
using System.Linq;
using OpenTK.Input;
using OpenTK.Graphics;
using System.Drawing;
using SIEngine.Other;
using SIEngine.BaseGeometry;

namespace SIEngine
{
    namespace GUI
    {
        public class TextBox : GUIObject
        {
            public string Text { get; set; }
            public Font TextFont { get; set; }
            public int CharacterLimit { get; set; }
            
            public TextBox ()
            {
                this.Text = "";
                CharacterLimit = 20;
                Size = new Vector(150, 20);
            }

            public override void InternalKeyDown(Key key)
            {
                switch (key)
                {
                    case Key.Space:
                        if (Text.Length + 1 <= CharacterLimit)
                            this.Text += " ";
                        break;
                    case Key.Back:
                        if (this.Text.Length < 1)
                            break;
                        this.Text = this.Text.Remove(this.Text.Length - 1);
                        break;
                    case Key.KeypadMinus:
                    case Key.Minus:
                        this.Text += "-";
                        break;
                }
                if (string.Compare(key.ToString().Remove(key.ToString().Length - 1), "Number") == 0)
                {
                    if (Text.Length + 1 <= CharacterLimit)
                        this.Text += key.ToString()[key.ToString().Length - 1];
                    return;
                }

                if (key.ToString().Length > 1)
                    return;

                bool capitalLetters = Parent.Keyboard[Key.LShift] || Parent.Keyboard[Key.RShift];
                if (Text.Length + 1 <= CharacterLimit)
                    this.Text += capitalLetters ? key.ToString() : key.ToString().ToLower();
            }

            public override void Draw()
            {
                CharacterLimit = (int)Size.X / ((int)TextPrinter.DefaultFont.Size - 5);

                GL.Disable(EnableCap.Texture2D);
                GL.MatrixMode(MatrixMode.Modelview);
                GL.PushMatrix();
                {
                    GL.Color3(Color.White);
                    GL.Begin(BeginMode.Quads);
                    {
                        GL.Vertex2(this.Location.X, this.Location.Y);
                        GL.Vertex2(this.Location.X + this.Size.X, this.Location.Y);
                        GL.Vertex2(this.Location.X + this.Size.X, this.Location.Y + this.Size.Y);
                        GL.Vertex2(this.Location.X, this.Location.Y + this.Size.Y);
                    }
                    GL.End();

                    GL.Color3(Color.Black);
                    GL.Begin(BeginMode.LineStrip);
                    {
                        GL.Vertex2(this.Location.X, this.Location.Y);
                        GL.Vertex2(this.Location.X + this.Size.X, this.Location.Y);
                        GL.Vertex2(this.Location.X + this.Size.X, this.Location.Y + this.Size.Y);
                        GL.Vertex2(this.Location.X, this.Location.Y + this.Size.Y);
                        GL.Vertex2(this.Location.X, this.Location.Y);
                    }
                    GL.End();

                    GL.Translate(this.Location.X + 5, this.Location.Y, 0);
                    TextPrinter.Print(this.Text + (this.State == ObjectState.Clicked ? "|" : " "), Color.Black);
                }
                GL.PopMatrix();
            }

            public override void InternalMouseUp(Vector mousePos)
            {
                this.State = ObjectState.Clicked;
            }
        }
    }
}