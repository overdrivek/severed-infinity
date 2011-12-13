using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIEngine;
using SIEngine.GUI;
using SIEngine.BaseGeometry;
using SIEngine.Graphics;

namespace SI.GUI
{
    public class Credits : GUIObject
    {
        private Label credit;
        private Button back;
        private MainMenu ParentMenu { get; set; }
        public bool Visible { get; set; }

        public Credits(MainMenu parent)
        {
            ParentMenu = parent;

            credit = new Label();
            credit.Location = new Vector(700, ParentMenu.ParentWindow.Size.Height - 50);
            credit.Text = "     Credits \n\nProgrammers \n   Traiko Dinev \n\nGame Designer"+
                "\n   Traiko Dinev \n\nLead Programmer \n   Traiko Dinev";

            back = new Button();
            back.ApplyStylishEffect();
            back.Image = "data/img/bck.bmp";
            back.Text = "Return";
            back.Location = new Vector(340, ParentMenu.ParentWindow.Size.Height + 130);
            back.Size.X = 80;

            ParentMenu.ParentWindow.AddChildren(this, back);
        }

        public override void Draw()
        {
            if (!Visible)
                return;

            if (credit.Location.Y > -200)
            {
                credit.Location.Y -= 0.5f;
                back.Location.Y -= 0.5f;
            }
            else
            {
                credit.Location.Y = ParentMenu.ParentWindow.Size.Height - 50;
                back.Location.Y = ParentMenu.ParentWindow.Size.Height + 130;
            }

            credit.Draw();
        }
    }
}
