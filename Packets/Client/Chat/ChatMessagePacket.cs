using BiscuitHeaders;
using Packets.Attributes;

namespace Packets.Client.Chat
{
    [PacketHeader(ClientHeaders.ChatMessage)]
    public class ChatMessagePacket : Packet
    {
        [PacketData(0)]
        public string Message { get; set; }
        [PacketData(1)]
        public string User { get; set; }
    }
}
