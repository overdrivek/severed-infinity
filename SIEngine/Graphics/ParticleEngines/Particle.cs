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
    /// <summary>
    /// This particle doesn't have a defined shape, size, color,
    /// mass, etc. It's the base for all other particle classes.
    /// </summary>
    public abstract class Particle
    {
        protected ParticleEmitter Parent { get; set; }
        public Vector Velocity { get; set; }
        public Vector Gravity { get; set; }
        public Vector Location { get; set; }
        public Color CurrentColor { get; set; }
        public Color TargetColor { get; set; }
        public Vector Scale { get; set; }
        public int AnimationTime { get; set; }

        public Texture Texture { get; set; }
        public string Image
        {
            set
            {
                if (value == null)
                    return;
                Texture = new Texture(value);
            }
        }

        public Particle(ParticleEmitter parent, Vector gravity, Vector location, 
            Color currentColor, Color targetColor, Vector velocity, Vector scale, string image, int time)
        {
            Parent = parent;
            Gravity = gravity;
            Location = location;
            CurrentColor = currentColor;
            TargetColor = targetColor;
            Velocity = velocity;
            Scale = scale;
            Image = image;
            AnimationTime = time;
        }
        public Particle()
        {
            Location = new Vector(0.0f, 0.0f, 0.0f);
            Velocity = new Vector(0.0f, 0.0f, 0.0f);
        }

        public void ShiftColor(float coef)
        {
            int r = GeneralMath.Interpolate(CurrentColor.R, TargetColor.R, coef);
            int g = GeneralMath.Interpolate(CurrentColor.G, TargetColor.G, coef);
            int b = GeneralMath.Interpolate(CurrentColor.B, TargetColor.B, coef);
            int a = GeneralMath.Interpolate(CurrentColor.A, TargetColor.A, coef);

            if (a < 0)
                return;

            CurrentColor = Color.FromArgb(a, r, g, b);
        }
        public abstract void Draw();
        public abstract void AnimationStep(int time);
    }
}
