using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship

{
    internal class Player
    {
        public string name;
        public bool human;
        public bool winner = false;
        public Board board;

        /// <summary>
        /// Constructor for a Player object. Human or CPU, both are considered Players.
        /// </summary>
        public Player(bool human)
        {
            this.human = human;
            if (!human)
            {
                this.name = UpdateName();
            } else
            {
                this.name = "Player1";
            }
            this.board = new Board(height: 10, width: 10, this);
        }

        /// <summary>
        /// Updates the computer player's name. Pass in a name string to use the other version that updates a human player's name.
        /// </summary>
        /// <returns>The string that will be used to rename the CPU player.</returns>
        public string UpdateName() // this cannot be static because it then wouldn't be usable in the constructor above
        {
            Array array = new string[4] { "Bethany", "Mona", "Eric", "Alexander" };
            Random rand = new();
            int index = rand.Next(array.Length);
            return array.GetValue(index).ToString();
        }

        /// <summary>
        /// Updates the human player's name. 
        /// </summary>
        /// <param name="name">The string used to update the human player's name.</param>
        public void UpdateName(string name)
        {
            this.name = name;
        }

        /// <summary>
        /// Getter for the Board object connected to this player.
        /// </summary>
        public Board Board
        {
            get { return board; }
        }
    }
}
