using System;

namespace Server.Server.Exceptions
{
    class ClientDisconnectedException : Exception
    {
        public ClientDisconnectedException(string name, Exception ex)
            : base(string.Format("Client '{0}' has been disconnected.\r\n--- Further information --- \r\n{1}\r\n{2}", name, ex.Message, ex.StackTrace))
        {

        }

        public ClientDisconnectedException(Guid id, Exception ex)
            : base(string.Format("Client '{0}' has been disconnected\r\n--- Further information --- \r\n{1}\r\n{2}", id, ex.Message, ex.StackTrace))
        {

        }
    }
}
