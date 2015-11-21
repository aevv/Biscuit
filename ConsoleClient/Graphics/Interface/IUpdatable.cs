using OpenTK;
using OpenTK.Input;
using System.Collections.Generic;
using MouseState = ConsoleClient.Input.MouseState;

namespace ConsoleClient.Graphics.Interface
{
    interface IUpdatable
    {
        void OnUpdateFrame(FrameEventArgs args, MouseState mouseState, List<Key> keyboardState, bool leftClick, bool rightClick, bool middleClick);

    }

}
