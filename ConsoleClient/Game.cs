using ConsoleClient.Chat;
using ConsoleClient.Config;
using ConsoleClient.Forms;
using ConsoleClient.Graphics;
using ConsoleClient.Network;
using ConsoleClient.Properties;
using ConsoleClient.Screens;
using ConsoleClient.Screens.Interface;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using Packets;
using Packets.Client.Chat;
using Packets.Server.Login;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using MouseState = ConsoleClient.Input.MouseState;

namespace ConsoleClient
{
    class Game : GameWindow
    {
        private readonly Connection _connection;
        private readonly FormManager _formManager;
        private readonly ChatManager _chatManager;
        private readonly ScreenManager _screenManager;
        private readonly ConfigManager<ConfigMappings> _configManager;

        private readonly List<Key> _keysPressed;
        private MouseState _mouseState;
        private Text _fpsText;
        private double _fpsTime;
        private int _frameCount;
        private bool _screenTransition;
        private double _transitionTimer = 0;
        private IScreen _oldScreen;
        private IScreen _newScreen;

        private bool _running = true;

        public User User { get; set; }
        public double Version { get; set; }
        public Connection Connection { get { return _connection; } }
        public ConfigManager<ConfigMappings> ConfigManager { get { return _configManager; } }
        public FormManager FormManager { get { return _formManager; } }
        public MouseState MouseState { get { return _mouseState; } }
        public ScreenManager ScreenManager { get { return _screenManager; } }
        public List<Key> KeyboardState { get { return _keysPressed.Select(s => s).ToList(); } }
        public Camera MainCamera { get; set; }

        public void SetTitle(string text)
        {
            Title = string.Format("Would you risk it... for a biscuit? ~ {0}", text);
        }

        public Game(Connection connection, FormManager manager, ConfigManager<ConfigMappings> config, double version = 0) :
            base(1024, 768, GraphicsMode.Default, "Would you risk it... for a biscuit? ~ ")
        {
            VSync = VSyncMode.Off;
            _connection = connection;
            _formManager = manager;
            _screenManager = ScreenManager.Resolve(this);
            _chatManager = new ChatManager(this);
            _configManager = config;
            _connection.AddSubscriber(_chatManager);
            Version = version;

            _keysPressed = new List<Key>();
            _mouseState = new MouseState();

            Mouse.ButtonDown += MouseButtonDown;
            Mouse.ButtonUp += MouseButtonUp;
            Keyboard.KeyDown += KeyboardButtonDown;
            Keyboard.KeyUp += KeyboardButtonUp;

            Closing += (sender, args) => PacketWriter.WritePacketAsync(new LogoutPacket(), _connection.Writer);
        }

        public T OpenForm<T>(string criteria = "") where T : BaseForm
        {
            return _formManager.OpenOrGetForm<T>(criteria);
        }

        public void GivePacket(Packet packet)
        {
            // Shouldn't listen to any right now
        }

        private void LoadScreens(EventArgs e)
        {
            MainCamera = new Camera();

            _screenManager.Active = new SplashScreen(this, "Splash");
            _screenManager.Active.OnLoad(e);

            _screenManager.AddScreen(new MainMenuScreen(this, "Main Menu"));
            _screenManager["Main Menu"].OnLoad(e);

            _screenManager.AddScreen(new WorldScreen(this, "World"));
            _screenManager["World"].OnLoad(e);

            _screenManager.AddScreen(new CharacterSelectScreen(this, "Chars"));
            _screenManager["Chars"].OnLoad(e);

            //_screenManager.AddScreen(new VBOTest(this, "VBO"));
            //_screenManager["VBO"].OnLoad(e);
            //_screenManager.Active = _screenManager["VBO"];
        }

        #region Input
        private void MouseButtonDown(object sender, MouseButtonEventArgs args)
        {
            switch (args.Button)
            {
                case MouseButton.Left:
                    _mouseState.LeftButton = true;
                    break;
                case MouseButton.Right:
                    _mouseState.RightButton = true;
                    break;
                case MouseButton.Middle:
                    _mouseState.MiddleButton = true;
                    break;
            }
        }

        private void MouseButtonUp(object sender, MouseButtonEventArgs args)
        {
            switch (args.Button)
            {
                case MouseButton.Left:
                    _mouseState.LeftButton = false;
                    break;
                case MouseButton.Right:
                    _mouseState.RightButton = false;
                    break;
                case MouseButton.Middle:
                    _mouseState.MiddleButton = false;
                    break;
            }
        }

        private void KeyboardButtonDown(object sender, KeyboardKeyEventArgs args)
        {
            _keysPressed.Add(args.Key);
        }

        private void KeyboardButtonUp(object sender, KeyboardKeyEventArgs args)
        {
            _keysPressed.Remove(args.Key);
        }
        #endregion

        #region OpenTK Methods
        protected override void OnLoad(EventArgs e)
        {
            GL.Enable(EnableCap.Texture2D);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);

            GL.EnableClientState(ArrayCap.VertexArray);

            GL.ClearColor(Color4.SlateGray);

            LoadScreens(e);
            _fpsText = new Text(location: new PointF(900f, 730f), colour: Color4.Yellow);

            // TODO: Move this sort of stuff to a loading screen.
            TextureManager.LoadImage(Resources.TILESET_2);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            _frameCount++;
            _fpsTime += e.Time;

            if (ConfigManager.TryGet(c => c.DisplayFPS))
            {
                if (_fpsTime > 0.125)
                {
                    _fpsTime = 0;
                    _fpsText.Value = string.Format("fps_{0}", _frameCount * 8);
                    _frameCount = 0;
                }
            }

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            Matrix4 ortho = Matrix4.CreateOrthographicOffCenter(0, Width, Height, 0, -1, 1);
            GL.MatrixMode(MatrixMode.Projection);
            GL.PushMatrix();
            GL.LoadMatrix(ref ortho);

            if (_screenTransition)
            {
                _oldScreen.OnRenderFrame(e);
                _newScreen.OnRenderFrame(e);
            }
            else
            {
                _screenManager.Active.OnRenderFrame(e);
            }

            _fpsText.OnRenderFrame(e);

            GL.PopMatrix();
            SwapBuffers();
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
            _mouseState.X = Mouse.X;
            _mouseState.Y = Mouse.Y;

            if (!_screenTransition)
            {
                _screenManager.Active.OnUpdateFrame(e);
            }
            else
            {
                _transitionTimer -= e.Time;

                if (_transitionTimer < 0)
                {
                    _transitionTimer = 0;
                }

                var oldO = _oldScreen.Camera.ScreenOffset;
                var newO = _newScreen.Camera.ScreenOffset;

                var move = _transitionTimer/0.5;
                var realMove = move*1024d;

                _oldScreen.Camera.ScreenOffset = new PointF((float) (-1024 + realMove), oldO.Y);
                _newScreen.Camera.ScreenOffset = new PointF((float) (0 + realMove), newO.Y);

                if (_transitionTimer == 0)
                {
                    _screenTransition = false;
                    _screenManager.Active = _newScreen;
                    _newScreen = null;
                    _oldScreen = null;
                }
            }

        }

        public void SwitchScreen(string name)
        {
            _screenTransition = true;
            _oldScreen = _screenManager.Active;
            _newScreen = _screenManager[name];
            _newScreen.Camera.ScreenOffset = new PointF(_oldScreen.Camera.ScreenOffset.X - 1024f, 0);
            _transitionTimer = 0.5;
        }
        #endregion
    }
}
