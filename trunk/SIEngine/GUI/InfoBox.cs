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

namespace SIEngine.GUI
{
    public class InfoBox : GUIObject
    {
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

        //animation
        private float animationCoef;
        private float currentAlpha, targetAlpha, startAlpha;
        private short currentCycle;

        /// <summary>
        /// Initializes a new instance of the InfoBox class.
        /// It flashes an exclamation mark 2 times, then moves
        /// to the target location.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="target"></param>
        public InfoBox(Window parent, Vector target, string message)
        {
            targetLocation = target + new Vector(20, 40);
            Location = target;
            Size = new Vector(100, 90);
            ExclamationLocation = new Vector((parent.Width - Size.X) / 2,
                (parent.Height - Size.Y) / 2);
            ExclamationSize = new Vector(50, 45);
            Size = new Vector(200, 200);
            Parent = parent;

            startAlpha = 0f;
            currentAlpha = startAlpha;
            targetAlpha = 1f;
            animationCoef = 0.0f;

            mainLabel = new Label();
            Message = message;
            mainLabel.Location = new Vector((Size.X - mainLabel.Size.X) / 2, Location.Y + 10);

            buttonOk = new Button();
            buttonOk.Image = "data/img/bck.bmp";
            buttonOk.ApplyStylishEffect();
            buttonOk.Location = new Vector((Size.X - buttonOk.Size.X) / 2, Size.Y - 10);
            buttonOk.Text = "OK";
            buttonOk.MouseClick += (pos) =>
            {
                MouseClick.Invoke(pos);
                Parent.Children.Remove(buttonOk);
                Parent.Children.Remove(this);
            };
        }

        public void Show()
        {
            Parent.Children.Add(this);
            Parent.Children.Add(buttonOk);
            Parent.Children.Add(mainLabel);
        }

        public void AnimationStep()
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
                animationCoef += 0.05f;
            }
            //otherwise, it's time to move the mark to the target location
            else
            {
                ExclamationLocation = ExclamationLocation.Interpolate(targetLocation, animationCoef);
                animationCoef += 0.01f;
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

                GL.Color4(Color.White);
                GeneralGraphics.InfoBoxFrame.SelectTexture();
                GeneralGraphics.DrawRectangle(Location, Size);
            }
            GeneralGraphics.DisableBlending();
            AnimationStep();
        }

        private bool isMoving = false;
        private Vector locationShift = new Vector(0f, 0f);
        public override void InternalMouseClick(Vector mousePos)
        {
            Console.WriteLine("start");
            if (Parent.Mouse.Y < Location.Y + 20)
            {
                locationShift.X = Location.X - mousePos.X;
                locationShift.Y = Location.Y - mousePos.Y;
                isMoving = true;

                Console.WriteLine("pow");
            }
        }

        public override void InternalMouseUp(Vector mousePos)
        {
            isMoving = false;
        }

        public override void InternalMouseMove(Vector mousePos)
        {
            if (!isMoving)
                return;
            Location = mousePos + locationShift;
        }
        #endregion
    }
}
