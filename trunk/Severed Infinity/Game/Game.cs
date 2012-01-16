using System;
using System.Collections.Generic;
using OpenTK;
using System.Linq;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using SIEngine.Other;
using SIEngine.Graphics;
using SIEngine.BaseGeometry;
using SIEngine.GUI;
using SIEngine.Physics;
using SIEngine.Graphics.ParticleEngines;
using SI.Properties;

namespace SI.Game
{
    public static class Game
    {
        private static int[] unlockedObjects;
        private static List<FlyingObject> objects;

        static Game()
        {
            foreach (var name in Settings.Default.UnlockedObjects)
            {

            }
        }
    }
}
