using BiscuitHeaders;
using ConsoleClient.Graphics;
using ConsoleClient.Graphics.Interface;
using OpenTK;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Drawing;
using MouseState = ConsoleClient.Input.MouseState;

namespace ConsoleClient.Mech
{
    class Entity : Drawable, IUpdatable
    {
        public Guid Id { get; private set; }
        public EntityTypes EntityType { get; private set; }

        public Entity(Guid id, EntityTypes type, PointF location)
        {
            Id = id;
            EntityType = type;
            Location = location;
            Size = new SizeF(32, 64);
            DrawOffset = new PointF(0, -48);
        }

        protected Entity()
        {
            EntityType = EntityTypes.Self;
        }

        public virtual void OnUpdateFrame(FrameEventArgs args, MouseState mouseState = default(MouseState), List<Key> keyboardState = null,
            bool leftClick = false, bool rightClick = false, bool middleClick = false)
        {

        }
    }
}
