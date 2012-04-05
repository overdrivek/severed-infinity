using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIEngine.GUI;
using SI.Game;
using SI.GUI;
using SIEngine.BaseGeometry;
using System.Drawing;
using Timer = System.Windows.Forms.Timer;

namespace SI.GUI
{
    public class PlayGameMenu : GUIObject
    {
        private Button resumeGameButton, newGameButton, backButton;
        private Timer mainTimer;
        private float curShift = -10.0f, destShift = 100.0f, shiftIncr = 1.5f;
        private bool canResume;

        public MainMenu MenuParent { get; private set; }
        public GameWindow Parent { get; private set; }

        public PlayGameMenu(MainMenu menuParent)
        {
            MenuParent = menuParent;
            Parent = menuParent.ParentWindow;

            resumeGameButton = new Button();
            resumeGameButton.Text = "Resume Game";
            resumeGameButton.Location = new Vector(255, 0);
            resumeGameButton.ApplyStylishEffect();
            resumeGameButton.Size.X -= 10;
            resumeGameButton.Image = "data/img/bck.bmp";

            resumeGameButton.MouseClick += (pos) =>
                {
                    Game.Game.ResumeGame();
                    Hide();
                    MenuParent.HideMenu();
                };

            newGameButton = new Button();
            newGameButton.Text = "New Game";
            newGameButton.Location = new Vector(250, 0);
            newGameButton.ApplyStylishEffect();
            newGameButton.Image = "data/img/bck.bmp";
            newGameButton.MouseClick += (pos) =>
                {
                    Game.Game.NewGame();
                    Hide();
                    MenuParent.HideMenu();
                };

            backButton = new Button();
            backButton.Text = "Back";
            backButton.Location = new Vector(265, 0);
            backButton.ApplyStylishEffect();
            backButton.Image = "data/img/bck.bmp";
            backButton.Size.X -= 30;
            backButton.MouseClick += (pos) => Hide();

            mainTimer = new Timer();
            mainTimer.Interval = 10;
            mainTimer.Tick += (o, e) =>
                {
                    if (curShift < destShift)
                    {
                        curShift += shiftIncr;

                        resumeGameButton.Location.Y -= curShift;
                        newGameButton.Location.Y -= curShift;
                        backButton.Location.Y -= curShift;

                        return;
                    }
                    mainTimer.Stop();
                };

            resumeGameButton.Location.Y = -250.5f;
            newGameButton.Location.Y = -220.5f;
            backButton.Location.Y = -1900.5f;
            Parent.AddChildren(resumeGameButton, newGameButton, backButton);

            canResume = Game.Game.CheckProgress();
            if (!canResume)
            {
                Parent.Children.Remove(resumeGameButton);
                Parent.Children.Add(this);
            }
        }

        public override void Draw()
        {
            resumeGameButton.Draw();
        }

        public void Show()
        {
            resumeGameButton.Location.Y = -220.5f;
            newGameButton.Location.Y = -250.5f;
            backButton.Location.Y = -190.5f;

            curShift = -35f;
            shiftIncr = 1.5f;
            destShift = 6.0f;

            mainTimer.Start();

            bool oldResume = Game.Game.CheckProgress();
            if (oldResume != canResume)
            {
                canResume = oldResume;
                if (canResume)
                {
                    Parent.Children.Remove(this);
                    Parent.Children.Add(resumeGameButton);
                }
                else
                {
                    Parent.Children.Remove(resumeGameButton);
                    Parent.Children.Add(this);
                }
            }
        }

        public void Hide()
        {
            curShift = -9;
            destShift = 50;
            shiftIncr = 1.5f;

            mainTimer.Start();  
        }
    }
}
