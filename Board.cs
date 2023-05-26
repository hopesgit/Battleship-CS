using System.Data;

namespace Battleship
{
    internal class Board
    {
        public Cell[] cells = Array.Empty<Cell>();
        public string[] cellCodes = Array.Empty<string>();
        public int height = 0;
        public int width = 0;
        public Player player;
        public List<Ship> ships = new();

        public Board(int height, int width, Player player)
        {
            this.height = height;
            this.width = width;
            this.player = player;
            
            // everything below this HAS to be able to be refactored
            Array alphabet = new string[26] {
                "A", "B", "C", "D", "E", "F", "G", "H", "I", "J",
                "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T",
                "U", "V", "W", "X", "Y", "Z"
            };

            int[] cell_numbers = Array.Empty<int>(); 
            Array.Resize(ref cell_numbers, width);

            for (int i = 1; i <= width; i++)
            {
                int index = i - 1;
                cell_numbers.SetValue(i, index);
            }

            string[] cell_letters = Array.Empty<string>();
            Array.Resize(ref cell_letters, cell_numbers.Length);

            foreach (int i in cell_numbers)
            {
                cell_letters.SetValue(alphabet.GetValue(i-1), i-1);
            }

            int total_cells = width * height;
            Array.Resize(ref cells, total_cells);
            Array.Resize(ref cellCodes, total_cells);

            int codeIndex = 0;
            foreach (string letter in cell_letters)
            {
                foreach (int number in cell_numbers)
                {
                    string code = $"{letter}{number}";
                    cellCodes.SetValue(code, codeIndex);
                    cells.SetValue(new Cell(code), codeIndex);
                    codeIndex++;
                }
            }
        }

        public Player Owner 
        { 
            get { return player; } 
        }

        /// <summary>
        /// Calculates the cells that the user could place a ship into.
        /// </summary>
        /// <returns>A string with all of the valid cell codes.</returns>
        public void AvailableCells()
        {
            Dictionary<string, string> targets = new();
            foreach (Cell cell in cells)
            {
                targets.Add(cell.code, cell.status);
            }
            
            Console.WriteLine("Your board currently looks like this: ");
            Breakdown(targets);
        }

        /// <summary>
        /// A list of cells that the opponent could target on this board. 
        /// </summary>
        /// <returns>The codes for each possible cell that could be fired upon, as a string array.</returns>
        public string[] PossibleTargets()
        {
            List<string> holder = new List<string>();
            var allCells = cells.Where(x => x.ValidTarget());
            foreach (Cell cell in allCells)
            {
                holder.Add(cell.code);
            }
            return holder.ToArray();
        }


        public void PlayerPossibleTargets()
        {
            Dictionary<string, string> targets = new();
            foreach (Cell cell in cells)
            {
                targets.Add(cell.code, cell.enemyStatus);
            }

            Breakdown(targets);
        }

        private void Breakdown(Dictionary<string, string> statuses)
        {
            int i = 1;
            string boardApprox = "|";

            while (i <= (width * height))
            {
                foreach (KeyValuePair<string, string> pair in statuses)
                {
                    boardApprox += $" {pair.Key}: {pair.Value} |";
                    if (i % width == 0)
                    {
                        Console.WriteLine(boardApprox);
                        boardApprox = "|";
                    }
                    i++;
                }
            }
        }

        /// <summary>
        /// Summons either the player or CPU ship placement routines. 
        /// If the player who owns this board is human, the human routine is called.
        /// </summary>
        public void PlaceShipSeq()
        {
            if (Owner.human) { PlayerPlaceShipSeq(); }
            else { CPUPlaceShipSeq(); }
        }

        /// <summary>
        /// A human player's ship placement sequence. 
        /// </summary>
        public void PlayerPlaceShipSeq() 
        {
            Ship battleship = new(4, "Battleship");
            Ship patrol = new(2, "Patrol Boat");
            Ship carrier = new(5, "Aircraft Carrier");
            Ship cruiser = new(3, "Cruiser");
            Ship submarine = new(3, "Submarine");

            PlayerPlaceShip(battleship);
            PlayerPlaceShip(patrol);
            PlayerPlaceShip(carrier);
            PlayerPlaceShip(cruiser);
            PlayerPlaceShip(submarine);
            Console.WriteLine($"Player {Owner.name}\'s ships have been placed.");
        }

