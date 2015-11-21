using BiscuitHeaders;
using DapperExtensions;
using DapperExtensions.Mapper;
using DataAccessLayer.Attributes;
using Packets;
using Packets.Client.Map;
using Server.Game.Exceptions;
using Server.Game.Mech;
using Server.Server.Client;
using Server.Server.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Server.Game.World
{
    [Table(Name = "Map")]
    class Map
    {
        [Column(Name = "Map_Name")]
        public string Name { get; set; }
        [Column(Name = "Map_Description")]
        public string Description { get; set; }
        [Column(Name = "Map_Id"), PrimaryKey(KeyType = KeyType.Guid)]
        public Guid Id { get; set; }
        [Column(Name = "Map_Active")]
        public bool Active { get; set; }

        private Chunk[,] _chunks;
        private bool _loaded;

        private readonly List<GameClient> _characters;
        private readonly List<Entity> _entities;

        public Map()
        {
            _characters = new List<GameClient>();
        }

        public void AddCharacter(GameClient client)
        {
            _characters.Add(client);
        }

        public void RemoveCharacter(GameClient client)
        {
            if (_characters.Contains(client))
                _characters.Remove(client);

            foreach (var c in _characters)
                PacketWriter.WritePacketAsync(new RemoveEntityPacket { EntityId = client.OnlineId }, c.Writer);
        }

        public Chunk this[int x, int y]
        {
            get { return _chunks[x, y]; }
        }

        public void LoadChunks()
        {
            if (_loaded) return;
            var data = DataConnection.Resolve().Repo;
            var predicate = Predicates.Field<Chunk>(c => c.MapId, Operator.Eq, Id);

            var chunks = data.GetList<Chunk>(predicate);
            int maxX = -1;
            int maxY = -1;

            foreach (var c in chunks)
            {
                if (c.X > maxX)
                    maxX = c.X;
                if (c.Y > maxY)
                    maxY = c.Y;
            }

            _chunks = new Chunk[maxX + 1, maxY + 1];

            foreach (var c in chunks)
            {
                _chunks[c.X, c.Y] = c;
                c.LoadChunk();
            }

            if (!ValidateMapIntegrity())
            {
                throw new MapIntegrityException(this);
            }

            _loaded = true;
        }

        public bool ValidateMapIntegrity()
        {
            for (int x = 0; x < _chunks.GetLength(0); x++)
            {
                for (int y = 0; y < _chunks.GetLength(1); y++)
                {
                    if (_chunks[x, y] == null)
                        return false;
                }
            }
            return true;
        }

        public void Process(long milis)
        {
            var chars = _characters.ToList();
            foreach (var p in chars)
            {
                var others = _characters.Where(c => c.OnlineId != p.OnlineId);
                foreach (var o in others)
                {
                    PacketWriter.WritePacketAsync(new EntityLocationPacket
                                                  {
                                                      EntityId = o.OnlineId,
                                                      EntityTypeId = (short)EntityTypes.Player,
                                                      X = o.Character.Location.X,
                                                      Y = o.Character.Location.Y
                                                  }, p.Writer);
                }
            }
        }

        public override string ToString()
        {
            return Name;
        }

        public void SendToPlayer(GameClient client)
        {
            var loc = client.Character.Location;

            var minX = (((int)loc.X) / 64 / 32) - 3;
            var minY = (((int)loc.Y) / 64 / 32) - 3;

            if (minX < 0)
                minX = 0;
            if (minY < 0)
                minY = 0;

            var maxX = minX + 3;
            var maxY = minY + 3;

            if (maxX > _chunks.GetLength(0))
                maxX = _chunks.GetLength(0);
            if (maxY > _chunks.GetLength(1))
                maxY = _chunks.GetLength(1);

            PacketWriter.WritePacket(new SetMapPacket { Name = Name }, client.Writer);

            for (int x = minX; x < maxX; x++)
            {
                for (int y = minY; y < maxY; y++)
                {
                    foreach (var data in this[x, y].GetDataAsString())
                    {
                        PacketWriter.WritePacket(
                            new GiveChunkPacket { X = x, Y = y, Data = data }, client.Writer);
                    }
                }
            }

            PacketWriter.WritePacket(new FinishMapPacket { X = loc.X, Y = loc.Y }, client.Writer);
        }
    }
}
