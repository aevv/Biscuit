using ConsoleClient.Audio;
using ConsoleClient.Graphics;
using ConsoleClient.Properties;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Input;
using System;
using System.Drawing;
using Image = ConsoleClient.Graphics.UI.Image;

namespace ConsoleClient.Screens
{
    class SplashScreen : BaseScreen
    {
        private Drawable _bg;
        private Text _startText;
        private Text _biscuitText;
        private Sound _menuMusic;

        public SplashScreen(Game game, string name)
            : base(game, name)
        {
        }

        public override void OnLoad(EventArgs args)
        {
            base.OnLoad(args);

            Camera = new Camera();

            _bg = new Image(new PointF(0, 0), new SizeF(1024, 768), "assets/gfx/background/menubg.png")
            {
                Camera = Camera
            };

            _biscuitText = new Text(size => 
                                new PointF((1024f / 2f) - (size.Width / 2f), (768f / 2f) - (size.Height / 2f)),
                                                         string.Format("Biscuit Dev Build {0}", _game.Version), 
                                                                            shadow: true, colour: Color4.White)
            {
                Camera = Camera
            };

            _startText = new Text(size => 
                                new PointF((1024f / 2f) - (size.Width / 2f), 
                                     (768f / 2f) - (size.Height / 2f) + 50),
                                                    "Press escape to begin",
                                shadow: true, colour: Color4.Orange) {Camera = Camera};

            _menuMusic = AudioManager.LoadFromFile(Resources.MENU_MUSIC);
            _menuMusic.Play(true, true);
        }

        public override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);

            if (KeyPress(Key.Escape))
            {
                ((MainMenuScreen)_game.ScreenManager["Main Menu"]).SetMusic(_menuMusic);
                _game.SwitchScreen("Main Menu");
            }
        }

        public override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);

            _bg.OnRenderFrame(args);
            _biscuitText.OnRenderFrame(args);
            _startText.OnRenderFrame(args);
        }
    }
}
