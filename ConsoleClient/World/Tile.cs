using ConsoleClient.Graphics;
using OpenTK.Graphics;
using System;
using System.Drawing;

namespace ConsoleClient.World
{
    internal class Tile : Drawable
    {
        public bool CanWalk { get; set; }

        public Tile(float x, float y, float width, float height, string texture, int tileX, int tileY, bool canWalk = true)
        {
            Location = new PointF(x, y);
            Size = new SizeF(width, height);
            if (tileX == -1 && tileY == -1)
            {
                Colour = Color4.Transparent;
            }
            else
            {
                Colour = Color4.White;
                SetTexture(texture);
            }
            SubDrawableSize = new SizeF(32, 32);
            SubDrawableLocation = new PointF((float)tileX * 32, (float)tileY * 32);
            CanWalk = canWalk;
        }
    }
}
