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
    public class ShockwaveParticleEmitter : ParticleEmitter
    {
        #region Fields and Properties
        //properties
        public float Scale { get; set; }
        public float ExplosionDuration { get; set; }
        public Color StartingColor { get; set; }
        public float RotationAngle { get; set; }

        //fields
        protected Vector particleSize = new Vector(1.5f, 1.5f);
        protected Texture defaultTexture = new Texture("data/img/ring.png");
        protected Vector particleSizeIncrease = new Vector(5.0f, 5.0f);
        protected Vector sizeIncreseShift = new Vector(-2.5f, -2.5f, 0.0f);
        protected Vector Position { get; set; }
        protected Color CurrentColor { get; set; }
        #endregion

        public override void SetInitialValues()
        {
            Position.X = 0.5f;
            Position.Y = 0.0f;
            Position.Z = 1.0f;
            CurrentColor = StartingColor;
            Size.X = particleSize.X;
            Size.Y = particleSize.Y;
            elapsedTime = 0;
        }

        public void AnimationStep(object sender, EventArgs evArgs)
        {
            if (Paused)
                return;

            if (elapsedTime * MainTimer.Interval >= ExplosionDuration)
                Pause();
            elapsedTime++;

            Size += particleSizeIncrease;
            if (CurrentColor.A >= 30)
                CurrentColor = Color.FromArgb(CurrentColor.A - 30, CurrentColor);
            else CurrentColor = Color.FromArgb(0, 0, 0, 0);
            Position += sizeIncreseShift;
        }

        public ShockwaveParticleEmitter()
        {
            Position = new Vector(0f, 0f);
            ExplosionDuration = 100;
            MainTimer = new Timer();
            MainTimer.Interval = 10;
            MainTimer.Tick += AnimationStep;
            Scale = 0.5f;
            RotationAngle = 100f;
            StartingColor = Color.OrangeRed;
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
                GL.Rotate(RotationAngle, 1.0f, 0.0f, 0.0f);
                defaultTexture.SelectTexture();
                GL.Color4(CurrentColor);

                GeneralGraphics.DrawRectangle(Position, Size);
            }
            GL.PopMatrix();

            GeneralGraphics.DisableTexturing();
            GeneralGraphics.DisableBlending();
        }
    }
}
