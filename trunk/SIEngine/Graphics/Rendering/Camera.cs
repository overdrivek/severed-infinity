using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIEngine.BaseGeometry;
using SIEngine.Other;
using SIEngine.GUI;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Windows.Forms;

namespace SIEngine.Graphics
{
    public static class Camera
    {
        private static Timer CameraTimer { get; set; }
        private static void TimerTick(object sender, EventArgs evArgs)
        {
            if (positionAcceleration.Z != 0)
            {
                Zoom += positionAcceleration.Z;
                int prevSign = Math.Sign(positionAcceleration.Z);

                positionAcceleration.Z -= Math.Sign(positionAcceleration.Z) *
                    positionAccelerationFadeOut.Z;

                //safety hack (float operator's a bitch)
                if (prevSign != Math.Sign(positionAcceleration.Z))
                    positionAcceleration.Z = 0;
            }

            if (positionAcceleration.X != 0)
            {
                Location.X += positionAcceleration.X;
                int prevSign = Math.Sign(positionAcceleration.X);

                positionAcceleration.X -= Math.Sign(positionAcceleration.X) *
                    positionAccelerationFadeOut.X;
                //safety hack (float operator's a bitch)
                if (prevSign != Math.Sign(positionAcceleration.X))
                    positionAcceleration.X = 0;
            }

            if (positionAcceleration.Y != 0)
            {
                Location.Y += positionAcceleration.Y;
                int prevSign = Math.Sign(positionAcceleration.Y);

                positionAcceleration.Y -= Math.Sign(positionAcceleration.Y) *
                    positionAccelerationFadeOut.Y;
                //safety hack (float operator's a bitch)
                if (prevSign != Math.Sign(positionAcceleration.Y))
                    positionAcceleration.Y = 0;
            }

            if (angularAcceleration.X != 0)
            {
                Angle.X += angularAcceleration.X;
                int prevSign = Math.Sign(angularAcceleration.X);

                angularAcceleration.X -= Math.Sign(angularAcceleration.X) * angularAccelerationFadeOut.X
                    * Sensitivity;
                //safety hack (float operator's a bitch)
                if (prevSign != Math.Sign(angularAcceleration.X))
                    angularAcceleration.X = 0;
            }

            if (angularAcceleration.Y != 0)
            {
                Angle.Y += angularAcceleration.Y;
                int prevSign = Math.Sign(angularAcceleration.Y);

                angularAcceleration.Y -= Math.Sign(angularAcceleration.Y) * angularAccelerationFadeOut.Y
                    * Sensitivity;
                //safety hack (float operator's a bitch)
                if (prevSign != Math.Sign(angularAcceleration.Y))
                    angularAcceleration.Y = 0;
            }
        }

        static Camera()
        {
            positionAcceleration = new DecimalVector(0m, 0m, 0m);
            positionAccelerationFadeOut = new DecimalVector(0m, 0m, 0m);
            Angle = new DecimalVector(0m, 0m, 0m);
            angularAcceleration = new DecimalVector(0m, 0m, 0m);
            angularAccelerationFadeOut = new DecimalVector(0m, 0m, 0m);
            Location = new DecimalVector(0m, 0m, 0m);
            Target = new DecimalVector(0m, 0m, 1m);
            ControlMode = Mode.Default;
            Up = new DecimalVector(0m, 1m, 0m);
            Sensitivity = 1;
            halfAngle = new DecimalVector(0m, 0m);
            halfSpeed = new DecimalVector(0m, 0m);

            CameraTimer = new Timer();
            CameraTimer.Interval = 10;
            CameraTimer.Tick += AnimationStep;
            CameraTimer.Tick += TimerTick;
            CameraTimer.Start();
        }

        public enum CameraMode
        {
            Overview = 0,
            OrthographicOverview = 1,
            Follow = 2,
            Picking = 3
        }
        public static CameraMode CurrentMode { get; set; }

        private static DecimalVector angularAcceleration;
        public static DecimalVector Angle { get; set; }
        private static DecimalVector angularAccelerationFadeOut;

