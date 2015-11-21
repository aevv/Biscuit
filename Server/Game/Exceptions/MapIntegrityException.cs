using Server.Game.World;
using System;

namespace Server.Game.Exceptions
{
    class MapIntegrityException : Exception
    {
        public MapIntegrityException(Map map)
            : base(string.Format("Map {0} contained missing or invalid chunks.", map))
        {

        }
    }
}
