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
        public bool sunk = false;

        public Ship(int length, string name)
        {
            this.length = length;
            this.name = name;
        }

        //public void Place(string[] points)
        //{
        //    // Console.WriteLine("Please choose your first point.");
        //}

        /// <summary>
        /// Getter for the name instance variable.
        /// </summary>
        public string Name { get { return name; } }

        /// <summary>
        /// Checks to see if all of the cells that this ship resides on have been struck. If so, it is considered sunk.
        /// </summary>
        /// <returns>Boolean.</returns>
        public bool Sunk()
        {
            if (sunk) { return sunk; } // return true if it's already true
            int cellCount = cells.Where(x => x.status == "hit").Count();
            if (cellCount.Equals(cells.Length)) { this.sunk = true; } // update sunken status if sunk is false and it shouldn't be
            return this.sunk;
        } 
    }
}