        private static DecimalVector positionAcceleration;
        private static DecimalVector positionAccelerationFadeOut;

        public static decimal Zoom
        {
            get { return Location.Z; }
            set { Location.Z = ExtensionMethods.Clamp<decimal>(value, GameConstants.MinZoom, GameConstants.MaxZoom); }
        }

        public static decimal Sensitivity { get; set; }

        public static DecimalVector Location { get; set; }
        public static DecimalVector Target { get; set; }
        private static DecimalVector Up { get; set; }

        public static DecimalVector ObjectToFollow { get; set; }

        public static void DoCameraTransformation(Window window)
        {
            switch (CurrentMode)
            {
                case CameraMode.OrthographicOverview:
                    SetOrthographicProjection(window);

                    GL.MatrixMode(MatrixMode.Modelview);
                    GL.LoadIdentity();
                    break;
                case CameraMode.Overview:
                    SetPerspectiveProjection(window);
                    RotateToAngle();
                    break;
                case CameraMode.Picking:
                    SetPickingProjection(window);
                    RotateToAngle();
                    break;
            }
        }

        private static void RotateToAngle()
        {
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();

            GL.Rotate((float)Angle.X, 1.0f, 0.0f, 0.0f);
            GL.Rotate((float)Angle.Y, 0.0f, 1.0f, 0.0f);
            GL.Translate((Vector3)Location);
        }

        #region Targeted Controls

        private static DecimalVector halfSpeed;
        private static DecimalVector halfAngle;
        private static bool useTargets;

        /// <summary>
        /// Moves the camera to a given point.
        /// </summary>
        /// <param name="position">The position to move the camera to.(Z plane)</param>
        /// <param name="time">The time for which the movement is completed in milliseconds.</param>
        public static void MoveTo(DecimalVector position, decimal time)
        {
            position.Z = -position.Z;
            position.Y = -position.Y;
            position.X = -position.X;
            time /= 2;
            useTargets = true;

            if (positionAcceleration.Z == 0m && Math.Abs(Zoom - position.Z) > GameConstants.ZoomErrorMargin)
                if (ControlMode == Mode.Smooth)
                {
                    int direction = -Math.Sign(Zoom - position.Z);
                    decimal distance = Math.Abs(Zoom - position.Z) / 2;

                    positionAcceleration.Z = direction * (2 * distance) / (time * time);
                    positionAccelerationFadeOut.Z = (-direction) * positionAcceleration.Z;

                    time -= 1;
                    halfSpeed.Z = positionAcceleration.Z * time;
                }
                else
                {
                    //do nothing
                }
            if (positionAcceleration.X == 0m && Math.Abs(Location.X - position.X) > GameConstants.ZoomErrorMargin)
                if (ControlMode == Mode.Smooth)
                {
                    int direction = -Math.Sign(Location.X - position.X);
                    decimal distance = Math.Abs(Location.X - position.X) / 2;

                    positionAcceleration.X = direction * (2 * distance) / (time * time);
                    positionAccelerationFadeOut.X = (-direction) * positionAcceleration.X;

                    time -= 1;
                    halfSpeed.X = positionAcceleration.X * time;
                }
                else
                {
                    //do nothing
                }

            if (positionAcceleration.Y == 0m && Math.Abs(Location.Y - position.Y) > GameConstants.ZoomErrorMargin)
                if (ControlMode == Mode.Smooth)
                {
                    int direction = -Math.Sign(Location.Y - position.Y);
                    decimal distance = Math.Abs(Location.Y - position.Y) / 2;

                    positionAcceleration.Y = direction * (2 * distance) / (time * time);
                    positionAccelerationFadeOut.Y = (-direction) * positionAcceleration.Y;

                    time -= 1;
                    halfSpeed.Y = positionAcceleration.Y * time;
                }
                else
                {
                    //do nothing
                }
        }

