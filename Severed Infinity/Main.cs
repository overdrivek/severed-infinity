using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics;
using SIEngine;
using SIEngine.GUI;
using SIEngine.Graphics;
using SIEngine.BaseGeometry;
using SIEngine.Physics;
using System.Threading;
using Vector = SIEngine.BaseGeometry.Vector;
using Button = SIEngine.GUI.Button;
using TextBox = SIEngine.GUI.TextBox;
using Label = SIEngine.GUI.Label;
using Object = SIEngine.GUI.Object;
using MainMenu = SI.GUI.MainMenu;

namespace SI
{
    class EntryPoint
    {
        static void Main (string[] Args)
        {
            GameWindow window = new GameWindow();
            window.BackgroundColor = Color.Wheat;
            Camera.Zoom = -350.0f;
            var bwe = new SI.Effects.ButtonWobbleEffect(new Button());
            window.Menu = new MainMenu(window);

            window.Run(30);
        }

    }
}