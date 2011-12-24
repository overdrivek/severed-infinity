using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIEngine.BaseGeometry;
using SIEngine.GUI;
using SIEngine.Other;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using Color = System.Drawing.Color;
using Object = SIEngine.GUI.Object;

namespace SIEngine.Graphics.ParticleEngines
{
    public class RectangleParticle : Particle
    {
        public Vector Size { get; set; }
        public float ColorCoefIncrease { get; set; }
        public float colorCoef = .0f;
        public RectangleParticle(ParticleEmitter parent, Vector gravity, Vector location,
            Color currentColor, Color targetColor, Vector velocity, Vector scale, Vector size, string image, int time)
            : base(parent, gravity, location, currentColor, targetColor, velocity, scale, image, time)
        {
            Size = size;
            ColorCoefIncrease = 0.1f;
        }

        public override void AnimationStep(int time)
        {
            if (time % AnimationTime != 0)
                return;

            Velocity += Gravity;
            Location += Velocity;

            ShiftColor(colorCoef);
            colorCoef += ColorCoefIncrease;
        }

        public override void Draw()
        {
            if (CurrentColor.A <= 10)
                return;

            GL.Enable(EnableCap.DepthTest);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.PushMatrix();
            {
                GL.Scale((Vector3)Scale);
                GL.Color4(CurrentColor);
                Texture.SelectTexture();

                GeneralGraphics.DrawRectangle(Location, Size);
            }
            GL.PopMatrix();
        }
    }
}
