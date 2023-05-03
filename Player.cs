using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship

{
    internal class Player
    {
        public string _name;
        public bool _human;
        public bool _winner;
        public Board _board;

        public Player(bool human)
        {
            if (human.Equals(true) || human.Equals(false))
            {
                _human = human;
                switch(human)
                {
                    case true:
                        _name = "Player1";
                        break;

                    case false:
                        UpdateName();
                        break;
                }
                _board = new Board(height: 4, width: 4, this);
                _winner = false;
            }
            else
            {
                throw new ArgumentException(message: "Human must be a boolean.");
            }
        }

        public void UpdateName()
        {
            Array array = new string[4] { "Bethany", "Mona", "Eric", "Alexander" };
            Random rand = new Random();
            int index = rand.Next(4);
            string name = array.GetValue(index).ToString();
            _name = name;
        }

        public void UpdateName(string name)
        {
            _name = name;
        }

        public Board Board
        {
            get { return _board; }
        }
    }
}
