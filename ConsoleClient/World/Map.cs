using ConsoleClient.Graphics;
using ConsoleClient.Mech;
using ConsoleClient.Network;
using ConsoleClient.Users;
using OpenTK;
using OpenTK.Graphics;
using Packets;
using Packets.Attributes;
using Packets.Client.Map;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Packets.Infrastructure;

namespace ConsoleClient.World
{
    class Map : PacketSubscriber
    {
        private Game _game;
        private readonly Character _self;

        private readonly List<Entity> _entities;

        public Camera Camera { get; set; }

        public Map(Game game, Character character)
        {
            _game = game;
            _self = character;
            _entities = new List<Entity>();
            _game.Connection.AddSubscriber(this);
            _chunks = new List<Chunk>();
        }

        public string Name { get; set; }
        public bool Loaded { get; private set; }
        public bool Loading { get; private set; }

        private readonly List<Chunk> _chunks;

        public void OnRenderFrame(FrameEventArgs args)
        {
            foreach (var chunk in _chunks)
            {
                chunk.OnRenderFrame(args);
            }

            foreach (var entity in _entities)
            {
                entity.OnRenderFrame(args);
            }

            _self.OnRenderFrame(args);
        }

        public void OnUpdateFrame(FrameEventArgs args)
        {
            foreach (var chunk in _chunks)
            {
                chunk.OnUpdateFrame(args);
            }

            foreach (var entity in _entities)
            {
                entity.OnUpdateFrame(args);
            }

            _self.OnUpdateFrame(args);
        }

        [PacketMethod(typeof(EntityLocationPacket))]
        public void MoveEntity(Packet p)
        {
            var packet = (EntityLocationPacket) p;
            var ent = _entities.FirstOrDefault(e => e.Id == packet.EntityId);

            if (ent == null)
            {
                ent = new Entity(packet.EntityId, packet.EntityType, new PointF(packet.X, packet.Y))
                {
                    Colour = Color4.Red,
                    Camera = Camera
                };
                _entities.Add(ent);
            }
            else
            {
                ent.Location = new PointF(packet.X, packet.Y);
            }
        }

        [PacketMethod(typeof(RemoveEntityPacket))]
        public void RemoveEntity(Packet p)
        {
            var packet = (RemoveEntityPacket) p;
            _entities.RemoveAll(e => e.Id == packet.EntityId);
        }

        [PacketMethod(typeof(SetMapPacket))]
        public void SetMap(Packet p)
        {
            var packet = (SetMapPacket) p;
            Name = packet.Name;
            Loading = true;
            Loaded = false;
        }

        [PacketMethod(typeof(GiveChunkPacket))]
        public void GiveChunk(Packet p)
        {
            if (Loading)
            {
                var packet = (GiveChunkPacket) p;
                var chunk = _chunks.FirstOrDefault(c => c.X == packet.X && c.Y == packet.Y) ??
                            new Chunk(packet.X, packet.Y);
                chunk.Camera = Camera;
                chunk.LoadFromString(packet.Data);
                _chunks.Add(chunk);
            }
        }

        [PacketMethod(typeof(FinishMapPacket))]
        public void FinishMap(Packet p)
        {
            var packet = (FinishMapPacket) p;
            _self.X = packet.X;
            _self.Y = packet.Y;
            Camera.CenteredLocation = new PointF(_self.X, _self.Y);
            Loading = false;
            Loaded = true;
        }
    }
}
