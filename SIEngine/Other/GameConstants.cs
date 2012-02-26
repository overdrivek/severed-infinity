using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIEngine.Other
{
    public static class GameConstants
    { 
        public const decimal MaxZoom = 100000.0m;
        public const decimal MinZoom = -100000.0m;

        public const decimal CameraErrorCoef = 0.90702947845804988662131519274376m;

        public const decimal MinCameraSensitivity = 100.0m;
        public const decimal CameraSensitivityModifier = 5.0m;
        public const decimal ZoomErrorMargin = 3.0m;
        public const decimal ZoomFadeOutErrorCoef = 0.8m;
        public const decimal ZoomHalfSpeedErrorCoef = 0.89m;

        public const decimal ZoomFactor = 0.5m;
        public const decimal ZoomAcceleration = 0.5m;
        public const decimal ZoomAccelerationFadeOut = 0.05m;

        public const decimal MaxLeftAngle = -100.0m;
        public const decimal MaxRightAngle = 100.0m;
        public const decimal AngularAcceleration = 1.7188m;
        public const decimal AngularAccelerationFadeOut = 0.2864m;

        public const decimal Speed = 0.5m;

        public const string InfoLogDirectory = "data/InfoLog.silog";
        public const string ErrorLogDirectory = "data/ErrorLog.silog";

        public const float DefaultSoundVolume = 0.1f;
        public const float DefaultMusicVolume = 0.3f;

        public const float MaxMusicVolume = 0.3f;
        public const float VolumeFadeOut = 0.003f;
    }
}