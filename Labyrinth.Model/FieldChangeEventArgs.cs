using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Labyrinth.Persistence;

namespace Labyrinth.Model
{
    public class FieldChangeEventArgs : EventArgs
    {
        private Field _field;
        private int _x;
        private int _y;
        public Field Field { get { return _field; } private set { _field = value; } }
        public int X { get { return _x; } private set { _x = value; } }
        public int Y { get { return _y; } private set { _y = value; } }

        public FieldChangeEventArgs(int x, int y, Field field)
        {
            Field = field;
            X = x;
            Y = y;
        }
    }
}
