using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship
{
    internal class Cell
    {
        public string _code;
        public string _status = "open";

        public Cell(string name)
        {
            _code = name;
        }

        public bool PlaceShip()
        {
            switch(_status)
            {
                case "open":
                    _status = "ship";
                    return true;
                default:
                    return false;
            }   
        }

        public void Fire()
        {
            switch (_status)
            {
                case "open":
                    _status = "miss";
                    break;

                case "ship":
                    _status = "hit";
                    break;

                default:
                    break;
            }
        }

        public string Code
        { 
            get { return _code; }
        }

        public bool IsAvailable()
        {
            if (_status != "open") { return false; }
            else { return true; }
        }
    }
}