        /// <summary>
        /// Gathers coordinates and sets the given ship to be placed.
        /// </summary>
        /// <param name="ship">The ship to be placed.</param>
        public void PlayerPlaceShip(Ship ship)
        {
            List<string> responses = new();
            string[] order = { "first", "second", "third", "fourth", "fifth" };
            AvailableCells();

            for (int index = 0; index < ship.length; index++)
            {
                Console.WriteLine($"Please choose the {order.GetValue(index)} coordinate for your {ship.Name} (length: {ship.length}): ");
                string response = GetCoordinate();
                responses.Add(response);
            }

            string[] coordinates = responses.ToArray<string>();
            bool placed = PlaceShip(coordinates, ship);
            if (!placed) { Console.WriteLine($"{ship.name} couldn't be placed. Please try again."); PlayerPlaceShip(ship); }
        }

        /// <summary>
        /// Places a ship on the cells this Board is responsible for.
        /// </summary>
        /// <param name="coordinates">
        /// an array of string coordinate names that represent where the ship will be placed
        /// <param>
        private bool PlaceShip(string[] coordinates, Ship ship)
        {
            // this needs refactoring
            bool validation = true;
            if (coordinates.Length != ship.length) { Console.WriteLine("Number of coordinates provided doesn't match the ship's length."); validation = false; }
            if (CheckCoords(coordinates) is false) { Console.WriteLine("That ship cannot be placed in that way."); return false; }
            
            foreach (string coordinate in coordinates)
            {
                var cellPoss = cells.Where(cell => cell.code == coordinate);
                if (!cellPoss.Any()) { Console.WriteLine($"Cell {coordinate} couldn't be found."); return false; }

                Cell selectedCell = cellPoss.First();
                bool placed = selectedCell.PlaceShip(ship);
                if (placed & player.human)
                {
                    ship.cells = ship.cells.Append(selectedCell).ToArray();
                    Console.WriteLine($"{ship.name} placed at {coordinate}.");
                }
                else if (placed) { ship.cells = ship.cells.Append(selectedCell).ToArray(); }
            }

            ships.Add( ship ); // adding the ship to the ships list for the board certifies that it has been placed correctly
            return validation;
        }

        private bool CheckCoords(string[] coordinates)
        {
            List<int> indices = new();
            foreach (string coordinate in coordinates)
            {
                var possibleCells = cells.Where(cell => cell.code == coordinate);
                if (!possibleCells.Any()) { continue; }
                Cell cellPoss = possibleCells.First();
                
                var index = Array.FindIndex(cells, cell => cell == cellPoss);
                indices.Add(index);
            }

            indices.Sort();
            indices = indices.Distinct().ToList<int>();
            if (indices.Count != coordinates.Length) { return false; }

            List<int> remainders = indices.Select(item => item % width).Distinct().ToList<int>(); // this is a horizontal placement check
            List<int> results = indices.Select(item => item / width).Distinct().ToList<int>(); // this is a vertical placement check

            int columnCountCheck = 1;
            if (remainders.Count == 1) {  columnCountCheck = 0; }
            else if (remainders.Count == coordinates.Length) { columnCountCheck = 2; }

            int rowCountCheck = 1;
            if (results.Count == 1) {  rowCountCheck = 0; }
            else if (results.Count == coordinates.Length) { rowCountCheck= 2; }

            bool verticalCheck = columnCountCheck == 0 && rowCountCheck == 2;
            bool horizontalCheck = columnCountCheck == 2 && rowCountCheck == 0;

            int firstItem = indices.First();
            int firstItemModulo = firstItem % width;
            bool dummy = indices.Select((value, index) => value - index * width == firstItem).All(item => item == true);
            bool shipVertical = indices.All(index => index % width == firstItemModulo) && ( dummy && verticalCheck); // this should check if the cells chosen are in the same column but different rows
            bool shipHorizontal = indices.Select((value, index) => value - index == firstItem).All(item => item == true) && horizontalCheck; 
            // this should check if the current set of coordinates are in sequence
            // they should also all be on the same row

            if (shipVertical & shipHorizontal) { return false; }
            else if (!shipVertical & !shipHorizontal) { return false; }
            else if (shipVertical is true) { return shipVertical; }
            else if (shipHorizontal is true) { return shipHorizontal; }
            else { return false; }
        }

