using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MapEditor.UI
{
    class Tile
    {
        public Panel Panel { get; set; }
        public int X { get; private set; }
        public int Y { get; private set; }

        public Tile(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}
