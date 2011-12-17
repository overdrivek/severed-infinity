using System;
using System.Linq;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;
using SIEngine.GUI;
using System.Drawing;
using System.Drawing.Imaging;
using SIEngine.BaseGeometry;
using System.IO;
using System.Text;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using SI.GUI;
using SI.Properties;
using SI.Other;

namespace SI
{
    public class GameWindow : Window
    {
        public MainMenu Menu { get; set; }

        public GameWindow()
        {
            int width = Settings.Default.ResolutionX;
            int height = Settings.Default.ResolutionY;
            Initialize(width, height, GameplayConstants.WindowName);
        }

        protected override void Draw()
        {
        }
    }
}
