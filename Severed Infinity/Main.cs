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
using SI.Properties;
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
            GeneralAudio.LoadSound("data/audio/exp/1.wav", "1");
            GeneralAudio.LoadSound("data/audio/exp/2.wav", "2");
            GeneralAudio.LoadSound("data/audio/exp/3.wav", "3");
            GeneralAudio.LoadSound("data/audio/exp/4.wav", "4");
            GeneralAudio.LoadSound("data/audio/exp/5.wav", "5");
            GeneralAudio.LoadSound("data/audio/exp/6.wav", "6");
            GeneralAudio.LoadSound("data/audio/exp/7.wav", "7");
            GeneralAudio.LoadSound("data/audio/exp.wav", "8");
            BackgroundMusic.AddSongs("Include4eto - Hindered No More", "Eric Clapton - Layla");

            if (Settings.Default.MusicStatus)
                BackgroundMusic.StartPlayback();
            
            var window = new GameWindow();
            window.BackgroundColor = Color.Wheat;
            Camera.Zoom = -50m;
            Camera.ControlMode = Camera.Mode.Smooth;

            BackgroundMusic.LinkToWindow(window);
            window.Menu = new MainMenu(window);

            if (Settings.Default.unlockStatus == null || Settings.Default.itemsShot == null)
                Game.Game.DeleteProgress();

            window.GameMenu = new IngameMenu(window);
            Game.Game.MainWindow = window;
            Game.Game.InitializeGame();

            window.Run(30);
        }

    }
}