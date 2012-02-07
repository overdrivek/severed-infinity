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
        static float logn(float x)
        {
            return (float)Math.Log(0.2, (x - 3 * x * x));
        }

        static void Main(string[] Args)
        {
            var window = new GameWindow();
            window.BackgroundColor = Color.Wheat;
            Game.Game.MainWindow = window;
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

            var button = new Button();
            button.Text = "Unlock";
            button.Location = new Vector(50, 20);
            button.ApplyStylishEffect();
            button.Image = "data/img/bck.bmp";
            button.MouseClick += (pos) =>
                {
                    //ModelManager.UnlockModel(window);

                    new Level(window, 30, 600).Start();
                };

            OBJModel model = new OBJModel("data/models/apple/apple.obj");
            model.ScaleFactor = 0.03f;
            Object obj = new Object();
            obj.Body = model;
            obj.Location = new Vector(-10, -10);
            //window.Add3DChildren(obj);
            model.CalculateReach();

            Gun gun = new Gun();
            window.Add3DChildren(gun);

            //window.AddChildren(button);

            window.Run(30);
        }

    }
}