using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SI.Other
{
    public static class GameplayConstants
    {
        public const string WindowName = "Severed Infinity";
        public const int FlyingObjectTime = 400;
        public const string ShooterModelPath = "data/models/satellite/satellite.obj";
        public const string FailLevelMessage = "You just failed!\n" +
            "How could you fail?\n" +
            "Come on, then, play\n" +
            "the level once again.\n" +
            "Press ESC to quit.";
        public const string ExplosionSoundName = "explosion";


        public const int ExplosionPoolSize = 20;
    }
}
