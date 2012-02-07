using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIEngine.Graphics;
using SI.Other;
using SI.Game;
using SIEngine.BaseGeometry;
using SI.Game.Cutscenes;
using SIEngine.Other;
using OpenTK.Input;

namespace SI.Game
{
    public static class Game
    {
        public static GameWindow MainWindow { get; set; }
        private static IntroScene intro;
        private static Laser mainLaser;

        public static int CurrentScore { get; private set; }
        public static int CurrentLevel { get; set; }
        public static int[,] Levels = new int[,]
        {
            {30, 600},
            {40, 1200}
        };

        static Game()
        {
            CurrentLevel = 0;

            mainLaser = new Laser(new Vector(0f, 0f, 50), new Vector(0f, 0f, 0f));

            MainWindow.Children3D.Add(mainLaser);
            MainWindow.Mouse.Move += (o, e) =>
            {
                mainLaser.Visible = false;
                if (MainWindow.MouseClicked)
                    mainLaser.Visible = true;

                Camera.CurrentMode = Camera.CameraMode.Overview;
                Camera.DoCameraTransformation(MainWindow);
                var unp = GeometryMath.UnProjectMouse(new Vector(MainWindow.Mouse.X,
                    MainWindow.Mouse.Y));
                mainLaser.Destination = new Vector(unp.X, unp.Y, 0f);
            };
            mainLaser.Visible = false;
        }

        public static void StartNextLevel(int score)
        {
            var nextLevel = new Level(MainWindow, Levels[CurrentLevel, 0], Levels[CurrentLevel, 1]);
            nextLevel.Start();

            mainLaser.Visible = true;
            CurrentScore += score;
            CurrentLevel++;
        }


        public static void PlayGame()
        {
            CheckProgress();
            //start game or display new game/continue menu
            intro = new IntroScene(MainWindow);
            intro.Start();
        }

        public static void SaveProgress()
        {
            
        }

        /// <summary>
        /// Checks if you have played earlier.
        /// </summary>
        /// <returns>True if you have a saved game.</returns>
        public static bool CheckProgress()
        {
            return false;
        }
    }
}
