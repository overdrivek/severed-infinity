using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Graphics.OpenGL;

namespace SIEngine.Other
{
    public static class Fixes
    {
        public static void ApplyTextureBugFix()
        {
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter,
                       (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter,
                (int)TextureMagFilter.Linear);
        }
    }
}