        /// <summary>
        /// Rotates the camera to look at a given point.
        /// </summary>
        /// <param name="target">The point to look at.</param>
        /// <param name="time">The time for which the rotation is completed in milliseconds.</param>
        public static void LookAt(DecimalVector target, decimal time)
        {
            useTargets = true;
            //opengl axis hack
            target.Y = -target.Y;
            time /= 2;

            #region X axis

            int direction = -Math.Sign(Angle.X - target.Y);

            //Console.WriteLine("distance:{0}", Math.Abs(Angle.X - target.Y));
            if (angularAcceleration.X == 0m && Math.Abs(Angle.X - target.Y) >= 5.0m)
                if (ControlMode == Mode.Smooth)
                {
                    decimal distance = Math.Abs(Angle.X - target.Y) / 2;

                    angularAcceleration.X = direction * (2 * distance) / (time * time);
                    angularAccelerationFadeOut.X = (-direction) * angularAcceleration.X;

                    time -= 1;
                    halfAngle.X = angularAcceleration.X * time;
                }
                else
                {
                    //do nothing
                }
            #endregion

            #region Y axis

            direction = -Math.Sign(Angle.Y - target.X);

            //Console.WriteLine("distance:{0}", Math.Abs(Angle.X - target.Y));
            if (angularAcceleration.Y == 0m && Math.Abs(Angle.Y - target.X) >= 5.0m)
                if (ControlMode == Mode.Smooth)
                {
                    decimal distance = Math.Abs(Angle.Y - target.X) / 2;

                    angularAcceleration.Y = direction * (2 * distance) / (time * time);
                    angularAccelerationFadeOut.Y = (-direction) * angularAcceleration.Y;

                    time -= 1;
                    halfAngle.Y = angularAcceleration.Y * time;
                }
                else
                {
                    //do nothing
                }
            #endregion
        }

        private static void AnimationStep(object sender, EventArgs evArgs)
        {
            if (!useTargets)
                return;

            if (ControlMode == Mode.Smooth)
            {
                if (Math.Abs(positionAcceleration.Z) > Math.Abs(halfSpeed.Z))
                    positionAccelerationFadeOut.Z = -positionAccelerationFadeOut.Z;
                if (Math.Abs(positionAcceleration.Y) > Math.Abs(halfSpeed.Y))
                    positionAccelerationFadeOut.Y = -positionAccelerationFadeOut.Y;
                if (Math.Abs(positionAcceleration.X) > Math.Abs(halfSpeed.X))
                    positionAccelerationFadeOut.X = -positionAccelerationFadeOut.X;
                if (Math.Abs(angularAcceleration.X) > Math.Abs(halfAngle.X))
                    angularAccelerationFadeOut.X = -angularAccelerationFadeOut.X;
                if (Math.Abs(angularAcceleration.Y) > Math.Abs(halfAngle.Y))
                    angularAccelerationFadeOut.Y = -angularAccelerationFadeOut.Y;
            }
            else
            {
                //um.. do nothing?
            }
        }
        #endregion

        #region Camera Control

        public enum Mode
        {
            Default = 0,
            Smooth = 1
        }
        public static Mode ControlMode { get; set; }

        public static void ZoomOut()
        {
            useTargets = false;
            positionAccelerationFadeOut.Z = GameConstants.ZoomAccelerationFadeOut;
            if (ControlMode == Mode.Default)
                Location.Z -= GameConstants.ZoomAcceleration;
            else //if (zoomAcceleration == 0)
                positionAcceleration.Z = -GameConstants.ZoomAcceleration;
        }

        public static void ZoomIn()
        {
            useTargets = false;
            positionAccelerationFadeOut.Z = GameConstants.ZoomAccelerationFadeOut;
            if (ControlMode == Mode.Default)
                Location.Z += GameConstants.ZoomAcceleration;
            else //if (zoomAcceleration == 0)
                positionAcceleration.Z = GameConstants.ZoomAcceleration;
        }

        public static void PanUp()
        {
            useTargets = false;
            angularAccelerationFadeOut.X = GameConstants.AngularAccelerationFadeOut;
            if (ControlMode == Mode.Default)
                Angle.X += GameConstants.AngularAcceleration * Sensitivity;
            else //if (angularAcceleration == 0)
                angularAcceleration.X = GameConstants.AngularAcceleration * Sensitivity;
        }

