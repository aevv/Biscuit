using BiscuitHeaders;
using Packets.Attributes;

namespace Packets.Server.Character
{
    [PacketHeader(ServerHeaders.CreateCharacter)]
    public class CreateCharacterPacket : Packet
    {
        [PacketData(0)]
        public string Name { get; set; }
    }
}
