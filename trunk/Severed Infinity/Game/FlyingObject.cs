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
using SIEngine.Physics;
using SIEngine.Graphics.ParticleEngines;
using System.Windows.Forms;
using Button = SIEngine.GUI.Button;
using Label = SIEngine.GUI.Label;
using Object = SIEngine.GUI.Object;

namespace SI.Game
{
    public class FlyingObject : Object
    {
        public bool Paused
        {
            set
            {
                if (value)
                    MainTimer.Start();
                else MainTimer.Stop();
            }
        }
        public PhysicsObject PhysicalBody { get; set; }
        public OBJModel GraphicalBody { get; set; }
        public Timer MainTimer { get; set; }
        private List<SmokeParticleEmitter> Smoke { get; set; }
        public int MaxEmitters { get; set; }

        private int currentEmitter = 0;
        private int elapsedTime = 0;

        public FlyingObject(string modelPath, float scale, int maxEmitters)
        {
            MaxEmitters = maxEmitters;

            PhysicalBody = new PhysicsObject();
            PhysicalBody.ParentObject = this;

            GraphicalBody = new OBJModel();
            GraphicalBody.ParseOBJFile(modelPath);
            GraphicalBody.ScaleFactor = scale;

            Smoke = new List<SmokeParticleEmitter>();
            for (int i = 0; i < MaxEmitters; ++i)
                Smoke.Add(new SmokeParticleEmitter(16));
            
            Location = new Vector(0.0f, 0.0f, 0.0f);
            MainTimer = new Timer();
            MainTimer.Interval = 10;
            MainTimer.Tick += AnimationStep;
        }

        public void Start()
        {
            PhysicalBody.Velocity = new Vector(GeneralMath.RandomFloat(-1.5f, 1.5f),
                GeneralMath.RandomFloat(1.5f, 3.5f), GeneralMath.RandomFloat(-0.5f, 0.5f));
            MainTimer.Start();

        }

        private void AnimationStep(object sedner, EventArgs evArgs)
        {
            PhysicalBody.ApplyNaturalForces();
            PhysicalBody.ModulatePhysics();

            //if (elapsedTime % 5 == 0)
            {
                //elapsedTime = 0;
                if (currentEmitter >= MaxEmitters)
                    currentEmitter = 0;

                Smoke[currentEmitter].Location.X = Location.X;
                Smoke[currentEmitter].Location.Y = Location.Y + 0.5f;
                Smoke[currentEmitter].Location.Z = Location.Z - 1.0f;

                Smoke[currentEmitter++].Start();
            }

        }

        public override void Draw()
        {
            foreach (var emitter in Smoke)
                emitter.Draw();

            GL.PushMatrix();
            {
                GL.Translate(Location.X, Location.Y, Location.Z);
                GraphicalBody.Draw();
            }
            GL.PopMatrix();
            
        }
    }
}
