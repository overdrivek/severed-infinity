using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIEngine.Other
{
    public static class GameConstants
    { 
        public const float MaxZoom = 100000.0f;
        public const float MinZoom = -100000.0f;

        public const float MinCameraSensitivity = 100.0f;
        public const float CameraSensitivityModifier = 5.0f;

        public const float ZoomFactor = 0.5f;
        public const float ZoomAcceleration = 0.5f;
        public const float ZoomAccelerationFadeOut = 0.05f;

        public const float MaxLeftAngle = -100.0f;
        public const float MaxRightAngle = 100.0f;
        public const float AngularAcceleration = 0.03f;
        public const float AngularAccelerationFadeOut = 0.005f;

        public const float Speed = 0.5f;

        public const string InfoLogDirectory = "data/InfoLog.silog";
        public const string ErrorLogDirectory = "data/ErrorLog.silog";
    }
}