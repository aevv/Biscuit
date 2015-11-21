using ConsoleClient.Screens.Interface;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleClient.Screens
{
    class ScreenManager
    {
        private static ScreenManager _instance;
        private readonly Game _game;
        public static ScreenManager Resolve(Game game)
        {
            return _instance ?? (_instance = new ScreenManager(game));
        }

        private ScreenManager(Game game)
        {
            _screens = new List<IScreen>();
            _game = game;
        }

        private readonly List<IScreen> _screens;
        private IScreen _activeScreen;

        public IScreen this[string name]
        {
            get { return _screens.First(s => s.Name == name); }
        }

        public void AddScreen(IScreen screen)
        {
            _screens.Add(screen);

            if (_screens.Count == 1)
                _activeScreen = screen;
        }

        public void RemoveScreen(IScreen screen)
        {
            _screens.Remove(screen);
        }

        public IScreen SetActiveByName(string name)
        {
            var screen = _screens.FirstOrDefault(s => s.Name == name);

            if (screen == null) return null;
            _activeScreen = screen;
            return _activeScreen;

            // TODO: throw?
        }

        public IScreen Active
        {
            get { return _activeScreen; }
            set
            {
                if (!_screens.Contains(value))
                {
                    _screens.Add(value);
                }

                _activeScreen = value;
                _game.SetTitle(_activeScreen.Name);
            }
        }
    }
}
