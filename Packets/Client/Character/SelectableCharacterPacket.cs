using System;
using BiscuitHeaders;
using Packets.Attributes;

namespace Packets.Client.Character
{
    [PacketHeader(ClientHeaders.SelectableCharacter)]
    public class SelectableCharacterPacket : Packet
    {
        [PacketData(0)]
        public string Name { get; set; }
        [PacketData(1)]
        public Guid Id { get; set; }
    }
}
