using System;

namespace Packets.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    class PacketDataAttribute : Attribute
    {
        public int Order { get; set; }
        public int Length { get; set; }
        public PacketDataAttribute(int order, int length = 0)
        {
            Order = order;
            Length = length;
        }
    }
}
