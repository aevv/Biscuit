using Packets.Attributes;
using System;
using System.Linq;

namespace Packets
{
    public class Packet
    {
        public short Header { get; set; }

        public Guid ClientID { get; set; }

        protected Packet()
        {
            var header = GetType().GetCustomAttributes(false).OfType<PacketHeaderAttribute>().FirstOrDefault();

            if (header != null)
            {
                Header = header.Header;
            }
        }
    }
}
