using ConsoleClient.Graphics;
using ConsoleClient.Misc;
using ConsoleClient.Screens.Interface;
using OpenTK;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using Packets.Infrastructure;
using MouseState = ConsoleClient.Input.MouseState;

namespace ConsoleClient.Screens
{
    abstract class BaseScreen : PacketSubscriber, IScreen
    {
        protected List<Key> _oldKeyState, _newKeyState;
        protected MouseState _oldMouseState, _newMouseState;
        protected Game _game;
        protected Queue<QueuedFunction> _deferralQueue; 

        public string Name { get; private set; }
        public bool Enabled { get; private set; }
        public bool Visible { get; private set; }
        public bool Loaded { get; private set; }
        public Camera Camera { get; protected set; }

        public BaseScreen(Game game, string name)
        {
            _game = game;
            Name = name;
            _oldKeyState = new List<Key>();
            _newKeyState = new List<Key>();
            _deferralQueue = new Queue<QueuedFunction>();
        }

        public virtual void Dispose()
        {
            _game = null;
        }

        public virtual void OnRenderFrame(FrameEventArgs args)
        {
            
        }

        /// <summary>
        /// Call this base OnUpdate before any processing on child for valid input states.
        /// </summary>
        /// <param name="args"></param>
        public virtual void OnUpdateFrame(FrameEventArgs args)
        {
            _oldMouseState = _newMouseState;
            _oldKeyState = _newKeyState;
            _newKeyState = _game.KeyboardState;
            _newMouseState = _game.MouseState;

            while (_deferralQueue.Count > 0)
            {
                var func = _deferralQueue.Dequeue();
                func.Function(func.Parameters);
            }
        }

        public virtual void OnLoad(EventArgs args)
        {
            
        }

        public void Show()
        {
            Visible = true;
            Enabled = true;
        }

        public void Hide()
        {
            Visible = false;
            Enabled = false;
        }

        #region Input helpers

        protected bool KeyPress(Key k)
        {
            return _newKeyState.Contains(k) && !_oldKeyState.Contains(k);
        }

        protected bool KeyHeld(Key k)
        {
            return _newKeyState.Contains(k) && _oldKeyState.Contains(k);
        }

        protected bool KeyDown(Key k)
        {
            return _newKeyState.Contains(k);
        }

        protected bool KeyUp(Key k)
        {
            return !_newKeyState.Contains(k);
        }

        protected int NumberedKeyPress()
        {
            for (int x = 109; x < 119; x ++)
            {
                if (_newKeyState.Contains((Key) x) && !_oldKeyState.Contains((Key) x))
                    return x - 109;
            }
            return -1;
        }

        protected Direction ArrowKeyPress()
        {
            for (int x = 45; x < 49; x++)
            {
                if (_newKeyState.Contains((Key) x) && !_oldKeyState.Contains((Key) x))
                    return (Direction) x;
            }
            return Direction.None;
        }

        protected Direction ArrowKeyHold()
        {
            for (int x = 45; x < 49; x++)
            {
                if (_newKeyState.Contains((Key)x) && _oldKeyState.Contains((Key)x))
                    return (Direction)x;
            }
            return Direction.None;
        }

        protected Direction[] ArrowKeyPresses()
        {
            List<Direction> dirs = new List<Direction>();
            for (int x = 45; x < 49; x++)
            {
                if (_newKeyState.Contains((Key)x) && !_oldKeyState.Contains((Key)x))
                    dirs.Add((Direction)x);
            }
            return dirs.ToArray();
        }

        protected Direction[] ArrowKeyHolds()
        {
            List<Direction> dirs = new List<Direction>();
            for (int x = 45; x < 49; x++)
            {
                if (_newKeyState.Contains((Key)x) && _oldKeyState.Contains((Key)x))
                    dirs.Add((Direction)x);
            }
            return dirs.ToArray();
        }

        protected bool LeftClick()
        {
            return _newMouseState.LeftButton && !_oldMouseState.LeftButton;
        }

        protected bool LeftHeld()
        {
            return _newMouseState.LeftButton && _oldMouseState.LeftButton;
        }

        protected bool RightClick()
        {
            return _newMouseState.LeftButton && !_oldMouseState.LeftButton;
        }

        protected bool RightHeld()
        {
            return _newMouseState.LeftButton && _oldMouseState.RightButton;
        }
        #endregion
    }
}
