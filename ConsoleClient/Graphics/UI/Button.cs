using ConsoleClient.Graphics.Interface;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Drawing;
using MouseState = ConsoleClient.Input.MouseState;

namespace ConsoleClient.Graphics.UI
{
    class Button : Drawable, IUpdatable
    {

        private readonly Action _action;

        public void OnUpdateFrame(FrameEventArgs args, MouseState mouseState, List<Key> keyboardState, 
            bool leftClick = false, bool rightClick = false, bool middleClick = false)
        {
            if (mouseState.X > CameraRelative.X && mouseState.X < CameraRelative.X + Size.Width)
            {
                if (mouseState.Y > CameraRelative.Y && mouseState.Y < CameraRelative.Y + Size.Height)
                {
                    if (leftClick)
                    {
                        _action();
                    }
                }
            }
        }

        public Button(PointF location, SizeF size, string path, Action action)
        {
            Location = location;
            Size = size;
            Colour = Color4.White;
            SetTexture(path);
            _action = action;
        }
    }
}
