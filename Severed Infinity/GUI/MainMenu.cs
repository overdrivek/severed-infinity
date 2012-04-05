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
        private Button play, quit, settings, about, highScores;
        private Skybox skybox;
        private Credits credits;
        private SettingsMenu settingsMenu;
        private PlayGameMenu playGameMenu;
        private float curShift = -10.0f, destShift = 100.0f, shiftIncr = 1.5f;
        public GameWindow ParentWindow { get; set; }

        public MainMenu(GameWindow window)
        {
            ParentWindow = window;
            skybox = new Skybox();
            ParentWindow.Children3D.Add(skybox);
            playGameMenu = new PlayGameMenu(this);

            play = new Button();
            play.ApplyStylishEffect();
            play.Text = "Play Game";
            play.Image = "data/img/bck.bmp";
            play.MouseClick += (pos) =>
                {
                    HideAllVisible();
                    playGameMenu.Show();
                };
            play.Location = new Vector(100, 120);

            settings = new Button();
            settings.ApplyStylishEffect();
            settings.Text = "Settings";
            //settings.Size.X = 100;
            settings.Image = "data/img/bck.bmp";
            settings.Location = new Vector(100, 150);
            settings.MouseClick += (pos) =>
                {
                    HideAllVisible();
                    settingsMenu.Visible = true;
                    settingsMenu.Show();
                };

            about = new Button();
            about.ApplyStylishEffect();
            about.Text = "About";
            //about.Size.X = 80;
            about.Image = "data/img/bck.bmp";
            about.Location = new Vector(100, 180);
            about.MouseClick += (pos) =>
                {
                    if (!credits.Visible) HideAllVisible();
                    credits.Visible = credits.Visible ? false : true;
                };

            highScores = new Button();
            highScores.ApplyStylishEffect();
            highScores.Text = "High Scores";
            highScores.Image = "data/img/bck.bmp";
            highScores.Location = new Vector(100, 210);
            highScores.MouseClick += (pos) =>
                {
                    HideAllVisible();
                    Game.Game.ShowScores();
                };

            quit = new Button();
            quit.ApplyStylishEffect();
            quit.Text = "Quit";
            //quit.Size.X = 60;
            quit.Image = "data/img/bck.bmp";
            quit.Location = new Vector(100, 240);
            quit.MouseClick += (pos) =>
                {
                    Environment.Exit(0);
                };

            credits = new Credits(this);
            credits.Visible = false;
            settingsMenu = new SettingsMenu(this);

            mainTimer.Tick += (o, e) =>
            {
                if (curShift < destShift)
                {
                    curShift += shiftIncr;

                    play.Location.X -= curShift;
                    settings.Location.X -= curShift;
                    about.Location.X -= curShift;
                    quit.Location.X -= curShift;
                    highScores.Location.X -= curShift;
                    return;
                }
                mainTimer.Stop();
            };

            ParentWindow.State = Window.WindowState.MainMenu;
            ParentWindow.AddChildren(play, settings, about, quit, credits, settingsMenu, highScores);

            LogManager.WriteInfo("Main menu created.");
        }

        /// <summary>
        /// Hides all visible subitems of the menu.
        /// </summary>
        private void HideAllVisible()
        {
            if (credits.Visible)
                credits.Visible = false;
            if (playGameMenu.Visible)
                playGameMenu.Hide();
            if (Game.Game.ShowingScores)
                Game.Game.HideScores();
        }

        /// <summary>
        /// Hides menu using the cool animation. To bring
        /// back use RenderVisibility.
        /// </summary>
        public void HideMenu()
        {
            curShift = -12.0f;
            destShift = 30.0f;
            shiftIncr = 1.5f;
            DoAnimation();
        }

        /// <summary>
        /// Shows the menu with cool animation. Hide instantly if
        /// visible is false.
        /// </summary>
        /// <param name="visible">True for cool animation.</param>
        public void RenderVisibility(bool visible)
        {
            if (visible)
            {
                ParentWindow.State = Window.WindowState.MainMenu;

                curShift = -30f;
                shiftIncr = 1.5f;
                destShift = -2.0f;
                DoAnimation();

                //i'm lazy
                play.Location.X = -185;
                settings.Location.X = -185;
                about.Location.X = -185;
                quit.Location.X = -185;
                highScores.Location.X = -185;
            }
            play.Visible = visible;
            quit.Visible = visible;
            settings.Visible = visible;
            about.Visible = visible;
            highScores.Visible = visible;
        }

        Timer mainTimer = new Timer();
        public void DoAnimation()
        {
            mainTimer.Interval = 10;
            mainTimer.Start();
        }
    }
}
