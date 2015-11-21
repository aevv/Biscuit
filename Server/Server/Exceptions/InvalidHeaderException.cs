using System;

namespace Server.Server.Exceptions
{
    class InvalidHeaderException : Exception
    {
        public InvalidHeaderException(short header)
            : base(string.Format("The header '{0}' was not recognised.", header))
        {

        }
    }
}
