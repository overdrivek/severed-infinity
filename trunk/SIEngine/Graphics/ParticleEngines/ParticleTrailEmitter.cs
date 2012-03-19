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
    public class ParticleTrailEmitter : ParticleEmitter
    {
         #region Fields and Properties
        //properties
        public float Scale { get; set; }
        public Vector Gravity { get; set; }
        public float FadeOutDuration { get; set; }
        public Color MainColor { get; set; }
        public int LaunchInterval { get; set; }
        protected List<RectangleParticle> Particles { get; set; }

        //fields
        protected float speed = 0.05f;
        protected int currentParticle = 0;
        protected Vector particleSize = new Vector(.5f, .5f);
        protected Vector particleSizeIncrease = new Vector(0.01f, 0.01f);
        protected Vector sizeIncreseShift = new Vector(-0.005f, -0.005f, -0.005f);
        protected Texture defaultTexture = new Texture("data/img/trail.png");

        #endregion

        public override void SetInitialValues()
        {
        }

        public void SetInitialValues(params RectangleParticle[] particles)
        {
            elapsedTime = 0;
            foreach (var particle in particles)
            {
                //Here we calculate the direction of our particle
                float vx = speed * GeneralMath.RandomFloat(-1.0f, 1.0f);
                float vy = speed * GeneralMath.RandomFloat(-1.0f, 1.0f);

                //next we should set the required values for the particle to work
                particle.Gravity = Gravity;
                particle.CurrentColor = Color.FromArgb(255, MainColor);
                particle.TargetColor = Color.FromArgb(0, MainColor);
                particle.Velocity.X = vx;
                particle.Velocity.Y = vy;
                particle.Size.X = particleSize.X;
                particle.Size.Y = particleSize.Y;
                particle.Location.X = GeneralMath.RandomFloat(-0.1f, 0.1f) + Location.X;
                particle.Location.Y = GeneralMath.RandomFloat(-0.1f, 0.1f) + Location.Y;
                particle.colorCoef = 0.0f;
                particle.ColorCoefIncrease = 10 / (FadeOutDuration);//GeneralMath.RandomFloat(0.01f, 0.0f);
            }
        }

        public void LaunchParticle()
        {
            currentParticle++;
            if (currentParticle >= MaxParticleCount)
                currentParticle = 0;

            SetInitialValues(Particles[currentParticle]);
        }

        public void AnimationStep(object sender, EventArgs evArgs)
        {
            if (Paused)
                return;

            elapsedTime ++;
            if(elapsedTime > FadeOutDuration)
                elapsedTime = 0;

            if (elapsedTime % LaunchInterval == 0)
                LaunchParticle();

            foreach (var part in Particles)
            {
                if (part.CurrentColor.A <= 10)
                    continue;

                part.Size += particleSizeIncrease;
                part.Location += Gravity;
                part.Location += sizeIncreseShift;
                part.AnimationStep(MainTimer.Interval * elapsedTime);
            }
        }

        public ParticleTrailEmitter(int numParticles, Color mainColor, int launchInterval)
        {
            MainColor = mainColor;

            LaunchInterval = launchInterval;
            MaxParticleCount = numParticles;
            MainTimer = new Timer();
            MainTimer.Tick += AnimationStep;
            MainTimer.Interval = 10;
            Scale = 1f;
            Gravity = new Vector(0.0f, 0.005f, 0.0f);
            FadeOutDuration = 1000;

            Particles = new List<RectangleParticle>();
            float z = 0.0f;
            for (int i = 0; i < MaxParticleCount; ++i)
            {
                //here we set the constant values of our particle
                var temp = new RectangleParticle(this);
                temp.AnimationTime = 10;
                temp.Location.Z = z;
                temp.Texture = defaultTexture;
                Particles.Add(temp);

                z += 0.1f;
            }
        }

        public override void Pause()
        {
            Paused = true;
            MainTimer.Stop();
        }
        public override void Start()
        {
            SetInitialValues(Particles[currentParticle]);
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
                //GL.Translate(Location.X, Location.Y, Location.Z);
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
