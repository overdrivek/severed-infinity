using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIEngine;
using SIEngine.GUI;
using SIEngine.BaseGeometry;
using SIEngine.Graphics;

namespace HW
{
    class MainMenu
    {
        private Button start, quit, settings, about;
        private Skybox skybox;
        private Window ParentWindow { get; set; }
        
        public MainMenu(Window window)
        {
            ParentWindow = window;
            skybox = new Skybox();

            start = new Button();
            start.ApplyStylishEffect();
            //start.Location = new Vector()
        }

        public void RenderVisibility(bool visible)
        {
            start.Visible = visible;
            quit.Visible = visible;
            settings.Visible = visible;
            about.Visible = visible;
        }
    }
}
