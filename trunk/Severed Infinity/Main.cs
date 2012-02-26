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
using SIEngine.Audio;

namespace SI
{
    class EntryPoint
    {
        static void Main(string[] Args)
        {
            //loads audio files
            GeneralAudio.LoadSound("data/audio/hmn.wav", "Include4eto - Hindered No More");
            GeneralAudio.LoadSound("data/audio/layla.wav", "Eric Clapton - Layla");
            BackgroundMusic.AddSongs("Include4eto - Hindered No More", "Eric Clapton - Layla");

            var window = new GameWindow();
            window.BackgroundColor = Color.Wheat;
            Camera.Zoom = -50m;
            Camera.ControlMode = Camera.Mode.Smooth;

            BackgroundMusic.LinkToWindow(window);

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

            var button = new Button();
            button.ButtonEffect.OverShadow = true;
            button.ButtonEffect.BorderEffect = true;
            //button.ApplyStylishEffect();
            
            button.Text = "Play";
            button.Location = new Vector(500, 500);

            button.MouseClick += (pos) =>
                {
                    //Game.Game.StartNextLevel();
                    BackgroundMusic.NextSong();
                };
            window.Children.Add(button);

            window.Run(30);
        }

    }
}