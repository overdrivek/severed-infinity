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
        public int MissedLimit { get; set; }
        public int CurrentlyMissed { get; set; }

        private Label scoreLabel, scoreText, missedText, missedLabel;
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
                {
                    CurrentlyMissed++;
                    missedLabel.Text = CurrentlyMissed.ToString() + "/" +
                        MissedLimit.ToString();

                    if (CurrentlyMissed >= MissedLimit)
                        FailLevel();
                    
                    flyingObjects.RemoveAt(i);
                }
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

            //update score in Score TableP
            Game.Score[((FlyingObject)pick).ModelReference]++;

            //Console.WriteLine(flyingObjects.Count);
            flyingObjects.Remove((FlyingObject)pick);
            //Console.WriteLine(flyingObjects.Count);
            CurrentScore += ((FlyingObject)pick).ModelReference.score;
            scoreLabel.Text = CurrentScore.ToString() + "/" + TargetScore.ToString();

            if (CurrentScore >= TargetScore)
                Complete();
        }

        public Level(GameWindow parent, int shootInterval, int targetScore, int missedLimit)
        {
            CurrentScore = 0;
            MissedLimit = missedLimit;
            TargetScore = targetScore;
            ShootInterval = shootInterval;
            explosion.Scale = .5f;

            Parent = parent;
            
            //score
            scoreLabel = new Label();
            scoreLabel.Location = new Vector(600, 10);
            scoreLabel.Text = "0/" + targetScore.ToString();

            scoreText = new Label();
            scoreText.Text = "Score:";
            scoreText.Location = new Vector(500, 10);
            
            //missed
            missedLabel = new Label();
            missedLabel.Location = new Vector(600, 30);
            missedLabel.Text = "0/" + missedLimit.ToString();

            missedText = new Label();
            missedText.Text = "Missed:";
            missedText.Location = new Vector(500, 30);

            flyingObjects = new List<FlyingObject>();
            mainTimer.Interval = 10;
            mainTimer.Tick += AnimationStep;

            Parent.AddChildren(scoreText, scoreLabel, missedText, missedLabel);
            Parent.Add3DChildren(explosion);

            Completed = false;
        }

        private void Shoot()
        {
            //3 is the blur stack size
            var managedModel = ModelManager.GetRandomModel();

            var fObject = new FlyingObject(Parent, managedModel.model, 3);
            fObject.Location = new Vector(-10 + GeneralMath.RandomInt() % 20, -20f, 0f);
            fObject.ModelReference = managedModel; 
            fObject.Start();

            flyingObjects.Add(fObject);
        }

        public void HideInterface()
        {
            mainTimer.Stop();
            Parent.Mouse.Move -= ExplodeObject;
            Parent.Children.Remove(scoreLabel);
            Parent.Children.Remove(scoreText);
            Parent.Children.Remove(missedLabel);
            Parent.Children.Remove(missedText);
            Game.ShowGun(false);

            Completed = true;
        }

        public void Complete()
        {
            ModelManager.UnlockModel(Parent);
            HideInterface();
        }
        
        public void FailLevel()
        {
            var info = new InfoBox(Parent, new Vector(200, 200), GameplayConstants.FailLevelMessage);
            info.ButtonText = "Restart";
            info.Show();
            HideInterface();

            info.OKClicked += (pos) =>
                {
                    Game.RestartLevel();
                };
        }

        public void Start()
        {
            mainTimer.Start();
            Parent.Mouse.Move += ExplodeObject;
        }
    }
}
