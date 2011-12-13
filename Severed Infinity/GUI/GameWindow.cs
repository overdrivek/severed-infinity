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

namespace SI
{
    public class GameWindow : Window
    {
        public MainMenu Menu { get; set; }
        public GameWindow(int width, int height, string title) : base(width, height, title)
        {
        }

        protected override void Draw()
        {
        }
    }
}
