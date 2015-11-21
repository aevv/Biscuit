using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Packets.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class PacketMethod : Attribute
    {
        public Type PacketType { get; private set; }
        public PacketMethod(Type packetType)
        {
            PacketType = packetType;
        }
    }
}
