using System;
using System.Collections.Generic;
using OpenTK;
using System.Linq;
using OpenTK.Graphics;
using System.Drawing;
using System.Windows.Forms;
using SIEngine.Graphics;
using OpenTK.Input;
using Vector = SIEngine.BaseGeometry.Vector;
using MouseEventArgs = OpenTK.Input.MouseEventArgs;
using KeyPressEventArgs = OpenTK.KeyPressEventArgs;

namespace SIEngine
{
    namespace GUI
    {
        public class Window : GameWindow
        {
            public Color BackgroundColor
            {
                set
                {
                    GL.ClearColor(value);
                }
            }
            public List<GUIObject> Children { get; set; }
            public List<Object> Children3D { get; set; }
            public List<GUIObject> Controls3D { get; set; }
            public enum WindowState
            {
                Intro = 0,
                MainMenu = 1,
                Game = 2,
                InGameMenu = 3,
                Credits = 4,
                Settings = 5,
                Quit = 6
            }

            public Window (int width, int height, string title) : base (width, height)
            {
                this.Title = title;
                Children = new List<GUIObject>();
                Children3D = new List<Object>();
                Controls3D = new List<GUIObject>();
                this.BackgroundColor = Color.Black;
                
                this.Mouse.ButtonUp += OnMouseUp;
                this.Mouse.Move += OnMouseMove;
                this.Keyboard.KeyDown += (o, e) =>
                {
                    OnKeyPress(e);
                };
                this.Mouse.ButtonDown += OnMouseClick;
            }

            private bool cursorShown = true;
            public bool ShowCursor
            {
                set
                {
                    cursorShown = value;

                    if (value) Cursor.Show();
                    else Cursor.Hide();
                }
            }

            protected override void OnResize(EventArgs e)
            {
                GL.Viewport(0, 0, this.Width, this.Height);
            }

            #region input

            private void OnMouseMove (object sender, MouseEventArgs evArgs)
            {
                foreach (GUIObject child in this.Children)
                {
                    child.CallEvent(GUIObject.EventType.MouseMove, new Vector(evArgs.X, evArgs.Y), null);
                }
            }

            private void OnMouseUp(object sender, MouseEventArgs evArgs)
            {
                foreach (GUIObject child in this.Children)
                {
                    child.CallEvent(GUIObject.EventType.MouseUp, new Vector(evArgs.X, evArgs.Y), null);
                }
            }

            private void OnMouseClick (object sender, MouseEventArgs evArgs)
            {
                foreach (GUIObject child in this.Children)
                {
                    child.CallEvent(GUIObject.EventType.MouseClick, new Vector(evArgs.X, evArgs.Y), null);
                }
            }

            protected void OnKeyPress (KeyboardKeyEventArgs evArgs)
            {
                foreach (GUIObject child in this.Children)
                {
                    child.CallEvent(GUIObject.EventType.KeyDown, child.Location, evArgs.Key);
                }
            }
            
            #endregion

            protected override void OnUpdateFrame(FrameEventArgs e)
            {
                Application.DoEvents();
            }

            protected override void OnRenderFrame(FrameEventArgs e)
            {
                GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

                Camera.CurrentMode = Camera.CameraMode.Overview;
                Camera.DoCameraTransformation(this);

                foreach (Object child in this.Children3D)
                    if(child.Visible)
                        child.Draw();
                foreach (GUIObject control3d in this.Controls3D)
                    if(control3d.Visible)
                        control3d.Draw();

                Camera.CurrentMode = Camera.CameraMode.OrthographicOverview;
                Camera.DoCameraTransformation(this);
                foreach (GUIObject child in this.Children)
                {
                    child.Draw();
                }

                SwapBuffers();
            }

        }
    }
}