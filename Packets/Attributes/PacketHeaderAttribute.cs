using BiscuitHeaders;
using System;

namespace Packets.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class PacketHeaderAttribute : Attribute
    {
        public short Header { get; private set; }
        public PacketHeaderAttribute(ClientHeaders header)
        {
            Header = (short)header;
        }

        public PacketHeaderAttribute(ServerHeaders header)
        {
            Header = (short)header;
        }
    }
}
