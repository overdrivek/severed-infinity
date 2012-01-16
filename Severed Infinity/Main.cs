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
using SI.Game;
using SIEngine.Input;
using Key = OpenTK.Input.Key;
using System.Threading;
using SIEngine.Graphics.Rendering;

namespace SI
{
    class EntryPoint
    {
        static void Main(string[] Args)
        {
            //safety hack
            if (Properties.Settings.Default.UnlockedObjects == null)
            {
                Properties.Settings.Default.UnlockedObjects = new System.Collections.Specialized.StringCollection();
                Properties.Settings.Default.Save();
            }
            
            var window = new GameWindow();
            window.BackgroundColor = Color.Wheat;
            Camera.Zoom = -50m;
            Camera.ControlMode = Camera.Mode.Smooth;

            window.Menu = new MainMenu(window);
            window.GameMenu = new IngameMenu(window);

            var exp = new ExplosionParticleEmitter(16);
            var wave = new ShockwaveParticleEmitter();
            var deb = new DebrisParticleEmitter(16);
            var smoke = new SmokeParticleEmitter(16);

            exp.Location = new Vector(-10f, 0f, -40f);
            wave.Location = new Vector(-10f, 0f, -40f);
            deb.Location = new Vector(-10f, 0f, -40f);

            //FlyingObject fo = new FlyingObject("data/models/knight/apple.obj",
            //    0.05f, 3);
            //fo.Location = new Vector(-10, -14, -40);

            OBJModel model = new OBJModel("data/models/apple/apple.obj");
            model.ScaleFactor = 0.1f;
            Object obj = new Object();
            obj.Location = new Vector(10f, 0f, 20f);
            obj.Body = model;

            Button reset = new Button();
            reset.Text = "Reset";
            reset.Location = new Vector(20.0f, 10.0f);
            reset.ApplyStylishEffect();

            bool rot = false;
            reset.MouseClick += (pos) =>
            {
                //exp.Start();
                //wave.Start();
                //deb.Start();
                
                //new InfoBox(window, new Vector(600f, 10f), "asdf").Show();
                //var fobj = new FlyingObject(window, model, 5);
                //fobj.Location = new Vector(0f, -20f, 0f);

                //fobj.Start();

                if (!rot)
                    Camera.RotateAround(new DecimalVector(10m, 0m, 20m));
                else Camera.StopRotation();
                rot = rot ? false : true;
            };


            //window.Children.Add(reset);
            //window.Children3D.Add(obj);

            window.Run(30);
        }

    }
}