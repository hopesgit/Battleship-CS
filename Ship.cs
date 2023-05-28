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

        /// <summary>
        /// Getter for the name instance variable.
        /// </summary>
        public string Name { get => name; }

        /// <summary>
        /// Checks to see if all of the cells that this ship resides on have been struck. If so, it is considered sunk.
        /// </summary>
        /// <returns>Boolean.</returns>
        public bool Sunk()
        {
            if (sunk) { return sunk; } // return true if it's already true
            int cellCount = cells.Where(x => x.status == "hit").Count();
            if (cellCount.Equals(cells.Length)) { this.sunk = true; } // update sunken status if sunk is false and it should be true
            return this.sunk;
        }
    }
}
