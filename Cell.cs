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

        public Cell(string name)
        {
            this.code = name;
        }

        public bool PlaceShip()
        {
            switch(status)
            {
                case "open":
                    status = "ship";
                    return true;
                default:
                    return false;
            }   
        }

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
        }

        public bool IsAvailable()
        {
            if (status == "open") { return true; }
            else { return false; }
        }
    }
}
