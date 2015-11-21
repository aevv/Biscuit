using ConsoleClient.Graphics.Interface;
using ConsoleClient.Graphics.UI;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System.Drawing;

namespace ConsoleClient.Graphics
{
    /// <summary>
    /// Drawable abstract class. Lets something be rendered easily with a set of common arguments.
    /// </summary>
    abstract class Drawable : IDrawable
    {
        public PointF Location { get; set; }
        public PointF CameraRelative {
            get
            {
                if (Camera == null)
                    return Location;

                return new PointF(Location.X + Camera.Location.X, Location.Y + Camera.Location.Y);
            }
        }
        public PointF DrawOffset { get; set; }
        public PointF SubDrawableLocation { get; set; }
        public Color4 Colour { get; set; }
        public SizeF Size { get; set; }
        public SizeF SubDrawableSize { get; set; }
        public float Rotation { get; set; }
        public Camera Camera { get; set; }
        public bool Textured { get; private set; }

        protected SizeF TextureSize { get; set; }

        public bool CanDraw
        {
            get
            {
                float xCam = 0f, yCam = 0f;

                if (Camera != null)
                {
                    xCam = Camera.X;
                    yCam = Camera.Y;
                }

                var x = Location.X;
                var y = Location.Y;

                var xMin = x + xCam;
                var xMax = x + Size.Width + xCam;
                var yMin = y + yCam;
                var yMax = y + Size.Height + yCam;

                if (xMax < 0 || yMax < 0)
                    return false;
                return !(xMin > 1024f) && !(yMin > 768f);
            }
        }

        protected int TextureId;
        private string _path;

        /// <summary>
        /// Used to render our drawable object.
        /// </summary>
        /// <param name="ea"></param>
        /// <param name="cameraRelative"></param>
        public bool OnRenderFrame(FrameEventArgs ea, bool cameraRelative = true)
        {
            if (!CanDraw)
                return false;

            GL.PushMatrix();
            GL.Color4(Colour);

            GL.Translate(Location.X + (Size.Width / 2), Location.Y + (Size.Height / 2), 0);
            GL.Rotate(Rotation, 0, 0, 1);
            GL.Translate(-Location.X - (Size.Width / 2), -Location.Y - (Size.Height / 2), 0);

            float x = Location.X;
            float y = Location.Y;

            if (cameraRelative && Camera != null)
            {
                x += Camera.X + Camera.ScreenOffset.X;
                y += Camera.Y + Camera.ScreenOffset.Y;
            }

            if (!DrawOffset.IsEmpty)
            {
                x += DrawOffset.X;
                y += DrawOffset.Y;
            }

            if (Textured)
            {
                float subX = 0, subY = 0, subH = 1, subW = 1;

                if (!SubDrawableSize.IsEmpty)
                {
                    subX = SubDrawableLocation.X == 0 ? 0 : SubDrawableLocation.X/TextureSize.Width;
                    subY = SubDrawableLocation.Y == 0 ? 0 : SubDrawableLocation.Y/TextureSize.Height;
                    subW = SubDrawableSize.Width == 0 ? 1 : 1 / SubDrawableSize.Width;
                    subH = SubDrawableSize.Height == 0 ? 1 : 1 / SubDrawableSize.Height;
                }

                GL.BindTexture(TextureTarget.Texture2D, TextureId);

                GL.Begin(BeginMode.Quads);
                GL.TexCoord2(subX, subY);
                GL.Vertex2(x, y);
                GL.TexCoord2(subX + subW, subY);
                GL.Vertex2(x + Size.Width, y);
                GL.TexCoord2(subX + subW, subY + subH);
                GL.Vertex2(x + Size.Width, y + Size.Height);
                GL.TexCoord2(subX, subY + subH);
                GL.Vertex2(x, y + Size.Height);
                GL.End();
            }
            else
            {
                GL.Disable(EnableCap.Texture2D);

                GL.Begin(BeginMode.Quads);
                GL.Vertex2(x, y);
                GL.Vertex2(x + Size.Width, y);
                GL.Vertex2(x + Size.Width, y + Size.Height);
                GL.Vertex2(x, y + Size.Height);
                GL.End();

                GL.Enable(EnableCap.Texture2D);
            }

            GL.PopMatrix();

            return true;
        }

        /// <summary>
        /// Set the texture for this drawable.
        /// </summary>
        /// <param name="filePath">Path of image to load texture from.</param>
        public void SetTexture(string filePath)
        {
            TextureId = TextureManager.LoadImage(filePath);

            if (TextureId == -1) return;
            Textured = true;
            _path = filePath;
            TextureSize = TextureManager.GetTextureSize(filePath);
        }

        /// <summary>
        /// Delete the texture from this drawable.
        /// </summary>
        public void DeleteTexture()
        {
            if (!Textured) return;
            TextureManager.UnloadImage(_path);
            TextureId = -1;
        }
    }
}
