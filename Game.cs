﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Battleship
{
    internal class Game
    {
        public string Name = "Battleship";
        public Player Player1 = new Player(human: true); // create player 1
        public Player Player2 = new Player(human: false); // create player 2

        public void Setup() // the method name here doesn't matter
        {
            Console.WriteLine("Welcome! Please give me your name: "); // asks for name
            string playername = Console.ReadLine().Trim(); // name input, trims off whitespace
            Player1.UpdateName(playername); // update p1's name
            Player2.UpdateName(); // update p2's name
            Console.WriteLine($"Greetings, {playername}. \nStarting {Name}...");
            Console.WriteLine($"You will be playing with {Player2._name}.");
            Player1.Board.PlaceShipSeq();
        }

        public void Load()
        {
            Console.WriteLine("Setting up game...");
            Run();
        }

        private void Run()
        {
            int turn = 0;
            string winnername = "";
            while (Player1._winner.Equals(false) && Player2._winner.Equals(false) && turn < 100)
            {
                // run game code
                ++turn;
                Console.WriteLine($"Available cells: {Player1._board.AvailableCells()}");
                // Player1._winner = true;
            }

            if (Player1._winner)
            {
                winnername = Player1._name;
                Console.WriteLine($"A winner is you, {winnername}!");
            }
            else if (Player2._winner)
            {
                winnername = Player2._name;
                Console.WriteLine($"A winner is you, {winnername}!");
            }
            else
            {
                Console.WriteLine("Game time exceeded. Stalemate called. Please try again.");
            }

        }
    }
}
