using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIEngine.Graphics;
using SI.Other;
using SI.Game;
using SI.Properties;
using SIEngine.BaseGeometry;
using SIEngine.GUI;
using SIEngine.Graphics.ParticleEngines;
using SI.Game.Cutscenes;
using SIEngine.Other;
using OpenTK.Input;

namespace SI.Game
{
    public static class Game
    {
        public static GameWindow MainWindow { get; set; }
        public static bool ShowingScores
        {
            get
            {
                return scoreTable.Visible;
            }
        }
        private static IntroScene intro;
        private static Gun mainGun;
        private static ScoreTable scoreTable;

        private static Explosion[] explosionPool;
        private static int currentExplosion = 0;

        public static Dictionary<ModelManager.ManagedModel, int> Score { get; set; }
        public static int CurrentScore { get; private set; }
        public static int CurrentLevel { get; set; }
        public static Level ActualLevel { get; set; }
        public static int[,] Levels = new int[,]
        {
            {25, 600, 20, 2},
            {30, 800, 25, 3},
            {35, 1000, 30, 4},
            {40, 1200, 35, 5}
        };

        static Game()
        {
        }

        public static void InitializeGame()
        {
            //initialize explosion pool
            explosionPool = new Explosion[GameplayConstants.ExplosionPoolSize];
            for (int i = 0; i < GameplayConstants.ExplosionPoolSize; ++i)
                explosionPool[i] = new Explosion(32, true);
            MainWindow.Add3DChildren(explosionPool);

            //initialize empty score table
            Score = new Dictionary<ModelManager.ManagedModel, int>();
            foreach (var model in ModelManager.modelBank)
                Score.Add(model, 0);
            scoreTable = new ScoreTable(MainWindow);

            //for restart level to work, we set the current level to -1
            CurrentLevel = -1;

            mainGun = new Gun();
            mainGun.Parent = MainWindow;
            MainWindow.Add3DChildren(mainGun);
            mainGun.Visible = false;

            LoadGame();
        }

        public static void ExplodeAt(Vector location, float scale)
        {
            explosionPool[currentExplosion].Scale = scale;
            explosionPool[currentExplosion].Location = location;
            explosionPool[currentExplosion].Explode();

            currentExplosion++;
            if (currentExplosion >= GameplayConstants.ExplosionPoolSize)
                currentExplosion = 0;
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
                Levels[CurrentLevel, 2], Levels[CurrentLevel, 3]);
            nextLevel.Start();
            ActualLevel = nextLevel;

            mainGun.Visible = true;
        }

        public static void EndLevel()
        {
            SaveProgress();

            scoreTable.Playing = true;
            scoreTable.Score = Score;
            scoreTable.Visible = true;
        }

        /// <summary>
        /// Shows the scores when not playing the game.
        /// </summary>
        public static void ShowScores()
        {
            scoreTable.Playing = false;

            scoreTable.Score = Score;
            scoreTable.Visible = true;
        }

        /// <summary>
        /// Um...well.. duh?
        /// </summary>
        public static void HideScores()
        {
            scoreTable.Visible = false;
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
        public static void NewGame()
        {
            intro = new IntroScene(MainWindow);
            intro.Start();
        }

        /// <summary>
        /// Resumes the game from the level saved in the settings.
        /// If that is -1, the tutorial is started.
        /// </summary>
        public static void ResumeGame()
        {
            LoadGame();

            MainWindow.State = Window.WindowState.Game;
            StartNextLevel();   
        }

        /// <summary>
        /// Loads the game from the save file.
        /// </summary>
        public static void LoadGame()
        {
            CurrentLevel = Settings.Default.CurrentLevel;

            for (int i = 0; i < Score.Count; ++i)
                Score[Score.ElementAt(i).Key] = Settings.Default.itemsShot[i];
        }

        /// <summary>
        /// Saves the current progress to the save file.
        /// </summary>
        public static void SaveProgress()
        {
            int i = 0;
            foreach (var entry in Score)
            {
                Settings.Default.itemsShot[i] = entry.Value;
                i++;
            }

            Settings.Default.CurrentLevel = CurrentLevel;
            Settings.Default.Save();
        }

        public static void QuitLevel()
        {
            if (ActualLevel == null)
                return;

            ActualLevel.FailLevel(false);
        }

        /// <summary>
        /// Checks if you have played earlier.
        /// </summary>
        /// <returns>True if you have a saved game.</returns>
        public static bool CheckProgress()
        {
            return Settings.Default.CurrentLevel != -1;
        }

        /// <summary>
        /// Deletes the save game file.
        /// </summary>
        /// <param name="deleteScores">If true, resets the high scores to 0.</param>
        public static void DeleteProgress(bool deleteScores = true)
        {
            Settings.Default.unlockStatus = new bool[1 << 5];
            for (int i = 0; i < Settings.Default.unlockStatus.Length; ++i)
                Settings.Default.unlockStatus[i] = false;
            Settings.Default.unlockStatus[0] = true;
            Settings.Default.CurrentLevel = -1;

            if (deleteScores)
            {
                Settings.Default.itemsShot = new int[ModelManager.modelBank.Count];

                for (int i = 0; i < Settings.Default.itemsShot.Length; ++i)
                    Settings.Default.itemsShot[i] = 0;
            }

            Settings.Default.Save();
        }
    }
}
