using BiscuitHeaders;
using Packets.Attributes;

namespace Packets.Client.Character
{
    [PacketHeader(ClientHeaders.CharacterSelectionResult)]
    public class CharacterSelectionResultPacket : Packet
    {
        [PacketData(0)]
        public bool Success { get; set; }
    }
}
