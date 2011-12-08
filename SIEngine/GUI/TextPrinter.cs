using System;
using System.Collections.Generic;
using OpenTK;
using System.Linq;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using SIEngine.Other;

namespace SIEngine.GUI
{
    public static class TextPrinter
    {
        public static Font DefaultFont { get; set; }
        public static OpenTK.Graphics.TextPrinter printer;
        static TextPrinter()
        {
            printer = new OpenTK.Graphics.TextPrinter(OpenTK.Graphics.TextQuality.Default);
            DefaultFont = new Font("Comic", 14, FontStyle.Bold, GraphicsUnit.Pixel);
            //TextFont = new Font("Arial", 14, FontStyle.Regular, GraphicsUnit.Pixel);
        }

        public static void Print (string text, Font font, Color color)
        {
            printer.Print(text, font, color);
        }
        public static void Print(string text, Color color)
        {
            printer.Print(text, DefaultFont, color);
        }
    }
}
