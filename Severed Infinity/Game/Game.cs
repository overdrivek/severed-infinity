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
        private static Gun mainGun;
        private static ScoreTable scoreTable;

        public static Dictionary<ModelManager.ManagedModel, int> Score { get; set; }
        public static int CurrentScore { get; private set; }
        public static int CurrentLevel { get; set; }
        public static int[,] Levels = new int[,]
        {
            {30, 600, 20},
            {40, 600, 15}
        };

        static Game()
        {
        }

        public static void InitializeGame()
        {
            Console.WriteLine("init");

            //initialize empty score table
            Score = new Dictionary<ModelManager.ManagedModel, int>();
            foreach (var model in ModelManager.modelBank)
                Score.Add(model, 0);
            scoreTable = new ScoreTable(MainWindow);

            //for restart level to work, we set the current level to -1
            CurrentLevel = -1;

            mainGun = new Gun();
            MainWindow.Add3DChildren(mainGun);
            mainGun.Visible = false;

        }

        /// <summary>
        /// Launches the default tutorial for this game.
        /// </summary>
        public static void StartTutorial()
        {
            var tutorial = new Tutorial(MainWindow);
            mainGun.Visible = false;
        }

        /// <summary>
        /// Starts the next level and launches a score table
        /// to show the current score. You should have set the
        /// values of the Score field beforehand.
        /// </summary>
        public static void StartNextLevel()
        {
            if (CurrentLevel + 1 < Levels.GetLength(0))
                CurrentLevel++;

            RestartLevel();
        }

        public static void RestartLevel()
        {
            var nextLevel = new Level(MainWindow, Levels[CurrentLevel, 0], Levels[CurrentLevel, 1],
                Levels[CurrentLevel, 2]);
            nextLevel.Start();

            mainGun.Visible = true;
        }

        public static void EndLevel()
        {
            scoreTable.Score = Score;
            scoreTable.Visible = true;
        }

        /// <summary>
        /// Sets the laser and gun visibility.
        /// </summary>
        /// <param name="visible">True for visible, false otherwise</param>
        public static void ShowGun(bool visible)
        {
            mainGun.Visible = visible;
        }

        /// <summary>
        /// Starts the game. Use only once in the beginning.
        /// </summary>
        public static void PlayGame()
        {
            CheckProgress();
            //start game or display new game/continue menu
            intro = new IntroScene(MainWindow);
            intro.Start();
        }

        /// <summary>
        /// Saves the current progress to the save file.
        /// </summary>
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
