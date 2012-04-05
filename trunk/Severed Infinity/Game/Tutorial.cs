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
using Timer = System.Windows.Forms.Timer;

namespace SI.Game
{
    public class Tutorial
    {
        public GameWindow Parent { get; set; }
        public Vector StartingLocation { get; set; }
        
        public int firstMessageTimeout = 8;
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
        private Timer timer;
        private bool firstTimeout = false;

        private void AnimationStep(object sender, EventArgs evArgs)
        {
            if (Math.Abs(-(float)Camera.Location.Z - fObject.Location.Z) <= 10 + (float)GameConstants.ZoomErrorMargin
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
                        timer.Tick += ExplodeObject;
                    };
                intro.Show();
            }
        }

        private void ExplodeObject(object sender, EventArgs evArgs)
        {
            if (!Parent.MouseClicked)
                return;

            Object pick = Picking.RayCastPick(Parent, fObject);
            if (pick != fObject)
                return;

            Parent.Children3D.Add(explosion);
            explosion.Location = fObject.Location;
            explosion.Explode();

            timer.Stop();
            timer.Dispose();
            Parent.Children3D.Remove(fObject);

            var info = new InfoBox(Parent, new Vector(200, 200), Messages[2]);
            info.Show();
            info.OKClicked += (pos) =>
                {
                    Camera.Location.X = 0m;
                    Camera.Location.Y = 0m;
                    Camera.Location.Z = -50m;

                    Game.StartNextLevel();
                };
        }

        public Tutorial(GameWindow parent)
        {
            Parent = parent;
            explosion.Scale = 0.5f;

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
            fObject = new FlyingObject(Parent, ModelManager.modelBank[0].model, 7);
            fObject.ShaderProgram = GeneralGraphics.SimulatedLighting;

            var physGui = new Object();
            physGui.Location = new Vector(0f, 0f, 0f);
            PhysicsObject obj = new PhysicsObject();
            obj.Velocity = fObject.PhysicalBody.Velocity;
            obj.ParentObject = physGui;

            for (int i = 0; i < 8; ++i)
            {
                obj.ModulatePhysics();
                obj.ApplyNaturalForces();
            }

            var target = new DecimalVector((decimal)physGui.Location.X,
                (decimal)physGui.Location.Y, (decimal)physGui.Location.Z + 10);
            Camera.MoveTo(target, 60);

            timer = new Timer();
            timer.Interval = 10;
            timer.Tick += AnimationStep;
            timer.Start();
            fObject.Start();
        }
    }
}
