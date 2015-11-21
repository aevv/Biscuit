using System;
using BiscuitHeaders;
using Packets.Attributes;

namespace Packets.Client.Map
{
    [PacketHeader(ClientHeaders.EntityLocation)]
    public class EntityLocationPacket : Packet
    {
        [PacketData(0)]
        public float X { get; set; }
        [PacketData(1)]
        public float Y { get; set; }
        [PacketData(2)]
        public Guid EntityId { get; set; }
        [PacketData(3)]
        public short EntityTypeId { get; set; }

        public EntityTypes EntityType
        {
            get { return (EntityTypes) EntityTypeId; }
        }

    }
}
