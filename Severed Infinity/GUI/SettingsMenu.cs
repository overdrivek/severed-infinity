using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIEngine.BaseGeometry;
using SIEngine.GUI;
using SIEngine.Audio;
using SI.Properties;
using Timer = System.Windows.Forms.Timer;

namespace SI.GUI
{
    class SettingsMenu : GUIObject
    {
        private bool soundStatus, musicStatus;
        
        private Label toggleMusicLabel, toggleSoundLabel, titleLabel;
        private Button toggleMusic, toggleSound;
        private Button deleteSaveGames, saveButton;
        public MainMenu ParentMenu { get; private set; }
        private Timer mainTimer;
        
        private float curShift = -10.0f, destShift = 100.0f, shiftIncr = 1.5f;
        private int elapsed = 0;
        public bool Visible
        {
            get
            {
                return base.Visible;
            }
            set
            {
                base.Visible = value;
                toggleSound.Visible = value;
                toggleSoundLabel.Visible = value;
                toggleMusic.Visible = value;
                toggleMusicLabel.Visible = value;
                saveButton.Visible = value;
            }
        }
        
        public SettingsMenu(MainMenu MenuParent)
        {
            Parent = MenuParent.ParentWindow;
            ParentMenu = MenuParent;
            soundStatus = Properties.Settings.Default.SoundStatus;
            musicStatus = Properties.Settings.Default.MusicStatus;

            mainTimer = new Timer();
            mainTimer.Interval = 10;

            titleLabel = new Label();
            titleLabel.Text = "Settings";
            titleLabel.Location = new Vector(100, -250.5f);

            toggleSoundLabel = new Label();
            toggleSoundLabel.Location = new Vector(130, -220.5f);
            toggleSoundLabel.Text = "Sound: " + (soundStatus ? "On " : "Off");
            
            toggleSound = new Button();
            toggleSound.Location = new Vector(270, -220.5f);
            toggleSound.ApplyStylishEffect();
            toggleSound.Size = new Vector(70, 20);
            toggleSound.Image = "data/img/bck.bmp";
            toggleSound.Text = "Toggle";
            toggleSound.MouseClick += (pos) => 
                {
                    soundStatus = soundStatus ? false : true;
                    toggleSoundLabel.Text = "Sound: " + (soundStatus ? "On " : "Off");
                };

            saveButton = new Button();
            saveButton.Location = new Vector(200, -30);
            saveButton.Image = "data/img/bck.bmp";
            saveButton.ApplyStylishEffect();
            saveButton.Text = "Save";
            saveButton.MouseClick += (pos) => SaveAndHide();

            toggleMusicLabel = new Label();
            toggleMusicLabel.Location = new Vector(130, -180.5f);
            toggleMusicLabel.Text = "Music: " + (musicStatus ? "On " : "Off");

            toggleMusic = new Button();
            toggleMusic.Location = new Vector(270, -180.5f);
            toggleMusic.ApplyStylishEffect();
            toggleMusic.Size = new Vector(70, 20);
            toggleMusic.Image = "data/img/bck.bmp";
            toggleMusic.Text = "Toggle";
            toggleMusic.MouseClick += (pos) =>
                {
                    musicStatus = musicStatus ? false : true;
                    toggleMusicLabel.Text = "Music: " + (musicStatus ? "On " : "Off");
                };

            deleteSaveGames = new Button();
            deleteSaveGames.Location = new Vector(150, -120.5f);
            deleteSaveGames.ApplyStylishEffect();
            deleteSaveGames.Image = "data/img/bck.bmp";
            deleteSaveGames.Text = "Delete saved games";
            deleteSaveGames.MouseClick += (pos) =>
                {
                    Game.Game.DeleteProgress(false);
                    Game.Game.LoadGame();
                    SaveAndHide();
                };
            deleteSaveGames.Size = new Vector(160, 25);

            Parent.AddChildren(toggleSound, saveButton, toggleMusicLabel, toggleMusic, deleteSaveGames);

            mainTimer.Tick += (o, e) =>
            {
                //lazy delay implementation
                //TODO: maybe fix
                elapsed++;
                if (elapsed < 10)
                    return;

                if (curShift < destShift)
                {
                    curShift += shiftIncr;

                    titleLabel.Location.Y -= curShift;
                    toggleSoundLabel.Location.Y -= curShift;
                    toggleSound.Location.Y -= curShift;
                    saveButton.Location.Y -= curShift;
                    toggleMusicLabel.Location.Y -= curShift;
                    toggleMusic.Location.Y -= curShift;
                    deleteSaveGames.Location.Y -= curShift;

                    return;
                }
                mainTimer.Stop();
            };
        }

        public void DoAnimation()
        {
            elapsed = 0;
            mainTimer.Start();
        }

        public void SaveAndHide()
        {
            //starts the sound loop if we chose so
            if (musicStatus != Settings.Default.MusicStatus)
                if(musicStatus)
                    BackgroundMusic.StartPlayback();
                else BackgroundMusic.StopPlayback();

            Settings.Default.SoundStatus = soundStatus;
            Settings.Default.MusicStatus = musicStatus;
            Settings.Default.Save();

            curShift = 20f;
            destShift = 50f;
            shiftIncr = 1.5f;
            DoAnimation();

            ParentMenu.RenderVisibility(true);
        }

        public void Show()
        {
            ParentMenu.HideMenu();

            titleLabel.Location.Y = -250.5f;
            toggleSoundLabel.Location.Y = -220.5f;
            toggleSound.Location.Y = -220.5f;
            saveButton.Location.Y = -30;
            toggleMusic.Location.Y = -180.5f;
            toggleMusicLabel.Location.Y = -180.5f;
            deleteSaveGames.Location.Y = -120.5f;

            curShift = -32f;
            shiftIncr = 1.5f;
            destShift = -2.0f;
            DoAnimation();
        }

        public override void Draw()
        {
            if (!Visible)
                return;

            titleLabel.Draw();
            toggleSoundLabel.Draw();
            toggleSound.Draw();
        }
    }
}