        public static void PanDown()
        {
            useTargets = false;
            angularAccelerationFadeOut.X = GameConstants.AngularAccelerationFadeOut;
            if (ControlMode == Mode.Default)
                Angle.X -= GameConstants.AngularAcceleration * Sensitivity;
            else //if (angularAcceleration == 0)
                angularAcceleration.X = -GameConstants.AngularAcceleration * Sensitivity;
        }

        public static void PanLeft()
        {
            useTargets = false;
            angularAccelerationFadeOut.Y = GameConstants.AngularAccelerationFadeOut;
            if (ControlMode == Mode.Default)
                Angle.Y += GameConstants.AngularAcceleration * Sensitivity;
            else //if (angularAcceleration == 0)
                angularAcceleration.Y = GameConstants.AngularAcceleration * Sensitivity;
        }

        public static void PanRight()
        {
            useTargets = false;
            angularAccelerationFadeOut.Y = GameConstants.AngularAccelerationFadeOut;
            if (ControlMode == Mode.Default)
                Angle.Y -= GameConstants.AngularAcceleration * Sensitivity;
            else //if (angularAcceleration == 0)
                angularAcceleration.Y = -GameConstants.AngularAcceleration * Sensitivity;
        }

        public static void MoveLeft()
        {
            useTargets = false;
            Location.X += GameConstants.Speed * (decimal)Math.Cos(Math.PI / 2 - (double)Angle.X);
            Location.Z += GameConstants.Speed * (decimal)Math.Sin(Math.PI / 2 - (double)Angle.X);
        }

        public static void MoveRight()
        {
            useTargets = false;
            Location.X -= GameConstants.Speed;
        }

        public static Vector3 XAxis = new Vector3(1.0f, 0.0f, 0.0f);
        public static Vector3 YAxis = new Vector3(0.0f, 1.0f, 0.0f);

        public static void MoveIn()
        {
            useTargets = false;
            Vector3 vec = Vector3.Transform((Vector3)Target,
                Matrix4.Mult(Matrix4.CreateFromAxisAngle(XAxis, -(float)Angle.X),
                    Matrix4.CreateFromAxisAngle(YAxis, (float)Angle.Y)));

            Location.X += (decimal)vec.X * GameConstants.Speed;
            Location.Y += (decimal)vec.Y * GameConstants.Speed;
            Zoom += (decimal)vec.Z * GameConstants.Speed;
        }

        public static void MoveOut()
        {
            useTargets = false;
            Vector3 vec = Vector3.Transform((Vector3)Target,
                Matrix4.Mult(Matrix4.CreateFromAxisAngle(XAxis, -(float)Angle.X),
                    Matrix4.CreateFromAxisAngle(YAxis, (float)Angle.Y)));

            Location.X -= (decimal)vec.X * GameConstants.Speed;
            Location.Y -= (decimal)vec.Y * GameConstants.Speed;
            Zoom -= (decimal)vec.Z * GameConstants.Speed;
        }



        #endregion

        #region Projection

        public static void SetOrthographicProjection(Window window)
        {
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(0, window.Width, window.Height, 0, 1, -1);

            GL.Disable(EnableCap.DepthTest);

        }

        public static void SetPerspectiveProjection(Window window)
        {
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            var projection = Matrix4.CreatePerspectiveFieldOfView((float)Math.PI / 4.0f, window.Width / window.Height, 0.1f, 1000.0f);
            GL.LoadMatrix(ref projection);

            GL.Enable(EnableCap.DepthTest);
        }

        public static void SetPickingProjection(Window window)
        {
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            
            int[] viewport = new int[]
            {
                0, 0,
                window.Width,
                window.Height
            };
            OpenTK.Graphics.Glu.PickMatrix(window.Mouse.X, window.Height - window.Mouse.Y, 5, 5, viewport);
            var projection = Matrix4.CreatePerspectiveFieldOfView((float)Math.PI / 4.0f, window.Width / window.Height, 0.1f, 1000.0f);
            GL.MultMatrix(ref projection);

        }
        #endregion
    }
}
