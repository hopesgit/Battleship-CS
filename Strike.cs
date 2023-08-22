using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship
{
    public class Strike
    {
        public string coordinateCode;
        public bool hit = false;
        public bool sunk = false;

        public Strike(string code, bool hit, bool sunk) 
        {
            coordinateCode = code;
            this.hit = hit;
            this.sunk = sunk;
        }
    }
}
