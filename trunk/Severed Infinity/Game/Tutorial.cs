using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIEngine.GUI;
using SIEngine.BaseGeometry;
using SIEngine.Graphics;
using SI.Other;
using SIEngine.Input;
using SIEngine.Graphics.ParticleEngines;
using SIEngine.Other;
using SIEngine.Physics;
using SI.Game;
using OpenTK.Input;
using Object = SIEngine.GUI.Object;

namespace SI.Game
{
    public class Tutorial
    {
        public GameWindow Parent { get; set; }
        public Vector StartingLocation { get; set; }
        
        public int firstMessageTimeout = 10;
        public string[] Messages = 
            {
                "This tutorial will teach\n" +
                "You the basics of\n the game!\n" +
                "Amazing!!! Press OK to\n" +
                "continue.",

                "Welcome to the game!\n" +
                "This is an apple.. Right!\n" + 
                "Shoot it!\n" + 
                "Oh, by just clicking on it...",

                "Good job!\nYou get the gist of it!\n" +
                "Now, let's explode\n" +
                "some apples!"
            };

        private int currentTime = 0;
        private FlyingObject fObject;
        private Explosion explosion = new Explosion(16, true);
        private bool firstTimeout = false;

        OBJModel model = new OBJModel("data/models/apple/apple.obj");

        private void AnimationStep(object sender, EventArgs evArgs)
        {
            if (Math.Abs(-(float)Camera.Location.Z - fObject.Location.Z) <= 20 + (float)GameConstants.ZoomErrorMargin
                && !firstTimeout)
            {
                Camera.RotateAround(new DecimalVector((decimal)fObject.Location.X, 0m,
                    (decimal)fObject.Location.Z));
                firstTimeout = true;
            }

            currentTime++;
            if (currentTime == firstMessageTimeout)
            {
                fObject.Paused = true;
               
                var intro = new InfoBox(Parent, new Vector(200, 200), Messages[1]);
                
                intro.OKClicked += (pos) =>
                    {
                        Camera.StopRotation();
                        Camera.MoveTo(new DecimalVector(0m, 0m, 50m), 40);
                        Camera.LookAt(new DecimalVector(0m, 0m, 0m), 40);
                        Parent.Mouse.ButtonDown += ExplodeObject;
                        Parent.Mouse.Move += MarkObject;
                    };
                intro.Show();
            }
        }

        private void ExplodeObject(object sender, MouseEventArgs evArgs)
        {
            Object pick = Picking.ColorPick(Parent, fObject);
            if (pick != fObject)
                return;

            Parent.Children3D.Add(explosion);
            explosion.Location = fObject.Location;
            explosion.Explode();

            Parent.Mouse.ButtonDown -= ExplodeObject;
            Parent.Mouse.Move -= MarkObject;
            Parent.Children3D.Remove(fObject);

            var info = new InfoBox(Parent, new Vector(200, 200), Messages[2]);
            info.Show();
            info.OKClicked += (pos) =>
                {
                    Camera.Location.X = 0m;
                    Camera.Location.Y = 0m;
                    Camera.Location.Z = 0m;
                };
        }

        private void MarkObject(object sender, MouseEventArgs evArgs)
        {
            Object pick = Picking.ColorPick(Parent, fObject);
            if (pick == fObject)
                fObject.Body.Stroke = true;
            else fObject.Body.Stroke = false;
        }

        public Tutorial(GameWindow parent)
        {
            Parent = parent;
            explosion.Scale = 0.5f;

            model.ScaleFactor = 0.03f;

            InfoBox welcomeBox = new InfoBox(Parent, new Vector(200, 200), Messages[0]);
            welcomeBox.Show();
            welcomeBox.OKClicked += (pos) =>
                {
                    Initialize();
                };
        }

        private void Initialize()
        {
            StartingLocation = new Vector(0f, 0f, 0f);
            fObject = new FlyingObject(Parent, model, 7);

            var physGui = new Object();
            physGui.Location = new Vector(0f, 0f, 0f);
            PhysicsObject obj = new PhysicsObject();
            obj.Velocity = fObject.PhysicalBody.Velocity;
            obj.ParentObject = physGui;

            for (int i = 0; i < 10; ++i)
            {
                obj.ModulatePhysics();
                obj.ApplyNaturalForces();
            }

            var target = new DecimalVector((decimal)physGui.Location.X,
                (decimal)physGui.Location.Y, (decimal)physGui.Location.Z + 20);
            Camera.MoveTo(target, 60);

            var timer = new System.Windows.Forms.Timer();
            timer.Interval = 10;
            timer.Tick += AnimationStep;
            timer.Start();
            fObject.Start();
        }
    }
}
