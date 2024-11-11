using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labyrinth.Model
{
    public class GameWonEventArgs : EventArgs
    {
        private int _steps;
        public int Steps { get { return _steps; } private set {_steps = value; } }
        public GameWonEventArgs(int steps){ Steps = steps; }
    }
}