        /// <summary>
        /// Handles the coordinate responses from the user.
        /// </summary>
        /// <returns>
        /// the coordinate string, if it's valid
        /// </returns>
        private static string GetCoordinate()
        {
            string? response = Console.ReadLine();
            InputSanitizer sanit = new();
            string coordinate = String.Empty;
            try 
            { 
#pragma warning disable CS8604 // Possible null reference argument.
                string corrected_coordinate = sanit.CoordinateInput(response); 
#pragma warning restore CS8604 // Possible null reference argument.
                coordinate = corrected_coordinate;
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
                GetCoordinate();
            }
            Console.WriteLine($"Response corrected to \"{coordinate}\".");
            return coordinate;
        }

        public void CPUPlaceShipSeq()
            // todo: code this more flexibly
        {
            Ship battleship = new(4, "Battleship");
            Ship patrol = new(2, "Patrol Boat");
            Ship carrier = new(5, "Aircraft Carrier");
            Ship cruiser = new(3, "Cruiser");
            Ship submarine = new(3, "Submarine");

            // for now I'm just going to place them in hardcoded spots

            string[] carrierCoords = new string[] { "A1", "B1", "C1", "D1", "E1" };
            string[] battleshipCoords = new string[] { "A2", "A3", "A4", "A5" };
            string[] cruiserCoords = new string[] { "B2", "B3", "B4" };
            string[] submarineCoords = new string[] {"D2", "D3", "D4"};
            string[] patrolCoords = new string[] { "D5", "E5" };

            PlaceShip(battleshipCoords, battleship);
            PlaceShip(patrolCoords, patrol);
            PlaceShip(carrierCoords, carrier);
            PlaceShip(cruiserCoords, cruiser); 
            PlaceShip(submarineCoords, submarine);
        }

        public void Fire()
        {
            string response = GetCoordinate();
            Cell? cell = cells.Where(x => x.code == response).FirstOrDefault();
            if (cell == null) 
            {
                Console.WriteLine("Cell not found. Please try again. ");
                Fire();
            }
            else if (cell != null & !cell.ValidTarget()) 
            {
                Console.WriteLine("Cell has already been struck. Please try another. ");
                Fire();
            }
            else
            {
                cell.Fire();
                Console.WriteLine($"It was a {cell.status}!");
                Ship? ship = cell.ship;
                bool sunk = ship?.Sunk() ?? false;
                if (ship != null & sunk) { Console.WriteLine($"You sank my {ship.name}!"); }
            }
        }

        public void Fire(string coordinate)
        {
            Cell? cell = cells.Where(x => x.code == coordinate).FirstOrDefault();
            if (cell == null)
            {
                Console.WriteLine("Cell not found. Please try again. ");
                Fire();
            }
            else if (cell != null & !cell.ValidTarget())
            {
                Console.WriteLine("Cell has already been struck. Please try another. ");
                Fire();
            }
            else
            {
                cell.Fire();
                Console.WriteLine($"It was a {cell.status}!");
                Ship? ship = cell.ship;
                bool sunk = ship?.Sunk() ?? false;
                if (cell.status == "hit" & sunk) { Console.WriteLine($"You sank my {ship.name}!"); } 
            }
        }

        public bool OutOfShips()
        {
            var shipCheck = ships.All(x => x.sunk == true);
            if (shipCheck == true) { return true; }
            else { return false; }
        }
    }
}