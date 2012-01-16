using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIEngine.GUI;
using System.Drawing;
using OpenTK.Graphics.OpenGL;
using SIEngine.Graphics;
using SIEngine.Other;
using SIEngine.BaseGeometry;
using OpenTK.Input;
using Timer = System.Windows.Forms.Timer;

namespace SIEngine.GUI
{
    public class InfoBox : GUIObject
    {
        public MouseEventDel OKClicked;
        public string Message
        {
            get
            {
                return mainLabel.Text;
            }
            set
            {
                mainLabel.Text = value;
            }
        }

        private Label mainLabel;
        private Button buttonOk;

        //location
        public Vector ExclamationLocation { get; set; }
        private Vector targetLocation;
        private Vector ExclamationSize { get; set; }
        private Timer MainTimer;

        //animation
        private float animationCoef;
        private float currentAlpha, targetAlpha, startAlpha;
        private short currentCycle;
        private float frameAlpha;
        private float frameTargetY;

        /// <summary>
        /// Initializes a new instance of the InfoBox class.
        /// It flashes an exclamation mark 2 times, then moves
        /// to the target location.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="target"></param>
        public InfoBox(Window parent, Vector target, string message, Vector size = null)
        {
            Location = new Vector(target.X, target.Y);
            OKClicked = new MouseEventDel((pos) => { });

            ExclamationSize = new Vector(60, 55);
            ExclamationLocation = new Vector((parent.Width - ExclamationSize.X) / 2,
                (parent.Height - ExclamationSize.Y) / 2);
            Size = new Vector(200, 10);
            Parent = parent;

            frameAlpha = 0f;
            startAlpha = 0f;
            currentAlpha = startAlpha;
            frameTargetY = 200;
            targetAlpha = 1f;
            animationCoef = 0.0f;

            mainLabel = new Label();
            Message = message;
            mainLabel.Location = new Vector(Location.X - 50 + message.Length * 4, Location.Y + 30);
            mainLabel.Visible = false;

            buttonOk = new Button();
            buttonOk.Image = "data/img/bck.bmp";
            buttonOk.ApplyStylishEffect();
            buttonOk.Visible = false;
            buttonOk.Size.X = 70;
            buttonOk.Location = new Vector(Location.X + (Size.X - buttonOk.Size.X) / 2, Location.Y + 160);
            buttonOk.Text = "OK";
            buttonOk.MouseClick += (pos) =>
            {
                OKClicked.Invoke(pos);

                Parent.Children.Remove(buttonOk);
                Parent.Children.Remove(mainLabel);
                Parent.Children.Remove(this);
            };

            Parent.Mouse.Move += MouseMoveFunc;

            targetLocation = target;
            targetLocation.X += Size.X / 2;
            targetLocation.Y += Size.Y / 2;

            MainTimer = new Timer();
            MainTimer.Interval = 10;
            MainTimer.Tick += AnimationStep;
            MainTimer.Start();
        }

        public void Show()
        {
            Parent.Children.Add(this);
            Parent.Children.Add(buttonOk);
            Parent.Children.Add(mainLabel);
        }

        public void AnimationStep(object sender, EventArgs evArgs)
        {
            //interrupts the animation once it's reached the target location
            if (animationCoef >= 1f && currentCycle >= 5)
                return;

            //shifts the icon so that it appears to be flashing
            if (animationCoef >= 1f && currentCycle < 5)
            {
                currentCycle++;
                targetAlpha = startAlpha;
                startAlpha = currentAlpha;
                animationCoef = 0f;
            }

            //when it's flashed two times, it's time to move the exclamation mark
            if (currentCycle < 5)
            {
                currentAlpha = GeneralMath.Interpolate(startAlpha, targetAlpha, animationCoef);
                animationCoef += 0.1f;
            }
            //otherwise, it's time to move the mark to the target location
            else
            {
                if (currentCycle != 6)
                {
                    ExclamationLocation = ExclamationLocation.Interpolate(targetLocation, animationCoef);
                    animationCoef += 0.05f;

                    if (animationCoef >= 1.0f)
                    {
                        Parent.Mouse.ButtonUp += (o, e) =>
                        {
                            if (e.X > ExclamationLocation.X && e.X < ExclamationLocation.X + ExclamationSize.X
                                && e.Y > ExclamationLocation.Y && e.Y < ExclamationLocation.Y + ExclamationSize.Y)
                            {
                                currentCycle = 6;
                                animationCoef = 0f;
                                currentAlpha = 0f;
                            }
                        };
                    }
                }
                else
                {
                    frameAlpha = 1f;
                    Size.Y = GeneralMath.Interpolate(Size.Y, frameTargetY, animationCoef);

                    animationCoef += 0.05f;
                    if (animationCoef >= 1f)
                    {
                        buttonOk.Visible = true;
                        mainLabel.Visible = true;
                    }
                }
            }
        }

        #region overrides
        public override void Draw()
        {
            GeneralGraphics.EnableTexturing();
            GeneralGraphics.EnableAlphaBlending();
            {
                GL.Color4(1f, 1f, 1f, currentAlpha);
                GeneralGraphics.ExclamationTexture.SelectTexture();
                GeneralGraphics.DrawRectangle(ExclamationLocation, ExclamationSize);

                GL.Color4(1f, 1f, 1f, frameAlpha);
                GeneralGraphics.InfoBoxFrame.SelectTexture();
                GeneralGraphics.DrawRectangle(Location, Size);
            }
            GeneralGraphics.DisableBlending();
        }

        private bool isMoving = false;
        private Vector locationShift = new Vector(0f, 0f);
        public override void InternalMouseDown(Vector mousePos)
        {
            if ( !((Parent.Mouse.X > buttonOk.Location.X && Parent.Mouse.X < buttonOk.Location.X+
                buttonOk.Size.X) && (Parent.Mouse.Y > buttonOk.Location.Y && Parent.Mouse.Y <
                buttonOk.Location.Y + buttonOk.Size.Y))
                && animationCoef >= 1.0f)
            {
                locationShift.X = Location.X - mousePos.X;
                locationShift.Y = Location.Y - mousePos.Y;
                isMoving = true;
            }
        }

        public override void InternalMouseUp(Vector mousePos)
        {
            isMoving = false;
        }

        public void MouseMoveFunc(object sender, MouseEventArgs mousePos)
        {
            if (!isMoving)
                return;

            float shiftX = mousePos.X + locationShift.X - Location.X,
                shiftY = mousePos.Y + locationShift.Y - Location.Y;
            Location.X += shiftX;
            Location.Y += shiftY;

            buttonOk.Location.X += shiftX;
            buttonOk.Location.Y += shiftY;

            mainLabel.Location.X += shiftX;
            mainLabel.Location.Y += shiftY;
        }
        #endregion
    }
}
