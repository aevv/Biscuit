using Logging;
using Packets;
using Packets.Client.Status;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Server.Server.Client
{
    class GameClientManager : IClientKiller
    {
        private readonly Dictionary<Guid, GameClient> _clients;
        public GameClientManager()
        {
            _clients = new Dictionary<Guid, GameClient>();
        }

        public void AddClient(GameClient clientToAdd)
        {
            _clients.Add(clientToAdd.OnlineId, clientToAdd);
            Out.Log(string.Format("Client Manager: Client '{0}'", clientToAdd), withDate: true, newline: false);
            Out.Green(" connected.", withDate: false);
        }

        public void RemoveClient(GameClient clientToRemove)
        {
            _clients.Remove(clientToRemove.OnlineId);
            Out.Log(string.Format("Client Manager: Client '{0}'", clientToRemove), withDate: true, newline: false);
            Out.Red(" disconnected.", withDate: false);
        }

        public string GetAll()
        {
            return string.Join("\nChar: ", _clients.Values);
        }

        public GameClient this[string name]
        {
            get { return _clients.Values.First(client => client.Account != null && client.Account.Username == name); }
        }

        public GameClient this[Guid id]
        {
            get { return _clients[id]; }
        }

        public void ShutDown()
        {
            foreach (var c in _clients)
            {
                PacketWriter.WritePacket(new ServerOfflinePacket(), c.Value.Writer);
            }
            _clients.Clear();
        }
    }
}
