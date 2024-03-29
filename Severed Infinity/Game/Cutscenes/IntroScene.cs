﻿using System;
using System.Collections.Generic;
using OpenTK;
using System.Linq;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using SIEngine.Other;
using SIEngine.Graphics;
using SIEngine.BaseGeometry;
using SIEngine.GUI;
using System.Windows.Forms;
using Button = SIEngine.GUI.Button;
using Label = SIEngine.GUI.Label;
using Object = SIEngine.GUI.Object;

namespace SI.Game.Cutscenes
{
    /// <summary>
    /// Basic class to launch the Intro CutScene.
    /// </summary>
    public class IntroScene : Object
    {
        private Button skip;
        private Label textControl;
        private Object arrow, gun;
        private Timer timer;
        private float fadeOut = 0.0f, destFade = 0.3f;
        private const string introText = @"Salute! I am the mad chemist and today we shall embark on
a journey..

Apprentice: Um, boss, let's go already...

Right. A fortnight ago i, the mad chemist, have invented a wondrous.. um..
invention! It is called the infinity modulator!!!

Apprentice: Come on, enough with the boring stuff.. It's the thing
on the right! Let's test it already!!!

I.. um.. we have decided to test this weapon in the skies,
where it can not harm anyone.. hopefully.

Apprentice: Boss, boss, here comes the first wave!!!

Prepare!
";
        private int counter = 0;
        /// <summary>
        /// The parent of this control.
        /// </summary>
        public new GameWindow Parent { get; set; }
        
        /// <summary>
        /// Initializes this class along with all the buttons and models.
        /// In general, this is a fairly slow process.
        /// </summary>
        /// <param name="parent">The parent of this control.</param>
        public IntroScene(GameWindow parent)
        {
            Parent = parent;

            arrow = new Object();
            gun = new Object();

            var temp = new OBJModel();
            temp.Color = Color.LightGoldenrodYellow;
            temp.ParseOBJFile("data/models/arrow/arrow.obj");
            temp.ScaleFactor = 1.5f;
            temp.Stroke = true;
            temp.Rotate = false;
            temp.RotationVector = new Vector(1.0f, 0.0f, 0.0f);            

            arrow.Body = temp;
            arrow.Visible = false;
            arrow.Location = new Vector(-1.5f, -11.3f);

            temp = new OBJModel();
            temp.ParseOBJFile("data/models/gun/gun.obj");
            temp.ScaleFactor = 0.2f;
            temp.Rotate = true;
            temp.RotationVector = new Vector(0.0f, 0.0f, 1.0f);
            
            gun.Body = temp;
            gun.Visible = false;
            gun.Location = new Vector(9.5f, -5.3f, 3.0f);

            textControl = new Label();
            textControl.Location = new Vector(20, 100);
            textControl.Text = "";
            textControl.IgnoreSize = true;
            parent.Children.Add(textControl);

            timer = new Timer();
            timer.Interval = 50;
            timer.Tick += (o, e) =>
                {
                    if (fadeOut < destFade)
                    {
                        fadeOut += 0.01f;
                        return;
                    }

                    if (counter >= introText.Length)
                        return;
                    textControl.Text = textControl.Text + introText[counter++].ToString();
                    if (counter > 20 && introText[counter - 1] == '!')
                    {
                        arrow.Visible = true;
                        gun.Visible = true;
                    }
                };

            skip = new Button();
            skip.Text = "Skip/Continue";
            skip.Location = new Vector(300, 500);
            skip.MouseClick += (pos) =>
                {
                    End();
                    Parent.State = Window.WindowState.Game;
                };

            Parent.Children.Add(skip);
            Parent.Children3D.Add(this);
            Visible = false;
            skip.Visible = false;
        }

        /// <summary>
        /// Starts the cut scene. You 
        /// should initialize it first.
        /// </summary>
        public void Start()
        {
            Visible = true;
            skip.Visible = true;
            timer.Start();
            Parent.State = Window.WindowState.Intro;
        }

        /// <summary>
        /// Ends the scene and launches the tutorial.
        /// </summary>
        public void End()
        {
            timer.Stop();
            skip.Visible = false;
            this.Visible = false;
            arrow.Visible = false;
            gun.Visible = false;
            counter = 0;
            textControl.Text = "";
            fadeOut = 0f;

            Game.StartTutorial();
        }

        /// <summary>
        /// Generic draw method.
        /// </summary>
        public override void Draw()
        {
            if (!Visible)
                return;

            Camera.Zoom = -50;

            GeneralGraphics.EnableAlphaBlending();
            GL.Color4(0.0f, 0.0f, 0.0f, fadeOut);
            GeneralGraphics.DrawRectangle(new Vector(-30.0f, -30.0f, -2.0f), new Vector(500.0f, 500.0f));
            GL.Color4(Color.White);

            GL.PushMatrix();
            {
                GL.Rotate(90.0f, 0.0f, 0.0f, 1.0f);
                arrow.Draw();
            }
            GL.PopMatrix();

            GL.PushMatrix();
            {
                GL.Rotate(-90.0f, 1.0f, 0.0f, 0.0f);
                gun.Draw();
            }
            GL.PopMatrix();
            GeneralGraphics.DrawFilled();
            GeneralGraphics.DisableBlending();
        }

    }
}
