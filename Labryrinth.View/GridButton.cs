using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labyrinth.View
{
    class GridButton : Button
    {
        public int _x;
        public int _y;

        public int GridX
        {
            get { return _x; }
            set { _x = value; }
        }
        public int GridY
        {
            get { return _y; }
            set { _y = value; }
        }
        public GridButton(int x, int y) { _x = x; _y = y; }
    }
}
