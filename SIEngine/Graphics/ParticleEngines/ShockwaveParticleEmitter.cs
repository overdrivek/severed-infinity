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
        Texture ShockWave = new Texture("data/img/ring.png");
        Vector Position;
        Color CurrentColor;

        public override void Revert()
        {
            Position = new Vector(0.5f, 3.0f, 2.0f);
            CurrentColor = Color.Orange;
            Size = new Vector(1.0f, 1.0f);
        }

        public ShockwaveParticleEmitter()
        {
            MainTimer = new Timer();
            MainTimer.Interval = 10;
            MainTimer.Start();
            MainTimer.Tick += (o, e) =>
                {
                    Size += new Vector(5.0f, 5.0f);
                    if (CurrentColor.A >= 30)
                        CurrentColor = Color.FromArgb(CurrentColor.A - 30, CurrentColor);
                    else CurrentColor = Color.FromArgb(0, 0, 0, 0);
                    Position += new Vector(-2.5f, -2.5f, 0.0f);
                };
            Revert();
        }

        public override void Draw()
        {
            if (CurrentColor.A <= 10)
                return;

            GeneralGraphics.UseDefaultShaderProgram();
            GeneralGraphics.EnableAlphaBlending();
            GeneralGraphics.EnableTexturing();

            GL.MatrixMode(MatrixMode.Modelview);
            GL.PushMatrix();
            {
                GL.Scale(0.2f, 0.2f, 0.2f);
                GL.Rotate(100.0f, 1.0f, 0.0f, 0.0f);
                ShockWave.SelectTexture();
                GL.Color4(CurrentColor);

                GeneralGraphics.DrawRectangle(Position, Size);
            }
            GL.PopMatrix();

            GeneralGraphics.DisableTexturing();
            GeneralGraphics.DisableBlending();
        }
    }
}
