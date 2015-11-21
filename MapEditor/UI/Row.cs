using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MapEditor.UI
{
    class Row
    {
        public List<Tile> Tiles {get { return _tiles; }}
        public Panel Panel { get; set; }
        public int Y { get; private set; }

        private readonly List<Tile> _tiles;

        public Row(int y)
        {
            Y = y;
            _tiles = new List<Tile>();
        }
    }
}
