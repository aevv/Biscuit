using ConsoleClient.Audio;
using ConsoleClient.Forms;
using ConsoleClient.Graphics;
using ConsoleClient.Graphics.UI;
using OpenTK;
using System;
using System.Drawing;
using OpenTK.Input;
using Image = ConsoleClient.Graphics.UI.Image;

namespace ConsoleClient.Screens
{
    class MainMenuScreen : BaseScreen
    {
        private Drawable _bg;
        private Text _startText;
        private Text _biscuitText;
        private Button _charactersButton;
        private Button _optionsButton;
        private Button _exitButton;
        private Sound _music;

        public MainMenuScreen(Game game, string name)
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

            _charactersButton = new Button(new PointF(100, 200), new SizeF(250, 100), "assets/gfx/buttons/play.png",
                () =>
                {
                    _game.SwitchScreen("Chars");
                    _music.Stop();
                }) {Camera = Camera};

            _optionsButton = new Button(
                new PointF(100, 350), new SizeF(250, 100), "assets/gfx/buttons/options.png", () =>
                {
                    if (!_game.FormManager.HasForm<OptionsForm>())
                    {
                        _game.FormManager.OpenOrGetForm<OptionsForm>().Run();
                    }
                }) {Camera = Camera};


            _exitButton = new Button(new PointF(100, 500), new SizeF(250, 100), "assets/gfx/buttons/exit.png",
                () => _game.Exit()) {Camera = Camera};
        }

        public override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);

            var leftClick = LeftClick();

            _charactersButton.OnUpdateFrame(args, _newMouseState, _newKeyState, leftClick);
            _optionsButton.OnUpdateFrame(args, _newMouseState, _newKeyState, leftClick);
            _exitButton.OnUpdateFrame(args, _newMouseState, _newKeyState, leftClick);

            if (KeyPress(Key.Escape))
            {
                _game.SwitchScreen("Splash");
            }
        }

        public override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);

            _bg.OnRenderFrame(args);
            _charactersButton.OnRenderFrame(args);
            _optionsButton.OnRenderFrame(args);
            _exitButton.OnRenderFrame(args);
        }

        public void SetMusic(Sound music)
        {
            _music = music;
        }
    }
}
