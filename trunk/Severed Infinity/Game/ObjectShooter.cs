using System;
using System.Collections.Generic;
using OpenTK;
using System.Linq;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using SIEngine.Other;
using SIEngine.Graphics;
using SIEngine.BaseGeometry;
using SIEngine.GUI;
using SI.Other;
using SIEngine.Graphics.ParticleEngines;
using System.Windows.Forms;
using Button = SIEngine.GUI.Button;
using Label = SIEngine.GUI.Label;
using Object = SIEngine.GUI.Object;

namespace SI.Game
{
    public class ObjectShooter : Object
    {
        private Timer MainTimer { get; set; }
        private Vector angle;
        private Explosion explosion;
        
        void AnimationStep()
        {

        }

        OBJModel model = new OBJModel("data/models/apple/apple.obj");

        /// <summary>
        /// Shoots a FlyingObject
        /// </summary>
        /// <returns></returns>
        public FlyingObject Shoot()
        {
            var fObject = new FlyingObject(null, model, 5);

            fObject.Start();
            angle.Z = (float)GeometryMath.RadianToDegree(
                GeometryMath.GetZAngle(fObject.PhysicalBody.Velocity));
            angle.Y = (float)GeometryMath.RadianToDegree(
                GeometryMath.GetYAngle(fObject.PhysicalBody.Velocity));
            explosion.Explode();

            return fObject;
        }

        public ObjectShooter()
        {
            explosion = new Explosion(16);
            explosion.Scale = 0.5f;

            MainTimer = new Timer();
            MainTimer.Interval = 10;
            angle = new Vector(0f, 0f, 0f);

            model.ScaleFactor = 0.01f;
        }

        public override void Draw()
        {
            GL.Color4(Color.White);
            GL.PushMatrix();
            {
                GL.Rotate(angle.Y, 0f, 1f, 0f);
                GL.Rotate(angle.Z, 0f, 0f, 1f);
                GL.Translate(Location.X, Location.Y, Location.Z);
                Body.Draw(null, true);
            }
            GL.PopMatrix();
            GL.PushMatrix();
            {
                GL.Translate(.5f, .5f, 0f);
                GL.Color4(Color.White);
                explosion.Draw();
            }
            GL.PopMatrix();
        }
    }
}
