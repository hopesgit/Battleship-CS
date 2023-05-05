using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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
        /// Calculates the cells that the user COULD place a ship into.
        /// </summary>
        /// <returns>A string with all of the valid cell codes.</returns>
        public string AvailableCells()
        {
            string codeList = "";

            var cellPoss = cells.Where(x => x.status == "open");
            foreach (Cell cell in cells)
            {
                if ( cellPoss.Contains(cell) ) 
                {
                    codeList += $"{cell.code} ";
                }
            }

            if (codeList.Equals("")) { return "None"; }
            return codeList;
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

        /// <summary>
        /// Summons either the player or CPU ship placement routines. 
        /// If the player who owns this board is human, the human routine is called.
        /// </summary>
        public void PlaceShipSeq()
        {
            if (Owner.human) { PlayerPlaceShipSeq(); }
            else { CPUPlaceShipSeq(); }
        }

        public void PlayerPlaceShipSeq() 
            // todo: add ship argument
            // once the ship argument is added, it should no longer create one
        {
            AvailableCells();
            Ship ship = new(4, "Battleship");

            Console.WriteLine($"Please choose the first coordinate for your {ship.Name}: ");
            var response1 = GetCoordinate();

            Console.WriteLine($"Please choose the second coordinate for your {ship.Name}: ");
            var response2 = GetCoordinate();

            Console.WriteLine($"Please choose the third coordinate for your {ship.Name}: ");
            var response3 = GetCoordinate();

            Console.WriteLine($"Please choose the fourth coordinate for your {ship.Name}: ");
            var response4 = GetCoordinate();

            string[] coordinates = { response1, response2, response3, response4 };
            PlaceShip(coordinates, ship);
        }

        /// <summary>
        /// Places a ship on the cells this Board is responsible for.
        /// </summary>
        /// <param name="coordinates">
        /// an array of string coordinate names that represent where the ship will be placed
        /// <param>
        private void PlaceShip(string[] coordinates, Ship ship)
        {
            // match up coordinates with Cell items
            // confirm valid placements
            // handle valid placements gven in non-sequential order
            // handle invalid placements:
            //   - off-grid placements (nonexistent)
            //   - placements attempting to wrap around the map (split ships)
            //   - placements where the ship is pieced up and placed around the map (fragmented ships)
            //   - too few coordinates given (short)

            if (coordinates.Length != ship.length) { throw new ArgumentException("Number of coordinates provided doesn't match the ship's length."); }
            // todo: check for placements being successive
            foreach (string coordinate in coordinates)
            {
                var cellPoss = cells.Where(cell => cell.code == coordinate);
                if (!cellPoss.Any())
                {
                    Console.WriteLine($"Cell {coordinate} couldn't be found."); // this checks for whether the Cell exists
                    // need to figure out what to do with this
                    // todo: repeat the whole ship placement chain
                }
                else
                {
                    Cell selectedCell = cellPoss.First();
                    bool placed = selectedCell.PlaceShip(ship);
                    if (placed & player.human)
                    {
                        Console.WriteLine($"{ship.name} placed at {coordinate}.");
                    }
                }
            }

            ships.Add( ship ); // adding the ship to the ships list for the board certifies that it has been placed correctly
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
                bool sunk = ship?.sunk ?? false;
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
                bool sunk = ship?.sunk ?? false;
                if (cell.status == "hit" & sunk) { Console.WriteLine($"You sank my {ship.name}!"); } // this fails if it's a miss
            }
        }
    }
}