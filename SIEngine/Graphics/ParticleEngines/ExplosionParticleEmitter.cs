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
    public class ExplosionParticleEmitter : ParticleEmitter
    {

        public ExplosionParticleEmitter(int numParticle)
        {
            MaxParticleCount = numParticle;
            Generator = new Random();
            Particles = new List<RectangleParticle>();

            Revert();
            MainTimer = new Timer();
            MainTimer.Interval = 10;
            MainTimer.Start();
            MainTimer.Tick += (o, e) =>
            {
                ElapsedTime++;
                foreach (var part in Particles)
                {
                    if (ElapsedTime % 7 == 0)
                    {
                        part.TargetColor = Color.FromArgb(0, Color.Brown);
                        part.ColorCoefIncrease = 0.01f;
                        part.Velocity = new Vector(part.Velocity.X * 0.1f, 0.001f, 0.0f);
                    }
                    else
                    {
                        part.Size += new Vector(1.0f, 1.0f);
                        part.Location += new Vector(-0.5f, -0.5f);
                    }
                    part.AnimationStep(MainTimer.Interval * ElapsedTime);
                }
            };

        }

        public float RandomFloat(float min, float max)
        {
            return min + (float)Generator.NextDouble() * (max - min);
        }

        public override void Revert()
        {
            Particles.Clear();
            Vector size = new Vector(0.5f, 0.5f);
            Vector gravity = new Vector(0.0f, 0.0f, 0.0f);
            float speed = 0.55f;

            float z = 0.01f;
            for (int i = 0; i < MaxParticleCount; ++i)
            {
                float x = speed * RandomFloat(-1.0f, 1.0f);
                float y = speed * RandomFloat(-1.0f, 1.0f);

                var particle = new RectangleParticle((ParticleEmitter)this, gravity, 
                    new Vector(RandomFloat(-1.0f, 1.0f), RandomFloat(-1.0f, 1.0f), z),
                    Color.FromArgb(255, Color.Orange),
                    Color.FromArgb(128, Color.Red),
                    new Vector(x, y, 0.0f),
                    new Vector(1.0f, 1.0f, 1.0f), size, "data/img/exp1.png", 10);
                particle.ColorCoefIncrease = RandomFloat(0.01f, 0.05f);
                particle.colorCoef = 0.0f;
                z += 0.01f;

                Particles.Add(particle);
            }
            //Particles.Reverse();
            ElapsedTime = 0;

        }

        public override void Draw()
        {
            GeneralGraphics.UseDefaultShaderProgram();
            GeneralGraphics.EnableAlphaBlending();
            GeneralGraphics.EnableTexturing();

            GL.MatrixMode(MatrixMode.Modelview);
            GL.PushMatrix();
            {
                GL.Scale(0.2f, 0.2f, 0.2f);
                foreach (var part in Particles)
                    part.Draw();
            }
            GL.PopMatrix();

            GeneralGraphics.DisableTexturing();
            GeneralGraphics.DisableBlending();
        }
    }
}
