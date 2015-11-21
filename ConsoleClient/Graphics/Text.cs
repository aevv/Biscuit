using System.Security.Cryptography.X509Certificates;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System;
using System.Drawing;

namespace ConsoleClient.Graphics
{
    class Text
    {
        private int _textureId;
        private string _text;
        public SizeF Size { get; private set; }
        public PointF Location { get; set; }
        public Color4 Colour { get; set; }
        public bool Shadow { get; set; }
        public Font Font { get; set; }
        public Camera Camera { get; set; }
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
                if (xMin > 1024f || yMin > 768f)
                    return false;

                return true;
            }
        }
        public string Value {
            get { return _text; }
            set
            {
                _text = value;
                SizeF size;
                _textureId = TextureManager.CreateTextTexture(_text, Font, out size);
                Size = size;
            }}

        public Text(string text = "", PointF location = default(PointF), Color4 colour = default(Color4), bool shadow = false)
        {
            Font = new Font("Calibri", 20);
            if (!string.IsNullOrEmpty(text))
            {
                SizeF size;
                _textureId = TextureManager.CreateTextTexture(text, Font, out size);
                Size = size;
            }
            Colour = colour;
            Location = location;
            Shadow = shadow;
        }

        public Text(Func<SizeF, PointF> location, string text = "", Color4 colour = default(Color4), bool shadow = false) : this(text, colour: colour, shadow: shadow)
        {
            Location = location(Size);
        }

        public void OnRenderFrame(FrameEventArgs args, bool cameraRelative = true)
        {
            GL.PushMatrix();

            GL.BindTexture(TextureTarget.Texture2D, _textureId);
            GL.LineWidth(2F);

            float x = Location.X;
            float y = Location.Y;



            if (cameraRelative && Camera != null)
            {
                x += Camera.X + Camera.ScreenOffset.X;
                y += Camera.Y + Camera.ScreenOffset.Y;
            }

            if (Shadow)
            {
                GL.Color4(0.0, 0.0, 0.0, 1.0f);
                GL.Begin(BeginMode.Quads);
                GL.TexCoord2(0, 0);
                GL.Vertex2(x + 1, y + 1);
                GL.TexCoord2(1, 0);
                GL.Vertex2(x + 1 + Size.Width, y + 1);
                GL.TexCoord2(1, 1);
                GL.Vertex2(x + 1 + Size.Width, y + 1 + Size.Height);
                GL.TexCoord2(0, 1);
                GL.Vertex2(x + 1, y + 1 + Size.Height);
                GL.End();
            }

            GL.Color4(Colour.R, Colour.G, Colour.B, 1.0f);
            GL.Begin(BeginMode.Quads);
            GL.TexCoord2(0, 0);
            GL.Vertex2(x, y);
            GL.TexCoord2(1, 0);
            GL.Vertex2(x + Size.Width, y);
            GL.TexCoord2(1, 1);
            GL.Vertex2(x + Size.Width, y + Size.Height);
            GL.TexCoord2(0, 1);
            GL.Vertex2(x, y + Size.Height);
            GL.End();

            GL.PopMatrix();
        }

        public void OnUpdateFrame(FrameEventArgs args)
        {
            
        }
    }
}
