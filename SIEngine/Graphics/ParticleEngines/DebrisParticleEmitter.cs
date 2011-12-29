using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIEngine.BaseGeometry;
using SIEngine.GUI;
using SIEngine.Other;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using Timer = System.Windows.Forms.Timer;
using Color = System.Drawing.Color;
using Object = SIEngine.GUI.Object;

namespace SIEngine.Graphics.ParticleEngines
{
    public class DebrisParticleEmitter : ParticleEmitter
    {
        #region Fields and Properties
        //properties
        public float Scale { get; set; }
        public Vector Gravity { get; set; }
        public float ExplosionDuration { get; set; }
        public Color StartingColor { get; set; }
        public Color EndColor { get; set; }
        protected List<RectangleParticle> Particles { get; set; }

        //fields
        protected float speed = 3f;
        protected Vector particleSize = new Vector(1.5f, 1.5f);
        protected static Texture[] images = new Texture[]
            {
                new Texture("data/img/debris/d1.png"),
                new Texture("data/img/debris/d2.png"),
                new Texture("data/img/debris/d3.png"),
                new Texture("data/img/debris/d4.png"),
                new Texture("data/img/debris/d5.png"),
                new Texture("data/img/debris/d6.png"),
                new Texture("data/img/debris/d7.png"),
                new Texture("data/img/debris/d8.png"),
                new Texture("data/img/debris/d9.png")
            };
        #endregion

        public override void SetInitialValues()
        {
            float z = 0.0f;
            elapsedTime = 0;
            foreach (var particle in Particles)
            {
                //Here we calculate the direction of our particle
                float vx = speed * GeneralMath.RandomFloat(-1.0f, 1.0f);
                float vy = speed * GeneralMath.RandomFloat(0.0f, 1.0f);
                float vz = speed * GeneralMath.RandomFloat(-1.0f, 1.0f);

                //next we should set the required values for the particle to work
                particle.Gravity = Gravity;
                particle.CurrentColor = StartingColor;
                particle.Velocity.X = vx;
                particle.Velocity.Y = vy;
                particle.Velocity.Z = vz;
                particle.Location.X = GeneralMath.RandomFloat(-1.0f, 1.0f);
                particle.Location.Y = GeneralMath.RandomFloat(-1.0f, 1.0f);
                particle.Location.Z = z;
                particle.colorCoef = 0.0f;
                particle.ColorCoefIncrease = GeneralMath.RandomFloat(0.03f, 0.05f);
                
                z += 0.1f;
            }
        }

        public void AnimationStep(object sender, EventArgs evArs)
        {
            if (Paused)
                return;

            elapsedTime++;
            if (elapsedTime * MainTimer.Interval >= ExplosionDuration)
                Pause();

            foreach (var part in Particles)
                part.AnimationStep(MainTimer.Interval * elapsedTime);
        }

        public DebrisParticleEmitter(int numParticles)
        {
            MaxParticleCount = numParticles;
            Particles = new List<RectangleParticle>();
            Gravity = new Vector(0f, -.5f, 0f); ;
            MainTimer = new Timer();
            MainTimer.Tick += AnimationStep;
            MainTimer.Interval = 10;
            Scale = 0.5f;
            ExplosionDuration = 100;
            StartingColor = Color.FromArgb(255, Color.Orange);
            EndColor = Color.FromArgb(255, Color.DarkGray);

            Particles = new List<RectangleParticle>();
            for (int i = 0; i < MaxParticleCount; ++i)
            {
                //here we set the constant values of our particle
                var temp = new RectangleParticle(this);
                temp.Size.X = particleSize.X;
                temp.Size.Y = particleSize.Y;
                temp.AnimationTime = 10;
                temp.TargetColor = EndColor;
                temp.Texture = images[GeneralMath.RandomInt() % images.Length];
                Particles.Add(temp);
            }
        }

        public override void Pause()
        {
            Paused = true;
            MainTimer.Stop();
        }
        public override void Start()
        {
            SetInitialValues();
            Paused = false;
            MainTimer.Start();
        }
        public override void Stop()
        {
        }

        public override void Draw()
        {
            if (Paused)
                return;

            GeneralGraphics.UseDefaultShaderProgram();
            GeneralGraphics.EnableAlphaBlending();
            GeneralGraphics.EnableTexturing();

            GL.MatrixMode(MatrixMode.Modelview);
            GL.PushMatrix();
            {
                GL.Translate(Location.X, Location.Y, Location.Z);
                GL.Scale(Scale, Scale, Scale);
                foreach (var part in Particles)
                    part.Draw();
            }
            GL.PopMatrix();

            GeneralGraphics.DisableTexturing();
            GeneralGraphics.DisableBlending();
        }
    }
}
