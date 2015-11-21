using Packets;
using Packets.Attributes;
using Packets.Client.Chat;
using Packets.Infrastructure;
using Server.Server.Client;
using System.Collections.Generic;

namespace Server.Server.Chat
{
    class ChatManager : PacketSubscriber
    {
        private static ChatManager _instance;
        private readonly List<GameClient> _clients;

        private ChatManager()
        {
            _clients = new List<GameClient>();
        }

        public static ChatManager Resolve()
        {
            return _instance ?? (_instance = new ChatManager());
        }

        public void Join(GameClient client)
        {
            _clients.Add(client);
        }

        [PacketMethod(typeof(ChatMessagePacket))]
        public void OnChatMessage(Packet packet)
        {
            //Console.WriteLine("{0} : {1}", packet.User, packet.Message);
            foreach (var c in _clients)
            {
                PacketWriter.WritePacketAsync(packet, c.Writer);
            }
        }
    }
}
