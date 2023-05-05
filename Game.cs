using System;
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
        public Player Player1 = new(human: true); // create player 1
        public Player Player2 = new(human: false); // create player 2

        /// <summary>
        /// Sets up the game. Creates the necessary Player and Board objects, as well as asking the player for their name.
        /// </summary>
        public void Setup()
        {
            Console.WriteLine("Welcome! Please give me your name: "); // asks for name
            string playername = Console.ReadLine().Trim(); // name input, trims off whitespace. I also don't really care if it's null
            Player1.UpdateName(playername); // update p1's name
            Player2.UpdateName(); // update p2's name
            Console.WriteLine($"Greetings, {playername}. \nStarting {Name}...");
            Console.WriteLine($"You will be playing with {Player2.name}.");
            Player1.Board.PlaceShipSeq();
            Player2.Board.PlaceShipSeq();
        }

        public void Load()
        {
            Console.WriteLine("Setting up game...");
            Run();
        }

        /// <summary>
        /// The meat of the game is here. This runs the turn functions. 
        /// </summary>
        private void Run() 
            // todo: break the turn functionality out into its own method
            // todo: break the win condition functionality out into its own method
        {
            int turn = 0;
            string winnername = "";
            while (Player1.winner.Equals(false) && Player2.winner.Equals(false) && turn < 100)
            {
                // run game code
                ++turn;
                Console.WriteLine($"Available cells: {Player1.board.AvailableCells()}");
                Player1.winner = true;
            }

            if (Player1.winner)
            {
                winnername = Player1.name;
                Console.WriteLine($"A winner is you, {winnername}!");
            }
            else if (Player2.winner)
            {
                winnername = Player2.name;
                Console.WriteLine($"A winner is you, {winnername}!");
            }
            else
            {
                Console.WriteLine("Game time exceeded. Stalemate called. Please try again.");
            }
        }
    }
}
