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
            if (zoomAcceleration != 0)
            {
                Zoom += zoomAcceleration;
                int prevSign = Math.Sign(zoomAcceleration);

                zoomAcceleration -= Math.Sign(zoomAcceleration) * GameConstants.ZoomAccelerationFadeOut;
                //safety hack (float operator's a bitch)
                if (prevSign != Math.Sign(zoomAcceleration))
                    zoomAcceleration = 0;
            }

            if (angularAcceleration.X != 0)
            {
                Angle.X += angularAcceleration.X;
                int prevSign = Math.Sign(angularAcceleration.X);

                angularAcceleration.X -= Math.Sign(angularAcceleration.X) * GameConstants.AngularAccelerationFadeOut 
                    * Sensitivity;
                //safety hack (float operator's a bitch)
                if (prevSign != Math.Sign(angularAcceleration.X))
                    angularAcceleration.X = 0;
            }

            if (angularAcceleration.Y != 0)
            {
                Angle.Y += angularAcceleration.Y;
                int prevSign = Math.Sign(angularAcceleration.Y);

                angularAcceleration.Y -= Math.Sign(angularAcceleration.Y) * GameConstants.AngularAccelerationFadeOut
                    * Sensitivity;
                //safety hack (float operator's a bitch)
                if (prevSign != Math.Sign(angularAcceleration.Y))
                    angularAcceleration.Y = 0;
            }
        }

        static Camera()
        {
            zoomAcceleration = 0;
            Angle = new Vector(0.0f, 0.0f, 0.0f);
            angularAcceleration = new Vector(0.0f, 0.0f, 0.0f);
            Location = new Vector(0.0f, 0.0f, 0.0f);
            Target = new Vector(0.0f, 0.0f, 1.0f);
            ControlMode = Mode.Default;
            Up = new Vector(0.0f, 1.0f, 0.0f);
            Sensitivity = 1;

            CameraTimer = new Timer();
            CameraTimer.Interval = 10;
            CameraTimer.Tick += TimerTick;
            CameraTimer.Start();
        }

        public enum CameraMode
        {
            Overview = 0,
            OrthographicOverview = 1,
            Follow = 2
        }
        public static CameraMode CurrentMode { get; set; }

        private static Vector angularAcceleration;
        public static Vector Angle { get; set; }

        private static float zoomAcceleration;
        public static float Zoom
        {
            get { return Location.Z; }
            set { Location.Z = ExtensionMethods.Clamp<float>(value, GameConstants.MinZoom, GameConstants.MaxZoom); }
        }

        public static float Sensitivity { get; set; }
        //public static float Sensitivity


        public static Vector Location { get; set; }
        public static Vector Target { get; set; }
        private static Vector Up { get; set; }

        public static Vector ObjectToFollow { get; set; }

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
                    LookAt();
                    break;
            }
        }

        private static void LookAt()
        {
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();

            GL.Rotate(GeometryMath.RadianToDegree(Angle.X), 1.0f, 0.0f, 0.0f);
            GL.Rotate(-GeometryMath.RadianToDegree(Angle.Y), 0.0f, 1.0f, 0.0f);
            GL.Translate((Vector3)Location);
            /*

            Matrix4 rotation = Matrix4.Mult(Matrix4.CreateRotationY(Angle.Y),
                Matrix4.CreateRotationX(Angle.X));

            Matrix4 lookAt = Matrix4.LookAt((Vector3)Location,
                (Vector3)Location + (Vector3)Target,
                (Vector3)Up);
            lookAt = Matrix4.Mult(rotation, lookAt);

            GL.LoadMatrix(ref lookAt);*/
            
        }

        #region Camera Control

        public enum Mode
        {
            Default = 0,
            Smooth = 1
        }
        public static Mode ControlMode  { get; set; }

        public static void ZoomOut()
        {
            if (ControlMode == Mode.Default)
                Location.Z -= GameConstants.ZoomAcceleration;
            else //if (zoomAcceleration == 0)
                zoomAcceleration = -GameConstants.ZoomAcceleration;
        }

        public static void ZoomIn()
        {
            if (ControlMode == Mode.Default)
                Location.Z += GameConstants.ZoomAcceleration;
            else //if (zoomAcceleration == 0)
                zoomAcceleration = GameConstants.ZoomAcceleration;
        }

        public static void PanUp()
        {
            if (ControlMode == Mode.Default)
                Angle.X += GameConstants.AngularAcceleration * Sensitivity;
            else //if (angularAcceleration == 0)
                angularAcceleration.X = GameConstants.AngularAcceleration * Sensitivity;
        }

        public static void PanDown()
        {
            if (ControlMode == Mode.Default)
                Angle.X -= GameConstants.AngularAcceleration * Sensitivity;
            else //if (angularAcceleration == 0)
                angularAcceleration.X = -GameConstants.AngularAcceleration * Sensitivity;
        }

        public static void PanLeft()
        {
            if (ControlMode == Mode.Default)
                Angle.Y += GameConstants.AngularAcceleration * Sensitivity;
            else //if (angularAcceleration == 0)
                angularAcceleration.Y = GameConstants.AngularAcceleration * Sensitivity;
        }

        public static void PanRight()
        {
            if (ControlMode == Mode.Default)
                Angle.Y -= GameConstants.AngularAcceleration * Sensitivity;
            else //if (angularAcceleration == 0)
                angularAcceleration.Y = -GameConstants.AngularAcceleration * Sensitivity;
        }

        public static void MoveLeft()
        {
            Location.X += (int)((double)GameConstants.Speed * Math.Cos(Math.PI / 2 - Angle.X));
            Location.Z += (int)((double)GameConstants.Speed * Math.Sin(Math.PI / 2 - Angle.X));
        }
        
        public static void MoveRight()
        {
            Location.X -= GameConstants.Speed;
        }

        private static Vector3 XAxis = new Vector3(1.0f, 0.0f, 0.0f);
        private static Vector3 YAxis = new Vector3(0.0f, 1.0f, 0.0f);

        public static void MoveIn()
        {
            Vector3 vec = Vector3.Transform((Vector3)Target,
                Matrix4.Mult( Matrix4.CreateFromAxisAngle(XAxis, -Angle.X),
                    Matrix4.CreateFromAxisAngle(YAxis, Angle.Y)));

            Location.X += vec.X * GameConstants.Speed;
            Location.Y += vec.Y * GameConstants.Speed;
            Zoom += vec.Z * GameConstants.Speed;
        }

        public static void MoveOut()
        {
            Vector3 vec = Vector3.Transform((Vector3)Target,
                Matrix4.Mult( Matrix4.CreateFromAxisAngle(XAxis, -Angle.X),
                    Matrix4.CreateFromAxisAngle(YAxis, Angle.Y)));

            Location.X -= vec.X * GameConstants.Speed;
            Location.Y -= vec.Y * GameConstants.Speed;
            Zoom -= vec.Z * GameConstants.Speed;
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
        #endregion
    }
}
