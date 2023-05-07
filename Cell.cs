using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship
{
    internal class Cell
    {
        public string code;
        public string status = "open";
        public Ship? ship;
        public string enemyStatus = "O";

        /// <summary>
        /// Constructor. Create a Cell with this.
        /// </summary>
        public Cell(string code)
        {
            this.code = code;
        }

        /// <summary>
        /// Connects a ship object to this cell. Used in conjunction with the other cells that the ship is to be placed on.
        /// </summary>
        /// <param name="ship">
        /// Reflects a Ship object. This is the Ship that is to be placed on this cell.
        /// </param>
        /// <returns>Boolean that reflects whether the ship could be placed or not.</returns>
        public bool PlaceShip(Ship ship)
        {
            switch(status)
            {
                case "open":
                    status = "ship";
                    this.ship = ship;
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Fires upon the cell. This changes the cell's status to "hit" or "miss" depending on whether a ship was placed there.
        /// </summary>
        public void Fire()
        {
            switch (status)
            {
                case "open":
                    status = "miss";
                    break;

                case "ship":
                    status = "hit";
                    break;

                default:
                    break;
            }
            EnemyStatus();
        }

        /// <summary>
        /// Checks this cell's status to report whether it's available for ship placement. True indicates that a ship can be placed here.
        /// </summary>
        /// <returns>Boolean.</returns>
        public bool IsAvailable()
        {
            if (status == "open") { return true; }
            else { return false; }
        }

        /// <summary>
        /// This is used to determine whether the Cell has already been fired upon. 
        /// If it has been fired upon, which is indicated with a status of "hit" or "miss", it is not a valid target.
        /// </summary>
        /// <returns>Boolean</returns>
        public bool ValidTarget()
        {
            if (status == "miss" | status == "hit") { return false; }
            else return true;
        }

        /// <summary>
        /// Setter for the "enemyStatus" class variable
        /// </summary>
        public void EnemyStatus()
        {
            enemyStatus = status switch
            {
                "hit" => "H",
				"miss" => "M",
				_ => "O",
			};
        }
    }
}
