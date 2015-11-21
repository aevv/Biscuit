using System;
using System.Collections.Generic;

namespace Packets.Infrastructure
{
    public abstract class PacketSubscriber
    {
        protected Dictionary<Type, Action<Packet>> _actions = new Dictionary<Type, Action<Packet>>();
        public PacketSubscriber()
        {
            if (PacketRegister.Register.ContainsKey(GetType()))
            {
                var myMethods = PacketRegister.Register[GetType()];

                foreach (var method in myMethods)
                {
                    var action = (Action<Packet>) Delegate.CreateDelegate(typeof (Action<Packet>), this, method.Value);
                    _actions.Add(method.Key, action);
                }
            }
        }

        public void ReceivePacket(Packet packet)
        {
            if (_actions.ContainsKey(packet.GetType()))
                _actions[packet.GetType()](packet);
        }
    }
}
