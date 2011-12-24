using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIEngine;
using SIEngine.GUI;
using SIEngine.BaseGeometry;
using SIEngine.Graphics;
using SIEngine.Logging;
using SI.Game.Cutscenes;
using Color = System.Drawing.Color;
using Timer = System.Windows.Forms.Timer;

namespace SI.GUI
{
    public class MainMenu
    {
        private Button play, quit, settings, about;
        private Skybox skybox;
        private Credits credits;
        private float curShift = -10.0f, destShift = 1000.0f;
        public GameWindow ParentWindow { get; set; }
        
        public MainMenu(GameWindow window)
        {
            ParentWindow = window;
            skybox = new Skybox();
            ParentWindow.Children3D.Add(skybox);

            play = new Button();
            play.ApplyStylishEffect();
            play.Text = "Play Game";
            play.Image = "data/img/bck.bmp";
            play.MouseClick += () =>
                {
                    IntroScene intro = new IntroScene(ParentWindow);
                    
                    Timer timer = new Timer();
                    timer.Interval = 10;
                    timer.Start();
                    timer.Tick += (o, e) =>
                        {
                            if (curShift < destShift)
                            {
                                curShift += 1.5f;

                                play.Location.X -= curShift;
                                settings.Location.X -= curShift;
                                about.Location.X -= curShift;
                                quit.Location.X -= curShift;
                                return;
                            }
                            timer.Stop();
                            timer.Dispose();
                        };
                };
            play.Location = new Vector(100, 120);

            settings = new Button();
            settings.ApplyStylishEffect();
            settings.Text = "Settings";
            //settings.Size.X = 100;
            settings.Image = "data/img/bck.bmp";
            settings.Location = new Vector(100, 150);

            about = new Button();
            about.ApplyStylishEffect();
            about.Text = "About";
            //about.Size.X = 80;
            about.Image = "data/img/bck.bmp";
            about.Location = new Vector(100, 180);
            about.MouseClick += () =>
                {
                    this.credits.Visible = this.credits.Visible ? false : true;
                };

            quit = new Button();
            quit.ApplyStylishEffect();
            quit.Text = "Quit";
            //quit.Size.X = 60;
            quit.Image = "data/img/bck.bmp";
            quit.Location = new Vector(100, 210);
            quit.MouseClick += () =>
                {
                    Environment.Exit(0);
                };

            credits = new Credits(this);

            ParentWindow.AddChildren(play, settings, about, quit, credits);

            LogManager.WriteInfo("Main menu created.");
        }

        public void RenderVisibility(bool visible)
        {
            play.Visible = visible;
            quit.Visible = visible;
            settings.Visible = visible;
            about.Visible = visible;
        }

        #region animation and effects
        private void EnterAnimation(int frame)
        {

        }
        #endregion
    }
}
