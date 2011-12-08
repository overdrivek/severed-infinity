using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIEngine.Other
{
    public static class GameConstants
    {
        public static float MaxZoom = 100000.0f;
        public static float MinZoom = -100000.0f;

        public static float MinCameraSensitivity = 100.0f;
        public static float CameraSensitivityModifier = 5.0f;

        public static float ZoomFactor = 0.5f;
        public static float ZoomAcceleration = 0.5f;
        public static float ZoomAccelerationFadeOut = 0.05f;

        public static float MaxLeftAngle = -100.0f;
        public static float MaxRightAngle = 100.0f;
        public static float AngularAcceleration = 0.03f;
        public static float AngularAccelerationFadeOut = 0.005f;

        public static float Speed = 0.5f;
    }
}
