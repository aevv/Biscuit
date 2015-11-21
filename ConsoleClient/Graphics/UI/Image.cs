using OpenTK.Graphics;
using System.Drawing;

namespace ConsoleClient.Graphics.UI
{
    class Image : Drawable
    {
        public Image(PointF location, SizeF size, string path)
        {
            Location = location;
            Size = size;
            Colour = Color4.White;
            SetTexture(path);
        }
    }
}
