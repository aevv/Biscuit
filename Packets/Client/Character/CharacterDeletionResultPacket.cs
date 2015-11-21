using BiscuitHeaders;
using Packets.Attributes;

namespace Packets.Client.Character
{
    [PacketHeader(ClientHeaders.CharacterDeletionResult)]
    public class CharacterDeletionResultPacket : Packet
    {

        [PacketData(0)]
        public bool Success { get; set; }

        [PacketData(1)]
        public string Reason { get; set; }

    }
}
