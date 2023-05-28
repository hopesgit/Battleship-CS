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
        {
            int turn = 0;
            while (turn < (Player1.Board.height * Player1.Board.width))
            {
                ++turn;
                Console.WriteLine($"~~~ Beginning turn {turn} ~~~");
                PlayerTurn();
                if (EvaluateWinner() is not null) { break; }
                CPUTurn();
                if (EvaluateWinner() is not null) { break; }
            }

            Console.WriteLine($"Today's win goes to {EvaluateWinner().name}!");
        }

        /// <summary>
        /// Logic for the player's turn. The player always goes before the CPU. 
        /// </summary>
        private void PlayerTurn()
        {
            Player1.Board.AvailableCells();
            Console.WriteLine($"{Player1.name}, please enter a cell to fire upon.");
            Console.WriteLine($"Here is what you know about the opponent's board: ('O' means that you have yet to attack that cell)");
            Player2.Board.PlayerPossibleTargets();

            Player2.Board.Fire();
        }

        /// <summary>
        /// Logic for the CPU's turn. The CPU always goes after the player.
        /// </summary>
        private void CPUTurn()
        {
            Console.WriteLine("The CPU is taking its turn...");
            string[] targets = Player1.Board.PossibleTargets();
            Random rand = new();
            int index = rand.Next(targets.Length);
            string selected = targets.GetValue(index).ToString();
            Console.WriteLine($"The CPU has chosen {selected}.");
            Player1.Board.Fire(selected);
        }

        private Player? EvaluateWinner()
        {
            bool p1Lose = Player1.Board.OutOfShips();
            bool p2Lose = Player2.Board.OutOfShips();
            if (p2Lose) { return Player1; }
            else if (p1Lose) { return Player2; }
            else { return null; }
        }
    }
}
