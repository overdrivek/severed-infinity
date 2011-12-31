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
using OpenTK.Graphics.OpenGL;
using Color = System.Drawing.Color;
using Timer = System.Windows.Forms.Timer;

namespace SI.GUI
{
    public class IngameMenu : GUIObject
    {
        Button resume, quit, settings;
        GameWindow Parent { get; set; }

        public IngameMenu(GameWindow parent)
        {
            Parent = parent;
            Visible = false;

            float shift = 30;
            float x = parent.Width / 2;
            float y = parent.Height / 2;

            resume = new Button();
            resume.ApplyStylishEffect();
            resume.Location = new Vector(x - resume.Size.X, y - shift * 3);
            resume.Text = "Resume";
            resume.Visible = false;
            resume.MouseClick += (pos) =>
                {
                    Hide();
                    Parent.State = Window.WindowState.Game;
                };

            settings = new Button();
            settings.ApplyStylishEffect();
            settings.Location = new Vector(x - settings.Size.X + 7, y - shift * 2);
            settings.Text = "Settings";
            settings.Visible = false;
            settings.MouseClick += (pos) =>
                {

                };

            quit = new Button();
            quit.ApplyStylishEffect();
            quit.Location = new Vector(x - quit.Size.X, y - shift);
            quit.Text = "Quit";
            quit.Visible = false;
            quit.MouseClick += (pos) =>
                {
                    Parent.Menu.RenderVisibility(true);
                    Hide();
                };

            Parent.AddChildren(resume, settings, quit, this);
        }

        public void Show()
        {
            this.Visible = true;
            quit.Visible = true;
            settings.Visible = true;
            resume.Visible = true;
            Parent.State = Window.WindowState.InGameMenu;
        }

        public void Hide()
        {
            this.Visible = false;
            resume.Visible = false;
            quit.Visible = false;
            settings.Visible = false;
        }

        public override void Draw()
        {
            if (!Visible)
                return;

            GeneralGraphics.DisableTexturing();
            GeneralGraphics.EnableAlphaBlending();
            GL.Color4(0f, 0f, 0f, 0.3f);
            GeneralGraphics.DrawRectangle(new Vector(0f, 0f), new Vector(Parent.Width, Parent.Height));
            GeneralGraphics.DisableBlending();
        }

    }
}
