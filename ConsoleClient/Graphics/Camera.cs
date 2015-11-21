using ConsoleClient.Misc;
using System.Drawing;

namespace ConsoleClient.Graphics
{
    class Camera
    {
        public float X { get; set; }
        public float Y { get; set; }

        public float Velocity { get; set; }

        public Camera()
        {
            Velocity = 2.0f;
        }

        public PointF Location
        {
            get { return new PointF(X + ScreenOffset.X, Y + ScreenOffset.Y); }
            set
            {
                X = value.X;
                Y = value.Y;
            }
        }

        public PointF CenteredLocation
        {
            get
            {
                return new PointF(X + (1024f / 2f) + ScreenOffset.X, Y + (768f / 2f) + ScreenOffset.Y);
            }
            set
            {
                X = (1024f/2f) - value.X;
                Y = (768f/2f) - value.Y;
            }
        }

        public PointF ScreenOffset { get; set; }

        public void Move(Direction direction)
        {
            switch (direction)
            {
                case Direction.Up:
                    Y += Velocity;
                    break;
                case Direction.Down:
                    Y -= Velocity;
                    break;
                case Direction.Left:
                    X += Velocity;
                    break;
                case Direction.Right:
                    X -= Velocity;
                    break;
            }
        }
    }
}
