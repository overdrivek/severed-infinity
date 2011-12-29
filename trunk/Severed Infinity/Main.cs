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
using IngameMenu = SI.GUI.IngameMenu;

//temp
using SI.Game.Cutscenes;
using SIEngine.Graphics.Shaders;
using SIEngine.Graphics.ParticleEngines;
using SI.Game;
using Key = OpenTK.Input.Key;

namespace SI
{
    class EntryPoint
    {
        static void Main(string[] Args)
        {
            GameWindow window = new GameWindow();
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
            smoke.Location = new Vector(-10f, 0f, -40f);

            FlyingObject fo = new FlyingObject("data/models/apple/apple.obj",
                0.05f, 200);

            Button reset = new Button();
            reset.Text = "Reset";
            reset.Location = new Vector(20.0f, 10.0f);
            reset.ApplyStylishEffect();
            reset.MouseClick += () =>
            {
                //exp.Start();
                //wave.Start();
                //deb.Start();
                //smoke.Start();
                fo.Location = new Vector(-10f, -15f, -40f);
                fo.Start();
            };

            window.Children.Add(reset);
            window.Add3DChildren(fo, smoke, exp, deb, wave);
    
            window.Run(30);
        }

    }
}