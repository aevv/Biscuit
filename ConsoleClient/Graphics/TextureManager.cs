using System.Drawing.Text;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using PixelFormat = System.Drawing.Imaging.PixelFormat;

namespace ConsoleClient.Graphics
{
    /// <summary>
    /// Class to manage texture loading and persistance, creating shared texture usage across all drawable objects.
    /// </summary>
    static class TextureManager
    {
        private static Dictionary<string, int> _textures = new Dictionary<string, int>();
        private static Dictionary<string, int> _textureReferences = new Dictionary<string, int>();
        private static Dictionary<string, SizeF> _textureSizes = new Dictionary<string, SizeF>();

        /// <summary>
        /// Attempt to unload a texture. If the texture is used by more than one object, will not fully unload.
        /// </summary>
        /// <param name="path">Path to the image representing the texture.</param>
        public static void UnloadImage(string path)
        {
            if (!_textureReferences.ContainsKey(path) || (--_textureReferences[path] != 0)) return;
            
            GL.DeleteTexture(_textures[path]);
            _textureReferences.Remove(path);
            _textures.Remove(path);
            _textureSizes.Remove(path);
        }

        /// <summary>
        /// Provides a handle to a generated texture.
        /// </summary>
        /// <param name="path">Path of image to generate texture for.</param>
        /// <returns>Int32 Handle to the texture.</returns>
        public static int LoadImage(string path)
        {
            if (_textures.ContainsKey(path))
            {
                _textureReferences[path]++;
                return _textures[path];
            }
            Bitmap bitmap = null;

            if (File.Exists(path))
            {
                bitmap = new Bitmap(Bitmap.FromFile(path));
            }
            else
            {
                return -1;
            }

            var textureId = GenerateFromBitmap(bitmap);
            _textures.Add(path, textureId);
            _textureReferences.Add(path, 1);
            _textureSizes.Add(path, new SizeF(bitmap.Size.Width, bitmap.Size.Height));

            bitmap.Dispose();

            return textureId;
        }

        public static SizeF GetTextureSize(string path)
        {
            return _textureSizes.ContainsKey(path) ? _textureSizes[path] : default(SizeF);
        }

        /// <summary>
        /// Create a texture of the given text in the given font.
        /// </summary>
        /// <param name="text">Text to display.</param>
        /// <param name="font">Font to display the text as.</param>
        /// <param name="size">Provides the size of the texture.</param>
        /// <returns>Int32 Handle to the texture.</returns>
        public static int CreateTextTexture(string text, Font font, out SizeF size)
        {
            int textureId;
            size = GetStringSize(text, font);

            using (Bitmap textBitmap = new Bitmap((int)size.Width, (int)size.Height))
            {
                GL.GenTextures(1, out textureId);
                GL.BindTexture(TextureTarget.Texture2D, textureId);
                BitmapData data =
                    textBitmap.LockBits(new Rectangle(0, 0, textBitmap.Width, textBitmap.Height),
                        ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, data.Width, data.Height, 0,
                    OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter,
                    (int)TextureMinFilter.Linear);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter,
                    (int)TextureMagFilter.Linear);
                GL.Finish();
                textBitmap.UnlockBits(data);

                var gfx = System.Drawing.Graphics.FromImage(textBitmap);
                gfx.Clear(Color.Transparent);
                gfx.TextRenderingHint = TextRenderingHint.AntiAlias;
                gfx.DrawString(text, font, new SolidBrush(Color.White), new RectangleF(0, 0, size.Width + 10, size.Height));

                BitmapData data2 = textBitmap.LockBits(new Rectangle(0, 0, textBitmap.Width, textBitmap.Height),
                    ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
                GL.TexSubImage2D(TextureTarget.Texture2D, 0, 0, 0, textBitmap.Width, textBitmap.Height, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data2.Scan0);
                textBitmap.UnlockBits(data2);
                gfx.Dispose();
            }

            return textureId;
        }

        private static SizeF GetStringSize(string text, Font font)
        {
            using (Bitmap temp = new Bitmap(1, 1))
            {
                System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(temp);
                SizeF temp2 = g.MeasureString(text, font);
                return temp2;
            }
        }

        private static int GenerateFromBitmap(Bitmap bitmap)
        {
            try
            {
                int texture;

                GL.Hint(HintTarget.PerspectiveCorrectionHint, HintMode.Nicest);
                GL.GenTextures(1, out texture);
                GL.BindTexture(TextureTarget.Texture2D, texture);

                BitmapData data = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                    ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, data.Width, data.Height, 0,
                    OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);

                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
                GL.Finish();

                bitmap.UnlockBits(data);

                return texture;
            }
            catch (Exception)
            {
                return -1;
            }
        }
    }
}
