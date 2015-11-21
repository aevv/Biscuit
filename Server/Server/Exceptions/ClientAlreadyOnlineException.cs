using Server.Server.Client;
using System;

namespace Server.Server.Exceptions
{
    class ClientAlreadyOnlineException : Exception
    {
        public ClientAlreadyOnlineException(GameClient client)
            : base(string.Format("The client {0} was already online.", client))
        {
        }
    }
}
