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
            this.human = human;
            if (!human)
            {
                this.name = UpdateName();
            } else
            {
                this.name = "Player1";
            }
            this.board = new Board(height: 5, width: 5, this);
        }

        public string UpdateName()
        {
            Array array = new string[4] { "Bethany", "Mona", "Eric", "Alexander" };
            Random rand = new();
            int index = rand.Next(array.Length);
            return array.GetValue(index).ToString();
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
