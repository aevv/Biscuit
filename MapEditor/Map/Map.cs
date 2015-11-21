using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapEditor.Map
{
    class Map
    {
        private readonly List<Chunk> _chunks;

        public Map()
        {
            _chunks = new List<Chunk>();
        }
    }
}
