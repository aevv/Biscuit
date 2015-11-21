using BiscuitHeaders;
using Packets.Attributes;

namespace Packets.Client.Map
{
    [PacketHeader(ClientHeaders.SetMap)]
    public class SetMapPacket : Packet
    {
        [PacketData(0)]
        public string Name { get; set; }
    }
}
