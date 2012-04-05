using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIEngine.GUI;
using SIEngine.BaseGeometry;
using SIEngine.Graphics;
using SI.Other;
using SIEngine.Input;
using SIEngine.Graphics.ParticleEngines;
using SIEngine.Other;
using SIEngine.Physics;
using SI.Game;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using Object = SIEngine.GUI.Object;

namespace SI.Game
{
    /// <summary>
    /// Draws the infinity modulator and rotates
    /// it according to where the mouse pointer is.
    /// Also shoots the laser when the user clicks.
    /// 
    /// WARNING: Use ONLY with the GameWindow class.
    /// </summary>
    public class Gun : Object
    {
        private OBJModel modulator = new OBJModel("data/models/gun/gun.obj");
        private Laser mainLaser;
        public new GameWindow Parent { get; set; }

        public Gun()
        {
            Location = new Vector(0f, -23f, 0f);

            modulator.ScaleFactor = 0.2f;
            mainLaser = new Laser(new Vector(0f, -19f, 0f), null);
            mainLaser.Length = 2f;
        }

        public override void Draw()
        {
            if (!Visible || Parent.State == Window.WindowState.InGameMenu)
                return;

            GL.MatrixMode(MatrixMode.Modelview);
            GL.PushMatrix();
            {

                Vector position = GeometryMath.UnProjectMouse(new Vector(Parent.Mouse.X,
                    Parent.Mouse.Y));
                mainLaser.Destination = position;

                position.Z = 0f;
                position.Y += Location.Y;
                
                float zAngle = (float)Math.PI - (float)Math.Atan2(position.X, position.Y);
                
                GL.Translate(0f, -23f, 0f);

                GL.Rotate(-90.0f, 1.0f, 0.0f, 0.0f);
                GL.Rotate((float)GeometryMath.RadianToDegree(zAngle), 0f, 1f, 0f);

                modulator.Draw();

                position.Y -= Location.Y;
                mainLaser.Location.X = 4f * (float)Math.Sin(zAngle);
                mainLaser.Location.Y = -19f - 4f * (float)Math.Cos(zAngle);
            }
            GL.PopMatrix();
            mainLaser.trail.Draw();

            //if(Parent.GetType().IsAssignableFrom(typeof(Window)))
            if(typeof(Window).IsAssignableFrom(Parent.GetType()))
                if(((GameWindow)Parent).MouseClicked)
                    mainLaser.Draw();
        }
    }
}
