using System;
using System.Collections.Generic;
using SIEngine.GUI;
using SIEngine.BaseGeometry;
using SI.Other;
using SIEngine.Input;
using SIEngine.Other;
using SIEngine.Audio;
using OpenTK.Input;
using Timer = System.Windows.Forms.Timer;
using Object = SIEngine.GUI.Object;

namespace SI.Game
{
    public class Level
    {
        private int currentTime = 0, shootQuant;
        public int ShootInterval { get; set; }
        public int TargetScore { get; set; }
        public int CurrentScore { get; set; }
        public int MissedLimit { get; set; }
        public int CurrentlyMissed { get; set; }
        public bool Completed { get; private set; }

        private Label scoreLabel, scoreText, missedText, missedLabel;
        private List<FlyingObject> flyingObjects;
        private Timer mainTimer = new Timer();
        public GameWindow Parent { get; set; }
        
        private void AnimationStep(object sender, EventArgs evArgs)
        {
            if (Parent.State == Window.WindowState.InGameMenu)
                return;

            currentTime++;
            if (currentTime % ShootInterval == 0)
                for (int i = 0; i < shootQuant; ++ i)
                    Shoot();
            for (int i = 0; i < flyingObjects.Count; ++ i )
                if (!flyingObjects[i].Alive)
                {
                    CurrentlyMissed++;
                    missedLabel.Text = CurrentlyMissed.ToString() + "/" +
                        MissedLimit.ToString();

                    if (CurrentlyMissed >= MissedLimit)
                    {
                        FailLevel();
                        return;
                    }

                    flyingObjects[i].Kill();
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
            Game.ExplodeAt(pick.Location, .5f);

            //update score in Score TableP
            Game.Score[((FlyingObject)pick).ModelReference]++;

            GeneralAudio.PlaySound((1 + GeneralMath.RandomInt() % 8).ToString());
            //Console.WriteLine(flyingObjects.Count);
            flyingObjects.Remove((FlyingObject)pick);
            //Console.WriteLine(flyingObjects.Count);
            CurrentScore += ((FlyingObject)pick).ModelReference.score;
            scoreLabel.Text = CurrentScore.ToString() + "/" + TargetScore.ToString();

            if (CurrentScore >= TargetScore)
                Complete();
        }

        /// <summary>
        /// Initializes a new level.
        /// </summary>
        /// <param name="parent">The parent window.</param>
        /// <param name="shootInterval">The interval between shooting items.</param>
        /// <param name="targetScore">The score to be reached before winning the level.</param>
        /// <param name="missedLimit">The items missed before you fail the level.</param>
        /// <param name="shootQ">The numbers of objects shot at once.</param>
        public Level(GameWindow parent, int shootInterval, int targetScore, int missedLimit,
            int shootQ)
        {
            CurrentScore = 0;
            MissedLimit = missedLimit;
            TargetScore = targetScore;
            ShootInterval = shootInterval;
            shootQuant = shootQ;

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
        
        /// <summary>
        /// Fails the current level.
        /// </summary>
        /// <param name="displayMessage">If true, displays a message with a restart option. Otherwise doesn't.</param>
        public void FailLevel(bool displayMessage = true)
        {
            HideInterface();

            if (!displayMessage)
                return;

            var info = new InfoBox(Parent, new Vector(200, 200), GameplayConstants.FailLevelMessage);
            info.ButtonText = "Restart";
            info.Show();

            info.OKClicked += (pos) => Game.RestartLevel();
        }

        public void Start()
        {
            mainTimer.Start();
            Parent.Mouse.Move += ExplodeObject;
        }
    }
}
