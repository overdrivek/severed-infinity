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
        public abstract class Window : GameWindow
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

            public Window()
            {
                //do nothing
            }

            public Window (int width, int height, string title)
            {
                Initialize(width, height, title);
            }

            public void Initialize(int width, int height, string title)
            {
                this.Width = width;
                this.Height = height;

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

            //Note: in the below function it is imperative that
            //we set the parent before drawing the control.
            //In case the control wants to use the property
            //the program will crash.

            #region Management
            public void AddChildren(params GUIObject[] children)
            {
                foreach (GUIObject child in children)
                {
                    child.Parent = this;
                    Children.Add(child);
                }
            }
            public void Add3DChildren(params Object[] children)
            {
                foreach (Object child in children)
                {
                    child.Parent = this;
                    Children3D.Add(child);
                }
            }
            #endregion

            #region input

            protected virtual void OnMouseMove(object sender, MouseEventArgs evArgs)
            {
                for (int i = 0; i < Children.Count; ++i)
                {
                    Children[i].CallEvent(GUIObject.EventType.MouseMove, new Vector(evArgs.X, evArgs.Y), null);
                }
            }

            protected virtual void OnMouseUp(object sender, MouseEventArgs evArgs)
            {
                //collection might be changed
                for (int i = 0; i < Children.Count; ++ i)
                {
                    Children[i].CallEvent(GUIObject.EventType.MouseUp, new Vector(evArgs.X, evArgs.Y), null);
                }
            }

            protected virtual void OnMouseClick(object sender, MouseEventArgs evArgs)
            {
                ///note: foreach crashes because we modify the collection when switching states
                ///e.g. by clicking the play game button

                //foreach (GUIObject child in this.Children)
                for (int i = 0; i < Children.Count; ++ i)
                {
                    Children[i].CallEvent(GUIObject.EventType.MouseClick, new Vector(evArgs.X, evArgs.Y), null);
                }
            }

            protected virtual void OnKeyPress(KeyboardKeyEventArgs evArgs)
            {
                for (int i = 0; i < Children.Count; ++i)
                {
                    Children[i].CallEvent(GUIObject.EventType.KeyDown, Children[i].Location, evArgs.Key);
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
                    if (control3d.Visible)
                        control3d.Draw();

                Camera.CurrentMode = Camera.CameraMode.OrthographicOverview;
                Camera.DoCameraTransformation(this);
                foreach (GUIObject child in this.Children)
                    if (child.Visible)
                        child.Draw();
                Draw();

                SwapBuffers();
            }

            protected abstract void Draw();

        }
    }
}