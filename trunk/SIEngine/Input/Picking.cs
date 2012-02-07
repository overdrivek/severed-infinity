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
using SIEngine.Other;
using OpenTK;
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
            GL.Disable(EnableCap.LineSmooth);
            GL.Disable(EnableCap.PointSmooth);

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

        public static Object RayCastPick(Window window, params Object[] objects)
        {
            float x = window.Mouse.X;
            float y = window.Height - window.Mouse.Y;

            Camera.CurrentMode = Camera.CameraMode.Overview;
            Camera.DoCameraTransformation(window);
            Vector position = GeometryMath.UnProjectMouse(new Vector(x, y));

            objects.OrderBy(obj => obj.Location.Z);

            float distance = 1000f;
            Object selected = null;

            //Console.WriteLine("{0} {1}", position.X, position.Y);

            foreach (var obj in objects)
            {
                if (position.X > (obj.Body.MinReach.X + obj.Location.X)
                    && position.X < (obj.Body.MaxReach.X + obj.Location.X)
                    && position.Y > (obj.Body.MinReach.Y - obj.Location.Y)
                    && position.Y < (obj.Body.MaxReach.Y - obj.Location.Y)
                    && distance > obj.Location.DistanceTo(position))
                {
                    distance = obj.Location.DistanceTo(position);
                    selected = obj;
                }        
            }
            
            return selected;
        }
    }
}
