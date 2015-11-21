using ConsoleClient.Graphics;
using ConsoleClient.Network;
using ConsoleClient.Users;
using ConsoleClient.World;
using Logging;
using OpenTK;
using Packets;
using Packets.Server.World;
using System;

namespace ConsoleClient.Screens
{
    class WorldScreen : BaseScreen
    {
        private Map _map;
        private Character _self;

        private double _charLocSendTime;
        private bool _hasMoved;
        private bool _mapRequested;
        public WorldScreen(Game game, string name)
            : base(game, name)
        {
            Camera = new Camera();
            _self = new Character { Camera = Camera };
            _map = new Map(game, _self) { Camera = Camera };
        }

        public override void OnLoad(EventArgs args)
        {
            base.OnLoad(args);
        }

        public override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);

            if (_map.Loaded)
            {
                _map.OnRenderFrame(args);
            }
        }

        public override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);

            if (!_mapRequested)
            {
                RequestMap();
            }

            if (_map.Loaded)
            {
                _map.OnUpdateFrame(args);
            }

            foreach (var direction in ArrowKeyHolds())
            {
                Camera.Move(direction);
                _self.Move(direction);
                _hasMoved = true;
            }

            _charLocSendTime += args.Time;
            if (_charLocSendTime > 0.025)
            {
                if (_hasMoved)
                {
                    _charLocSendTime = 0;
                    Out.Log(string.Format("{0}, {1}", _self.X, _self.Y));
                    PacketWriter.WritePacketAsync(new MoveCharacterPacket { X = _self.Location.X, Y = _self.Location.Y },
                        _game.Connection.Writer);
                    _hasMoved = false;
                }
            }
        }

        public void GivePacket(Packet packet)
        {
        }

        private void RequestMap()
        {
            PacketWriter.WritePacketAsync(new RequestMapPacket(), _game.Connection.Writer);
            _mapRequested = true;
        }
    }
}
