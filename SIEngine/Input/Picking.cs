using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIEngine;
using SIEngine.BaseGeometry;
using SIEngine.GUI;
using SIEngine.Graphics;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using Object = SIEngine.GUI.Object;

namespace SIEngine.Input
{
    public static class Picking
    {
        /// <summary>
        /// Picks objects using OpenGl picking API. VERY SLOW!
        /// Use this only if the number of objects to pick exclude 255^4
        /// or unless you know precisely what you are doing.
        /// </summary>
        /// <param name="objects">The objects to pick.</param>
        /// <returns></returns>
        public static List<int> Pick(Window window, params Object[] objects)
        {
            List<int> picks = null;
            int[] buffer = new int[1 << 5];

            GL.SelectBuffer(1 << 5, buffer);
            GeneralGraphics.PickingMode();
            Camera.CurrentMode = Camera.CameraMode.Picking;
            Camera.DoCameraTransformation(window);

            foreach(Object obj in objects)
            {
                GL.PushName(obj.Name);
                    obj.PickDraw();
                GL.PopName();
            }

            int hits = GeneralGraphics.RenderMode();
            
            if (hits != 0)
            {
                picks = new List<int>();
                for (int i = 0; i < hits; ++i)
                    picks.Add(buffer[i * 4 + 3]);
            }

            return picks;
        }

        public static Object ColorPick(Window window, params Object[] objects)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            Camera.CurrentMode = Camera.CameraMode.Overview;
            Camera.DoCameraTransformation(window);
            GeneralGraphics.DisableTexturing();
            GeneralGraphics.DisableBlending();
            
            foreach (Object obj in objects)
            {
                GL.Color4(obj.pickColor);
                obj.PickDraw();
            }

            byte[] pixels = new byte[4];
            
            GL.ReadPixels(window.Mouse.X, window.Height - window.Mouse.Y, 1, 1, PixelFormat.Rgba,
                PixelType.UnsignedByte, pixels);

            Color pickedColor = Color.FromArgb(pixels[3], pixels[0], pixels[1], pixels[2]);
            Object target = objects.FirstOrDefault(obj => obj.pickColor == pickedColor);

            return target;
        }
    }
}
