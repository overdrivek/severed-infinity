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
    public abstract class ParticleEmitter : Object
    {
        protected int MaxParticleCount { get; set; }
        protected List<RectangleParticle> Particles { get; set; }
        protected bool Paused { get; set; }
        protected Random Generator { get; set; }

        protected int ElapsedTime { get; set; }
        protected Timer MainTimer { get; set; }
        public int TimerIterval
        {
            set
            {
                if (MainTimer == null)
                    return;
                MainTimer.Interval = value;
            }
            get
            {
                return MainTimer.Interval;
            }
        }

        public abstract void Revert();
    }
}
