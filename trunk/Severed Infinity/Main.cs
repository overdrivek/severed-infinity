using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using SIEngine;
using SIEngine.GUI;
using SIEngine.Graphics;
using SIEngine.BaseGeometry;
using SIEngine.Physics;
using System.Threading;
using SI.Game;
using Vector = SIEngine.BaseGeometry.Vector;
using Button = SIEngine.GUI.Button;
using TextBox = SIEngine.GUI.TextBox;
using Label = SIEngine.GUI.Label;
using Object = SIEngine.GUI.Object;
using MainMenu = SI.GUI.MainMenu;
using IngameMenu = SI.GUI.IngameMenu;

//temp
using SI.Game.Cutscenes;
using SIEngine.Graphics.Shaders;
using SIEngine.Graphics.ParticleEngines;
using SIEngine.Input;
using Key = OpenTK.Input.Key;
using SIEngine.Graphics.Rendering;
using SIEngine.Other;

namespace SI
{
    class EntryPoint
    {
        static void Main(string[] Args)
        {
            var window = new GameWindow();
            window.BackgroundColor = Color.Wheat;
            Camera.Zoom = -50m;
            Camera.ControlMode = Camera.Mode.Smooth;
            
            //if (Properties.Settings.Default.unlockStatus == null)
            {
                Properties.Settings.Default.unlockStatus = new bool[1 << 5];
                for (int i = 0; i < Properties.Settings.Default.unlockStatus.Length; ++ i)
                    Properties.Settings.Default.unlockStatus[i] = false;
                Properties.Settings.Default.unlockStatus[0] = true;
                Console.WriteLine("crap");

                Properties.Settings.Default.Save();
            }

            window.Menu = new MainMenu(window);
            window.GameMenu = new IngameMenu(window);
            Game.Game.MainWindow = window;
            Game.Game.InitializeGame();

            window.Run(30);
        }

    }
}