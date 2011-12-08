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

            public delegate void MouseEventDel();
            public delegate void KeyEventDel(Key argument);
            public MouseEventDel MouseOver;
            public MouseEventDel MouseOut;
            public MouseEventDel MouseClick;
            public MouseEventDel MouseUp;
            public KeyEventDel KeyDown;

            public GUIObject ()
            {
                this.MouseOver = new MouseEventDel(InternalMouseOver);
                this.MouseOut = new MouseEventDel(InternalMouseOut);
                this.MouseClick = new MouseEventDel(InternalMouseClick);
                this.State = ObjectState.Normal;
                this.KeyDown = new KeyEventDel(InternalKeyDown);
                this.MouseUp = new MouseEventDel(InternalMouseUp);
                Visible = true;
                Size = new Vector(150, 20);
            }

            public void CallEvent(EventType type, Vector position, Key? argument)
            {
                if (Location == null || !Visible)
                    return;

                if (!(position.X >= this.Location.X && position.X <= this.Size.X + this.Location.X
                    && position.Y >= this.Location.Y && position.Y <= this.Location.Y + this.Size.Y))
                {
                    if (this.State == ObjectState.Clicked && type == EventType.MouseMove)
                    {
                        MouseOut.Invoke();
                        return;
                    }

                    if (this.State == ObjectState.Normal || 
                        (this.State == ObjectState.Clicked && type != EventType.MouseClick))
                        return;

                    MouseOut.Invoke();
                    this.State = ObjectState.Normal;
                    return;
                }

                switch (type)
                {
                    case EventType.MouseClick:
                        MouseClick.Invoke();
                        this.State = ObjectState.Clicked;
                        break;
                    case EventType.MouseUp:
                        this.State = ObjectState.Hover;
                        MouseUp.Invoke();
                        break;
                    case EventType.MouseMove:
                        if (this.State == ObjectState.Hover || this.State == ObjectState.Clicked)
                            return;
                        MouseOver.Invoke();
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
            public virtual void InternalMouseOver() { }
            public virtual void InternalMouseOut() { }
            public virtual void InternalMouseClick() { }
            public virtual void InternalMouseUp() { }
            public virtual void InternalKeyDown (Key key) { }
        }
    }
}