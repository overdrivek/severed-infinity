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
using Timer = System.Windows.Forms.Timer;
using Object = SIEngine.GUI.Object;

namespace SI.Game
{
    public class Level
    {
        private int currentTime = 0;
        public int ShootInterval { get; set; }
        public int TargetScore { get; set; }
        public int CurrentScore { get; set; }
        private Label scoreLabel, scoreText;
        private List<FlyingObject> flyingObjects;
        private Explosion explosion = new Explosion(32, true);
        private Timer mainTimer = new Timer();
        public GameWindow Parent;
        public bool Completed { get; private set; }
        
        private void AnimationStep(object sender, EventArgs evArgs)
        {
            currentTime++;
            if (currentTime % ShootInterval == 0)
                Shoot();
            for (int i = 0; i < flyingObjects.Count; ++ i )
                if (!flyingObjects[i].Alive)
                    flyingObjects.RemoveAt(i);
        }

        private void ExplodeObject(object sender, MouseEventArgs evArgs)
        {
            if (!Parent.MouseClicked)
                return;

            Object pick = Picking.RayCastPick(Parent, flyingObjects.ToArray());
            if (pick == null)
                return;
            ((FlyingObject)pick).Kill();
            explosion.Location = pick.Location;
            explosion.Explode();

            Console.WriteLine(flyingObjects.Count);
            flyingObjects.Remove((FlyingObject)pick);
            Console.WriteLine(flyingObjects.Count);
            CurrentScore += 10 + GeneralMath.RandomInt() % 20;
            scoreLabel.Text = CurrentScore.ToString() + "/" + TargetScore.ToString();

            if (CurrentScore >= TargetScore)
                Complete();
        }

        public Level(GameWindow parent, int shootInterval, int targetScore)
        {
            CurrentScore = 0;
            TargetScore = targetScore;
            ShootInterval = shootInterval;
            explosion.Scale = .5f;

            Parent = parent;
            scoreLabel = new Label();
            scoreLabel.Location = new Vector(600, 10);
            scoreLabel.Text = "0/" + targetScore.ToString();

            scoreText = new Label();
            scoreText.Text = "Score:";
            scoreText.Location = new Vector(500, 10);

            flyingObjects = new List<FlyingObject>();
            mainTimer.Interval = 10;
            mainTimer.Tick += AnimationStep;

            Parent.AddChildren(scoreText, scoreLabel);
            Parent.Add3DChildren(explosion);

            Completed = false;
        }

        //OBJModel model = new OBJModel("data/models/cake/pie.obj");
        //OBJModel model = new OBJModel("data/models/apple/apple.obj");
        
        private void Shoot()
        {
            var fObject = new FlyingObject(Parent, ModelManager.GetRandomModel(), 3);
            fObject.Location = new Vector(-10 + GeneralMath.RandomInt() % 20, -20f, 0f);
            fObject.Start();

            flyingObjects.Add(fObject);
        }

        public void Complete()
        {
            ModelManager.UnlockModel(Parent);
            mainTimer.Stop();
            Parent.Mouse.Move -= ExplodeObject;

            Completed = true;
        }

        public void Start()
        {
            mainTimer.Start();
            Parent.Mouse.Move += ExplodeObject;
        }
    }
}
