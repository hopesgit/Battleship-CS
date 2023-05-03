using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship
{
    internal class Ship
    {
        public int _length;
        public string _name;
        public Cell[] _cells = Array.Empty<Cell>();

        public Ship(int length, string name)
        {
            _length = length;
            _name = name;
        }

        public void Place(string[] points)
        {
            // Console.WriteLine("Please choose your first point.");
        }

        public string Name { get { return _name; } }
    }
}
