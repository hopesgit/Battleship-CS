namespace Battleship
{
    internal class InputSanitizer
    {
        public string CoordinateInput(string input)
        {
            Console.WriteLine($"\"{input}\"");
            if (input == null | input == String.Empty) { throw new ArgumentException("Invalid placement. Please try again."); }
            else
            {
                string upper = input.ToUpper();
                return upper;
            }
        }
    }
}
