using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIEngine.GUI;
using System.Drawing;
using OpenTK.Graphics.OpenGL;
using SIEngine.Graphics;
using SIEngine.Other;
using SIEngine.BaseGeometry;
using OpenTK.Input;
using Timer = System.Windows.Forms.Timer;

namespace SI.Game
{
    public class ScoreTable : GUIObject
    {
        private Button closeButton;

        /// <summary>
        /// The first value is the number of the objects shot
        /// and the second is a reference to the model.
        /// </summary>
        public Dictionary<ModelManager.ManagedModel, int> Score { get; set; }
        
        public ScoreTable(Window parent)
        {
            Size = new Vector(500, 300);
            Location = new Vector(200, 100);
            Visible = false;

            closeButton = new Button();
            closeButton.ApplyStylishEffect();
            closeButton.Image = "data/img/bck.bmp";
            closeButton.Location = new Vector(370, 350);
            closeButton.Visible = false;
            closeButton.Text = "Close";

            closeButton.MouseClick += (pos) =>
                {
                    Camera.StopRotation();
                    Camera.MoveTo(new DecimalVector(0m, 0m, 50m), 40);
                    Camera.LookAt(new DecimalVector(0m, 0m, 0m), 40);

                    Game.StartNextLevel();
                    Visible = false; 
                    closeButton.Visible = false;
                };

            parent.Children.Add(this);
            parent.Children.Add(closeButton);
        }

        private bool visible;
        public new bool Visible
        {
            get
            {
                return visible;
            }
            set
            {
                visible = value;
                if(closeButton != null)
                    closeButton.Visible = value;
                if (value)
                    Camera.RotateAround(new DecimalVector(0m, 0m, 0m));
            }
        }

        public override void Draw()
        {
            if (!Visible)
                return;
            
            GeneralGraphics.EnableAlphaBlending();
            GL.Color4(0, 0, 0, 0.3f);
            GeneralGraphics.DrawRectangle(Location, Size);

            //now we draw the score itself.
            GL.Translate(Location.X + 200, Location.Y + 20, 0);
            TextPrinter.Print("Scores", Color.Black);
            GL.Translate(-Location.X - 200, -Location.Y - 20, 0);

            float y = Location.Y + 50;
            int total = 0;
            foreach (var entry in Score)
            {
                string caption = entry.Value + " x " + entry.Key.name + " = ";
                string score = (entry.Value * entry.Key.score).ToString();
                total += entry.Value * entry.Key.score;

                GL.Translate(Location.X + 100, y, 0);
                TextPrinter.Print(caption, Color.Black);

                GL.Translate(200, 0, 0);
                TextPrinter.Print(score, Color.LightGray);

                GL.Translate(-Location.X - 300, -y, 0);
                y += 50;
            }

            GL.Translate(Location.X + Size.X - 240, y, 0);
            TextPrinter.Print("Total: " + total.ToString(), Color.LightGray);
            GL.Translate(- Location.X - Size.X + 240, -y, 0);
        }
    }
}
