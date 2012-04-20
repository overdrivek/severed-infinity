using System;
using System.Linq;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;
using SIEngine.GUI;
using System.Drawing;
using System.Drawing.Imaging;
using SIEngine.BaseGeometry;
using System.IO;
using System.Text;
using OpenTK;
using SI.GUI;
using SI.Properties;
using SI.Other;
using SIEngine.Logging;
using SIEngine.Graphics;

namespace SI
{
    public class GameWindow : Window
    {
        public MainMenu Menu { get; set; }
        public IngameMenu GameMenu { get; set; }
        public WindowState State { get; set; }
        public bool MouseClicked { get; set; }
        private Skybox skybox;

        public GameWindow()
        {
            int width = Settings.Default.ResolutionX;
            int height = Settings.Default.ResolutionY;
            Initialize(width, height, GameplayConstants.WindowName);

            LogManager.WriteInfo("Main window created successfully.");

            Mouse.ButtonDown += (o, e) => MouseClicked = true;
            Mouse.ButtonUp += (o, e) => MouseClicked = false;

            skybox = new Skybox();
            Children3D.Add(skybox);
        }
        
        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            base.OnKeyPress(e);
            if (e.KeyChar == 27)
            {
                if (GameMenu != null)
                {
                    if (State == WindowState.Game)
                        GameMenu.Show();
                    else if (State == WindowState.InGameMenu)
                    {
                        GameMenu.Hide();
                        State = WindowState.Game;
                    }
                }
            }
        }
    }
}
