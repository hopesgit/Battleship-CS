using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship
{
    internal class Ship
    {
        public int length;
        public string name;
        public Cell[] cells = Array.Empty<Cell>();

        public Ship(int length, string name)
        {
            this.length = length;
            this.name = name;
        }

        public void Place(string[] points)
        {
            // Console.WriteLine("Please choose your first point.");
        }

        public string Name { get { return name; } }
    }
}
