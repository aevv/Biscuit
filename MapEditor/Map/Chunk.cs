using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapEditor.Map
{
    class Chunk
    {
        public int X { get; set; }
        public int Y { get; set; }

        private readonly List<Layer> _layers;

        public Chunk()
        {
            _layers = new List<Layer>();
        }
    }
}
