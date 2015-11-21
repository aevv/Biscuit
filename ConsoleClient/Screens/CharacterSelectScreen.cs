using ConsoleClient.Forms;
using ConsoleClient.Graphics;
using ConsoleClient.Graphics.UI;
using ConsoleClient.Misc;
using ConsoleClient.Network;
using ConsoleClient.Properties;
using ConsoleClient.Users;
using Logging;
using OpenTK;
using OpenTK.Graphics;
using Packets;
using Packets.Attributes;
using Packets.Client.Character;
using Packets.Server.Character;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace ConsoleClient.Screens
{
    class CharacterSelectScreen : BaseScreen
    {
        private bool _requestedCharacters;
        private List<Character> _characters;
        private List<Text> _charTexts;
        private List<Button> _delButtons;
        private int _charCount;
        private Button _charCreateButton;

        public CharacterSelectScreen(Game game, string name)
            : base(game, name)
        {
        }

        public override void OnLoad(EventArgs args)
        {
            base.OnLoad(args);
            Camera = new Camera();
            _characters = new List<Character>();
            _charTexts = new List<Text>();
            _delButtons = new List<Button>();

            _charCreateButton = new Button(new PointF(100, 200), new SizeF(250, 100), "assets/gfx/buttons/play.png",
                () =>
                {
                    if (!_game.FormManager.HasForm<CharCreateForm>())
                    {
                        _game.FormManager.OpenOrGetForm<CharCreateForm>().Run();
                    }
                }) {Camera = Camera};

            Camera.ScreenOffset = new PointF(200, 200);
            _game.Connection.AddSubscriber(this);
        }

        public override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);

            _charCreateButton.OnUpdateFrame(args, _newMouseState, _newKeyState, LeftClick());

            if (!_requestedCharacters)
            {
                RequestCharacters();
            }

            foreach (var t in _charTexts)
            {
                t.OnUpdateFrame(args);
            }

            foreach (var b in _delButtons)
                b.OnUpdateFrame(args, _newMouseState, _newKeyState, LeftClick());

            var num = NumberedKeyPress();
            if (num != -1)
            {
                if (_characters.Count >= num)
                {
                    Select(num);
                }
            }
        }

        public override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);

            _charCreateButton.OnRenderFrame(args);

            // TODO: Cross threaded list safety.
            for (int x = 0; x < _charTexts.Count; x++)
            {
                _charTexts[x].OnRenderFrame(args);
            }

            foreach (var b in _delButtons) //_delButtons throws null reference
            {
                b.OnRenderFrame(args);
            }
        }

        [PacketMethod(typeof (SelectableCharacterPacket))]
        public void OnSelectableCharacter(Packet packet)
        {
            var p = (SelectableCharacterPacket)packet;
            Out.Green(string.Format("Name: {0}, Id: {1}", p.Name, p.Id));

            Character character = new Character
            {
                Name = p.Name,
                Id = p.Id
            };

            Text text = new Text(colour: Color4.White, shadow: true, location: new PointF(600, _charCount * 75));
            text.Camera = Camera;
            _characters.Add(character);
            _charTexts.Add(text);
            _deferralQueue.Enqueue(new QueuedFunction
            {
                Parameters = new List<object> { _charCount, character },
                Function = (args) => text.Value = string.Format("{0} - level x something ({1})", args[1], args[0])
            });
            _deferralQueue.Enqueue(new QueuedFunction
            {
                Parameters = new List<object> { _charCount, p.Name, p.Id },
                Function = args => _delButtons.Add(new Button(new PointF(300, (int)(args[0]) * 75),
                    new SizeF(250, 100),
                    "assets/gfx/buttons/options.png", () =>
                        PacketWriter.WritePacketAsync(new DeleteCharacterPacket { Id = (Guid)args[2], Name = (string)args[1] }, _game.Connection.Writer)))
            });
            _charCount++;
            
        }

        [PacketMethod(typeof (CharacterSelectionResultPacket))]
        public void OnSelectionResult(Packet packet)
        {
            var p = (CharacterSelectionResultPacket)packet;
            if (p.Success)
            {
                _game.SwitchScreen("World");
            }
        }

        [PacketMethod(typeof (CharacterDeletionResultPacket))]
        public void OnDeletionResult(Packet packet)
        {
            RequestCharacters();
        }

        private void Select(int number)
        {
            PacketWriter.WritePacketAsync(new SelectCharacterPacket { Id = _characters[number].Id }, _game.Connection.Writer);
        }

        private void RequestCharacters()
        {
            _requestedCharacters = true;
            PacketWriter.WritePacketAsync(new RequestCharactersPacket(), _game.Connection.Writer);
            _charTexts.Clear();
            _delButtons.Clear();
            _charCount = 0;
        }
    }
}
