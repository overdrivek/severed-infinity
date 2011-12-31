using System;
using System.Linq;
using System.Collections.Generic;
using OpenTK;
using System.Drawing;
using SIEngine.BaseGeometry;
using OpenTK.Input;

namespace SIEngine
{
    namespace GUI
    {
        public abstract class GUIObject
        {
            public enum EventType
            {
                MouseMove = 0,
                MouseClick = 1,
                KeyDown = 2,
                MouseUp = 3
            }
            public enum ObjectState
            {
                Normal = 0,
                Hover = 1,
                Clicked = 2
            }
            public ObjectState State { get; set; }
            
            /// <summary>
            /// The size of the control.
            /// </summary>
            public Vector Size { get; set; }
            /// <summary>
            /// The position of the control.
            /// </summary>
            public Vector Location { get; set; }
            /// <summary>
            /// Sets the visibility of the control.
            /// </summary>
            public bool Visible { get; set; }
            /// <summary>
            /// The name of the control.
            /// </summary>
            public int Name { get; set; }
            public Window Parent { get; set; }

            public delegate void MouseEventDel(Vector mousePos);
            public delegate void KeyEventDel(Key argument);
            public MouseEventDel MouseMove;
            public MouseEventDel MouseOver;
            public MouseEventDel MouseOut;
            public MouseEventDel MouseClick;
            public MouseEventDel MouseUp;
            public KeyEventDel KeyDown;

            public GUIObject ()
            {
                MouseOver = new MouseEventDel(InternalMouseOver);
                MouseOut = new MouseEventDel(InternalMouseOut);
                MouseClick = new MouseEventDel(InternalMouseClick);
                MouseMove = new MouseEventDel(InternalMouseMove);
                State = ObjectState.Normal;
                KeyDown = new KeyEventDel(InternalKeyDown);
                MouseUp = new MouseEventDel(InternalMouseUp);
                Visible = true;
                Location = new Vector(0f, 0f, 0f);
                Size = new Vector(150, 20);
            }

            //this is created to increase performance
            private Vector mousePosition = new Vector(0f, 0f);
            public void CallEvent(EventType type, Vector position, Key? argument, params object[] a)
            {
                if (Parent != null)
                {
                    mousePosition.X = Parent.Mouse.X;
                    mousePosition.Y = Parent.Mouse.Y;
                }

                if (Location == null || !Visible)
                    return;

                if (!(position.X >= this.Location.X && position.X <= this.Size.X + this.Location.X
                    && position.Y >= this.Location.Y && position.Y <= this.Location.Y + this.Size.Y))
                {
                    if (this.State == ObjectState.Clicked && type == EventType.MouseMove)
                    {
                        MouseOut.Invoke(mousePosition);
                        return;
                    }

                    if (this.State == ObjectState.Normal || 
                        (this.State == ObjectState.Clicked && type != EventType.MouseClick))
                        return;

                    MouseOut.Invoke(mousePosition);
                    this.State = ObjectState.Normal;
                    return;
                }

                switch (type)
                {
                    case EventType.MouseClick:
                        MouseClick.Invoke(mousePosition);
                        this.State = ObjectState.Clicked;
                        break;
                    case EventType.MouseUp:
                        this.State = ObjectState.Hover;
                        MouseUp.Invoke(mousePosition);
                        break;
                    case EventType.MouseMove:
                        if (this.State == ObjectState.Hover || this.State == ObjectState.Clicked)
                            return;
                        MouseOver.Invoke(mousePosition);
                        this.State = ObjectState.Hover;
                        break;
                    case EventType.KeyDown:
                        if (this.State != ObjectState.Clicked)
                            return;
                        KeyDown.Invoke(argument.Value);
                        break;
                }
            }

            public abstract void Draw();
            public virtual void InternalMouseOver(Vector mousePos) { }
            public virtual void InternalMouseOut(Vector mousePos) { }
            public virtual void InternalMouseClick(Vector mousePos) { }
            public virtual void InternalMouseUp(Vector mousePos) { }
            public virtual void InternalMouseMove(Vector mousePos) { }
            public virtual void InternalKeyDown (Key key) { }
        }
    }
}