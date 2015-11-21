using DapperExtensions;
using Logging;
using Packets;
using Packets.Attributes;
using Packets.Infrastructure;
using Packets.Server.World;
using Server.Game.World;
using Server.Server.Client;
using Server.Server.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Server.Game
{
    class GameLoop : PacketSubscriber
    {
        private List<GameClient> _clients;
        private List<Map> _maps;
        private bool _loaded;

        private long _saveTimer;

        public Map PrimaryMap { get; private set; }

        public GameLoop()
        {
            _clients = new List<GameClient>();
            _maps = new List<Map>();
        }

        public void AddClient(GameClient client)
        {
            client.Character.Location = client.Character.GetCharacterLocation(_maps.First().Id);
            _clients.Add(client);

            _maps.First(m => m.Id == client.Character.Location.MapId).AddCharacter(client);
        }

        public void RemoveClient(GameClient client)
        {
            if (!_clients.Contains(client)) return;
            _clients.Remove(client);
            foreach (Map m in _maps)
                m.RemoveCharacter(client);
            client.Save();
        }

        public void Load()
        {
            if (_loaded) return;
            Out.Log("Loading maps...");
            var data = DataConnection.Resolve();
            var pred = Predicates.Field<Map>(m => m.Active, Operator.Eq, true);
            var maps = data.Repo.GetList<Map>(pred);

            foreach (var map in maps)
            {
                try
                {
                    Out.Log(string.Format("Loading map '{0}'...", map));
                    map.LoadChunks();
                    _maps.Add(map);
                    Out.Green(string.Format("Map '{0}' loaded!", map));
                }
                catch (Exception ex)
                {
                    Out.Red(string.Format("Map '{0}' failed to load. Reason:\n{1}.", map, ex));
                }
            }
            Out.Log(string.Format("Map loading finished, total maps: {0}.", _maps.Count));
            PrimaryMap = _maps.FirstOrDefault();
        }

        // The main game loop, called continuously. 
        public void Tick(long timeSinceLastTick)
        {
            _saveTimer += timeSinceLastTick;

            if (_saveTimer > 10000)
            {
                Save();
            }

            foreach (Map map in _maps)
            {
                map.Process(timeSinceLastTick);
            }
        }

        [PacketMethod(typeof(MoveCharacterPacket))]
        public void OnMoveCharacter(Packet packet)
        {
            var p = (MoveCharacterPacket)packet;
            var player = _clients.First(c => c.OnlineId == p.ClientID);
            player.Character.Location.X = p.X;
            player.Character.Location.Y = p.Y;
        }

        [PacketMethod(typeof (RequestMapPacket))]
        public void OnRequestMap(Packet packet)
        {
            var client = _clients.First(c => c.OnlineId == packet.ClientID);
            _maps.First(m => m.Id == client.Character.Location.MapId).SendToPlayer(client);
        }

        public void Save()
        {
            Out.Cyan("Saving data to database...");
            foreach (var c in _clients)
            {
                c.Save();
            }
            _saveTimer = 0;
            Out.Green("Saved!");
        }
    }
}
