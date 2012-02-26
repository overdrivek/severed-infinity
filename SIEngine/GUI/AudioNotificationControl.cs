using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIEngine.Graphics;
using SIEngine.Audio;
using SIEngine.BaseGeometry;
using System.Drawing;
using OpenTK.Graphics.OpenGL;

namespace SIEngine.GUI
{
    public class AudioNotificationControl : GUIObject
    {
        public static Vector messageLocation = new Vector(0f, 0f, 0f), 
            messageSize = new Vector(800f, 60f);
        public static float targetOpacity = 0.5f;
        public static float opacityShift = 0.01f;

        private short shiftDirection;
        public float CurrentOpacity { get; set; }
        public Sound MessageSound { get; set; }

        public void ResetShifting()
        {
            shiftDirection = 1;
            CurrentOpacity = 0f;
        }

        public void ShiftOpacity()
        {
            if (CurrentOpacity >= targetOpacity)
                return;
            CurrentOpacity += opacityShift * shiftDirection;
        }
        
        public override void Draw()
        {
            if (MessageSound == null || !Visible)
                return;

            GeneralGraphics.EnableAlphaBlending();
            GL.Color4(0f, 0f, 0f, CurrentOpacity);
            GeneralGraphics.DrawRectangle(messageLocation, messageSize);

            GL.Translate(messageSize.X / 2 - (TextPrinter.DefaultFont.Height / 2) * (MessageSound.Name.Length / 2),
                (messageSize.Y - TextPrinter.DefaultFont.Height) / 2 - 4, 0f);
            TextPrinter.Print(MessageSound.Name, Color.FromArgb((int)(255.0f * CurrentOpacity), Color.White));

            GL.Translate(- (messageSize.X / 2 - (TextPrinter.DefaultFont.Height / 2) * (MessageSound.Name.Length / 2)),
                -((messageSize.Y - TextPrinter.DefaultFont.Height) / 2 - 4), 0f);
        }
    }
}
