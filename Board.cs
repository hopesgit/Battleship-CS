using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Battleship
{
    internal class Board
    {
        public Cell[] _cells = Array.Empty<Cell>();
        public string[] _cellCodes = Array.Empty<string>();
        public int _height = 0;
        public int _width = 0;
        public Player _player;
        public Ship[] _ships = Array.Empty<Ship>();

        public Board(int height, int width, Player player)
        {
            _height = height;
            _width = width;
            _player = player;
            
            Array alphabet = new string[26] {
                "A", "B", "C", "D", "E", "F", "G", "H", "I", "J",
                "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T",
                "U", "V", "W", "X", "Y", "Z"
            };

            int[] cell_numbers = Array.Empty<int>(); 
            Array.Resize(ref cell_numbers, _width);

            for (int i = 1; i <= _width; i++)
            {
                int index = i - 1;
                cell_numbers.SetValue(i, index);
            }
            // Console.WriteLine($"cell_numbers length: {cell_numbers.Length}");

            string[] cell_letters = Array.Empty<string>();
            Array.Resize(ref cell_letters, cell_numbers.Length);

            foreach (int i in cell_numbers)
            {
                cell_letters.SetValue(alphabet.GetValue(i-1), i-1);
            }
            // Console.WriteLine($"Cell letters: {cell_letters}");

            int total_cells = _width * _height;
            Array.Resize(ref _cells, total_cells);
            Array.Resize(ref _cellCodes, total_cells);

            int codeIndex = 0;
            foreach (string letter in cell_letters)
            {
                foreach (int number in cell_numbers)
                {
                    string code = $"{letter}{number}";
                    // Console.WriteLine($"Code: {code}");
                    _cellCodes.SetValue(code, codeIndex);
                    _cells.SetValue(new Cell(code), codeIndex);
                    codeIndex++;
                }
            }
        }

        public Player Owner 
        { 
            get { return _player; } 
        }

        public string AvailableCells()
        {
            string cell_list = "";

            foreach (Cell cell in _cells)
            {
                if (cell.IsAvailable())
                {
                    int index = cell_list.Length - 1;
                    if (index < 0) {  index = 0; };
                    cell_list.Insert(index, (cell.Code + ", "));
                }
            }

            if (cell_list.Equals(String.Empty)) { cell_list = "None"; }
            return cell_list;
        }

        /// <summary>
        /// Summons either the player or CPU ship placement routines. 
        /// If the player who owns this board is human, the human routine is called.
        /// </summary>
        public void PlaceShipSeq()
        {
            if (Owner._human) { PlayerPlaceShipSeq(); }
            //else { CPUPlaceShipSeq(); }
        }

        public static void PlayerPlaceShipSeq()
        {
            Ship ship1 = new Ship(4, "Battleship");
            /// the following lines are spaced out for readability 

            Console.WriteLine($"Please choose the first coordinate for your {ship1.Name}: ");
            var response1 = GetCoordinate();

            Console.WriteLine($"Please choose the second coordinate for your {ship1.Name}: ");
            var response2 = GetCoordinate();

            Console.WriteLine($"Please choose the third coordinate for your {ship1.Name}: ");
            var response3 = GetCoordinate();

            Console.WriteLine($"Please choose the fourth coordinate for your {ship1.Name}: ");
            var response4 = GetCoordinate();

            string[] coordinates = { response1, response2, response3, response4 };
            PlaceShip(coordinates);
        }

        /// <summary>
        /// Places a ship on the cells this Board is responsible for.
        /// </summary>
        /// <param name="coordinates">
        /// an array of string coordinate names that represent where the ship will be placed
        /// <param>
        private static bool PlaceShip(string[] coordinates)
        {
            /// return true;
            
        }

        /// <summary>
        /// Handles the coordinate responses from the user.
        /// </summary>
        /// <returns>
        /// the coordinate string, if it's valid
        /// </returns>
        private static string GetCoordinate()
        {
            string response = Console.ReadLine();
            InputSanitizer sanit = new();
            string coordinate = String.Empty;
            try 
            { 
                string corrected_coordinate = sanit.CoordinateInput(response); 
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
    }
}