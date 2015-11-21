using ConsoleClient.Mech;
using ConsoleClient.Misc;
using OpenTK.Graphics;
using System;
using System.Drawing;

namespace ConsoleClient.Users
{
    sealed class Character : Entity
    {
        public string Name { get; set; }
        public Guid Id { get; set; }

        public float X
        {
            get { return Location.X; }
            set { Location = new PointF(value, Location.Y); }
        }

        public float Y
        {
            get { return Location.Y; }
            set { Location = new PointF(Location.X, value); }
        }

        public float Velocity { get; set; }

        public void Move(Direction direction)
        {
            switch (direction)
            {
                case Direction.Up:
                    Y -= Velocity;
                    break;
                case Direction.Down:
                    Y += Velocity;
                    break;
                case Direction.Left:
                    X -= Velocity;
                    break;
                case Direction.Right:
                    X += Velocity;
                    break;
            }
        }

        public Character()
            : this(new PointF(0, 0), 2.0f)
        {
        }

        public Character(PointF location, float velocity)
        {
            Size = new SizeF(32, 64);
            DrawOffset = new PointF(0, -48);
            Location = location;
            Velocity = velocity;
            Colour = Color4.White;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
