using BiscuitHeaders;
using Packets.Attributes;

namespace Packets.Client.Map
{
    [PacketHeader(ClientHeaders.FinishMap)]
    public class FinishMapPacket : Packet
    {
        [PacketData(0)]
        public float X { get; set; }
        [PacketData(1)]
        public float Y { get; set; }
    }
}
