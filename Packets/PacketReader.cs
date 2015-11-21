using Packets.Attributes;
using Packets.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Packets
{
    public class PacketReader
    {
        private static PacketReader _instance;

        public static PacketReader Resolve<T>() where T : PacketReader
        {
            return _instance ?? (_instance = Activator.CreateInstance<T>());
        }

        protected object _lock = new object();
        protected bool _initialised;
        protected readonly Dictionary<short, Type> _mappings = new Dictionary<short, Type>();

        protected readonly Dictionary<Type, List<PropertyInfo>> _packetProperties =
            new Dictionary<Type, List<PropertyInfo>>();

        protected PacketReader()
        {

        }

        public virtual void InitialiseMapping()
        {

        }

        // TODO: Optimise.
        public virtual Packet ReadPacket(short header, BinaryReader reader)
        {
            var packet = Activator.CreateInstance(_mappings[header]);
            List<PropertyInfo> props = null;
            if (_packetProperties.ContainsKey(_mappings[header]))
            {
                props = _packetProperties[_mappings[header]];
            }
            else
            {
                props =
                    _mappings[header].GetProperties()
                        .Where(p => p.GetCustomAttributes(false).OfType<PacketDataAttribute>().Any())
                        .OrderBy(p => p.GetCustomAttributes(false).OfType<PacketDataAttribute>().First().Order)
                        .ToList();
                _packetProperties.Add(_mappings[header], props);
            }

            foreach (var prop in props)
            {
                prop.SetValue(packet, reader.ReadAny(prop.PropertyType));
            }
            return (Packet)packet;
        }

        public virtual bool CanRead(short header)
        {
            return _mappings.ContainsKey(header);
        }

        public Type this[short header]
        {
            get
            {
                return _mappings.ContainsKey(header) ? _mappings[header] : null;
            }
        }
    }
}
