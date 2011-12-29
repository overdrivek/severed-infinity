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
using System.Windows.Forms;
using Button = SIEngine.GUI.Button;
using Label = SIEngine.GUI.Label;
using Object = SIEngine.GUI.Object;

namespace SI.Game
{
    public class ObjectShooter
    {
        private Timer MainTimer { get; set; }

        void AnimationStep()
        {

        }

        public void Shoot()
        {

        }

        public ObjectShooter()
        {
            MainTimer = new Timer();
            MainTimer.Interval = 10;

        }
    }
}
