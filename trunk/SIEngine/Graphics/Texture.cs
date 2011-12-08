using System;
using System.Linq;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using System.Drawing.Imaging;
using SIEngine.BaseGeometry;
using System.IO;

using PixelFormat = OpenTK.Graphics.OpenGL.PixelFormat;

namespace SIEngine
{
    namespace Graphics
    {
        /// <summary>
        /// Represents a 2D OpenGL texture.
        /// </summary>
        public class Texture
        {
            /// <summary>
            /// The texture ID.
            /// </summary>
            public int Location { get; set; }
            /// <summary>
            /// The size of the texture.
            /// </summary>
            public Vector Size { get; set; }

            #region constructors
            public Texture(TextureMinFilter minFiler, TextureMagFilter magFilter)
            {
                GL.Enable(EnableCap.Texture2D);
                this.Location = GL.GenTexture();
                SetTextureParameters(minFiler, magFilter);

                Other.Fixes.ApplyTextureBugFix();
            }

            /// <summary>
            /// Initializes an empty texture..
            /// </summary>
            /// <param name="magFilter">The magnification filter.</param>
            /// <param name="minFiler">The min filter.</param>
            /// <param name="width">The width of the texture.</param>
            /// <param name="height">The height of the texture</param>
            public Texture(TextureMinFilter minFiler, TextureMagFilter magFilter, int width, int height)
            {
                GL.Enable(EnableCap.Texture2D);
                this.Location = GL.GenTexture();
                SetTextureParameters(minFiler, magFilter);
                GL.BindTexture(TextureTarget.Texture2D, this.Location);
                EmptyTexture(width, height);

                Other.Fixes.ApplyTextureBugFix();
            }

            /// <summary>
            /// Loads the texture from a file.
            /// </summary>
            /// <param name="path">The path of the texture.</param>
            public Texture (string path, TextureMinFilter minFilter, TextureMagFilter magFilter)
            {
                GL.Enable(EnableCap.Texture2D);
                this.Location = GL.GenTexture();
                SetTextureParameters(minFilter, magFilter);
                SelectTexture();
                LoadTextureFromFile(path);

                Other.Fixes.ApplyTextureBugFix();
            }

            public Texture (string path)
            {
                GL.Enable(EnableCap.Texture2D);
                this.Location = GL.GenTexture();
                SetTextureParameters(TextureMinFilter.Nearest, TextureMagFilter.Linear);
                SelectTexture();
                LoadTextureFromFile(path);

                Other.Fixes.ApplyTextureBugFix();
            }
            #endregion

            #region texture loading

            /// <summary>
            /// Allocates an empty texture.
            /// </summary>
            /// <param name="width">The width of the texture.</param>
            /// <param name="height">The height of the texture.</param>
            public void EmptyTexture (int width, int height)
            {
                this.Size.X = width;
                this.Size.Y = height;
                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba8, width, height,
                    0, PixelFormat.Rgba, PixelType.UnsignedByte, IntPtr.Zero);
            }

            /// <summary>
            /// Gets the format of the image to load.
            /// </summary>
            /// <param name="path">The path of the image.</param>
            public void LoadTextureFromFile (string path)
            {
                try
                {
                    switch (Path.GetExtension(path))
                    {
                        case ".bmp":
                            LoadImageBMP(path);
                            break;
                        case ".jpg":
                        case ".JPG":
                        case ".png":
                            LoadImageJPG(path);
                            //LoadImageTarga(path);
                            break;
                    }
                }
                catch (Exception exc)
                {

                }
            }

            /// <summary>
            /// Loads an image from a BMP file.
            /// </summary>
            /// <param name="path">The path of the image.</param>
            public void LoadImageBMP (string path)
            {
                Bitmap bmp = new Bitmap(path);
                BitmapData data = bmp.LockBits(new System.Drawing.Rectangle(0, 0, bmp.Width, bmp.Height),
                    ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                this.Size = new Vector(bmp.Width, bmp.Height);
                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bmp.Width, bmp.Height,
                        0, PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
                bmp.UnlockBits(data);
                bmp = null;

                GC.Collect();
            }

            public void LoadImageJPG (string path)
            {
                Image image = Image.FromFile(path);
                image.RotateFlip(RotateFlipType.RotateNoneFlipY);
                path = Path.ChangeExtension(path, ".bmp");
                image.Save(path, ImageFormat.Bmp);
                image = null;
                LoadImageBMP(path);
            }

            /// <summary>
            /// Sets texture parameters. Only for internal use.
            /// </summary>
            /// <param name="minFiler">The min filter.</param>
            /// <param name="magFilter">The magnif filter.</param>
            private void SetTextureParameters (TextureMinFilter minFiler, TextureMagFilter magFilter)
            {
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter,
                    (int)minFiler);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter,
                    (int)magFilter);
            }
            #endregion

            /// <summary>
            /// Binds the texture.
            /// </summary>
            public void SelectTexture ()
            {
                GL.BindTexture(TextureTarget.Texture2D, this.Location);
            }
        }
    }
}