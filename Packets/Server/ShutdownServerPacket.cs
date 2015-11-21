using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BiscuitHeaders;
using Packets.Attributes;

namespace Packets.Server
{
    [PacketHeader(ServerHeaders.Test)]
    public class ShutdownServerPacket : Packet
    {
    }
}
