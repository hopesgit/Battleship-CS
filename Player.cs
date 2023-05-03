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

        public Player(bool human)
        {
            if (human.Equals(true) || human.Equals(false))
            {
                this.human = human;
                switch(human)
                {
                    case true:
                        this.name = "Player1";
                        break;

                    case false:
                        UpdateName();
                        break;
                }
                this.board = new Board(height: 4, width: 4, this);
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
            this.name = name;
        }

        public void UpdateName(string name)
        {
            this.name = name;
        }

        public Board Board
        {
            get { return board; }
        }
    }
}
