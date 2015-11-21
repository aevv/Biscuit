using Packets.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Packets.Infrastructure
{
    public static class PacketRegister
    {
        public static Dictionary<Type, Dictionary<Type, MethodInfo>> Register = new Dictionary<Type, Dictionary<Type, MethodInfo>>();

        public static void CreateRegistry()
        {
            var types = Assembly.GetCallingAssembly().GetTypes();

            foreach (var type in types)
            {
                var packetMethods =
                    type.GetMethods().Where(m => m.GetCustomAttributes(false).OfType<PacketMethod>().Any()).ToList();

                if (packetMethods.Any())
                {
                    Register.Add(type, new Dictionary<Type, MethodInfo>());

                    foreach (var method in packetMethods)
                    {
                        var t = method.GetCustomAttributes(false).OfType<PacketMethod>().First().PacketType;
                        Register[type].Add(t, method);
                    }
                }
            }
        }
    }
}
